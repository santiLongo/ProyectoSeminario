using Dapper;

namespace Seminario.Datos.Dapper;

public class DbExecutor
{
    private readonly IDbSession _session;

    public DbExecutor(IDbSession session)
    {
        _session = session;
    }

    public Task<IEnumerable<T>> Query<T>(string sql, object param)
    {
        return _session.Connection.QueryAsync<T>(
            sql, param, _session.Transaction
        );
    }
}