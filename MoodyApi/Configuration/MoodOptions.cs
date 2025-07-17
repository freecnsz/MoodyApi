using MoodyApi.Models;

namespace MoodyApi.Configuration
{
    /// <summary>
    /// Configuration options for mood-based responses.
    /// These can be loaded from appsettings.json or passed manually.
    /// </summary>
    public class MoodOptions
    {
        /// <summary>
        /// Enables sarcastic responses in error handling.
        /// </summary>
        public bool EnableSarcasm { get; set; } = true;

        /// <summary>
        /// Enables motivational messages in successful responses.
        /// </summary>
        public bool EnableMotivation { get; set; } = true;

        /// <summary>
        /// Enables karma-based messages.
        /// </summary>
        public bool EnableKarma { get; set; } = true;

        /// <summary>
        /// Enables time-based messages.
        /// </summary>
        public bool EnableTimeBased { get; set; } = true;

        /// <summary>
        /// Defines how many successful requests are needed to trigger a karma message.
        /// </summary>
        public int KarmaThreshold { get; set; } = 5;

        /// <summary>
        /// The mood type to use for generating messages.
        /// </summary>
        public MoodType Mode { get; set; } = MoodType.Neutral;
    }
}
