using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ViajeServices.Add.Command;
using Seminario.Services.ViajeServices.Add.Handler;
using Seminario.Services.ViajeServices.Get.Command;
using Seminario.Services.ViajeServices.Get.Handler;
using Seminario.Services.ViajeServices.Get.Model;
using Seminario.Services.ViajeServices.Update.Command;
using Seminario.Services.ViajeServices.Update.Handler;

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
    [SeminarioResponse]
    public async Task Add([FromBody] AddViajeCommand command)
    {
        var handler = new AddViajeHandler(_ctx);
        await handler.Handle(command);
    }
    
    [HttpPost]
    [Route("update")]
    [SeminarioResponse]
    public async Task Update([FromBody] UpdateViajeCommand command)
    {
        var handler = new UpdateViajeHandler(_ctx);
        await handler.Handle(command);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<GetViajeModel> Get([FromQuery] GetViajeCommand command)
    {
        var handler = new GetViajeHandler(_ctx);
        return await handler.Handle(command);
    }
}