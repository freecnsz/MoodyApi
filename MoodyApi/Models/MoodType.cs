namespace MoodyApi.Models
{
    /// <summary>
    /// Represents the different types of moods the API can respond with.
    /// </summary>
    public enum MoodType
    {
        /// <summary>
        /// Neutral, standard responses.
        /// </summary>
        Neutral,
        /// <summary>
        /// Motivational, positive responses.
        /// </summary>
        Motivational,
        /// <summary>
        /// Sarcastic, humorous responses.
        /// </summary>
        Sarcastic,
        /// <summary>
        /// Karma-based, user activity dependent responses.
        /// </summary>
        KarmaBased,
        /// <summary>
        /// Time-based, context-aware responses.
        /// </summary>
        TimeBased,
        /// <summary>
        /// Error-based, fallback responses for errors.
        /// </summary>
        ErrorBased
    }
}