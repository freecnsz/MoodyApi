using System.Net.Http;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoodyApi.Extensions;
using MoodyApi.Models;

namespace MoodyApi.Tests.Integration
{
    public class MoodApiIntegrationTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MoodApiIntegrationTests()
        {
            var builder = WebApplication.CreateBuilder();
            
            builder.Services.AddMoodyApi(options =>
            {
                options.Mode = MoodType.Motivational;
            });

            builder.WebHost.UseTestServer();
            
            var app = builder.Build();

            // Initialize Mood API
            Mood.Initialize(app.Services);
            
            app.UseMoodyApi();
            
            app.MapGet("/test", () => new { message = "Hello World" });
            app.MapGet("/error", () => Results.BadRequest("Error occurred"));
            
            app.Start();
            
            _server = app.GetTestServer();
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Get_ShouldReturnWrappedResponse_WithMoodData()
        {
            // Act
            var response = await _client.GetAsync("/test");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            
            var wrappedResponse = JsonSerializer.Deserialize<JsonElement>(content);
            wrappedResponse.GetProperty("success").GetBoolean().Should().BeTrue();
            wrappedResponse.GetProperty("statusCode").GetInt32().Should().Be(200);
            wrappedResponse.GetProperty("mood").GetProperty("message").GetString().Should().NotBeNullOrEmpty();
            wrappedResponse.GetProperty("mood").GetProperty("mood").GetInt32().Should().Be((int)MoodType.Motivational);
        }

        [Fact]
        public async Task Get_ShouldIncludeOriginalData_InWrappedResponse()
        {
            // Act
            var response = await _client.GetAsync("/test");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var wrappedResponse = JsonSerializer.Deserialize<JsonElement>(content);
            wrappedResponse.GetProperty("data").GetProperty("message").GetString().Should().Be("Hello World");
        }

        [Fact]
        public async Task Get_ShouldHandleErrorResponses_Correctly()
        {
            // Act
            var response = await _client.GetAsync("/error");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var wrappedResponse = JsonSerializer.Deserialize<JsonElement>(content);
            wrappedResponse.GetProperty("success").GetBoolean().Should().BeFalse();
            wrappedResponse.GetProperty("statusCode").GetInt32().Should().Be(400);
            wrappedResponse.GetProperty("mood").GetProperty("message").GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get_ShouldTrackUserRequests_WhenUserIdHeaderProvided()
        {
            // Arrange
            var userId = "test-user-123";
            _client.DefaultRequestHeaders.Add("X-User-ID", userId);

            // Act
            await _client.GetAsync("/test");
            await _client.GetAsync("/test");
            var response = await _client.GetAsync("/test");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var wrappedResponse = JsonSerializer.Deserialize<JsonElement>(content);
            var karmaScore = wrappedResponse.GetProperty("mood").GetProperty("karmaScore").GetInt32();
            karmaScore.Should().Be(3);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }
    }
}