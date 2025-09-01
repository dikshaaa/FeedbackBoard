using System.Collections.Generic;

namespace FeedbackBoard.Application.DTOs
{
    /// <summary>
    /// Response DTO for feedback statistics
    /// </summary>
    public class FeedbackStatsDto
    {
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
    }
}
