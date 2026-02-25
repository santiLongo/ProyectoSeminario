using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.Mantenimiento.GetAll.Command;
using Seminario.Services.Mantenimiento.GetAll.Response;

namespace Seminario.Services.Mantenimiento.GetAll.Handler;

public class MantenimientoGetAllHandler
{
    private readonly DbExecutor _executor;

    public MantenimientoGetAllHandler(IDbSession session)
    {
        _executor = new DbExecutor(session);
    }

    public async Task<List<MantenimientoGetAllResponse>> HandleAsync(MantenimientoGetAllCommand command)
    {
        var p = new DynamicParameters();
        p.Add("@camion", command.Camion);
        p.Add("@taller", command.Taller);
        p.Add("@fechaDesde", command.FechaEntradaDesde);
        p.Add("@fechaHasta", command.FechaEntradaHasta);
        p.Add("@estado", command.Estado);
        //
        var sql = @"
                    SELECT
                        man.idMantenimiento IdMantenimiento,
                        man.titulo Titulo,
                        cam.Patente Camion,
                        man.fechaEntrada FechaEntrada,
                        man.fechaSalida FechaSalida,
                        tal.nombre Taller,
                        man.importe Importe,
                        pago.total TotalPagado,
                        man.suspendido Suspendido
                    FROM mantenimiento man
                    INNER JOIN camion cam ON cam.idCamion = man.idVehiculo
                    INNER JOIN taller tal ON tal.idTaller = man.idTaller
                    LEFT JOIN LATERAL ( SELECT SUM(monto) total FROM pago
                                                INNER JOIN `pago/mantenimiento` pagman ON pagman.idPago = pago.idPago
                                                WHERE pagman.idMantenimiento = man.idMantenimiento) pago ON TRUE
                    WHERE
                        (
                            (@estado = 1 AND (man.fechaSalida is null AND IFNULL(man.suspendido,0) = 0)) OR
                            (@estado = 2 AND ((IFNULL(pago.total,0) < man.importe || man.importe <= 0 || man.importe is null)
                                                    AND IFNULL(man.suspendido,0) = 0 AND man.fechaSalida is not null)) OR
                            (@estado = 3 AND ( IFNULL(pago.total,0) >= man.importe AND IFNULL(man.suspendido,0) = 0)) OR
                            (@estado = 4 AND IFNULL(man.suspendido,0) = 1) OR
                            (@estado = 5)
                        )
                        AND (@camion is null or man.idVehiculo = @camion)
                        AND (@taller is null or man.idTaller = @taller)
                        AND (@fechaDesde is null or man.fechaEntrada >= @fechaDesde)
                        AND (@fechaHasta is null or man.fechaEntrada <= @fechaHasta)";
        //
        var datos = await _executor.ExecuteAsync<MantenimientoGetAllQuery>(sql, p);
        //
        var response = new List<MantenimientoGetAllResponse>();

        foreach (var dato in datos)
        {
            var item = new MantenimientoGetAllResponse
            {
                IdMantenimiento = dato.IdMantenimiento,
                Titulo = dato.Titulo,
                Camion = dato.Camion,
                FechaEntrada = dato.FechaEntrada,
                FechaSalida = dato.FechaSalida,
                Taller = dato.Taller,
                Importe = dato.Importe,
            };

            if (dato.Suspendido)
            {
                item.Estado = "Suspendido";
                response.Add(item);
                continue;
            }

            if (dato.FechaSalida == null)
            {
                item.Estado = "En Mantenimiento";
                response.Add(item);
                continue;
            }
            
            if (dato.Importe == null)
            {
                item.Estado = "Pendiente de Pago";
                response.Add(item);
                continue;
            }
            
            if (dato.Importe > dato.TotalPagado)
            {
                item.Estado = "Pendiente de Pago";
                response.Add(item);
                continue;
            }

            item.Estado = "Pagado";
            response.Add(item);
        }
        
        return response;
    }
}

internal class MantenimientoGetAllQuery
{
    public int IdMantenimiento { get; set; }
    public string Titulo { get; set; }
    public string Camion  { get; set; }
    public DateTime? FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public string Taller { get; set; }
    public decimal? Importe { get; set; }
    public decimal? TotalPagado { get; set; }
    public bool Suspendido { get; set; }
}