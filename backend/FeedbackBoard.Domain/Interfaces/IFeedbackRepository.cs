using FeedbackBoard.Domain.Entities;
using FeedbackBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackBoard.Domain.Interfaces
{
    /// <summary>
    /// Specific repository interface for Feedback entities with additional methods
    /// </summary>
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetFeedbackSortedAsync(SortBy sortBy, SortDirection direction);
        Task<IEnumerable<Feedback>> GetFeedbackByRatingAsync(int rating);
        Task<double> GetAverageRatingAsync();
        Task<int> GetTotalFeedbackCountAsync();
    }
}
