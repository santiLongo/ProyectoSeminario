using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboTipoCamion : IGetComboData, ISetSession
{
    private DbExecutor _executor;
    
    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
                Select 
                    idTipoCamion AS Numero,
                    Descripcion  AS Descripcion
                From tipocamion";
        return _executor.Execute<ComboIntModel>(sql);
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }
}