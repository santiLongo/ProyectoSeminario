using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboCamion : IGetComboData, ISetSession, ISetExtraParams
{
    private DbExecutor _executor;
    private int? _tipoCamion;
    private string? _marca;
    private string? _modelo;
    
    
    public IEnumerable<ICombo> GetCombo()
    {
        var p = new DynamicParameters();
        p.Add("@modelo", _modelo);
        p.Add("@marca", _marca);
        p.Add("@tipoCamion", _tipoCamion);
        
        var sql = @"
               select
                    idCamion   Numero,
                    CONCAT(Patente,' ','(',Marca,',',Modelo,')') Descripcion
                from camion
                where   (@tipoCamion IS NULL OR idTipoCamion = @tipoCamion)
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
        _tipoCamion = extraParams.TryGetValue("tipoCamion", out value) ? Convert.ToInt32(value) : null;
    }
}