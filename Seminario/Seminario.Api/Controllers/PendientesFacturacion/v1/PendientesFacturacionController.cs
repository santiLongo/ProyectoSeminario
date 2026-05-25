using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaMantenimiento;
using Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaViaje;
using Seminario.Services.FacturasServices.PendientesFacturacion.GetMantenimientosPendientes;
using Seminario.Services.FacturasServices.PendientesFacturacion.GetViajesPendientes;

namespace Seminario.Api.Controllers.PendientesFacturacion.v1;

[ApiController]
[Route("api/v1/pendientes-facturacion")]
public class PendientesFacturacionController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public PendientesFacturacionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("viajes")]
    [SeminarioResponse]
    public async Task<IList<GetViajesPendientesResponse>> GetViajesPendientes()
    {
        var handler = new GetViajesPendientesHandler(_ctx);
        return await handler.HandleAsync();
    }

    [HttpGet("mantenimientos")]
    [SeminarioResponse]
    public async Task<IList<GetMantenimientosPendientesResponse>> GetMantenimientosPendientes()
    {
        var handler = new GetMantenimientosPendientesHandler(_ctx);
        return await handler.HandleAsync();
    }

    [HttpPost("viajes/{idViaje:int}/generar")]
    [SeminarioResponse]
    public async Task<int> GenerarFacturaViaje(int idViaje, [FromBody] GenerarFacturaViajeCommand command)
    {
        var handler = new GenerarFacturaViajeHandler(_ctx);
        return await handler.HandleAsync(idViaje, command);
    }

    [HttpPost("mantenimientos/{idMantenimiento:int}/generar")]
    [SeminarioResponse]
    public async Task<int> GenerarFacturaMantenimiento(int idMantenimiento, [FromBody] GenerarFacturaMantenimientoCommand command)
    {
        var handler = new GenerarFacturaMantenimientoHandler(_ctx);
        return await handler.HandleAsync(idMantenimiento, command);
    }
}
