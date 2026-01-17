using Dapper;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Datos.Dapper;

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

    public DbSession(AppDbContext ctx)
    {
        Connection  = new MySqlConnection(ctx.Database.GetDbConnection().ConnectionString);
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

