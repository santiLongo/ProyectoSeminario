using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.Entidades;
using Seminario.Services.CamionCrud.GetAll.Command;
using Seminario.Services.CamionCrud.GetAll.Response;

namespace Seminario.Services.CamionCrud.GetAll.Handler;

public class CamionGetAllHandler
{
    private readonly DbExecutor _executor;

    public CamionGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<List<CamionGetAllResponse>> HandleAsync(CamionGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@camion", command.Camion);
        p.Add("@marca", command.Marca);
        p.Add("@modelo", command.Modelo);
        p.Add("@tipoCamion", command.TipoCamion);
        //
        var sql = @"
                    select
                        cam.idCamion                Id,
                        cam.Patente                 Patente,
                        cam.Marca                   Marca,
                        cam.Modelo                  Modelo,
                        cam.NroChasis               NroChasis,
                        cam.NroMotor                NroMotor,
                        tipo.Descripcion            TipoCamion,
                        viaje.FechaDescarga         UltimoViaje,
                        mantenimiento.fechaSalida   UltimoMantenimiento,
                        cam.FechaAlta               FechaAlta,
                        cam.FechaBaja               FechaBaja
                    from camion cam
                    LEFT JOIN tipocamion tipo ON tipo.idTipoCamion = cam.idTipoCamion
                    LEFT JOIN LATERAL ( SELECT * FROM viaje
                        WHERE viaje.idCamion = cam.idCamion
                        ORDER BY FechaPartida desc LIMIT 1) viaje ON TRUE
                    LEFT JOIN LATERAL ( SELECT * FROM mantenimiento
                                        WHERE mantenimiento.idVehiculo = cam.idCamion
                                        ORDER BY FechaPartida desc LIMIT 1) mantenimiento ON TRUE
                    where   (@camion IS NULL OR cam.idCamion = @camion)
                        AND (@tipoCamion IS NULL OR cam.idTipoCamion = @tipoCamion)
                        AND (@marca IS NULL OR cam.Marca = @marca)
                        AND (@modelo IS NULL OR cam.Modelo = @modelo)";
        //
        var response = await _executor.ExecuteAsync<CamionGetAllResponse>(sql, p);
        
        return response.ToList();
    }
}