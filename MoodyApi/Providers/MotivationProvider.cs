using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Providers
{
    /// <summary>
    /// Provides motivational messages.
    /// </summary>
    public class MotivationProvider : IMessageProvider
    {
        private static readonly string[] Messages = new[]
        {
            "You are capable of amazing things.",
            "Believe in yourself and all that you are.",
            "Don't watch the clock; do what it does. Keep going.",
            "Dream big. Work hard. Stay focused.",
        };

        public string GetMessage()
        {
            var index = Random.Shared.Next(Messages.Length);
            return Messages[index];
        }
    }
}
