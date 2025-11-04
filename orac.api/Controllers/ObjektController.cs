using System.Reflection.Metadata.Ecma335;
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
    }
}