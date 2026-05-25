using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.FacturasServices.AnularFactura;
using Seminario.Services.FacturasServices.EnviarAfip;
using Seminario.Services.FacturasServices.FacturasEmitidas.ConfirmarEmitida;
using Seminario.Services.FacturasServices.FacturasEmitidas.CrearEmitidaConViaje;
using Seminario.Services.FacturasServices.FacturasEmitidas.CrearEmitidaSinViaje;
using Seminario.Services.FacturasServices.GetAllFacturas;
using Seminario.Services.FacturasServices.GetFactura;
using Seminario.Datos.Entidades;

namespace Seminario.Api.Controllers.FacturasEmitidas.v1;

[ApiController]
[Route("api/v1/facturas-emitidas")]
public class FacturasEmitidasController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    private readonly IHttpClientFactory _httpFactory;

    public FacturasEmitidasController(IAppDbContext ctx, IHttpClientFactory httpFactory)
    {
        _ctx = ctx;
        _httpFactory = httpFactory;
    }

    [HttpPost("add-con-viaje")]
    [SeminarioResponse]
    public async Task<int> AddConViaje([FromBody] CrearEmitidaConViajeCommand command)
    {
        var handler = new CrearEmitidaConViajeHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpPost("add-sin-viaje")]
    [SeminarioResponse]
    public async Task<int> AddSinViaje([FromBody] CrearEmitidaSinViajeCommand command)
    {
        var handler = new CrearEmitidaSinViajeHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<GetAllFacturasResponse>> GetAll(
        [FromQuery] DataSourceRequest request,
        [FromQuery] GetAllFacturasCommand command)
    {
        command.TipoFactura = (int)Factura.TipoFactura.Emitida;
        var handler = new GetAllFacturasHandler(_ctx);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }

    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<GetFacturaResponse> Get([FromQuery] GetFacturaCommand command)
    {
        var handler = new GetFacturaHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpPost("anular")]
    [SeminarioResponse]
    public async Task Anular([FromBody] AnularFacturaCommand command)
    {
        var handler = new AnularFacturaHandler(_ctx);
        await handler.HandleAsync(command);
    }

    [HttpPost("{id:int}/afip/emitir")]
    [SeminarioResponse]
    public async Task<RespuestaAfipDto> EmitirAfip(int id, [FromBody] EnviarFacturaAfipCommand command)
    {
        command.IdFactura = id;
        var http = _httpFactory.CreateClient("easyafip");
        var handler = new EnviarFacturaAfipHandler(_ctx, http);
        return await handler.HandleAsync(command);
    }

    [HttpPost("{id:int}/confirmar")]
    [SeminarioResponse]
    public async Task Confirmar(int id, [FromBody] ConfirmarEmitidaCommand command)
    {
        var handler = new ConfirmarEmitidaHandler(_ctx);
        await handler.HandleAsync(id, command);
    }
}
