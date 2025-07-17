using FluentAssertions;
using MoodyApi.Providers;

namespace MoodyApi.Tests.Providers
{
    public class KarmaProviderTests
    {
        [Fact]
        public void GetMessage_ShouldReturnPhilosophicalMessage()
        {
            // Arrange
            var provider = new KarmaProvider();

            // Act
            var message = provider.GetMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMessage_ShouldContainPhilosophicalElements()
        {
            // Arrange
            var provider = new KarmaProvider();
            var philosophicalWords = new[] { "seek", "universe", "balance", "soul", "echo", "karma", "cosmic" };

            // Act
            var message = provider.GetMessage();

            // Assert
            philosophicalWords.Should().Contain(word => 
                message.ToLower().Contains(word.ToLower()));
        }
    }
}