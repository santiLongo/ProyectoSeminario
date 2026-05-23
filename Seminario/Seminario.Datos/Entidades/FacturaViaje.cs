namespace Seminario.Datos.Entidades;

public class FacturaViaje
{
    public int IdFacturaViaje { get; set; }

    public int IdFactura { get; set; }

    public int IdViaje { get; set; }

    public decimal MontoViaje { get; set; }


    // Navigation Properties
    public virtual Factura Factura { get; set; }

    public virtual Viaje Viaje { get; set; }
}