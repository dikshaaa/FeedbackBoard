using AutoMapper;
using FeedbackBoard.Application.DTOs;
using FeedbackBoard.Application.Interfaces;
using FeedbackBoard.Domain.Entities;
using FeedbackBoard.Domain.Enums;
using FeedbackBoard.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackBoard.Application.Services
{
    /// <summary>
    /// Service implementation for feedback operations
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<FeedbackService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto createFeedbackDto)
        {
            try
            {
                _logger.LogInformation("🎯 START: Creating new feedback from user: {Name}", createFeedbackDto.Name);
                
                // DEBUG: Log the incoming DTO details
                _logger.LogDebug("📥 INPUT DTO - Name: '{Name}', Rating: {Rating}, Message: '{Message}' (Length: {MessageLength})", 
                    createFeedbackDto.Name, createFeedbackDto.Rating, createFeedbackDto.Message, createFeedbackDto.Message?.Length ?? 0);

                var feedback = _mapper.Map<Feedback>(createFeedbackDto);
                
                // DEBUG: Log the mapped entity details
                _logger.LogDebug("🔄 MAPPED ENTITY - ID: {Id}, Name: '{Name}', Rating: {Rating}, Message: '{Message}', CreatedAt: {CreatedAt}, IsValid: {IsValid}", 
                    feedback.Id, feedback.Name, feedback.Rating, feedback.Message, feedback.CreatedAt, feedback.IsValid());
                
                if (!feedback.IsValid())
                {
                    _logger.LogWarning("❌ VALIDATION FAILED for feedback from user: {Name}, Rating: {Rating}", 
                        createFeedbackDto.Name, createFeedbackDto.Rating);
                    throw new ArgumentException("Invalid feedback data");
                }

                _logger.LogDebug("✅ VALIDATION PASSED - Proceeding to save feedback");
                
                var createdFeedback = await _unitOfWork.Feedbacks.AddAsync(feedback);
                _logger.LogDebug("💾 ADDED TO REPOSITORY - Entity ID: {Id}", createdFeedback.Id);
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogDebug("💿 SAVED TO DATABASE - Changes committed successfully");

                var result = _mapper.Map<FeedbackDto>(createdFeedback);
                _logger.LogDebug("📤 RESPONSE DTO - ID: {Id}, Name: '{Name}', Rating: {Rating}", 
                    result.Id, result.Name, result.Rating);

                _logger.LogInformation("🎉 SUCCESS: Created feedback with ID: {Id} in {ElapsedMs}ms", createdFeedback.Id, DateTime.UtcNow.Subtract(feedback.CreatedAt).TotalMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 ERROR: Failed to create feedback for user: {Name}. Error Type: {ErrorType}, Message: {ErrorMessage}", 
                    createFeedbackDto?.Name ?? "Unknown", ex.GetType().Name, ex.Message);
                
                if (ex.InnerException != null)
                {
                    _logger.LogError("🔍 INNER EXCEPTION: {InnerExceptionType}: {InnerExceptionMessage}", 
                        ex.InnerException.GetType().Name, ex.InnerException.Message);
                }
                
                throw;
            }
        }

        public async Task<FeedbackDto?> GetFeedbackByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving feedback with ID: {Id}", id);

                var feedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
                if (feedback == null)
                {
                    _logger.LogWarning("Feedback with ID {Id} not found", id);
                    return null;
                }

                return _mapper.Map<FeedbackDto>(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbackAsync(FeedbackQueryDto? query = null)
        {
            try
            {
                _logger.LogInformation("Retrieving all feedback with query parameters");

                IEnumerable<Feedback> feedbacks;

                if (query?.Rating.HasValue == true)
                {
                    feedbacks = await _unitOfWork.Feedbacks.GetFeedbackByRatingAsync(query.Rating.Value);
                }
                else if (query?.SortBy.HasValue == true)
                {
                    var sortDirection = query.SortDirection ?? SortDirection.Descending;
                    feedbacks = await _unitOfWork.Feedbacks.GetFeedbackSortedAsync(query.SortBy.Value, sortDirection);
                }
                else
                {
                    // Default: sort by creation date, newest first
                    feedbacks = await _unitOfWork.Feedbacks.GetFeedbackSortedAsync(SortBy.CreatedAt, SortDirection.Descending);
                }

                _logger.LogInformation("Retrieved {Count} feedback entries", feedbacks.Count());
                return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback list");
                throw;
            }
        }

        public async Task<FeedbackDto> UpdateFeedbackAsync(Guid id, CreateFeedbackDto updateFeedbackDto)
        {
            try
            {
                _logger.LogInformation("Updating feedback with ID: {Id}", id);

                var existingFeedback = await _unitOfWork.Feedbacks.GetByIdAsync(id);
                if (existingFeedback == null)
                {
                    throw new ArgumentException($"Feedback with ID {id} not found");
                }

                // Update properties
                existingFeedback.Name = updateFeedbackDto.Name;
                existingFeedback.Message = updateFeedbackDto.Message;
                existingFeedback.Rating = updateFeedbackDto.Rating;

                if (!existingFeedback.IsValid())
                {
                    throw new ArgumentException("Invalid feedback data");
                }

                var updatedFeedback = await _unitOfWork.Feedbacks.UpdateAsync(existingFeedback);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated feedback with ID: {Id}", id);
                return _mapper.Map<FeedbackDto>(updatedFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("🗑️ START: Deleting feedback with ID: {Id}", id);

                var exists = await _unitOfWork.Feedbacks.ExistsAsync(id);
                if (!exists)
                {
                    _logger.LogWarning("❌ FEEDBACK NOT FOUND: Feedback with ID {Id} not found for deletion", id);
                    return false;
                }

                _logger.LogDebug("✅ FEEDBACK EXISTS: Proceeding to delete feedback with ID: {Id}", id);
                
                await _unitOfWork.Feedbacks.DeleteAsync(id);
                _logger.LogDebug("💾 DELETED FROM REPOSITORY: Feedback {Id} removed from repository", id);
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogDebug("💿 SAVED CHANGES: Database changes committed for feedback deletion {Id}", id);

                _logger.LogInformation("🎉 SUCCESS: Successfully deleted feedback with ID: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 ERROR: Failed to delete feedback with ID: {Id}. Error Type: {ErrorType}, Message: {ErrorMessage}", 
                    id, ex.GetType().Name, ex.Message);
                
                if (ex.InnerException != null)
                {
                    _logger.LogError("🔍 INNER EXCEPTION: {InnerExceptionType}: {InnerExceptionMessage}", 
                        ex.InnerException.GetType().Name, ex.InnerException.Message);
                }
                
                throw;
            }
        }

        public async Task<FeedbackStatsDto> GetFeedbackStatsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving feedback statistics");

                var totalCount = await _unitOfWork.Feedbacks.GetTotalFeedbackCountAsync();
                var averageRating = await _unitOfWork.Feedbacks.GetAverageRatingAsync();

                // Get rating distribution
                var allFeedbacks = await _unitOfWork.Feedbacks.GetAllAsync();
                var ratingDistribution = allFeedbacks
                    .GroupBy(f => f.Rating)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Ensure all ratings 1-5 are represented
                for (int i = 1; i <= 5; i++)
                {
                    if (!ratingDistribution.ContainsKey(i))
                    {
                        ratingDistribution[i] = 0;
                    }
                }

                var stats = new FeedbackStatsDto
                {
                    TotalFeedbacks = totalCount,
                    AverageRating = Math.Round(averageRating, 2),
                    RatingDistribution = ratingDistribution
                };

                _logger.LogInformation("Retrieved feedback statistics: {TotalCount} total, {AverageRating} average rating", 
                    totalCount, averageRating);

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback statistics");
                throw;
            }
        }
    }
}
