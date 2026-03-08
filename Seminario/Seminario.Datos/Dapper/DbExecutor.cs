using Dapper;

namespace Seminario.Datos.Dapper;

public interface IDbExecutor
{
    Task<IEnumerable<T>> ExecuteAsync<T>(string sql, DynamicParameters? param = null);
    IEnumerable<T> Execute<T>(string sql, DynamicParameters? param = null);
    T? ExecuteFirstOrDefault<T>(string sql, DynamicParameters? param = null);
    Task<T?> ExecuteFirstOrDefaultAsync<T>(string sql, DynamicParameters? param = null);
}


public class DbExecutor : IDbExecutor
{
    private readonly IDbSession _session;

    public DbExecutor(IDbSession session)
    {
        _session = session;
    }

    public Task<IEnumerable<T>> ExecuteAsync<T>(string sql, DynamicParameters? param = null)
    {
        return _session.Connection.QueryAsync<T>(
            sql, param, _session.Transaction
        );
    }
    
    
    public IEnumerable<T> Execute<T>(string sql, DynamicParameters? param = null)
    {
        return _session.Connection.Query<T>(
            sql, param, _session.Transaction
        );
    }
    
    public T? ExecuteFirstOrDefault<T>(string sql, DynamicParameters? param = null)
    {
        return _session.Connection.QueryFirstOrDefault<T>(
            sql, param, _session.Transaction
        );
    }
    
    public Task<T?> ExecuteFirstOrDefaultAsync<T>(string sql, DynamicParameters? param = null)
    {
        return _session.Connection.QueryFirstOrDefaultAsync<T>(
            sql, param, _session.Transaction
        );
    }
}