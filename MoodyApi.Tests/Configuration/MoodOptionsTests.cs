using FluentAssertions;
using MoodyApi.Configuration;
using MoodyApi.Models;

namespace MoodyApi.Tests.Configuration
{
    public class MoodOptionsTests
    {
        [Fact]
        public void MoodOptions_ShouldHaveCorrectDefaults()
        {
            // Act
            var options = new MoodOptions();

            // Assert
            options.EnableSarcasm.Should().BeTrue();
            options.EnableMotivation.Should().BeTrue();
            options.EnableKarma.Should().BeTrue();
            options.EnableTimeBased.Should().BeTrue();
            options.KarmaThreshold.Should().Be(5);
            options.Mode.Should().Be(MoodType.Neutral);
        }

        [Fact]
        public void MoodOptions_ShouldAllowPropertyModification()
        {
            // Arrange
            var options = new MoodOptions();

            // Act
            options.EnableSarcasm = false;
            options.Mode = MoodType.Sarcastic;
            options.KarmaThreshold = 10;

            // Assert
            options.EnableSarcasm.Should().BeFalse();
            options.Mode.Should().Be(MoodType.Sarcastic);
            options.KarmaThreshold.Should().Be(10);
        }
    }
}