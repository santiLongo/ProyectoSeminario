using Dapper;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Services.CobrosServices.GetAll.Command;
using Seminario.Services.CobrosServices.GetAll.Response;

namespace Seminario.Services.CobrosServices.GetAll.Handler;

public class CobrosGetAllHandler
{
    private readonly DbExecutor _executor;
    private readonly IAppDbContext _ctx;

    public CobrosGetAllHandler(IDbSession session, IAppDbContext ctx)
    {
        _executor = new DbExecutor(session);
        _ctx = ctx;
    }

    public async Task<List<CobrosGetAllResponse>> HandleAsync(CobrosGetAllCommand command)
    {
        var response = new List<CobrosGetAllResponse>();
        //
        var p = new DynamicParameters();
        p.Add("@nroViaje", command.NroViaje);
        p.Add("@fechaDesde", command.FechaDesde);
        p.Add("@fechaHasta", command.FechaHasta);
        p.Add("@formaPago", command.FormaPago);
        p.Add("@estado", command.Estado);
        //
        var sql = @"Select
                        cobro.idCobro  AS IdCobro,
                        viaje.nroViaje AS NroViaje,
                        cobro.FechaRecibo AS FechaRecibo,
                        cobro.Monto AS Monto,
                        moneda.Descripcion  AS Moneda,
                        cobro.TipoCambio    AS TipoCambio,
                        formapago.Descripcion   AS FormaPago,
                        cheque.idPagoCheque AS IdCheque,
                        cheque.fechaDeposito AS FechaDeposito,
                        IFNULL(cheque.rechazado, 0)    AS Rechazado,
                        IFNULL(anulado.idCobro, 0) AS Anulado
                    FROM cobro
                    INNER JOIN viaje ON viaje.idViaje = cobro.IdViaje
                    INNER JOIN moneda ON moneda.idMoneda = cobro.idMoneda
                    INNER JOIN formapago ON formapago.idFormaPago = cobro.idFormaPago
                    LEFT JOIN cobro anulado on anulado.cobroAnulado = cobro.idCobro
                    LEFT JOIN pagocheque cheque ON cheque.idPagoCheque = cobro.idPagoCheque
                    WHERE cobro.Monto > 0 
                    AND (
                        -- SOLO LOS COBRADOS
                            (@estado = 1 AND anulado.idCobro IS NULL
                                AND (cheque.idPagoCheque IS NULL 
                                     OR (cheque.rechazado = 0 AND cheque.fechaDeposito IS NOT NULL)))
                        -- SOLO LOS PENDIENTES (CHEQUES)
                        OR  (@estado = 2 AND anulado.idCobro IS NULL
                                AND (cheque.idPagoCheque IS NOT NULL 
                                     AND cheque.rechazado = 0 
                                     AND cheque.fechaDeposito IS NULL))
                        -- SOLO LOS ANULADOS
                        OR  (@estado = 3 AND anulado.idCobro IS NOT NULL)
                        -- TODOS
                        OR  (@estado = 4)
                    )
                    AND (@fechaDesde is null OR @fechaDesde <= cobro.FechaRecibo)
                    AND (@fechaHasta is null OR @fechaHasta >= cobro.FechaRecibo)
                    AND (@nroViaje IS NULL OR @nroViaje = viaje.nroViaje)
                    AND (@formaPago IS NULL OR @formaPago = cobro.idFormaPago) ";
        //
        var cobros = await _executor.ExecuteAsync<CobrosGetAllQuery>(sql, p);

        foreach (var cobro in cobros)
        {
            var dato = new CobrosGetAllResponse
            {
                IdCobro = cobro.IdCobro,
                NroViaje = cobro.NroViaje,
                FechaRecibo = cobro.FechaRecibo,
                Monto = cobro.Monto,
                Moneda = cobro.Moneda,
                TipoCambio = cobro.TipoCambio,
                FormaPago = cobro.FormaPago
            };

            if (cobro.Anulado)
            {
                dato.Estado = "Anulado";
                response.Add(dato);
                continue;
            }

            if (cobro.IdCheque.HasValue)
            {
                if (cobro.Rechazado)
                {
                    await _ctx.CobrosRepo.Anular(cobro.IdCobro);
                    dato.Estado = "Anulado";
                    response.Add(dato);
                    continue;
                }
                
                if(cobro.FechaDeposito == null)
                {
                    dato.Estado = "Pendiente";
                    response.Add(dato);
                    continue;
                }
            }
            
            dato.Estado = "Cobrado";
            response.Add(dato);
        }
        
        return response;
    }
}

internal class CobrosGetAllQuery
{
    public int IdCobro { get; set; }
    public string NroViaje { get; set; }
    public DateTime FechaRecibo { get; set; }
    public decimal Monto  { get; set; }
    public int Estado { get; set; }
    public string Moneda  { get; set; }
    public float? TipoCambio { get; set; }
    public string FormaPago { get; set; }
    public int? IdCheque { get; set; }
    public DateTime? FechaDeposito { get; set; }
    public bool Rechazado { get; set; }
    public bool Anulado { get; set; }
}