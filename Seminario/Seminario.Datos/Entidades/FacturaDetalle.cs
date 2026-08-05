using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("facturadetalle")]
public class FacturaDetalle
{
    [Key]
    public int IdFacturaDetalle { get; set; }

    public int IdFactura { get; set; }

    public int Orden { get; set; }

    public string Descripcion { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal PorcentajeIva { get; set; }

    public decimal PrecioIva { get; set; }
    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public int? IdViaje { get; set; }
    public int? IdMantenimiento { get; set; }
    public int? IdCompraRepuesto { get; set; }

    public virtual Factura Factura { get; set; }
}
