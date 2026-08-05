using Microsoft.AspNetCore.Mvc;
using Seminario.Core.Dapper;
using Seminario.Core.DataSourceResult.Clases;
using Seminario.Core.DataSourceResult.ExtesionMethods;
using Seminario.Core.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ChequesServices.Commands.GetAll;

namespace Seminario.Api.Controllers.Cheques.v1;

[Route("api/v1/cheques")]
[ApiController]
public class ChequesController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public ChequesController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<ChequesGetAllResponse>> GetAll([FromQuery] ChequesGetAllCommand command,
        [FromQuery] DataSourceRequest request, [FromServices] IDbExecutor _ex)
    {
        var handler = new ChequesGetAllHandler(_ex);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }
}