using System.Data;

namespace Seminario.Datos.Factories.ExecuteFactory.Interface;

public interface IExecuteQuery<TResult>
{
    Task<TResult> Execute(IDbConnection connection, IDbTransaction? transaction);
}