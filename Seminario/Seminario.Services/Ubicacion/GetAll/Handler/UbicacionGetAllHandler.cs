using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.Ubicacion.GetAll.Command;
using Seminario.Services.Ubicacion.GetAll.Response;

namespace Seminario.Services.Ubicacion.GetAll.Handler;

public class UbicacionGetAllHandler
{
    private readonly DbExecutor _executor;

    public UbicacionGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<List<UbicacionGetAllResponse>> HandleAsync(UbicacionGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@provincia", command.Provincia);
        p.Add("@pais", command.Pais);
        //
        var sql = @"
                select
                    loc.idLocalidad AS IdLocalidad,
                    loc.Descripcion AS Localidad,
                    prov.Descripcion AS Provincia,
                    pais.Descripcion AS Pais,
                    loc.userName AS UserName,
                    loc.userDateTime AS UserDateTime
                from localidad loc
                LEFT JOIN provincia prov ON prov.idProvincia = loc.idProvincia
                LEFT JOIN pais ON pais.idPais = prov.idPais
                WHERE   (@provincia IS NULL OR loc.idProvincia = @provincia)
                    AND (@pais IS NULL OR prov.idPais = @pais)";
        //
        var response = await _executor.ExecuteAsync<UbicacionGetAllResponse>(sql, p);
        return response.ToList();
    }
}