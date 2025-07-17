using FluentAssertions;
using MoodyApi.Providers;

namespace MoodyApi.Tests.Providers
{
    public class ErrorBasedProviderTests
    {
        [Fact]
        public void GetMessage_ShouldReturnErrorRelatedMessage()
        {
            // Arrange
            var provider = new ErrorBasedProvider();

            // Act
            var message = provider.GetMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMessage_ShouldContainErrorRelatedWords()
        {
            // Arrange
            var provider = new ErrorBasedProvider();
            var errorWords = new[] { "error", "oops", "problem", "bug", "exception", "houston", "technical" };

            // Act
            var message = provider.GetMessage();

            // Assert
            errorWords.Should().Contain(word => 
                message.ToLower().Contains(word.ToLower()));
        }
    }
}