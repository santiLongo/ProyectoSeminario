using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.FacturasServices.CrearFactura;

public class ValidarFacturaRecibida : IValidarFactura
{
    public async Task ValidarAsync(CrearFacturaCommand command, IAppDbContext ctx)
    {
        if (command.TipoOrigen == 1)
        {
            if (command.IdProveedor == null)
                throw new InvalidOperationException("Se debe informar el proveedor para facturas de tipo Proveedor");

            var proveedor = await ctx.ProveedorRepo.FindByIdAsync(command.IdProveedor.Value);
            if (proveedor == null)
                throw new InvalidOperationException("El proveedor informado no existe");
        }
        else if (command.TipoOrigen == 2)
        {
            if (command.IdTaller == null)
                throw new InvalidOperationException("Se debe informar el taller para facturas de tipo Taller");

            var taller = await ctx.TallerRepo.FindByIdAsync(command.IdTaller.Value);
            if (taller == null)
                throw new InvalidOperationException("El taller informado no existe");
        }
        else
        {
            throw new InvalidOperationException("TipoOrigen debe ser 1 (Proveedor) o 2 (Taller)");
        }
    }
}
