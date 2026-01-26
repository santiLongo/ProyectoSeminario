using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Services.ViajeServices.Add.Command;
using Seminario.Services.ViajeServices.Add.Handler;
using Seminario.Services.ViajeServices.CargarDescarga.Command;
using Seminario.Services.ViajeServices.CargarDescarga.Handler;
using Seminario.Services.ViajeServices.ForzarEstado.Command;
using Seminario.Services.ViajeServices.ForzarEstado.Handler;
using Seminario.Services.ViajeServices.Get.Command;
using Seminario.Services.ViajeServices.Get.Handler;
using Seminario.Services.ViajeServices.Get.Model;
using Seminario.Services.ViajeServices.GetAll.Command;
using Seminario.Services.ViajeServices.GetAll.Handler;
using Seminario.Services.ViajeServices.GetAll.Model;
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
    
    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<List<GetAllViajeModel>> GetAll([FromQuery] GetAllViajeCommand command, [FromServices] IDbSession session)
    {
        var handler = new GetAllViajeHandler(session);
        return await handler.Handle(command);
    }
    
    [HttpPost("forzar-estado")]
    [SeminarioResponse]
    public async Task ForzarEstado([FromBody] ForzarEstadoCommand command)
    {
        var handler = new ForzarEstadoHandler(_ctx);
        await handler.Handle(command);
    }
    
    [HttpPost("cargar-descarga")]
    [SeminarioResponse]
    public async Task CargarDescarga([FromBody] CargarDescargaViajeCommand command)
    {
        var handler = new CargarDescargaViajeHandler(_ctx);
        await handler.Handle(command);
    }
}