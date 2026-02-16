using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.FormularioViaje.BuscoChofer.Response;

namespace Seminario.Services.FormularioViaje.BuscoChofer;

public class BuscoChoferHandler
{
    private readonly DbExecutor _executor;

    public BuscoChoferHandler(DbExecutor executor)
    {
        _executor = executor;
    }

    public async Task<BuscoChoferResponse?> HandleAsync(int IdChofer)
    {
        var p = new DynamicParameters();
        p.Add("@chofer", IdChofer);
        //
        var sql = @"SELECT dni as Dni, nroRegistro as NroRegistro FROM chofer WHERE idChofer = @chofer";
        //
        return await _executor.ExecuteFirstOrDefaultAsync<BuscoChoferResponse>(sql, p);
    }
}