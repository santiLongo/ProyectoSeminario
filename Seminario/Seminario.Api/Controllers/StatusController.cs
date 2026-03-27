using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Api.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IAppDbContext _ctx;

        public StatusController(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("sayHello")]
        [AllowAnonymous]
        public async Task<IActionResult> SayHello()
        {
            IEnumerable<Seminario.Datos.Entidades.Especialidad> especialidades;
            //
            try
            {
                especialidades = await _ctx.EspecialidadRepo.GetAll();
            }
            catch (MySqlException ex)
            {
                return Ok(ex.Message);
            }
            //
            return Ok($"Hoy soy bueno en todo esto: {especialidades.ToList()}");
        }
    }
}
