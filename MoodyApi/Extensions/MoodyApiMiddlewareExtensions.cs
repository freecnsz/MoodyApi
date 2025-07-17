#nullable enable
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodyApiMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="moodEngine">The mood engine instance.</param>
        /// <param name="options">The mood options.</param>
        public MoodyApiMiddleware(RequestDelegate next, MoodEngine moodEngine, MoodOptions options)
        {
            _next = next;
            _moodEngine = moodEngine;
            _options = options;
        }

        /// <summary>
        /// Handles the HTTP request and injects mood-based response data.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            // Store the original response body stream
            var originalBodyStream = context.Response.Body;

            try
            {
                // Create a new memory stream for the response body
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                // Process the request
                await _next(context);

                // Read the response body
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                // Generate mood response
                var userId = context.Request.Headers["X-User-ID"].FirstOrDefault() ?? 
                           context.Connection.RemoteIpAddress?.ToString() ?? 
                           "anonymous";

                var moodResponse = _moodEngine.GetMoodResponse(_options.Mode, userId);

                // Create wrapped response
                var wrappedResponse = new
                {
                    success = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300,
                    statusCode = context.Response.StatusCode,
                    data = string.IsNullOrEmpty(responseBody) ? null : JsonSerializer.Deserialize<object>(responseBody),
                    mood = moodResponse
                };

                // Write the wrapped response
                var wrappedJson = JsonSerializer.Serialize(wrappedResponse, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                
                var wrappedBytes = Encoding.UTF8.GetBytes(wrappedJson);

                // Reset the response body stream
                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = wrappedBytes.Length;

                await context.Response.Body.WriteAsync(wrappedBytes, 0, wrappedBytes.Length);
            }
            catch (Exception)
            {
                // Restore original stream in case of error
                context.Response.Body = originalBodyStream;
                throw;
            }
        }
    }
}