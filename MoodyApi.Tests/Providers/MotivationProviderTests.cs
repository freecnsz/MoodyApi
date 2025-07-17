using FluentAssertions;
using MoodyApi.Providers;

namespace MoodyApi.Tests.Providers
{
    public class MotivationProviderTests
    {
        [Fact]
        public void GetMessage_ShouldReturnMotivationalMessage()
        {
            // Arrange
            var provider = new MotivationProvider();

            // Act
            var message = provider.GetMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
            message.Should().MatchRegex(@"[A-Z].*[.!]$"); // Should start with capital and end with punctuation
        }

        [Fact]
        public void GetMessage_ShouldReturnPositiveMessage()
        {
            // Arrange
            var provider = new MotivationProvider();
            var positiveWords = new[] { "capable", "believe", "dream", "success", "amazing", "focused" };

            // Act
            var message = provider.GetMessage();

            // Assert
            positiveWords.Should().Contain(word => message.ToLower().Contains(word.ToLower()));
        }
    }
}