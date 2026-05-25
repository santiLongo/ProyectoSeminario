using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.CrearFactura;

public static class FacturaCreadorResolver
{
    public static ICrearFactura Resolver(Factura.TipoFactura tipo)
    {
        return tipo switch
        {
            Factura.TipoFactura.Emitida => new CrearFacturaEmitida(new ValidarFacturaEmitida()),
            Factura.TipoFactura.Recibida => new CrearFacturaRecibida(new ValidarFacturaRecibida()),
            _ => throw new InvalidOperationException($"Tipo de factura no soportado: {tipo}")
        };
    }
}
