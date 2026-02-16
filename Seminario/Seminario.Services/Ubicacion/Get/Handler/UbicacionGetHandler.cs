using System.Net;
using Dapper;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Dapper;
using Seminario.Services.Ubicacion.Get.Command;
using Seminario.Services.Ubicacion.Get.Response;

namespace Seminario.Services.Ubicacion.Get.Handler;

public class UbicacionGetHandler
{
    private readonly DbExecutor _executor;

    public UbicacionGetHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<UbicacionGetResponse> HandleAsync(UbicacionGetCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@id", command.Id);
        //
        var sql = @"
                select
                    loc.idLocalidad AS Id,
                    loc.Descripcion AS Descripcion,
                    prov.idProvincia AS IdProvincia,
                    pais.idPais AS IdPais
                from localidad loc
                         LEFT JOIN provincia prov ON prov.idProvincia = loc.idProvincia
                         LEFT JOIN pais ON pais.idPais = prov.idPais
                WHERE   loc.idLocalidad = @id";
        //
        var response = await _executor.ExecuteFirstOrDefaultAsync<UbicacionGetResponse>(sql, p);

        if (response == null)
            throw new SeminarioException("No se encontro la localidad", HttpStatusCode.NotFound);

        return response;
    }
}