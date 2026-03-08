using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboSemis : IGetComboData, ISetSession, ISetExtraParams
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
                    where   
                            idTipoCamion in (4,5)
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