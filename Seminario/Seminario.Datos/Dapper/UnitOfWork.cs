using Dapper;
using System.Data;

namespace Seminario.Datos.Dapper;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransaction();
    Task Commit();
    Task Rollback();
}
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;

    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task BeginTransaction()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        _transaction = _connection.BeginTransaction();
        return Task.CompletedTask;
    }

    public Task Commit()
    {
        _transaction.Commit();
        return Task.CompletedTask; 
    }

    public Task Rollback()
    {
        _transaction.Rollback();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}

