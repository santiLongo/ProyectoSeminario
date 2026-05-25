using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.CrearFactura;

public static class FacturaCalculos
{
    public static (decimal Subtotal, decimal TotalConIva, List<FacturaDetalle> Detalles) CalcularDetalles(
        List<CrearFacturaDetalleCommand> detallesCmd)
    {
        decimal subtotal = 0;
        decimal totalConIva = 0;
        int orden = 1;
        var detalles = new List<FacturaDetalle>();

        foreach (var dCmd in detallesCmd)
        {
            var subtotalDetalle = dCmd.Cantidad!.Value * dCmd.PrecioUnitario!.Value;
            var ivaDetalle = subtotalDetalle * (dCmd.PorcentajeIva / 100m);
            var totalDetalle = subtotalDetalle + ivaDetalle;

            subtotal += subtotalDetalle;
            totalConIva += totalDetalle;

            detalles.Add(new FacturaDetalle
            {
                Orden = orden++,
                Descripcion = dCmd.Descripcion,
                Cantidad = dCmd.Cantidad.Value,
                PrecioUnitario = dCmd.PrecioUnitario.Value,
                PorcentajeIva = dCmd.PorcentajeIva,
                Subtotal = subtotalDetalle,
                Total = totalDetalle
            });
        }

        return (subtotal, totalConIva, detalles);
    }
}
