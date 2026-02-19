using Dapper;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.Repositorios;
using Seminario.Services.ChoferesCrud.GetAll.Response;

namespace Seminario.Services.ChoferesCrud.GetAll.Handler;

public class ChoferesGetAllHandler
{
    private readonly DbExecutor _executor;
    
    public ChoferesGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<List<ChoferesGetAllResponse>> HandleAsync(ChoferesGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@estado", command.Estado);

        var sql = @"
                SELECT
                    ch.idChofer Id,
                    ch.nombre Nombre,
                    ch.apellido Apellido,
                    ch.dni  Dni,
                    ch.direccion Direccion,
                    ch.telefono Telefono,
                    ch.nroRegistro NroRegistro,
                    viaje.FechaDescarga UltimoViaje,
                    ch.fechaAlta FechaAlta,
                    ch.fechaBaja FechaBaja
                FROM chofer ch
                LEFT JOIN LATERAL ( SELECT * FROM viaje
                                             WHERE viaje.idChofer = ch.idChofer
                                                AND viaje.FechaDescarga IS NOT NULL
                                             ORDER BY idViaje DESC
                                             LIMIT 1) viaje ON TRUE
                WHERE (@estado = 3 
                           OR (@estado = 1 AND ch.fechaBaja IS NULL) 
                           OR (@estado = 2 AND ch.fechaBaja IS NOT NULL))";

        var response = await _executor.ExecuteAsync<ChoferesGetAllResponse>(sql, p);
        
        return response.ToList();
    }
    
}