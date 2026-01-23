using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CamionCrud.Upsert.Command;
using Seminario.Services.CamionCrud.Upsert.Handler;

namespace Seminario.Api.Controllers.CamionController.v1;

[ApiController]
[Route("api/v1/camion")]
public class CamionController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public CamionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpPost]
    [Route("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] UpsertCamionCommand command)
    {
        var handler = new UpsertCamionHandler(_ctx);
        await handler.Handle(command);
    }
}