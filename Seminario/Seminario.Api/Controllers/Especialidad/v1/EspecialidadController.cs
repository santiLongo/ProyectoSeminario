using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.EspecialidadServices.Upsert.Command;
using Seminario.Services.EspecialidadServices.Upsert.Handler;

namespace Seminario.Api.Controllers.Especialidad.v1;

[ApiController]
[Route("api/v1/especialidad")]
public class EspecialidadController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public EspecialidadController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<Datos.Entidades.Especialidad>> GetAll([FromQuery] DataSourceRequest request)
    {
        var response = await _ctx.EspecialidadRepo.GetAll();
        return response.ToDataSourceResult(request);
    }

    [HttpPost("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] EspecialidadUpsertCommand command)
    {
        var handler = new EspecialidadUpsertHandler(_ctx);
        await handler.HandleAsync(command);
    }
}