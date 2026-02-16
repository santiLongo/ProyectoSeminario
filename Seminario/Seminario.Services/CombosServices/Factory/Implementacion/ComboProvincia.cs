using Dapper;
using Microsoft.AspNetCore.Http;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboProvincia : IGetComboData, ISetSession, ISetExtraParams
{
    private DbExecutor _executor;
    private int? _pais;

    public IEnumerable<ICombo> GetCombo()
    {
        var p = new DynamicParameters();
        p.Add("@pais", _pais);
        
        var sql = @"
                select
                    prov.idProvincia   Numero,
                    prov.Descripcion Descripcion
                from provincia prov
                LEFT JOIN pais ON pais.IdPais =  prov.IdPais
                WHERE (@pais IS NULL OR @pais = prov.idPais)";
        
        return _executor.Execute<ComboIntModel>(sql, p).ToList();
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public void SetExtraParams(Dictionary<string, string> extraParams)
    {
        _pais =  extraParams.TryGetValue("pais", out var value) ? Convert.ToInt32(value) : null;
    }
}