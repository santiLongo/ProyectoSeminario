using Microsoft.AspNetCore.Mvc;
using ProyectoSeminario.Services;

namespace ProyectoSeminario.Controllers.ComboController.v1
{
    [ApiController]
    [Route("api/ComboController")]
    public class ComboController : ControllerBase
    {

        private readonly AppDbContext _ctx;

        public ComboController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{comboName}")]
        public IActionResult GetCombo(string comboName)
        {
            comboName = comboName.ToLower();

            switch (comboName)
            {
                case "empresas":
                    break;
                case "paises":
                    break;
                case "vendedores":
                    break;
                default:
                    return NotFound($"No existe combo con nombre '{comboName}'.");
            }

            return NotFound($"No existe combo con nombre '{comboName}'.");
        }
    }
}