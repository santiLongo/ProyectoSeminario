using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;

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
        public IActionResult SayHello()
        {
            return Ok("Tamo activo");
        }
    }
}
