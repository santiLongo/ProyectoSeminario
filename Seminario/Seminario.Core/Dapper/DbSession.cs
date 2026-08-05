using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Seminario.Core.Dapper;

public interface IDbSession : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }
    Task BeginTransaction();
    Task Commit();
    Task Rollback();
}
public class DbSession : IDbSession
{
    public IDbConnection Connection { get; set; }
    public IDbTransaction? Transaction { get; private set; }

    public DbSession(IConfiguration configuration)
    {
        Connection  = new MySqlConnection(configuration.GetConnectionString("ConnectionMySql"));
        Connection.Open();
    }
    
    public Task BeginTransaction()
    {
        if (Connection.State != ConnectionState.Open)
            Connection.Open();

        Transaction = Connection.BeginTransaction();
        return Task.CompletedTask;
    }

    public Task Commit()
    {
        Transaction.Commit();
        return Task.CompletedTask; 
    }

    public Task Rollback()
    {
        Transaction.Rollback();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        Connection?.Dispose();
    }
}

