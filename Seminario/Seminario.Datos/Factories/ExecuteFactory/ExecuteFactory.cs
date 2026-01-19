using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.Factories.ExecuteFactory.Interface;

namespace Seminario.Datos.Factories.ExecuteFactory;

public abstract class ExecuteFactory
{
    protected abstract IExecuteQuery<TResult> CrearExecute<TResult>();

    public async Task<TResult> Execute<TResult>(AppDbContext ctx)
    {
        using var dbSession = new DbSession(ctx);

        try
        {
            await dbSession.BeginTransaction();

            var executor = CrearExecute<TResult>();

            var result = await executor.Execute(
                dbSession.Connection,
                dbSession.Transaction
            );

            await dbSession.Commit();
            return result;
        }
        catch (Exception ex)
        {
            await dbSession.Rollback();
            throw ex;
        }
    }
}