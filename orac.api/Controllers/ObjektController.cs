using System.Text;
using Microsoft.AspNetCore.Mvc;
using orac.api.Interfaces;
using orac.api.Models;
using orac.api.DTOS;

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

        /// <summary>
        /// Dohvati sve objekte iz baze.
        /// </summary>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _objektService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NotFound(ApiResponse<object>.Error("No objects found."));
                }

                return Ok(ApiResponse<IEnumerable<ObjektDto>>.Success(result, "Objects retrieved successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }

        /// <summary>
        /// Dohvati pojedinačni objekt po njegovom ID-u.
        /// </summary>
        /// <param name="id">Jedinstveni identifikator objekta (veći od 0).</param>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<object>.Error("ID must be greater than zero."));
            }

            try
            {
                var result = await _objektService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(ApiResponse<object>.Error($"Object with ID {id} not found."));
                }

                return Ok(ApiResponse<ObjektDto>.Success(result, "Object retrieved successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obriši objekt prema ID-u.
        /// </summary>
        /// <param name="id">ID objekta koji se briše.</param>
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<object>.Error("ID must be greater than zero"));
            }

             try
            {
                var result = await _objektService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(ApiResponse<object>.Error($"Object with ID {id} not found."));
                }
                return Ok(ApiResponse<string>.Success("Object has been successfully deleted.", "Object deleted successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }

        /// <summary>
        /// Ažuriraj postojeći objekt.
        /// </summary>
        /// <param name="objektDto">Podaci objekta za ažuriranje (mora sadržavati `IdObjekta`).</param>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] ObjektDto objektDto)
        {
            if (objektDto == null || objektDto.IdObjekta <= 0)
            {
                return BadRequest(ApiResponse<object>.Error("Invalid object for update."));
            }

            try
            {
                var result = await _objektService.UpdateAsync(objektDto);
                if (result == null)
                {
                    return NotFound(ApiResponse<object>.Error($"Object with ID {objektDto.IdObjekta} not found."));
                }

                return Ok(ApiResponse<ObjektDto>.Success(result, "Object updated successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }

        /// <summary>
        /// Kreiraj novi objekt u kolekciji.
        /// </summary>
        /// <param name="objektDto">Podaci novog objekta.</param>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] ObjektDto objektDto)
        {
            if (objektDto == null)
            {
                return BadRequest(ApiResponse<object>.Error("Invalid object for creation."));
            }

            try
            {
                var result = await _objektService.CreateAsync(objektDto);
                return CreatedAtAction(nameof(GetById), new { id = result?.IdObjekta }, ApiResponse<ObjektDto>.Success(result!, "Object created successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }


        /// <summary>
        /// Dohvati filtriranu listu objekata prema kriterijima.
        /// </summary>
        /// <param name="searchTerm">Pojam za pretragu.</param>
        /// <param name="searchField">Polje pretrage (npr. wildcard, naziv, opis).</param>
        /// <param name="dostupnaDostava">Filtriraj po dostupnosti dostave.</param>
        /// <param name="cjenovniRang">Filtriraj po cjenovnom rangu.</param>
        /// <param name="minOcjena">Minimalna prosječna ocjena.</param>
        /// <param name="maxOcjena">Maksimalna prosječna ocjena.</param>
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
                    return NotFound(ApiResponse<object>.Error("No objects match the search criteria."));
                }   

                return Ok(ApiResponse<IEnumerable<ObjektDto>>.Success(result, "Filtered objects retrieved successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }

        /// <summary>
        /// Dohvati najbolje ocijenjene objekte (ocjena >= 4.5).
        /// </summary>
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
                    return NotFound(ApiResponse<object>.Error("No objects match the search criteria."));
                }
                return Ok(ApiResponse<IEnumerable<ObjektDto>>.Success(result, "Top rated objects retrieved successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }

        /// <summary>
        /// Dohvati objekte najbliže zadanom geografskom položaju unutar opcionalnog radijusa.
        /// </summary>
        /// <param name="latitude">Geografska širina korisnika.</param>
        /// <param name="longitude">Geografska dužina korisnika.</param>
        /// <param name="radiusInKm">Radijus pretrage u kilometrima (opcionalno).</param>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetObjektiClosestToYou([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] double? radiusInKm = null)
        {
            try
            {
                var result = await _objektService.GetObjektiClosestToYouAsync(latitude, longitude, radiusInKm);
                if (result == null || !result.Any())
                {
                    return NotFound(ApiResponse<object>.Error("No objects match the search criteria."));
                }
                return Ok(ApiResponse<IEnumerable<ObjektDto>>.Success(result, "Nearby objects retrieved successfully."));
            } catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"An error occurred on the server: {ex.Message}"));
            }
        }   

        /// <summary>
        /// Eksportiraj predanu kolekciju objekata u CSV format.
        /// </summary>
        /// <param name="objekti">Kolekcija objekata za eksport.</param>
        /// <returns>CSV sadržaj kao binarni file response (preuzimanje).</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> ExportCsv([FromBody] IEnumerable<DTOS.ObjektDto> objekti)
        {
            var csv = await _objektService.ExportToCsvFromDataAsync(objekti);
            var bytes = Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv; charset=utf-8", "kafici_restorani.csv");
        }

        /// <summary>
        /// Eksportiraj predanu kolekciju objekata u JSON format.
        /// </summary>
        /// <param name="objekti">Kolekcija objekata za eksport.</param>
        [HttpPost("[action]")]
        public async Task<IActionResult> ExportJson([FromBody] IEnumerable<DTOS.ObjektDto> objekti)
        {
            var json = await _objektService.ExportToJsonFromDataAsync(objekti);
            var bytes = Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json; charset=utf-8", "kafici_restorani.json");
        }
    }
}