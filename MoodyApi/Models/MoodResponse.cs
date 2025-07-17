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
    }
}
