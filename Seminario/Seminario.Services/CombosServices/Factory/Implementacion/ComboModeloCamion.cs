using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboModeloCamion : IGetComboData, ISetSession, ISetExtraParams
{
    private DbExecutor _executor;
    private string? _marca;

    public IEnumerable<ICombo> GetCombo()
    {
        var p = new DynamicParameters();
        p.Add("@marca", _marca);
        
        var sql = @"
                Select
                    Modelo AS Numero,
                    Modelo  AS Descripcion
                From camion
                WHERE (@marca IS NULL OR Marca = @marca)
                GROUP BY Modelo";
        
        return _executor.Execute<ComboStringModel>(sql, p).ToList();
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public void SetExtraParams(Dictionary<string, string> extraParams)
    {
        extraParams.TryGetValue("marca", out var value);
        _marca = value;
    }
    
}