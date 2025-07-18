#nullable enable
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides error-specific messages.
    /// </summary>
    public class ErrorBasedProvider : BaseProvider
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBasedProvider"/> class.
        /// </summary>
        /// <param name="logger">An optional logger instance.</param>
        public ErrorBasedProvider(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Retrieves a random error-specific message.
        /// </summary>
        /// <returns>A randomly selected error message string.</returns>
        protected override string GetMessageInternal()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}