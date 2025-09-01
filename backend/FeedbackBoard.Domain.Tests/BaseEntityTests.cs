using FluentAssertions;
using FeedbackBoard.Domain.Entities;

namespace FeedbackBoard.Domain.Tests;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void BaseEntity_Constructor_ShouldInitializeDefaultValues()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().NotBe(Guid.Empty);
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        entity.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void BaseEntity_SetId_ShouldUpdateId()
    {
        // Arrange
        var entity = new TestEntity();
        var newId = Guid.NewGuid();

        // Act
        entity.Id = newId;

        // Assert
        entity.Id.Should().Be(newId);
    }

    [Fact]
    public void BaseEntity_UpdateTimestamp_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var entity = new TestEntity();
        var originalCreatedAt = entity.CreatedAt;
        
        // Wait a bit to ensure timestamp difference
        Thread.Sleep(10);

        // Act
        entity.UpdatedAt = DateTime.UtcNow;

        // Assert
        entity.UpdatedAt.Should().BeAfter(originalCreatedAt);
        entity.CreatedAt.Should().Be(originalCreatedAt); // CreatedAt should not change
    }
}
