// Updated Public API with better error handling
#nullable enable
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoodyApi.Models;
using MoodyApi.Core;

namespace MoodyApi
{
    /// <summary>
    /// Public API for generating mood-based messages.
    /// </summary>
    public static class Mood
    {
        private static MoodEngine? _moodEngine;
        private static ILogger? _logger;

        /// <summary>
        /// Initializes the Mood API with a service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider containing MoodEngine.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            try
            {
                _moodEngine = serviceProvider.GetRequiredService<MoodEngine>();
                _logger = serviceProvider.GetService<ILogger<MoodEngine>>();
                _logger?.LogInformation("Mood API initialized successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to initialize Mood API");
                throw;
            }
        }

        /// <summary>
        /// Returns a random message for the given mood type.
        /// </summary>
        /// <param name="mood">The desired mood.</param>
        /// <returns>A mood-appropriate message string.</returns>
        public static string Get(MoodType mood)
        {
            EnsureInitialized();

            try
            {
                return _moodEngine!.GetMessage(mood);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting mood message for {MoodType}", mood);
                return "Something went wrong, but we're still here!";
            }
        }

        /// <summary>
        /// Returns a complete mood response for the given mood type.
        /// </summary>
        /// <param name="mood">The desired mood.</param>
        /// <param name="userId">Optional user identifier for karma tracking.</param>
        /// <returns>A MoodResponse object with content and metadata.</returns>
        public static MoodResponse GetResponse(MoodType mood, string? userId = null)
        {
            EnsureInitialized();

            try
            {
                return _moodEngine!.GetMoodResponse(mood, userId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting mood response for {MoodType}", mood);
                return new MoodResponse
                {
                    Message = "Something went wrong, but we're still here!",
                    Mood = MoodType.ErrorBased,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        private static void EnsureInitialized()
        {
            if (_moodEngine == null)
            {
                throw new InvalidOperationException("Mood API is not initialized. Call Mood.Initialize() first.");
            }
        }
    }
}