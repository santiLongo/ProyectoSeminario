using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProyectoSeminario.Commands.MaestroCliente.Commands.GetAllCommand;
using ProyectoSeminario.Commands.MaestroCliente.Handlers.GetAll;
using ProyectoSeminario.Services;

namespace ProyectoSeminario.Controllers
{

    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : ControllerBase
    {

        private readonly AppDbContext _ctx;

        public ClienteController(AppDbContext dbContextOptions)
        {
            _ctx = dbContextOptions;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll([FromQuery]GetAllClienteCommand command)
        {
            var handler = new GetAllClienteHandler(_ctx);
            var clientes = handler.Handle(command);

            return Ok(clientes);
        }
    }
}
