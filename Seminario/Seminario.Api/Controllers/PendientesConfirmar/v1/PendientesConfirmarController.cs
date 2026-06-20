using Microsoft.AspNetCore.Mvc;
using Seminario.Core.DataSourceResult.Clases;
using Seminario.Core.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Core.DataSourceResult.ExtesionMethods;
using Seminario.Services.FacturasServices.DeleteFactura;
using Seminario.Services.FacturasServices.GetAllFacturas;

namespace Seminario.Api.Controllers.PendientesConfirmar.v1;

[ApiController]
[Route("api/v1/pendientes-confirmar")]
public class PendientesConfirmarController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public PendientesConfirmarController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<GetAllFacturasResponse>> GetAll(
        [FromQuery] DataSourceRequest request,
        [FromQuery] GetAllFacturasCommand command)
    {
        command.Confirmada = false;
        command.Anulada = false;
        var handler = new GetAllFacturasHandler(_ctx);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }

    [HttpPost("delete")]
    [SeminarioResponse]
    public async Task Delete([FromBody] DeleteFacturaCommand command)
    {
        var handler = new DeleteFacturaHandler(_ctx);
        await handler.HandleAsync(command);
    }
}