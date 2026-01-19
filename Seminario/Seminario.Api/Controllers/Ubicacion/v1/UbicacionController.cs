using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Ubicacion.Upsert.Command;
using Seminario.Services.Ubicacion.Upsert.Handler;

namespace Seminario.Api.Controllers.Ubicacion.v1;
[Route("api/v1/ubicacion")]
public class UbicacionController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public UbicacionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpPost]
    [Route("upsert")]
    public async Task Upsert([FromBody] UpsertUbicacionCommand command)
    {
        var handler = new UpsertUbicacionHandler(_ctx);
        await handler.Handle(command);
    }
}