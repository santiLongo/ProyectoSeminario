using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("facturamantenimiento")]
public class FacturaMantenimiento
{
    public int IdFacturaMantenimiento { get; set; }

    public int IdFactura { get; set; }

    public int IdMantenimiento { get; set; }

    public decimal ImporteMantenimiento { get; set; }


    // Navigation Properties
    public virtual Factura Factura { get; set; }

    public virtual Mantenimiento Mantenimiento { get; set; }
}