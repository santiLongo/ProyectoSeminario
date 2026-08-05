using Seminario.Core.Dapper;

namespace Seminario.Services.CombosServices.Factory.Interface;

public interface ISetSession
{
    void SetSession(IDbSession session);
}