using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ChoferesCrud.Upsert.Command;
using Seminario.Services.ChoferesCrud.Upsert.Handler;

namespace Seminario.Api.Controllers.ChoferesController.v1;

[ApiController]
[Route("api/v1/choferes")]
public class ChoferesController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public ChoferesController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost]
    [Route("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] UpsertChoferCommand command)
    {
        var handler = new UpsertChoferHandler(_ctx);
        await handler.Handle(command);
    }
}