#nullable enable
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MoodyApi.Configuration;
using MoodyApi.Core;
using MoodyApi.Models;

namespace MoodyApi.Extensions
{
    /// <summary>
    /// ASP.NET Core middleware extension for injecting mood-based messages into responses.
    /// </summary>
    public static class MoodyApiMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware that wraps HTTP responses with mood-based messages.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configure">An optional configuration action for mood options.</param>
        /// <returns>The modified application builder.</returns>
        public static IApplicationBuilder UseMoodyApi(this IApplicationBuilder app, Action<MoodOptions>? configure = null)
        {
            return app.UseMiddleware<MoodyApiMiddleware>();
        }
    }

    /// <summary>
    /// Middleware that adds mood-based messages to HTTP responses.
    /// </summary>
    public class MoodyApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MoodEngine _moodEngine;
        private readonly MoodOptions _options;
        private readonly ILogger<MoodyApiMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodyApiMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="moodEngine">The mood engine used to generate mood responses.</param>
        /// <param name="options">The mood options configuration.</param>
        /// <param name="logger">The logger instance.</param>
        public MoodyApiMiddleware(RequestDelegate next, MoodEngine moodEngine, MoodOptions options, ILogger<MoodyApiMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _moodEngine = moodEngine ?? throw new ArgumentNullException(nameof(moodEngine));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Processes an HTTP request and wraps the response with mood-based messages.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (ShouldSkipWrapping(context))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            try
            {
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                await _next(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                object? data = ParseResponseData(responseBody, context.Response.ContentType);

                var userId = GetUserId(context);
                var moodResponse = _moodEngine.GetMoodResponse(_options.Mode, userId);

                var wrappedResponse = new
                {
                    success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300,
                    statusCode = context.Response.StatusCode,
                    data = data,
                    mood = moodResponse
                };

                await WriteWrappedResponse(context, originalBodyStream, wrappedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MoodyApiMiddleware");
                context.Response.Body = originalBodyStream;
                throw;
            }
        }

        private bool ShouldSkipWrapping(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            if (path != null)
            {
                if (path.Contains("swagger") || path.Contains("api-docs") ||
                    path.Contains("css") || path.Contains("js") || path.Contains("images") ||
                    path.Contains("fonts") || path.Contains("favicon") ||
                    path.Contains("health") || path.Contains("ready") || path.Contains("live") ||
                    path.Contains("metrics") || path.Contains("prometheus"))
                {
                    return true;
                }
            }

            var acceptHeader = context.Request.Headers["Accept"].FirstOrDefault();
            if (acceptHeader != null &&
                !acceptHeader.Contains("application/json") &&
                !acceptHeader.Contains("*/*") &&
                !acceptHeader.Contains("text/plain"))
            {
                return true;
            }

            return false;
        }

        private object? ParseResponseData(string responseBody, string? contentType)
        {
            try
            {
                if (string.IsNullOrEmpty(responseBody))
                    return null;

                if (contentType?.Contains("application/json") == true)
                    return JsonSerializer.Deserialize<object>(responseBody);

                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse response data as JSON, returning as string");
                return responseBody;
            }
        }

        private string GetUserId(HttpContext context)
        {
            try
            {
                return context.Request.Headers["X-User-ID"].FirstOrDefault() ??
                       context.Connection.RemoteIpAddress?.ToString() ??
                       "anonymous";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get user ID, using anonymous");
                return "anonymous";
            }
        }

        private async Task WriteWrappedResponse(HttpContext context, Stream originalBodyStream, object wrappedResponse)
        {
            try
            {
                var wrappedJson = JsonSerializer.Serialize(wrappedResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var wrappedBytes = Encoding.UTF8.GetBytes(wrappedJson);

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = wrappedBytes.Length;

                await context.Response.Body.WriteAsync(wrappedBytes, 0, wrappedBytes.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write wrapped response");
                throw;
            }
        }
    }
}
