using FeedbackBoard.Domain.Enums;

namespace FeedbackBoard.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for feedback query parameters
    /// </summary>
    public class FeedbackQueryDto
    {
        public SortBy? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; } = Domain.Enums.SortDirection.Descending;
        public int? Rating { get; set; }
    }
}
