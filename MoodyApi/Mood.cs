using MoodyApi.Core;
using MoodyApi.Models;

namespace MoodyApi
{
    /// <summary>
    /// Public API for generating mood-based messages.
    /// </summary>
    public static class Mood
    {
        /// <summary>
        /// Returns a random message for the given mood type.
        /// </summary>
        /// <param name="mood">The desired mood.</param>
        /// <returns>A mood-appropriate message string.</returns>
        public static string Get(MoodType mood)
        {
            return MoodEngine.GetMessage(mood);
        }
    }
}
