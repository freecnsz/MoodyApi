using FluentAssertions;
using MoodyApi.Providers;

namespace MoodyApi.Tests.Providers
{
    public class NeutralProviderTests
    {
        [Fact]
        public void GetMessage_ShouldReturnNonEmptyString()
        {
            // Arrange
            var provider = new NeutralProvider();

            // Act
            var message = provider.GetMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMessage_ShouldReturnDifferentMessages_WhenCalledMultipleTimes()
        {
            // Arrange
            var provider = new NeutralProvider();
            var messages = new HashSet<string>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                messages.Add(provider.GetMessage());
            }

            // Assert - Should have at least 2 different messages in 100 calls
            messages.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public void GetMessage_ShouldReturnValidNeutralMessage()
        {
            // Arrange
            var provider = new NeutralProvider();
            var expectedMessages = new[]
            {
                "Request processed successfully.",
                "Operation completed.",
                "Response generated.",
                "API call handled.",
                "Service is running normally."
            };

            // Act
            var message = provider.GetMessage();

            // Assert
            expectedMessages.Should().Contain(message);
        }
    }
}