using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboChofer : IGetComboData, ISetSession
{
    private DbExecutor _executor;
    
    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
                select
                    idChofer as                     Numero,
                    CONCAT(nombre,' ',apellido) as  Descripcion
                from chofer";

        return _executor.Execute<ComboIntModel>(sql).ToList();
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }
}