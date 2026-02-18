using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboMarcaCamion : IGetComboData, ISetSession
{
    private DbExecutor _executor;
    
    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
                Select
                    Marca AS Numero,
                    Marca  AS Descripcion
                From camion
                GROUP BY Marca";
        return _executor.Execute<ComboStringModel>(sql);
    }

    public void SetSession(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }
}