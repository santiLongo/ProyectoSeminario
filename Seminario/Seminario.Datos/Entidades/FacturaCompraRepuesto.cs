using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("facturacomprarepuesto")]
public class FacturaCompraRepuesto
{
    public int IdFacturaCompraRepuesto { get; set; }

    public int IdFactura { get; set; }

    public int IdCompraRepuesto { get; set; }

    public decimal ImporteCompra { get; set; }


    // Navigation Properties
    public virtual Factura Factura { get; set; }

    public virtual CompraRepuesto CompraRepuesto { get; set; }
}