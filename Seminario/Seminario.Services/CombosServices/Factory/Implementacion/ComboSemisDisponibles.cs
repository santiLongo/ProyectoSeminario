using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboSemisDisponibles : IGetComboData, ISetSession, ISetExtraParams
{
    private DbExecutor _executor;
    private string? _marca;
    private string? _modelo;
    
    
    public IEnumerable<ICombo> GetCombo()
    {
        var p = new DynamicParameters();
        p.Add("@modelo", _modelo);
        p.Add("@marca", _marca);
        
        var sql = @"
               select
                        camion.idCamion   Numero,
                        CONCAT(Patente,' ','(',Marca,',',Modelo,')') Descripcion
                    from camion
                    LEFT JOIN LATERAL ( Select * From mantenimiento
                                        where  idVehiculo = camion.idCamion
                                           AND fechaSalida IS NULL
                                        LIMIT 1) mante ON TRUE
                    LEFT JOIN LATERAL ( Select * From viaje
                                        where  viaje.idCamion = camion.idCamion
                                            AND viaje.Estado = 1
                                        LIMIT 1) viaje ON TRUE
                    where   camion.FechaBaja IS NULL
                        AND mante.idMantenimiento IS NULL
                        AND viaje.idViaje IS NULL   
                        AND idTipoCamion in (4,5)
                        AND (@marca IS NULL OR Marca = @marca)
                        AND (@modelo IS NULL OR Marca = @modelo)";

        return _executor.Execute<ComboIntModel>(sql, p).ToList();
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public void SetExtraParams(Dictionary<string, string> extraParams)
    {
        extraParams.TryGetValue("marca", out var value);
        _marca = value;
        extraParams.TryGetValue("modelo", out value);
        _modelo = value;
    }
    
}