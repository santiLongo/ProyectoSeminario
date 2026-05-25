using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.FacturasServices.CrearFactura;

public interface IValidarFactura
{
    Task ValidarAsync(CrearFacturaCommand command, IAppDbContext ctx);
}
