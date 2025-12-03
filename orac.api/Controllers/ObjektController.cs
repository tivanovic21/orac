using System.Text;
using Microsoft.AspNetCore.Mvc;
using orac.api.Interfaces;

namespace orac.api.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class ObjektController : ControllerBase
    {
        private readonly IObjektService _objektService;

        public ObjektController(IObjektService objektService)
        {
            _objektService = objektService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _objektService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NotFound("No objects found.");
                }

                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be greater than zero.");
            }

            try
            {
                var result = await _objektService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Object with ID {id} not found.");
                }

                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Došlo je do greške na serveru: {ex.Message}");
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be greater than zero");
            }

             try
            {
                var result = await _objektService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound($"Object with ID {id} not found.");
                }
                return Ok("Object has been successfully deleted.");
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] DTOS.ObjektDto objektDto)
        {
            if (objektDto == null || objektDto.IdObjekta <= 0)
            {
                return BadRequest("Invalid object for update.");
            }

            try
            {
                var result = await _objektService.UpdateAsync(objektDto);
                if (result == null)
                {
                    return NotFound($"Object with ID {objektDto.IdObjekta} not found.");
                }

                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] DTOS.ObjektDto objektDto)
        {
            if (objektDto == null)
            {
                return BadRequest("Invalid object for creation.");
            }

            try
            {
                var result = await _objektService.CreateAsync(objektDto);
                return CreatedAtAction(nameof(GetById), new { id = result?.IdObjekta }, result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string searchTerm = "", 
            [FromQuery] string searchField = "wildcard",
            [FromQuery] bool? dostupnaDostava = null,
            [FromQuery] string? cjenovniRang = null,
            [FromQuery] decimal? minOcjena = null,
            [FromQuery] decimal? maxOcjena = null)
        {
            try
            {
                var result = await _objektService.GetFilteredAsync(
                    searchTerm, 
                    searchField, 
                    dostupnaDostava,
                    cjenovniRang,
                    minOcjena, 
                    maxOcjena
                );

                if (result == null || !result.Any())
                {
                    return NotFound("No objects match the search criteria.");
                }   

                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTopRatedObjekti()
        {
            try
            {
                var result = await _objektService.GetFilteredAsync(
                    searchTerm: "",
                    searchField: "wildcard",
                    dostupnaDostava: null,
                    cjenovniRang: null,
                    minOcjena: 4.5m,
                    maxOcjena: null
                );
                if (result == null || !result.Any())
                {
                    return NotFound("No objects match the search criteria.");
                }
                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetObjektiClosestToYou([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] double? radiusInKm = null)
        {
            try
            {
                var result = await _objektService.GetObjektiClosestToYouAsync(latitude, longitude, radiusInKm);
                if (result == null || !result.Any())
                {
                    return NotFound("No objects match the search criteria.");
                }
                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred on the server: {ex.Message}");
            }
        }   

        [HttpPost("[action]")]
        public async Task<IActionResult> ExportCsv([FromBody] IEnumerable<DTOS.ObjektDto> objekti)
        {
            var csv = await _objektService.ExportToCsvFromDataAsync(objekti);
            var bytes = Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv; charset=utf-8", "kafici_restorani.csv");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ExportJson([FromBody] IEnumerable<DTOS.ObjektDto> objekti)
        {
            var json = await _objektService.ExportToJsonFromDataAsync(objekti);
            var bytes = Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json; charset=utf-8", "kafici_restorani.json");
        }
    }
}