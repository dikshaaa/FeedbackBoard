using AutoMapper;
using FluentAssertions;
using FeedbackBoard.Application.DTOs;
using FeedbackBoard.Application.Interfaces;
using FeedbackBoard.Application.Services;
using FeedbackBoard.Domain.Entities;
using FeedbackBoard.Domain.Enums;
using FeedbackBoard.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace FeedbackBoard.Application.Tests;

public class FeedbackServiceTests
{
    private readonly Mock<IFeedbackRepository> _mockFeedbackRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<FeedbackService>> _mockLogger;
    private readonly FeedbackService _feedbackService;

    public FeedbackServiceTests()
    {
        _mockFeedbackRepository = new Mock<IFeedbackRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<FeedbackService>>();
        
        _mockUnitOfWork.Setup(x => x.Feedbacks).Returns(_mockFeedbackRepository.Object);
        
        _feedbackService = new FeedbackService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateFeedbackAsync_WithValidDto_ShouldCreateAndReturnMappedFeedback()
    {
        // Arrange
        var createDto = new CreateFeedbackDto
        {
            Name = "John Doe",
            Message = "Great service!",
            Rating = 5
        };

        var feedback = new Feedback
        {
            Name = createDto.Name,
            Message = createDto.Message,
            Rating = createDto.Rating
        };

        var feedbackDto = new FeedbackDto
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Message = createDto.Message,
            Rating = createDto.Rating
        };

        _mockMapper.Setup(x => x.Map<Feedback>(createDto)).Returns(feedback);
        _mockFeedbackRepository.Setup(x => x.AddAsync(It.IsAny<Feedback>())).ReturnsAsync(feedback);
        _mockMapper.Setup(x => x.Map<FeedbackDto>(feedback)).Returns(feedbackDto);

        // Act
        var result = await _feedbackService.CreateFeedbackAsync(createDto);

        // Assert
        result.Should().BeEquivalentTo(feedbackDto);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_WithExistingId_ShouldReturnMappedFeedback()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var feedback = new Feedback { Id = feedbackId, Name = "John", Message = "Test", Rating = 5 };
        var feedbackDto = new FeedbackDto { Id = feedbackId, Name = "John", Message = "Test", Rating = 5 };

        _mockFeedbackRepository.Setup(x => x.GetByIdAsync(feedbackId)).ReturnsAsync(feedback);
        _mockMapper.Setup(x => x.Map<FeedbackDto>(feedback)).Returns(feedbackDto);

        // Act
        var result = await _feedbackService.GetFeedbackByIdAsync(feedbackId);

        // Assert
        result.Should().BeEquivalentTo(feedbackDto);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        _mockFeedbackRepository.Setup(x => x.GetByIdAsync(feedbackId)).ReturnsAsync((Feedback?)null);

        // Act
        var result = await _feedbackService.GetFeedbackByIdAsync(feedbackId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllFeedbackAsync_WithDefaultQuery_ShouldReturnSortedFeedback()
    {
        // Arrange
        var feedbacks = new List<Feedback>
        {
            new() { Name = "John", Message = "Good", Rating = 4 },
            new() { Name = "Jane", Message = "Excellent", Rating = 5 }
        };

        var feedbackDtos = new List<FeedbackDto>
        {
            new() { Name = "John", Message = "Good", Rating = 4 },
            new() { Name = "Jane", Message = "Excellent", Rating = 5 }
        };

        _mockFeedbackRepository
            .Setup(x => x.GetFeedbackSortedAsync(SortBy.CreatedAt, SortDirection.Descending))
            .ReturnsAsync(feedbacks);
        _mockMapper.Setup(x => x.Map<IEnumerable<FeedbackDto>>(feedbacks)).Returns(feedbackDtos);

        // Act
        var result = await _feedbackService.GetAllFeedbackAsync();

        // Assert
        result.Should().BeEquivalentTo(feedbackDtos);
    }

    [Fact]
    public async Task GetAllFeedbackAsync_WithRatingFilter_ShouldReturnFilteredFeedback()
    {
        // Arrange
        var query = new FeedbackQueryDto { Rating = 5 };
        var feedbacks = new List<Feedback>
        {
            new() { Name = "Jane", Message = "Excellent", Rating = 5 }
        };
        var feedbackDtos = new List<FeedbackDto>
        {
            new() { Name = "Jane", Message = "Excellent", Rating = 5 }
        };

        _mockFeedbackRepository.Setup(x => x.GetFeedbackByRatingAsync(5)).ReturnsAsync(feedbacks);
        _mockMapper.Setup(x => x.Map<IEnumerable<FeedbackDto>>(feedbacks)).Returns(feedbackDtos);

        // Act
        var result = await _feedbackService.GetAllFeedbackAsync(query);

        // Assert
        result.Should().BeEquivalentTo(feedbackDtos);
    }

    [Fact]
    public async Task UpdateFeedbackAsync_WithExistingFeedback_ShouldUpdateAndReturnMappedFeedback()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        var updateDto = new CreateFeedbackDto
        {
            Name = "Updated Name",
            Message = "Updated Message",
            Rating = 4
        };

        var existingFeedback = new Feedback
        {
            Id = feedbackId,
            Name = "Original Name",
            Message = "Original Message",
            Rating = 3
        };

        var updatedFeedbackDto = new FeedbackDto
        {
            Id = feedbackId,
            Name = updateDto.Name,
            Message = updateDto.Message,
            Rating = updateDto.Rating
        };

        _mockFeedbackRepository.Setup(x => x.GetByIdAsync(feedbackId)).ReturnsAsync(existingFeedback);
        _mockFeedbackRepository.Setup(x => x.UpdateAsync(existingFeedback)).ReturnsAsync(existingFeedback);
        _mockMapper.Setup(x => x.Map<FeedbackDto>(existingFeedback)).Returns(updatedFeedbackDto);

        // Act
        var result = await _feedbackService.UpdateFeedbackAsync(feedbackId, updateDto);

        // Assert
        result.Should().BeEquivalentTo(updatedFeedbackDto);
        existingFeedback.Name.Should().Be(updateDto.Name);
        existingFeedback.Message.Should().Be(updateDto.Message);
        existingFeedback.Rating.Should().Be(updateDto.Rating);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteFeedbackAsync_WithExistingFeedback_ShouldReturnTrue()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        _mockFeedbackRepository.Setup(x => x.ExistsAsync(feedbackId)).ReturnsAsync(true);

        // Act
        var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);

        // Assert
        result.Should().BeTrue();
        _mockFeedbackRepository.Verify(x => x.DeleteAsync(feedbackId), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteFeedbackAsync_WithNonExistingFeedback_ShouldReturnFalse()
    {
        // Arrange
        var feedbackId = Guid.NewGuid();
        _mockFeedbackRepository.Setup(x => x.ExistsAsync(feedbackId)).ReturnsAsync(false);

        // Act
        var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);

        // Assert
        result.Should().BeFalse();
        _mockFeedbackRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task GetFeedbackStatsAsync_ShouldReturnCorrectStats()
    {
        // Arrange
        var totalCount = 10;
        var averageRating = 4.2;
        var allFeedbacks = new List<Feedback>
        {
            new() { Rating = 5 },
            new() { Rating = 5 },
            new() { Rating = 4 },
            new() { Rating = 3 },
            new() { Rating = 2 }
        };

        _mockFeedbackRepository.Setup(x => x.GetTotalFeedbackCountAsync()).ReturnsAsync(totalCount);
        _mockFeedbackRepository.Setup(x => x.GetAverageRatingAsync()).ReturnsAsync(averageRating);
        _mockFeedbackRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(allFeedbacks);

        // Act
        var result = await _feedbackService.GetFeedbackStatsAsync();

        // Assert
        result.TotalFeedbacks.Should().Be(totalCount);
        result.AverageRating.Should().Be(Math.Round(averageRating, 2));
        result.RatingDistribution.Should().ContainKeys(1, 2, 3, 4, 5);
        result.RatingDistribution[5].Should().Be(2);
        result.RatingDistribution[4].Should().Be(1);
        result.RatingDistribution[3].Should().Be(1);
        result.RatingDistribution[2].Should().Be(1);
        result.RatingDistribution[1].Should().Be(0);
    }
}
