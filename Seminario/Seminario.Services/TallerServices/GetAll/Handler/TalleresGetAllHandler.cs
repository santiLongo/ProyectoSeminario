using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.TallerServices.GetAll.Command;
using Seminario.Services.TallerServices.GetAll.Response;

namespace Seminario.Services.TallerServices.GetAll.Handler;

public class TalleresGetAllHandler
{
    private readonly DbExecutor _executor;

    public TalleresGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<IEnumerable<TalleresGetAllResponse>> HandleAsync(TalleresGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@localidad", command.Localidad);
        p.Add("@provincia", command.Provincia);
        p.Add("@especialidad", command.Especialidad);
        //
        var sql = @"
                    SELECT
                        taller.idTaller IdTaller,
                        taller.nombre Nombre,
                        taller.cuit Cuit,
                        taller.telefono Telefono,
                        loc.Descripcion Localidad,
                        prov.Descripcion Provincia
                    FROM taller
                    INNER JOIN localidad loc ON loc.idLocalidad = taller.idLocalidad
                    INNER JOIN provincia prov ON prov.idProvincia = loc.idProvincia
                    LEFT JOIN LATERAL ( SELECT * FROM `taller/especialidad` taesp
                                                 WHERE (@especialidad is null OR @especialidad = taesp.idEspecialidad)
                                                        AND taesp.idTaller = taller.idTaller LIMIT 1) espe ON TRUE
                    WHERE espe.idTallerEspecialidad is not null
                        AND (@localidad is null or @localidad = loc.idLocalidad)
                        AND (@provincia is null or @provincia = prov.idProvincia)";
        //
        return await _executor.ExecuteAsync<TalleresGetAllResponse>(sql, p);
    }
}