using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("facturaviaje")]
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