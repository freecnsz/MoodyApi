#nullable enable
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{

    /// <summary>
    /// Provides cryptic, philosophical karma-based messages.
    /// </summary>
    public class KarmaProvider : BaseProvider
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
        /// Initializes a new instance of the <see cref="KarmaProvider"/> class.
        /// </summary>
        /// <param name="logger">An optional logger instance.</param>
        public KarmaProvider(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Retrieves a random karma-based message.
        /// </summary>
        /// <returns>A randomly selected karma message string.</returns>
        protected override string GetMessageInternal()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}