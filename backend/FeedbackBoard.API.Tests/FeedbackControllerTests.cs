using FluentAssertions;
using FeedbackBoard.API.Controllers;
using FeedbackBoard.Application.DTOs;
using FeedbackBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FeedbackBoard.API.Tests;

public class FeedbackControllerTests
{
    private readonly Mock<IFeedbackService> _mockFeedbackService;
    private readonly Mock<ILogger<FeedbackController>> _mockLogger;
    private readonly FeedbackController _controller;

    public FeedbackControllerTests()
    {
        _mockFeedbackService = new Mock<IFeedbackService>();
        _mockLogger = new Mock<ILogger<FeedbackController>>();
        _controller = new FeedbackController(_mockFeedbackService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllFeedback_ShouldReturnOkWithFeedbackList()
    {
        // Arrange
        var query = new FeedbackQueryDto();
        var feedbackList = new List<FeedbackDto>
        {
            new() { Id = Guid.NewGuid(), Name = "John", Message = "Great!", Rating = 5 },
            new() { Id = Guid.NewGuid(), Name = "Jane", Message = "Good", Rating = 4 }
        };

        _mockFeedbackService.Setup(x => x.GetAllFeedbackAsync(query))
                           .ReturnsAsync(feedbackList);

        // Act
        var result = await _controller.GetAllFeedback(query);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(feedbackList);
    }

    [Fact]
    public async Task GetFeedbackById_WithExistingId_ShouldReturnOkWithFeedback()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var feedback = new FeedbackDto { Id = feedbackId, Name = "John", Message = "Great!", Rating = 5 };

        _mockFeedbackService.Setup(x => x.GetFeedbackByIdAsync(feedbackId))
                           .ReturnsAsync(feedback);

        // Act
        var result = await _controller.GetFeedbackById(feedbackId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(feedback);
    }

    [Fact]
    public async Task GetFeedbackById_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        _mockFeedbackService.Setup(x => x.GetFeedbackByIdAsync(feedbackId))
                           .ReturnsAsync((FeedbackDto?)null);

        // Act
        var result = await _controller.GetFeedbackById(feedbackId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateFeedback_WithValidDto_ShouldReturnCreatedAtActionWithFeedback()
    {
        // Arrange
        var createDto = new CreateFeedbackDto
        {
            Name = "John Doe",
            Message = "Great service!",
            Rating = 5
        };

        var createdFeedback = new FeedbackDto 
        { 
            Id = Guid.NewGuid(), 
            Name = createDto.Name, 
            Message = createDto.Message, 
            Rating = createDto.Rating 
        };

        _mockFeedbackService.Setup(x => x.CreateFeedbackAsync(createDto))
                           .ReturnsAsync(createdFeedback);

        // Act
        var result = await _controller.CreateFeedback(createDto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(_controller.GetFeedbackById));
        createdResult.RouteValues!["id"].Should().Be(createdFeedback.Id);
        createdResult.Value.Should().BeEquivalentTo(createdFeedback);
    }

    [Fact]
    public async Task UpdateFeedback_WithExistingId_ShouldReturnOkWithUpdatedFeedback()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var updateDto = new CreateFeedbackDto
        {
            Name = "Updated Name",
            Message = "Updated Message",
            Rating = 4
        };

        var updatedFeedback = new FeedbackDto 
        { 
            Id = feedbackId, 
            Name = updateDto.Name, 
            Message = updateDto.Message, 
            Rating = updateDto.Rating 
        };

        _mockFeedbackService.Setup(x => x.UpdateFeedbackAsync(feedbackId, updateDto))
                           .ReturnsAsync(updatedFeedback);

        // Act
        var result = await _controller.UpdateFeedback(feedbackId, updateDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(updatedFeedback);
    }

    [Fact]
    public async Task UpdateFeedback_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var updateDto = new CreateFeedbackDto
        {
            Name = "Name",
            Message = "Message",
            Rating = 5
        };

        _mockFeedbackService.Setup(x => x.UpdateFeedbackAsync(feedbackId, updateDto))
                           .ThrowsAsync(new ArgumentException($"Feedback with ID {feedbackId} not found"));

        // Act
        var result = await _controller.UpdateFeedback(feedbackId, updateDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteFeedback_WithExistingId_ShouldReturnNoContent()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        _mockFeedbackService.Setup(x => x.DeleteFeedbackAsync(feedbackId))
                           .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteFeedback(feedbackId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteFeedback_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        _mockFeedbackService.Setup(x => x.DeleteFeedbackAsync(feedbackId))
                           .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteFeedback(feedbackId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetFeedbackStats_ShouldReturnOkWithStats()
    {
        // Arrange
        var stats = new FeedbackStatsDto
        {
            TotalFeedbacks = 25,
            AverageRating = 4.2,
            RatingDistribution = new Dictionary<int, int>
            {
                { 1, 2 }, { 2, 3 }, { 3, 5 }, { 4, 8 }, { 5, 7 }
            }
        };

        _mockFeedbackService.Setup(x => x.GetFeedbackStatsAsync())
                           .ReturnsAsync(stats);

        // Act
        var result = await _controller.GetFeedbackStats();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(stats);
    }
}
