using MoodyApi.Models;
using Microsoft.Extensions.DependencyInjection;
using MoodyApi.Core;

namespace MoodyApi
{
    /// <summary>
    /// Public API for generating mood-based messages.
    /// </summary>
    public static class Mood
    {
        private static MoodEngine? _moodEngine;

        /// <summary>
        /// Initializes the Mood API with a service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider containing MoodEngine.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _moodEngine = serviceProvider.GetRequiredService<MoodEngine>();
        }

        /// <summary>
        /// Returns a random message for the given mood type.
        /// </summary>
        /// <param name="mood">The desired mood.</param>
        /// <returns>A mood-appropriate message string.</returns>
        public static string Get(MoodType mood)
        {
            if (_moodEngine == null)
            {
                throw new InvalidOperationException("Mood API is not initialized. Call Mood.Initialize() first.");
            }

            return _moodEngine.GetMessage(mood);
        }

        /// <summary>
        /// Returns a complete mood response for the given mood type.
        /// </summary>
        /// <param name="mood">The desired mood.</param>
        /// <param name="userId">Optional user identifier for karma tracking.</param>
        /// <returns>A MoodResponse object with content and metadata.</returns>
        public static MoodResponse GetResponse(MoodType mood, string? userId = null)
        {
            if (_moodEngine == null)
            {
                throw new InvalidOperationException("Mood API is not initialized. Call Mood.Initialize() first.");
            }

            return _moodEngine.GetMoodResponse(mood, userId);
        }
    }
}