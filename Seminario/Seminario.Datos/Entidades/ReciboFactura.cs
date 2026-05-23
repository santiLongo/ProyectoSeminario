using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("recibofactura")]
public class ReciboFactura
{
    [Key]
    public int IdReciboFactura { get; set; }

    public int IdRecibo { get; set; }
    public int IdFactura { get; set; }
    public decimal ImporteAplicado { get; set; }

    public virtual Recibo Recibo { get; set; }
    public virtual Factura Factura { get; set; }
}