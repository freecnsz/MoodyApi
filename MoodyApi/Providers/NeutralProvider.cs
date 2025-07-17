using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides neutral, standard responses.
    /// </summary>
    public class NeutralProvider : IMessageProvider
    {
        private static readonly string[] Messages = new[]
        {
            "Request processed successfully.",
            "Operation completed.",
            "Response generated.",
            "API call handled.",
            "Service is running normally."
        };

        public string GetMessage()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}