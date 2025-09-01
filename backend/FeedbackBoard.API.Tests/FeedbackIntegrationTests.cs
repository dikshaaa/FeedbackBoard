using FluentAssertions;
using FeedbackBoard.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FeedbackBoard.API.Tests;

public class FeedbackIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public FeedbackIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public async Task CreateFeedback_ShouldCreateAndReturnFeedback()
    {
        // Arrange
        var createDto = new CreateFeedbackDto
        {
            Name = "John Doe",
            Message = "This is a test feedback for integration testing",
            Rating = 5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/feedback", createDto, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdFeedback = await response.Content.ReadFromJsonAsync<FeedbackDto>(_jsonOptions);
        createdFeedback.Should().NotBeNull();
        createdFeedback!.Name.Should().Be(createDto.Name);
        createdFeedback.Message.Should().Be(createDto.Message);
        createdFeedback.Rating.Should().Be(createDto.Rating);
        createdFeedback.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetAllFeedback_ShouldReturnFeedbackList()
    {
        // Act
        var response = await _client.GetAsync("/api/feedback");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var feedbackList = await response.Content.ReadFromJsonAsync<List<FeedbackDto>>(_jsonOptions);
        feedbackList.Should().NotBeNull();
        feedbackList.Should().BeOfType<List<FeedbackDto>>();
    }

    [Fact]
    public async Task GetFeedbackById_WithValidId_ShouldReturnFeedback()
    {
        // Arrange - First create a feedback
        var createDto = new CreateFeedbackDto
        {
            Name = "Jane Smith",
            Message = "Test feedback for get by ID",
            Rating = 4
        };

        var createResponse = await _client.PostAsJsonAsync("/api/feedback", createDto, _jsonOptions);
        var createdFeedback = await createResponse.Content.ReadFromJsonAsync<FeedbackDto>(_jsonOptions);

        // Act
        var response = await _client.GetAsync($"/api/feedback/{createdFeedback!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var feedback = await response.Content.ReadFromJsonAsync<FeedbackDto>(_jsonOptions);
        feedback.Should().NotBeNull();
        feedback!.Id.Should().Be(createdFeedback.Id);
        feedback.Name.Should().Be(createDto.Name);
    }

    [Fact]
    public async Task GetFeedbackById_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/feedback/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateFeedback_WithValidId_ShouldUpdateAndReturnFeedback()
    {
        // Arrange - First create a feedback
        var createDto = new CreateFeedbackDto
        {
            Name = "Original Name",
            Message = "Original Message",
            Rating = 3
        };

        var createResponse = await _client.PostAsJsonAsync("/api/feedback", createDto, _jsonOptions);
        var createdFeedback = await createResponse.Content.ReadFromJsonAsync<FeedbackDto>(_jsonOptions);

        var updateDto = new CreateFeedbackDto
        {
            Name = "Updated Name",
            Message = "Updated Message",
            Rating = 5
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/feedback/{createdFeedback!.Id}", updateDto, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedFeedback = await response.Content.ReadFromJsonAsync<FeedbackDto>(_jsonOptions);
        updatedFeedback.Should().NotBeNull();
        updatedFeedback!.Name.Should().Be(updateDto.Name);
        updatedFeedback.Message.Should().Be(updateDto.Message);
        updatedFeedback.Rating.Should().Be(updateDto.Rating);
    }

    [Fact]
    public async Task GetFeedbackStats_ShouldReturnFeedbackStats()
    {
        // Act
        var response = await _client.GetAsync("/api/feedback/stats");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var stats = await response.Content.ReadFromJsonAsync<FeedbackStatsDto>(_jsonOptions);
        stats.Should().NotBeNull();
        stats!.TotalFeedbacks.Should().BeGreaterThanOrEqualTo(0);
        stats.AverageRating.Should().BeGreaterThanOrEqualTo(0);
        stats.RatingDistribution.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteFeedback_WithValidId_ShouldDeleteFeedback()
    {
        // Arrange - First create a feedback
        var createDto = new CreateFeedbackDto
        {
            Name = "To Be Deleted",
            Message = "This feedback will be deleted",
            Rating = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/feedback", createDto, _jsonOptions);
        var createdFeedback = await createResponse.Content.ReadFromJsonAsync<FeedbackDto>(_jsonOptions);

        // Act
        var response = await _client.DeleteAsync($"/api/feedback/{createdFeedback!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify feedback was deleted
        var getResponse = await _client.GetAsync($"/api/feedback/{createdFeedback.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateFeedback_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidDto = new CreateFeedbackDto
        {
            Name = "", // Invalid - empty name
            Message = "Valid message",
            Rating = 5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/feedback", invalidDto, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
