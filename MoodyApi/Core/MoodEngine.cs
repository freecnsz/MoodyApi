#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MoodEngine>? _logger;

        /// <summary>
        /// Creates an instance of MoodEngine with configuration and providers.
        /// </summary>
        /// <param name="options">Configuration options controlling mood behavior.</param>
        /// <param name="providers">Dictionary of mood providers keyed by MoodType.</param>
        /// <param name="userTracker">User tracking service for karma calculations.</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        public MoodEngine(MoodOptions options, Dictionary<MoodType, IMessageProvider> providers, UserTracker userTracker, ILogger<MoodEngine>? logger = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _providers = providers ?? throw new ArgumentNullException(nameof(providers));
            _userTracker = userTracker ?? throw new ArgumentNullException(nameof(userTracker));
            _logger = logger;

            _logger?.LogInformation("MoodEngine initialized with {ProviderCount} providers", _providers.Count);
        }

        /// <summary>
        /// Generates a mood-based response object based on mood settings.
        /// </summary>
        /// <param name="moodType">Optional override for specific mood.</param>
        /// <param name="userId">Optional user identifier for karma tracking.</param>
        /// <returns>A MoodResponse object with content and metadata.</returns>
        public MoodResponse GetMoodResponse(MoodType? moodType = null, string? userId = null)
        {
            try
            {
                var typeToUse = moodType ?? ResolveMoodFromOptions();

                // Track user if provided
                if (!string.IsNullOrEmpty(userId))
                {
                    _userTracker.RecordRequest(userId);
                }

                var message = GetMessage(typeToUse);

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

                _logger?.LogDebug("Generated mood response: {Mood} for user: {UserId}", typeToUse, userId ?? "anonymous");
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error generating mood response for user {UserId}", userId);
                return new MoodResponse
                {
                    Message = "Something went wrong, but we're still here!",
                    Mood = MoodType.ErrorBased,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Gets a simple message for the given mood type.
        /// </summary>
        /// <param name="moodType">The mood type.</param>
        /// <returns>A mood-appropriate message.</returns>
        public string GetMessage(MoodType moodType)
        {
            try
            {
                if (_providers.TryGetValue(moodType, out var provider))
                {
                    return provider.GetMessage();
                }

                _logger?.LogWarning("No provider found for mood type: {MoodType}", moodType);
                return "No mood available.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting message for mood type: {MoodType}", moodType);
                return "Something went wrong, but we're still here!";
            }
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