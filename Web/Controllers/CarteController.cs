using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarteController : ControllerBase
    {
        private readonly ICarteService _carteService;

        public CarteController(ICarteService carteService)
        {
            _carteService = carteService;
        }

        [HttpPost("CreateM3alem")]
        public async Task<IActionResult> CreateM3alem([FromBody] ClienteModel model)
        {
            var result = await _carteService.CreateM3alem(model);
            return Ok(result);
        }

        [HttpPost("CreateArtisan")]
        public async Task<IActionResult> CreateArtisan([FromBody] ClienteModel model)
        {
            var result = await _carteService.CreateArtisan(model);
            return Ok(result);
        }

        [HttpPost("ConfirmationM3alem")]
        public async Task<IActionResult> ConfirmationM3alem(string otpCode, long clienteId)
        {
            var result = await _carteService.ConfirmationM3alem(otpCode, clienteId);
            return Ok(result);
        }

        [HttpPost("ConfirmationArtisan")]
        public async Task<IActionResult> ConfirmationArtisan(string otpCode, long clienteId)
        {
            var result = await _carteService.ConfirmationArtisan(otpCode, clienteId);
            return Ok(result);
        }
    }
}
