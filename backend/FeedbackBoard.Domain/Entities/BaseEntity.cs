using System;

namespace FeedbackBoard.Domain.Entities
{
    /// <summary>
    /// Base entity class containing common properties for all entities
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
