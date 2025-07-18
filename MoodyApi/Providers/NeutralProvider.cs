#nullable enable
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides neutral, standard responses.
    /// </summary>
    public class NeutralProvider : BaseProvider
    {
        private static readonly string[] Messages = new[]
        {
            "Request processed successfully.",
            "Operation completed.",
            "Response generated.",
            "API call handled.",
            "Service is running normally."
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="NeutralProvider"/> class.
        /// </summary>
        /// <param name="logger">An optional logger instance.</param>
        public NeutralProvider(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Retrieves a random neutral message.
        /// </summary>
        /// <returns>A randomly selected neutral message string.</returns>
        protected override string GetMessageInternal()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}