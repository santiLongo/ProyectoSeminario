using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.Entidades;
using Seminario.Services.EventosService.GetAll.Command;
using Seminario.Services.EventosService.GetAll.Response;

namespace Seminario.Services.EventosService.GetAll.Handler;

public class EventosGetAllHandler
{
    private readonly DbExecutor _executor;

    public EventosGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<IEnumerable<EventosGetAllResponse>> HandleAsync(EventosGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@desde", command.FechaDesde);
        p.Add("@hasta", command.FechaHasta);
        //
        var sql = @"
                    SELECT
                        IdEvento,
                        Titulo,
                        FechaEvento
                    FROM eventos
                    WHERE @desde <= FechaEvento AND @hasta >= FechaEvento AND Inactivo = 0";
        
        return await _executor.ExecuteAsync<EventosGetAllResponse>(sql, p);
    }
}