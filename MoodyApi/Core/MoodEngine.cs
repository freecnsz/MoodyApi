using MoodyApi.Configuration;
using MoodyApi.Models;
using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Core
{
    /// <summary>
    /// Core engine to resolve mood-specific messages based on configuration.
    /// </summary>
    public class MoodEngine
    {
        private readonly MoodOptions _options;
        private readonly Dictionary<MoodType, IMessageProvider> _providers;
        private readonly UserTracker _userTracker;

        /// <summary>
        /// Creates an instance of MoodEngine with configuration and providers.
        /// </summary>
        /// <param name="options">Configuration options controlling mood behavior.</param>
        /// <param name="providers">Dictionary of mood providers keyed by MoodType.</param>
        /// <param name="userTracker">User tracking service for karma calculations.</param>
        public MoodEngine(MoodOptions options, Dictionary<MoodType, IMessageProvider> providers, UserTracker userTracker)
        {
            _options = options;
            _providers = providers;
            _userTracker = userTracker;
        }

        /// <summary>
        /// Generates a mood-based response object based on mood settings.
        /// </summary>
        /// <param name="moodType">Optional override for specific mood.</param>
        /// <param name="userId">Optional user identifier for karma tracking.</param>
        /// <returns>A MoodResponse object with content and metadata.</returns>
        public MoodResponse GetMoodResponse(MoodType? moodType = null, string? userId = null)
        {
            var typeToUse = moodType ?? ResolveMoodFromOptions();
            
            // Track user if provided
            if (!string.IsNullOrEmpty(userId))
            {
                _userTracker.RecordRequest(userId);
            }

            var message = _providers.TryGetValue(typeToUse, out var provider)
                ? provider.GetMessage()
                : "No mood available.";

            var response = new MoodResponse
            {
                Message = message,
                Mood = typeToUse,
                Timestamp = DateTime.UtcNow
            };

            // Add karma score if user is tracked
            if (!string.IsNullOrEmpty(userId))
            {
                response.KarmaScore = _userTracker.GetKarmaScore(userId);
            }

            return response;
        }

        /// <summary>
        /// Gets a simple message for the given mood type.
        /// </summary>
        /// <param name="moodType">The mood type.</param>
        /// <returns>A mood-appropriate message.</returns>
        public string GetMessage(MoodType moodType)
        {
            return _providers.TryGetValue(moodType, out var provider)
                ? provider.GetMessage()
                : "No mood available.";
        }

        /// <summary>
        /// Resolves the appropriate mood type based on configuration flags.
        /// </summary>
        private MoodType ResolveMoodFromOptions()
        {
            // Check for specific mode override
            if (_options.Mode != MoodType.Neutral)
            {
                return _options.Mode;
            }

            // Fallback to flag-based resolution
            if (_options.EnableSarcasm) return MoodType.Sarcastic;
            if (_options.EnableMotivation) return MoodType.Motivational;
            if (_options.EnableKarma) return MoodType.KarmaBased;
            if (_options.EnableTimeBased) return MoodType.TimeBased;
            
            return MoodType.Neutral;
        }
    }
}