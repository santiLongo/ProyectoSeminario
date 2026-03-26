using Dapper;
using Seminario.Datos.Dapper;
using Seminario.Datos.ExtensionMethods;
using Seminario.Datos.StoredProcedures;

namespace Seminario.Services.ChequesServices.Commands.GetAll
{
    public class ChequesGetAllHandler
    {
        private readonly IDbExecutor _executor;

        public ChequesGetAllHandler(IDbExecutor executor)
        {
            _executor = executor;
        }

        public async Task<List<ChequesGetAllResponse>> HandleAsync(ChequesGetAllCommand command)
        {
            var p = BuiildParameters(command);

            var sql = Querys.GetAllCheques;

            var response = await _executor.ExecuteAsync<ChequesGetAllResponse>(sql, p);

            foreach (var item in response) 
            {
                if (!item.IdMantenimiento.IsNullOrZero())
                {
                    item.Pago = item.MantenimientoDesc;
                    continue;
                }

                if (!item.IdCompraRepuesto.IsNullOrZero())
                {
                    item.Pago = "Compra de Repuesto";
                    continue;
                }

                item.Pago = "No Tiene";
            }

            return response.ToList();
        }

        private DynamicParameters BuiildParameters(ChequesGetAllCommand command)
        {
            var p = new DynamicParameters();

            p.Add("@soloPropios", command.SoloPropios);

            switch (command.Estado)
            {
                case EstadosCheques.ParaCobrar:
                    p.Add("@estado", EstadosCheques.ParaCobrar);
                    p.Add("@hoy", DateTime.Today);
                    break;
                case EstadosCheques.PorCobrar:
                    p.Add("@estado", EstadosCheques.PorCobrar);
                    p.Add("@cobroDesde", command.FechaCobroDesde);
                    p.Add("@cobroHasta", command.FechaCobroHasta);
                    break;
                case EstadosCheques.Cobrados:
                    p.Add("@estado", EstadosCheques.Cobrados);
                    break;
                case EstadosCheques.Rechazados:
                    p.Add("@estado", EstadosCheques.Rechazados);
                    break;
                case EstadosCheques.Todos:
                default:
                    p.Add("@estado", EstadosCheques.Todos);
                    p.Add("@cobroDesde", command.FechaCobroDesde);
                    p.Add("@cobroHasta", command.FechaCobroHasta);
                    break;
            }

            return p;
        }
    }
}
