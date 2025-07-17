using FluentAssertions;
using MoodyApi.Providers;

namespace MoodyApi.Tests.Providers
{
    public class SarcasmProviderTests
    {
        [Fact]
        public void GetMessage_ShouldReturnSarcasticMessage()
        {
            // Arrange
            var provider = new SarcasmProvider();

            // Act
            var message = provider.GetMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMessage_ShouldContainSarcasticElements()
        {
            // Arrange
            var provider = new SarcasmProvider();
            var sarcasticIndicators = new[] { "Oh", "Wow", "great", "thrilled", "shocking", "delightfully" };

            // Act
            var message = provider.GetMessage();

            // Assert
            sarcasticIndicators.Should().Contain(indicator => 
                message.ToLower().Contains(indicator.ToLower()));
        }
    }
}