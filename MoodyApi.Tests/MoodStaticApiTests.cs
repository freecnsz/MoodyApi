using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MoodyApi.Extensions;
using MoodyApi.Models;

namespace MoodyApi.Tests
{
    public class MoodStaticApiTests
    {
        [Fact]
        public void Get_ShouldThrowException_WhenNotInitialized()
        {
            // Act & Assert
            var action = () => MoodyApi.Mood.Get(MoodType.Neutral);
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("*not initialized*");
        }

        [Fact]
        public void Get_ShouldReturnMessage_WhenInitialized()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddMoodyApi();
            var serviceProvider = services.BuildServiceProvider();
            
            MoodyApi.Mood.Initialize(serviceProvider);

            // Act
            var message = MoodyApi.Mood.Get(MoodType.Motivational);

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetResponse_ShouldReturnMoodResponse_WhenInitialized()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddMoodyApi();
            var serviceProvider = services.BuildServiceProvider();
            
            MoodyApi.Mood.Initialize(serviceProvider);

            // Act
            var response = MoodyApi.Mood.GetResponse(MoodType.Sarcastic, "test-user");

            // Assert
            response.Should().NotBeNull();
            response.Message.Should().NotBeNullOrEmpty();
            response.Mood.Should().Be(MoodType.Sarcastic);
            response.KarmaScore.Should().Be(1);
        }
    }
}