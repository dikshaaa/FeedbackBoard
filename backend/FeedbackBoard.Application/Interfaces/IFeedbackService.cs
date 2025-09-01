using FeedbackBoard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackBoard.Application.Interfaces
{
    /// <summary>
    /// Interface for feedback service operations
    /// </summary>
    public interface IFeedbackService
    {
        Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto createFeedbackDto);
        Task<FeedbackDto?> GetFeedbackByIdAsync(Guid id);
        Task<IEnumerable<FeedbackDto>> GetAllFeedbackAsync(FeedbackQueryDto? query = null);
        Task<FeedbackDto> UpdateFeedbackAsync(Guid id, CreateFeedbackDto updateFeedbackDto);
        Task<bool> DeleteFeedbackAsync(Guid id);
        Task<FeedbackStatsDto> GetFeedbackStatsAsync();
    }
}
