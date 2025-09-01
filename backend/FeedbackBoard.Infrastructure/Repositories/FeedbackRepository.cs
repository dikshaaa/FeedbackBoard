using Microsoft.EntityFrameworkCore;
using FeedbackBoard.Domain.Entities;
using FeedbackBoard.Domain.Interfaces;
using FeedbackBoard.Domain.Enums;
using FeedbackBoard.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackBoard.Infrastructure.Repositories
{
    /// <summary>
    /// Feedback repository implementation with specialized methods
    /// </summary>
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(FeedbackBoardDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackSortedAsync(SortBy sortBy, SortDirection direction)
        {
            var query = _dbSet.AsQueryable();

            query = sortBy switch
            {
                SortBy.CreatedAt => direction == SortDirection.Ascending 
                    ? query.OrderBy(f => f.CreatedAt) 
                    : query.OrderByDescending(f => f.CreatedAt),
                SortBy.Rating => direction == SortDirection.Ascending 
                    ? query.OrderBy(f => f.Rating) 
                    : query.OrderByDescending(f => f.Rating),
                SortBy.Name => direction == SortDirection.Ascending 
                    ? query.OrderBy(f => f.Name) 
                    : query.OrderByDescending(f => f.Name),
                _ => query.OrderByDescending(f => f.CreatedAt)
            };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackByRatingAsync(int rating)
        {
            return await _dbSet
                .Where(f => f.Rating == rating)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync()
        {
            if (!await _dbSet.AnyAsync())
                return 0;

            return await _dbSet.AverageAsync(f => f.Rating);
        }

        public async Task<int> GetTotalFeedbackCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
