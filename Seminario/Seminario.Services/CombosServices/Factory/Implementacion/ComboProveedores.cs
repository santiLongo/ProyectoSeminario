using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;
public class ComboProveedores : IGetComboData, ISetSession
{
    private IDbExecutor _dbExecutor;
    public IEnumerable<ICombo> GetCombo()
    {
        var sql = @"
                select
                    idProveedor   Numero,
                    razonSocial Descripcion
                from proveedor";

        return _dbExecutor.Execute<ComboIntModel>(sql);
    }

    public void SetSession(IDbSession session)
    {
        _dbExecutor = new DbExecutor(session);
    }
}