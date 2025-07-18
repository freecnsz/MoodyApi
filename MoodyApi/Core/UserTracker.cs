#nullable enable
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace MoodyApi.Core
{
    /// <summary>
    /// Tracks user interactions for karma-based responses with automatic cleanup.
    /// </summary>
    public class UserTracker : IDisposable
    {
        private readonly ConcurrentDictionary<string, UserStats> _userStats = new();
        private readonly Timer _cleanupTimer;
        private readonly ILogger<UserTracker>? _logger;
        private readonly TimeSpan _userExpirationTime = TimeSpan.FromHours(24);
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTracker"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to use for logging (optional).</param>
        public UserTracker(ILogger<UserTracker>? logger = null)
        {
            _logger = logger;
            // Run cleanup every hour
            _cleanupTimer = new Timer(CleanupExpiredUsers, null, TimeSpan.FromHours(1), TimeSpan.FromHours(1));
        }

        /// <summary>
        /// Records a request for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void RecordRequest(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger?.LogWarning("Attempted to record request for null or empty userId");
                return;
            }

            try
            {
                _userStats.AddOrUpdate(userId,
                    new UserStats { RequestCount = 1, LastRequestTime = DateTime.UtcNow },
                    (key, stats) => new UserStats
                    {
                        RequestCount = stats.RequestCount + 1,
                        LastRequestTime = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error recording request for user {UserId}", userId);
            }
        }

        /// <summary>
        /// Gets the karma score for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The karma score.</returns>
        public int GetKarmaScore(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return 0;
            }

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

        /// <summary>
        /// Gets the total number of tracked users.
        /// </summary>
        public int GetTrackedUserCount()
        {
            return _userStats.Count;
        }

        /// <summary>
        /// Cleans up expired user data to prevent memory leaks.
        /// </summary>
        private void CleanupExpiredUsers(object? state)
        {
            try
            {
                lock (_lock)
                {
                    var cutoffTime = DateTime.UtcNow - _userExpirationTime;
                    var expiredUsers = _userStats
                        .Where(kvp => kvp.Value.LastRequestTime < cutoffTime)
                        .Select(kvp => kvp.Key)
                        .ToList();

                    foreach (var userId in expiredUsers)
                    {
                        _userStats.TryRemove(userId, out _);
                    }

                    if (expiredUsers.Count > 0)
                    {
                        _logger?.LogInformation("Cleaned up {Count} expired user records", expiredUsers.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during user cleanup");
            }
        }

        /// <summary>
        /// Releases the resources used by the <see cref="UserTracker"/> instance.
        /// </summary>
        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }

        private class UserStats
        {
            public int RequestCount { get; set; }
            public DateTime LastRequestTime { get; set; }
        }
    }
}