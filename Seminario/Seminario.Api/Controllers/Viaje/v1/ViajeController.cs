using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ViajeServices.Add.Command;
using Seminario.Services.ViajeServices.Add.Handler;

namespace Seminario.Api.Controllers.Viaje.v1;

[ApiController]
[Route("api/v1/viaje")]
public class ViajeController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public ViajeController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpPost]
    [Route("add")]
    public async Task Add([FromBody] AddViajeCommand command)
    {
        var handler = new AddViajeHandler(_ctx);
        await handler.Handle(command);
    }
}