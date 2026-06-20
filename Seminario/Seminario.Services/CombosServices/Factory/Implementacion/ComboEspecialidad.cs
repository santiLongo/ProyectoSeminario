using Seminario.Datos.Dapper;
using Seminario.Core.Type.ComboTypes;
using Seminario.Core.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboEspecialidad: IGetComboData, ISetSession
{
    private DbExecutor _executor;

    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
                    select
                        idEspecialidad   Numero,
                        RTRIM(Descripcion) Descripcion
                    from especialidad";

        return _executor.Execute<ComboIntModel>(sql).ToList();
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }
    
}