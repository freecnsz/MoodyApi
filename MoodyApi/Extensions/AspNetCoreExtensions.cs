using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MoodyApi.Configuration;
using MoodyApi.Models;

namespace MoodyApi.Extensions
{
    /// <summary>
    /// ASP.NET Core middleware extension for injecting mood-based messages into responses.
    /// </summary>
    public static class MoodyApiMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware that overrides successful HTTP responses with a mood-based message.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configure">An optional configuration action for mood options.</param>
        /// <returns>The modified application builder.</returns>
        public static IApplicationBuilder UseMoodyApi(this IApplicationBuilder app, Action<MoodOptions>? configure = null)
        {
            var options = new MoodOptions();
            configure?.Invoke(options);

            return app.Use(async (context, next) =>
            {
                // Process the request pipeline
                await next();

                // Only intercept successful (200 OK) responses
                if (context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    // Generate mood message
                    var message = Mood.Get(options.Mode);

                    var response = new MoodResponse
                    {
                        Mood = options.Mode,
                        Message = message
                    };

                    // Replace response body with mood response
                    context.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(response);
                    await context.Response.WriteAsync(json);
                }
            });
        }
    }
}
