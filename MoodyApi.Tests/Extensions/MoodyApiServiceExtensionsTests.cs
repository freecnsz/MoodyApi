using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MoodyApi.Configuration;
using MoodyApi.Core;
using MoodyApi.Extensions;
using MoodyApi.Models;
using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Tests.Extensions
{
    public class MoodyApiServiceExtensionsTests
    {
        [Fact]
        public void AddMoodyApi_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMoodyApi();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<MoodOptions>().Should().NotBeNull();
            serviceProvider.GetService<UserTracker>().Should().NotBeNull();
            serviceProvider.GetService<MoodEngine>().Should().NotBeNull();
            serviceProvider.GetService<Dictionary<MoodType, IMessageProvider>>().Should().NotBeNull();
        }

        [Fact]
        public void AddMoodyApi_ShouldConfigureOptionsCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMoodyApi(options =>
            {
                options.Mode = MoodType.Sarcastic;
                options.KarmaThreshold = 10;
                options.EnableMotivation = false;
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<MoodOptions>();

            // Assert
            options.Mode.Should().Be(MoodType.Sarcastic);
            options.KarmaThreshold.Should().Be(10);
            options.EnableMotivation.Should().BeFalse();
        }

        [Fact]
        public void AddMoodyApi_ShouldRegisterAllProviders()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMoodyApi();
            var serviceProvider = services.BuildServiceProvider();
            var providers = serviceProvider.GetRequiredService<Dictionary<MoodType, IMessageProvider>>();

            // Assert
            providers.Should().HaveCount(6);
            providers.Should().ContainKey(MoodType.Neutral);
            providers.Should().ContainKey(MoodType.Motivational);
            providers.Should().ContainKey(MoodType.Sarcastic);
            providers.Should().ContainKey(MoodType.KarmaBased);
            providers.Should().ContainKey(MoodType.TimeBased);
            providers.Should().ContainKey(MoodType.ErrorBased);
        }
    }
}