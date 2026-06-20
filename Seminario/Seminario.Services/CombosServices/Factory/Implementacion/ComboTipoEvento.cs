using Seminario.Datos.Dapper;
using Seminario.Core.Type.ComboTypes;
using Seminario.Core.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboTipoEvento : IGetComboData, ISetSession
{
    private DbExecutor _executor;

    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
         Select 
             IdTipo AS Numero,
             Nombre  AS Descripcion
         From tiposevento";
        return _executor.Execute<ComboIntModel>(sql);
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }
}
