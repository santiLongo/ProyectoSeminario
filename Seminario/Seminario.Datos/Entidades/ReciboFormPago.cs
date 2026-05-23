using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("reciboformapago")]
public class ReciboFormaPago
{
    [Key]
    public int IdReciboFormaPago { get; set; }

    public int IdRecibo { get; set; }
    public int IdFormaPago { get; set; }
    public decimal Monto { get; set; }
    public int? IdPagoCheque { get; set; }

    public virtual Recibo Recibo { get; set; }
    public virtual FormaPago FormaPago { get; set; }
    public virtual PagoCheque? PagoCheque { get; set; }
}