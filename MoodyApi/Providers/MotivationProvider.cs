#nullable enable
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{

    /// <summary>
    /// Provides motivational messages.
    /// </summary>
    public class MotivationProvider : BaseProvider
    {
        private static readonly string[] Messages = new[]
        {
            "You are capable of amazing things.",
            "Believe in yourself and all that you are.",
            "Don't watch the clock; do what it does. Keep going.",
            "Dream big. Work hard. Stay focused.",
            "Success is not final, failure is not fatal: it is the courage to continue that counts.",
            "The future belongs to those who believe in the beauty of their dreams."
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MotivationProvider"/> class.
        /// </summary>
        /// <param name="logger">An optional logger instance.</param>
        public MotivationProvider(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Retrieves a random motivational message.
        /// </summary>
        /// <returns>A randomly selected motivational message string.</returns>
        protected override string GetMessageInternal()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}