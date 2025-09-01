using Microsoft.AspNetCore.Mvc;

namespace FeedbackBoard.API.Controllers
{
    /// <summary>
    /// Base API controller with common functionality
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Handles error responses consistently
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>Appropriate error response</returns>
        protected IActionResult HandleException(Exception ex)
        {
            // Log the exception (logger would be injected in derived classes)
            
            return ex switch
            {
                ArgumentNullException => BadRequest(new { error = "Required parameter is null" }),
                ArgumentException => BadRequest(new { error = ex.Message }),
                _ => StatusCode(500, new { error = "An internal server error occurred" })
            };
        }
    }
}
