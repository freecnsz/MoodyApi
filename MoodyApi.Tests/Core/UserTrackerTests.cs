using FluentAssertions;
using MoodyApi.Core;

namespace MoodyApi.Tests.Core
{
    public class UserTrackerTests
    {
        [Fact]
        public void RecordRequest_ShouldIncrementRequestCount()
        {
            // Arrange
            var tracker = new UserTracker();
            var userId = "test-user";

            // Act
            tracker.RecordRequest(userId);
            tracker.RecordRequest(userId);
            tracker.RecordRequest(userId);

            // Assert
            var karmaScore = tracker.GetKarmaScore(userId);
            karmaScore.Should().Be(3);
        }

        [Fact]
        public void GetKarmaScore_ShouldReturnZero_ForUnknownUser()
        {
            // Arrange
            var tracker = new UserTracker();

            // Act
            var karmaScore = tracker.GetKarmaScore("unknown-user");

            // Assert
            karmaScore.Should().Be(0);
        }

        [Fact]
        public void HasReachedKarmaThreshold_ShouldReturnTrue_WhenThresholdReached()
        {
            // Arrange
            var tracker = new UserTracker();
            var userId = "test-user";
            var threshold = 5;

            // Act
            for (int i = 0; i < threshold; i++)
            {
                tracker.RecordRequest(userId);
            }

            // Assert
            tracker.HasReachedKarmaThreshold(userId, threshold).Should().BeTrue();
        }

        [Fact]
        public void HasReachedKarmaThreshold_ShouldReturnFalse_WhenThresholdNotReached()
        {
            // Arrange
            var tracker = new UserTracker();
            var userId = "test-user";
            var threshold = 5;

            // Act
            for (int i = 0; i < threshold - 1; i++)
            {
                tracker.RecordRequest(userId);
            }

            // Assert
            tracker.HasReachedKarmaThreshold(userId, threshold).Should().BeFalse();
        }

        [Fact]
        public async Task RecordRequest_ShouldHandleConcurrentRequests()
        {
            // Arrange
            var tracker = new UserTracker();
            var userId = "test-user";
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => tracker.RecordRequest(userId)));
            }

            await Task.WhenAll(tasks);

            // Assert
            var karmaScore = tracker.GetKarmaScore(userId);
            karmaScore.Should().Be(100);
        }
    }
}