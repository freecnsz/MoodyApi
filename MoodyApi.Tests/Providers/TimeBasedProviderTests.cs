using FluentAssertions;
using MoodyApi.Providers;

namespace MoodyApi.Tests.Providers
{
    public class TimeBasedProviderTests
    {
        [Fact]
        public void GetMessage_ShouldReturnTimeAppropriateMessage()
        {
            // Arrange
            var provider = new TimeBasedProvider();

            // Act
            var message = provider.GetMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMessage_ShouldContainTimeRelatedWords()
        {
            // Arrange
            var provider = new TimeBasedProvider();
            var timeWords = new[] { "morning", "afternoon", "evening", "night", "day", "time", "early", "late" };

            // Act
            var message = provider.GetMessage();

            // Assert
            timeWords.Should().Contain(word => 
                message.ToLower().Contains(word.ToLower()));
        }
    }
}