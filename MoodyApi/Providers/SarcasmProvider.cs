using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides sarcastic responses.
    /// </summary>
    public class SarcasmProvider : IMessageProvider
    {
        private static readonly string[] Messages = new[]
        {
            "Oh great, another API call. I'm thrilled. 🙄",
            "Wow, you're back again? Shocking.",
            "Because clearly the server had nothing better to do.",
            "Just what I needed today – more traffic!",
        };

        public string GetMessage()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}
