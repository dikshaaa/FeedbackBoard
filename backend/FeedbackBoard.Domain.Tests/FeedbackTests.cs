using FluentAssertions;
using FeedbackBoard.Domain.Entities;

namespace FeedbackBoard.Domain.Tests;

public class FeedbackTests
{
    [Fact]
    public void Feedback_Constructor_ShouldInitializeProperties()
    {
        // Act
        var feedback = new Feedback();

        // Assert
        feedback.Id.Should().NotBe(Guid.Empty);
        feedback.Name.Should().Be(string.Empty);
        feedback.Message.Should().Be(string.Empty);
        feedback.Rating.Should().Be(0);
        feedback.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Feedback_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var feedback = new Feedback();
        var name = "John Doe";
        var message = "Great service!";
        var rating = 5;

        // Act
        feedback.Name = name;
        feedback.Message = message;
        feedback.Rating = rating;

        // Assert
        feedback.Name.Should().Be(name);
        feedback.Message.Should().Be(message);
        feedback.Rating.Should().Be(rating);
    }

    [Fact]
    public void IsValid_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var feedback = new Feedback
        {
            Name = "John Doe",
            Message = "Great service!",
            Rating = 5
        };

        // Act
        var result = feedback.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Message", 5)]
    [InlineData(null, "Message", 5)]
    [InlineData("   ", "Message", 5)]
    public void IsValid_WithInvalidName_ShouldReturnFalse(string name, string message, int rating)
    {
        // Arrange
        var feedback = new Feedback
        {
            Name = name,
            Message = message,
            Rating = rating
        };

        // Act
        var result = feedback.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("John", "", 5)]
    [InlineData("John", null, 5)]
    [InlineData("John", "   ", 5)]
    public void IsValid_WithInvalidMessage_ShouldReturnFalse(string name, string message, int rating)
    {
        // Arrange
        var feedback = new Feedback
        {
            Name = name,
            Message = message,
            Rating = rating
        };

        // Act
        var result = feedback.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("John", "Message", 0)]
    [InlineData("John", "Message", 6)]
    [InlineData("John", "Message", -1)]
    public void IsValid_WithInvalidRating_ShouldReturnFalse(string name, string message, int rating)
    {
        // Arrange
        var feedback = new Feedback
        {
            Name = name,
            Message = message,
            Rating = rating
        };

        // Act
        var result = feedback.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void IsValid_WithValidRatings_ShouldReturnTrue(int rating)
    {
        // Arrange
        var feedback = new Feedback
        {
            Name = "John Doe",
            Message = "Test message",
            Rating = rating
        };

        // Act
        var result = feedback.IsValid();

        // Assert
        result.Should().BeTrue();
    }
}
