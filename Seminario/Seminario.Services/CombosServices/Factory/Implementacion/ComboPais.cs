using Seminario.Datos.Dapper;
using Seminario.Core.Type.ComboTypes;
using Seminario.Core.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboPais : IGetComboData, ISetSession
{
    private DbExecutor _executor;
    
    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
                select
                    idPais as      Numero,
                    Descripcion as Descripcion
                from pais";

        return _executor.Execute<ComboIntModel>(sql).ToList();
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }
}