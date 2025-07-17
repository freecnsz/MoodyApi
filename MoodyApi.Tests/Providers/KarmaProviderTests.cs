using FluentAssertions;
using MoodyApi.Providers;
using System;
using System.Linq;
using System.Reflection;

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
            var philosophicalWords = new[] { "seek", "universe", "balance", "soul", "echo", "karma", "cosmic" };
            var messages = typeof(KarmaProvider)
                .GetField("Messages", BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null) as string[];

            // Assert
            messages.Should().Contain(msg => philosophicalWords.Any(word => msg.ToLower().Contains(word.ToLower())));
        }
    }
}