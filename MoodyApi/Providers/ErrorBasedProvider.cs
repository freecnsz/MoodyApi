using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides error-specific messages.
    /// </summary>
    public class ErrorBasedProvider : IMessageProvider
    {
        private static readonly string[] Messages = new[]
        {
            "Oops! Something went sideways.",
            "Error 404: Mood not found, but here's a backup.",
            "Technical difficulties, but we're rolling with it.",
            "Houston, we have a problem... but also a solution.",
            "Bug detected, attitude adjusted accordingly.",
            "Exception caught, resilience activated."
        };

        public string GetMessage()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}