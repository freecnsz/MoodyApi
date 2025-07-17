using FluentAssertions;
using MoodyApi.Configuration;
using MoodyApi.Core;
using MoodyApi.Models;
using MoodyApi.Providers;
using MoodyApi.Providers.Interfaces;

namespace MoodyApi.Tests.Core
{
    public class MoodEngineTests
    {
        private readonly MoodEngine _moodEngine;
        private readonly MoodOptions _options;
        private readonly UserTracker _userTracker;

        public MoodEngineTests()
        {
            _options = new MoodOptions();
            _userTracker = new UserTracker();
            
            var providers = new Dictionary<MoodType, IMessageProvider>
            {
                [MoodType.Neutral] = new NeutralProvider(),
                [MoodType.Motivational] = new MotivationProvider(),
                [MoodType.Sarcastic] = new SarcasmProvider(),
                [MoodType.KarmaBased] = new KarmaProvider(),
                [MoodType.TimeBased] = new TimeBasedProvider(),
                [MoodType.ErrorBased] = new ErrorBasedProvider()
            };

            _moodEngine = new MoodEngine(_options, providers, _userTracker);
        }

        [Fact]
        public void GetMessage_ShouldReturnMessage_ForValidMoodType()
        {
            // Act
            var message = _moodEngine.GetMessage(MoodType.Motivational);

            // Assert
            message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMessage_ShouldReturnDefaultMessage_ForInvalidMoodType()
        {
            // Act
            var message = _moodEngine.GetMessage((MoodType)999);

            // Assert
            message.Should().Be("No mood available.");
        }

        [Fact]
        public void GetMoodResponse_ShouldReturnValidResponse()
        {
            // Act
            var response = _moodEngine.GetMoodResponse(MoodType.Neutral);

            // Assert
            response.Should().NotBeNull();
            response.Message.Should().NotBeNullOrEmpty();
            response.Mood.Should().Be(MoodType.Neutral);
            response.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void GetMoodResponse_ShouldIncludeKarmaScore_WhenUserIdProvided()
        {
            // Arrange
            var userId = "test-user";
            _userTracker.RecordRequest(userId);
            _userTracker.RecordRequest(userId);

            // Act
            var response = _moodEngine.GetMoodResponse(MoodType.Neutral, userId);

            // Assert
            response.KarmaScore.Should().Be(3); // 2 previous + 1 from this call
        }

        [Fact]
        public void GetMoodResponse_ShouldNotIncludeKarmaScore_WhenUserIdNotProvided()
        {
            // Act
            var response = _moodEngine.GetMoodResponse(MoodType.Neutral);

            // Assert
            response.KarmaScore.Should().BeNull();
        }

        [Theory]
        [InlineData(MoodType.Neutral)]
        [InlineData(MoodType.Motivational)]
        [InlineData(MoodType.Sarcastic)]
        [InlineData(MoodType.KarmaBased)]
        [InlineData(MoodType.TimeBased)]
        [InlineData(MoodType.ErrorBased)]
        public void GetMoodResponse_ShouldHandleAllMoodTypes(MoodType moodType)
        {
            // Act
            var response = _moodEngine.GetMoodResponse(moodType);

            // Assert
            response.Should().NotBeNull();
            response.Mood.Should().Be(moodType);
            response.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMoodResponse_ShouldResolveMoodFromOptions_WhenNoMoodTypeProvided()
        {
            // Arrange
            _options.Mode = MoodType.Sarcastic;

            // Act
            var response = _moodEngine.GetMoodResponse();

            // Assert
            response.Mood.Should().Be(MoodType.Sarcastic);
        }
    }
}