using Microsoft.AspNetCore.Mvc;
using orac.api.DTOS;

namespace orac.api.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class OpenApiController : ControllerBase
    {
        /// <summary>
        /// Retrieves the OpenAPI specification from a JSON file.
        /// </summary>
        [HttpGet("[action]")]
        public IActionResult GetOpenApiSpec()
        {
            try
            {
                var specPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "openapi.json");
                if (!System.IO.File.Exists(specPath))
                {
                    return NotFound(ApiResponse<object>.Error("OpenAPI specification file not found."));
                }

                var specContent = System.IO.File.ReadAllText(specPath);
                var parsed = System.Text.Json.JsonSerializer.Deserialize<object>(specContent);
                return Ok(ApiResponse<object>.Success(parsed!, "OpenAPI specification retrieved."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred while reading the OpenAPI specification: {ex.Message}"));
            }
        }
    }
}