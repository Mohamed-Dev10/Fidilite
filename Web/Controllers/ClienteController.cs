using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteBOService _clienteBOService;

        public ClienteController(IClienteBOService clienteBOService)
        {
            _clienteBOService = clienteBOService;
        }

        // Étape 1 : valide + envoie l'OTP par WhatsApp, renvoie le token
        [HttpPost("InitCreate")]
        public async Task<IActionResult> InitCreate([FromBody] ClienteModel model)
        {
            var result = await _clienteBOService.InitCreate(model);
            return Ok(result);
        }

        // Étape 2 : vérifie l'OTP puis crée la carte (FIDELITE + MARKET + WhatsApp)
        [HttpPost("ConfirmCreate")]
        public async Task<IActionResult> ConfirmCreate([FromQuery] string token, [FromQuery] string otpCode)
        {
            var result = await _clienteBOService.ConfirmCreate(token, otpCode);
            return Ok(result);
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] CarteListFilterModel filter)
        {
            var result = await _clienteBOService.GetList(filter);
            return Ok(result);
        }
    }
}
