using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes.Interface;

namespace Seminario.Services.CombosServices.Factory.Interface;

public interface ISetSession
{
    void SetSession(IDbSession session);
}