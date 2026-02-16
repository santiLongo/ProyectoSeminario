using Dapper;
using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Dapper;
using Seminario.Services.FormularioViaje.BuscoChofer;
using Seminario.Services.FormularioViaje.BuscoChofer.Response;
using Seminario.Services.ViajeServices.Add.Command;

namespace Seminario.Api.Controllers.FormularioViaje.v1;

[ApiController]
[Route("api/v1/formulario-viaje")]
public class FormularioViajeController : ControllerBase
{
    private readonly DbExecutor _executor;

    public FormularioViajeController(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    [HttpGet("cuit-cliente")]
    [SeminarioResponse]
    public async Task<object> CuitCliente([FromQuery] int idCliente)
    {
        var p = new DynamicParameters();
        p.Add("@cliente", idCliente);
        //
        var sql = @"SELECT cuit FROM cliente WHERE idCliente = @cliente";
        //
        var response = await _executor.ExecuteFirstOrDefaultAsync<string>(sql, p);
        return new { Cuit = response };
    }
    
    [HttpGet("busco-chofer")]
    [SeminarioResponse]
    public async Task<BuscoChoferResponse?> BuscoChofer([FromQuery] int idChofer)
    {
        var handler = new BuscoChoferHandler(_executor);
        return await handler.HandleAsync(idChofer);
    }
    
    [HttpGet("ultimo-mantenimiento")]
    [SeminarioResponse]
    public async Task<object> UltimoMantenimiento([FromQuery] int idCamion)
    {
        var p = new DynamicParameters();
        p.Add("@camion", idCamion);
        //
        var sql = "SELECT fechaSalida FROM mantenimiento WHERE idVehiculo = @camion ORDER BY fechaSalida desc limit 1";
        //
        var response =  await _executor.ExecuteFirstOrDefaultAsync<DateTime?>(sql, p);
        return new { UltimoMantenimiento = response };
    }
}