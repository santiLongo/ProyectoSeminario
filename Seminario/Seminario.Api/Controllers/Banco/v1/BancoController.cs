using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.BancoCrud.Delete.Command;
using Seminario.Services.BancoCrud.Delete.Handler;
using Seminario.Services.BancoCrud.GetAll.Command;
using Seminario.Services.BancoCrud.GetAll.Handler;
using Seminario.Services.BancoCrud.GetAll.Model;
using Seminario.Services.BancoCrud.Upsert.Command;
using Seminario.Services.BancoCrud.Upsert.Handler;

namespace Seminario.Api.Controllers.Banco.v1
{
    [Route("api/v1/banco")]
    [ApiController]
    public class BancoController : ControllerBase
    {
        private readonly IAppDbContext _ctx;

        public BancoController(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<List<GetAllBancoModel>> GetAll([FromQuery] GetAllBancoCommand command)
        {
            var handler = new GetAllBancoHandler(_ctx);
            return await handler.Handle(command);
        }

        [HttpPost]
        [Route("upsert")]
        public async Task Upsert([FromBody] UpsertBancoCommand command)
        {
            var handler = new UpsertBancoHandler(_ctx);
            await handler.Handle(command);
        }

        [HttpPost]
        [Route("delete")]
        public async Task Delete([FromBody] DeleteBancoCommand command)
        {
            var handler = new DeleteBancoHandler(_ctx);
            await handler.Handle(command);
        }
    }
}
