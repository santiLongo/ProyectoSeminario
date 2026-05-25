using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.FacturasServices.CrearFactura;

public interface ICrearFactura
{
    Task<int> CrearAsync(CrearFacturaCommand command, IAppDbContext ctx);
}
