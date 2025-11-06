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
            var result = await _objektService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFiltered([FromQuery] string searchTerm, [FromQuery] string searchField = "wildcard")
        {
            var result = await _objektService.GetFilteredAsync(searchTerm, searchField);
            return Ok(result);
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