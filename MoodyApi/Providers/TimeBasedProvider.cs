#nullable enable
using Microsoft.Extensions.Logging;

namespace MoodyApi.Providers
{

    /// <summary>
    /// Provides time-based messages depending on the current hour.
    /// </summary>
    public class TimeBasedProvider : BaseProvider
    {
        private static readonly Dictionary<TimeOfDay, string[]> Messages = new()
        {
            [TimeOfDay.Morning] = new[]
            {
                "Good morning! Ready to start fresh?",
                "Rise and shine! Another day of possibilities.",
                "Morning energy is the best energy!",
                "The early bird catches the API response."
            },
            [TimeOfDay.Afternoon] = new[]
            {
                "Afternoon productivity in full swing!",
                "Midday momentum is strong.",
                "Afternoon calls are always welcome.",
                "Peak performance time!"
            },
            [TimeOfDay.Evening] = new[]
            {
                "Evening requests have a special charm.",
                "Winding down but still serving.",
                "Evening is for reflection and responses.",
                "Twilight brings thoughtful interactions."
            },
            [TimeOfDay.Night] = new[]
            {
                "Burning the midnight oil?",
                "Night owls deserve the best responses.",
                "Late night, but still here for you.",
                "The server never sleeps, just like you."
            }
        };


        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBasedProvider"/> class.
        /// </summary>
        /// <param name="logger">An optional logger instance.</param>
        public TimeBasedProvider(ILogger? logger = null) : base(logger) { }

        /// <summary>
        /// Retrieves a random message based on the current time of day.
        /// </summary>
        protected override string GetMessageInternal()
        {
            var timeOfDay = GetCurrentTimeOfDay();
            var messages = Messages[timeOfDay];
            var index = Random.Shared.Next(messages.Length);
            return messages[index];
        }

        private TimeOfDay GetCurrentTimeOfDay()
        {
            var hour = DateTime.Now.Hour;
            return hour switch
            {
                >= 6 and < 12 => TimeOfDay.Morning,
                >= 12 and < 17 => TimeOfDay.Afternoon,
                >= 17 and < 21 => TimeOfDay.Evening,
                _ => TimeOfDay.Night
            };
        }

        private enum TimeOfDay
        {
            Morning,
            Afternoon,
            Evening,
            Night
        }
    }
}
