using System.Data;
using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Services.Dashboard.GetHome.Model;

namespace Seminario.Services.Dashboard.Repositorio.DashboardRepo
{
    public interface IDashboardRepo
    {
        Task<Cabezera> GetCabezera();
        Task<Alertas> GetAlertas();
        Task<Cards> GetCards();
        Task<List<Viaje>> GetViajes();
        Task<List<Mantenimiento>> GetMantenimientos();
        Task<Finanzas> GetFinanzas();
    }
    public class DashboardRepo(IDbConnection connection) : IDashboardRepo
    {
        private readonly IDbConnection _connection = connection;
        public async Task<Cabezera> GetCabezera()
        {
            var sql = @"
                        SELECT 
                            -- Viajes activos
                            (SELECT COUNT(*) 
                             FROM viaje vi 
                             WHERE vi.FechaPartida IS NOT NULL 
                               AND vi.FechaDescarga IS NULL
                            ) AS ViajesActivos,

                            -- Camiones en mantenimiento
                            (SELECT COUNT(*) 
                             FROM camion cam 
                             INNER JOIN mantenimiento man ON man.idVehiculo = cam.idCamion 
                             WHERE man.fechaSalida IS NULL
                            ) AS CamionesEnMantenimiento,

                            -- Saldo mensual
                            (
                                -- Total cobros del mes
                                (SELECT IFNULL(SUM(c.Monto), 0)
                                 FROM cobro c
                                 WHERE MONTH(c.FechaRecibo) = MONTH(CURDATE()) 
                                   AND YEAR(c.FechaRecibo) = YEAR(CURDATE())
                                )
                                -
                                -- Total gastos del mes
                                (
                                    (SELECT IFNULL(SUM(rep.importeAplicado), 0)
                                     FROM pago p
                                     INNER JOIN `pago/comprarepuesto` rep ON rep.idPago = p.idPago
                                     WHERE MONTH(p.fechaPago) = MONTH(CURDATE()) 
                                       AND YEAR(p.fechaPago) = YEAR(CURDATE())
                                    )
                                    +
                                    (SELECT IFNULL(SUM(mant.importeAplicado), 0)
                                     FROM pago p
                                     INNER JOIN `pago/mantenimiento` mant ON mant.idPago = p.idPago
                                     WHERE MONTH(p.fechaPago) = MONTH(CURDATE()) 
                                       AND YEAR(p.fechaPago) = YEAR(CURDATE())
                                    )
                                )
                            ) AS SaldoMensual;";
            //
            return await _connection.ExecuteScalarAsync<Cabezera>(sql);
        }

        public Task<Alertas> GetAlertas()
        {
            throw new NotImplementedException();
        }

        public Task<Cards> GetCards()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Viaje>> GetViajes()
        {
            var sql = @"
                            SELECT 
	                            idViaje 			IdViaje,
                                dest.descripcion 	Destino,
                                camion.Patente		Camion
                            FROM viaje
                            INNER JOIN ubicacion dest ON dest.idUbicacion = viaje.idDestino
                            INNER JOIN camion camion ON camion.idCamion = viaje.idCamion
                            WHERE viaje.FechaDescarga IS NOT NULL
                            ORDER BY viaje.FechaDescarga DESC";
            var data = await _connection.QueryAsync<Viaje>(sql);

            return data.ToList();
        }

        public async Task<List<Mantenimiento>> GetMantenimientos()
        {
            var sql = @"SELECT 
	                        mant.idMantenimiento	IdManteniemiento,
                            camion.Patente			Camion,
                            taller.nombre			Taller
                        FROM mantenimiento mant
                        INNER JOIN camion ON camion.idCamion = mant.idVehiculo
                        INNER JOIN taller ON taller.idTaller = mant.idTaller
                        WHERE mant.fechaSalida IS NULL";

            var data = await _connection.QueryAsync<Mantenimiento>(sql);

            return data.ToList();
        }

        public Task<Finanzas> GetFinanzas()
        {
            var sql = @"
                        SELECT
                            (SELECT
                                SUM(via.MontoTotal) - SUM(cob.Monto) PendienteCobrar	
                            FROM cobro cob
                            INNER JOIN viaje via ON via.idViaje = cob.idCobro) AS PendienteCobrar,
                            (SELECT
                                SUM(pago.monto) ChequesPorPagar	
                            FROM pago
                            INNER JOIN pagocheque cheque ON cheque.idPago = pago.idPago
                            WHERE cheque.fechaDeposito < DATE_ADD(CURDATE(), INTERVAL 7 DAY)) as ChequesPorPagar,
                            (SELECT
                                SUM(cobro.Monto) ChequesPorCobrar	
                            FROM cobro
                            INNER JOIN cobrocheque cheque ON cheque.idCobro = cobro.idCobro
                            WHERE cheque.fechaDepositado < DATE_ADD(CURDATE(), INTERVAL 7 DAY)) as ChequesPorCobrar";

            throw new NotImplementedException();
        }
    }
}
