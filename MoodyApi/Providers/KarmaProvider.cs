using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides cryptic, philosophical karma-based messages.
    /// </summary>
    public class KarmaProvider : IMessageProvider
    {
        private static readonly string[] Messages = new[]
        {
            "What you seek is also seeking you.",
            "Every request returns in kind.",
            "Your past calls echo in the server's soul.",
            "Balance is found in even requests and even responses.",
            "The universe responds to those who ask with intention.",
            "Your digital footprint creates ripples in the cosmic code."
        };

        /// <summary>
        /// Returns a random karma-related message.
        /// </summary>
        public string GetMessage()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}