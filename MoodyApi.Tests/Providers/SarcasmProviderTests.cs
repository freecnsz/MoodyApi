using FluentAssertions;
using MoodyApi.Providers;
using System;
using System.Linq;
using System.Reflection;

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
            var sarcasticIndicators = new[] { "Oh", "Wow", "great", "thrilled", "shocking", "delightfully" };
            var messages = typeof(SarcasmProvider)
                .GetField("Messages", BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null) as string[];

            // Assert
            messages.Should().Contain(msg => sarcasticIndicators.Any(indicator => msg.ToLower().Contains(indicator.ToLower())));
        }
    }
}