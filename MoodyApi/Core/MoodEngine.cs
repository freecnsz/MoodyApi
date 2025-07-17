using MoodyApi.Models;
using MoodyApi.Providers;
using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Core
{
    /// <summary>
    /// Engine for resolving mood-based message providers.
    /// </summary>
    public static class MoodEngine
    {
        /// <summary>
        /// Gets a mood-based message.
        /// </summary>
        /// <param name="mood">The mood type.</param>
        /// <returns>A message string matching the selected mood.</returns>
        public static string GetMessage(MoodType mood)
        {
            IMessageProvider provider = mood switch
            {
                MoodType.Sarcastic => new SarcasmProvider(),
                MoodType.Motivational => new MotivationProvider(),
                MoodType.KarmaBased => new KarmaProvider(),
                _ => throw new ArgumentOutOfRangeException(nameof(mood), "Unsupported mood type.")
            };

            return provider.GetMessage();
        }
    }
}
