namespace MoodyApi.Providers.Interfaces
{
    /// <summary>
    /// Interface for mood-based message providers.
    /// </summary>
    public interface IMessageProvider
    {
        /// <summary>
        /// Gets a mood-specific message.
        /// </summary>
        /// <returns>The message string.</returns>
        string GetMessage();
    }
}