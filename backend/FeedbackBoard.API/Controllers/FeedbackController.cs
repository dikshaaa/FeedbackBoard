using FeedbackBoard.Application.DTOs;
using FeedbackBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FeedbackBoard.API.Controllers
{
    /// <summary>
    /// Controller for managing feedback operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : BaseApiController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(
            IFeedbackService feedbackService,
            ILogger<FeedbackController> logger)
        {
            _feedbackService = feedbackService ?? throw new ArgumentNullException(nameof(feedbackService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new feedback entry
        /// </summary>
        /// <param name="createFeedbackDto">The feedback data</param>
        /// <returns>The created feedback</returns>
        [HttpPost]
        [ProducesResponseType(typeof(FeedbackDto), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFeedback([FromBody, Required] CreateFeedbackDto createFeedbackDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var feedback = await _feedbackService.CreateFeedbackAsync(createFeedbackDto);
                return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating feedback");
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves all feedback entries with optional filtering and sorting
        /// </summary>
        /// <param name="query">Query parameters for filtering and sorting</param>
        /// <returns>List of feedback entries</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FeedbackDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllFeedback([FromQuery] FeedbackQueryDto? query = null)
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbackAsync(query);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback list");
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a specific feedback entry by ID
        /// </summary>
        /// <param name="id">The feedback ID</param>
        /// <returns>The feedback entry</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FeedbackDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFeedbackById([FromRoute] Guid id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound(new { message = $"Feedback with ID {id} not found" });
                }

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback with ID: {FeedbackId}", id);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates an existing feedback entry
        /// </summary>
        /// <param name="id">The feedback ID</param>
        /// <param name="updateFeedbackDto">The updated feedback data</param>
        /// <returns>The updated feedback</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(FeedbackDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFeedback([FromRoute] Guid id, [FromBody, Required] CreateFeedbackDto updateFeedbackDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var feedback = await _feedbackService.UpdateFeedbackAsync(id, updateFeedbackDto);
                return Ok(feedback);
            }
            catch (ArgumentException ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback with ID: {FeedbackId}", id);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a feedback entry
        /// </summary>
        /// <param name="id">The feedback ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteFeedback([FromRoute] Guid id)
        {
            try
            {
                var deleted = await _feedbackService.DeleteFeedbackAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = $"Feedback with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting feedback with ID: {FeedbackId}", id);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves feedback statistics
        /// </summary>
        /// <returns>Feedback statistics</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(FeedbackStatsDto), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFeedbackStats()
        {
            try
            {
                var stats = await _feedbackService.GetFeedbackStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving feedback statistics");
                return HandleException(ex);
            }
        }
    }
}
