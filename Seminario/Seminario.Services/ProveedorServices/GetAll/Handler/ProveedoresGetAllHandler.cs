using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.ProveedorServices.GetAll.Command;
using Seminario.Services.ProveedorServices.GetAll.Response;

namespace Seminario.Services.ProveedorServices.GetAll.Handler;

public class ProveedoresGetAllHandler
{
    private readonly DbExecutor _executor;

    public ProveedoresGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<IEnumerable<ProveedoresGetAllResponse>> HandleAsync(ProveedoresGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@localidad", command.Localidad);
        p.Add("@provincia", command.Provincia);
        p.Add("@especialidad", command.Especialidad);
        //
        var sql = @"
                    SELECT
                        proveedor.idProveedor IdProveedor,
                        proveedor.razonSocial RazonSocial,
                        proveedor.cuit Cuit,
                        proveedor.direccion Direccion,
                        proveedor.responsable Responsable,
                        loc.Descripcion Localidad,
                        prov.Descripcion Provincia
                    FROM proveedor
                             INNER JOIN localidad loc ON loc.idLocalidad = proveedor.idLocalidad
                             INNER JOIN provincia prov ON prov.idProvincia = loc.idProvincia
                             LEFT JOIN LATERAL ( SELECT * FROM `proveedor/especialidad` presp
                                                 WHERE (@especialidad is null OR @especialidad = presp.idEspecialidad)
                                                   AND presp.idProveedor = proveedor.idProveedor LIMIT 1) espe ON TRUE
                    WHERE espe.idProveedorEspecialidad is not null
                      AND (@localidad is null or @localidad = loc.idLocalidad)
                      AND (@provincia is null or @provincia = prov.idProvincia)";
        //
        return await _executor.ExecuteAsync<ProveedoresGetAllResponse>(sql, p);
    }
}