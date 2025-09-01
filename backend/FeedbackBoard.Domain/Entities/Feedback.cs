using System;
using System.ComponentModel.DataAnnotations;

namespace FeedbackBoard.Domain.Entities
{
    /// <summary>
    /// Represents a feedback entry in the system
    /// </summary>
    public class Feedback : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        /// <summary>
        /// Validates the feedback entity
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(Message) && 
                   Rating >= 1 && 
                   Rating <= 5;
        }
    }
}
