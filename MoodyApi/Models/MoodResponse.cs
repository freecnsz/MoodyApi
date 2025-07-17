using System;

namespace MoodyApi.Models
{
    /// <summary>
    /// Represents the response returned by the Moody API.
    /// </summary>
    public class MoodResponse
    {
        /// <summary>
        /// The message content based on the mood type.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The mood that was used to generate the message.
        /// </summary>
        public MoodType Mood { get; set; }

        /// <summary>
        /// UTC timestamp when the response was generated.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional karma score, if applicable.
        /// </summary>
        public int? KarmaScore { get; set; } = null;
    }
}