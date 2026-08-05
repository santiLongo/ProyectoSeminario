using Dapper;
using Seminario.Core.Dapper;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.CamionCrud.Archivos.GetAll;

public class ArchivoCamionesGetAllHandler
{
    private readonly IDbExecutor _executor;

    public ArchivoCamionesGetAllHandler(IDbExecutor executor)
    {
        _executor = executor;
    }

    public async Task<List<ArchivoCamionesGetAllResponse>> HandleAsync(int idCamion)
    {
        const string sql = @"
                            SELECT
                                arch.Id Id,
                                arch.Archivo Nombre,
                                camion.Patente Camion,
                                arch.Fecha Fecha,
                                arch.UserName Usuario
                            FROM ArchivosCamiones arch
                            INNER JOIN camion ON camion.idCamion = arch.IdCamion
                            where arch.IdCamion = @camion";

        var p = new DynamicParameters();
        p.Add("@camion", idCamion);

        var reponse = await _executor.ExecuteAsync<ArchivoCamionesGetAllResponse>(sql, p);

        return reponse.ToList();
    }
}