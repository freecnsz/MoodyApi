#nullable enable
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{

    /// <summary>
    /// Provides sarcastic responses.
    /// </summary>
    public class SarcasmProvider : BaseProvider
    {
        private static readonly string[] Messages = new[]
        {
            "Oh great, another API call. I'm thrilled. 🙄",
            "Wow, you're back again? Shocking.",
            "Because clearly the server had nothing better to do.",
            "Just what I needed today – more traffic!",
            "Another request? How delightfully predictable.",
            "Well, well, well... look who's here again."
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SarcasmProvider"/> class.
        /// </summary>
        /// <param name="logger">An optional logger instance.</param>
        public SarcasmProvider(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Retrieves a random sarcastic message.
        /// </summary>
        /// <returns>A randomly selected sarcastic message string.</returns>
        protected override string GetMessageInternal()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}