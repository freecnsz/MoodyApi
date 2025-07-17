using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoodyApi.Core
{
    /// <summary>
    /// Tracks user interactions for karma-based responses.
    /// </summary>
    public class UserTracker
    {
        private readonly ConcurrentDictionary<string, UserStats> _userStats = new();

        /// <summary>
        /// Records a request for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void RecordRequest(string userId)
        {
            _userStats.AddOrUpdate(userId, 
                new UserStats { RequestCount = 1, LastRequestTime = DateTime.UtcNow },
                (key, stats) => new UserStats 
                { 
                    RequestCount = stats.RequestCount + 1, 
                    LastRequestTime = DateTime.UtcNow 
                });
        }

        /// <summary>
        /// Gets the karma score for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The karma score.</returns>
        public int GetKarmaScore(string userId)
        {
            return _userStats.TryGetValue(userId, out var stats) ? stats.RequestCount : 0;
        }

        /// <summary>
        /// Checks if a user has reached the karma threshold.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="threshold">The karma threshold.</param>
        /// <returns>True if threshold is reached.</returns>
        public bool HasReachedKarmaThreshold(string userId, int threshold)
        {
            return GetKarmaScore(userId) >= threshold;
        }

        private class UserStats
        {
            public int RequestCount { get; set; }
            public DateTime LastRequestTime { get; set; }
        }
    }
}