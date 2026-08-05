using Microsoft.AspNetCore.Mvc;
using Seminario.Core.ArchivoManager;
using Seminario.Core.Dapper;
using Seminario.Core.DataSourceResult.Clases;
using Seminario.Core.DataSourceResult.ExtesionMethods;
using Seminario.Core.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.CamionCrud.Archivos.Borrar;
using Seminario.Services.CamionCrud.Archivos.GetAll;
using Seminario.Services.CamionCrud.Archivos.Subir;

namespace Seminario.Api.Controllers.CamionController.v1;

[ApiController]
[Route("api/v1/archivosCamiones")]
public class ArchivosCamionesController : ControllerBase
{
    private readonly IArchivosManager _archivosManager;

    private readonly IAppDbContext _ctx;

    public ArchivosCamionesController(IAppDbContext ctx, IArchivosManager archivosManager)
    {
        _ctx = ctx;
        _archivosManager = archivosManager;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<ArchivoCamionesGetAllResponse>> GetAll([FromQuery] int idCamion,
        [FromQuery] DataSourceRequest request, [FromServices] IDbExecutor executor)
    {
        var handler = new ArchivoCamionesGetAllHandler(executor);
        var response = await handler.HandleAsync(idCamion);
        return response.ToDataSourceResult(request);
    }

    [HttpPost("save")]
    [SeminarioResponse]
    public async Task Guardar([FromBody] SubirArchivosCamionesCommand command)
    {
        var handler = new SubirArchivosCamionesHandler(_ctx, _archivosManager);
        await handler.HandleAsync(command);
    }
    
    [HttpPost("delete")]
    [SeminarioResponse]
    public async Task Delete([FromBody] DeleteArchivoCamionCommand command)
    {
        var handler = new DeleteArchivoCamionHandler(_ctx, _archivosManager);
        await handler.HandleAsync(command);
    }
}