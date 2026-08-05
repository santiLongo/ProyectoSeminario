using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("Gastos")]
public class Gasto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdGasto { get; set; }

    public DateTime Fecha { get; set; }
    public decimal Importe { get; set; }
    public string Descripcion { get; set; }
    public int IdCategoria { get; set; }

    public EstadoComprobante Estado { get; set; } = EstadoComprobante.Pendiente;
    public TipoComprobante Tipo { get; set; }
    
    public int? IdFactura { get; set; } 
}

public enum EstadoComprobante
{
    SinComprobante = 1,
    Pendiente      = 2,
    Documentado    = 3
}

public enum TipoComprobante
{
    Factura = 1,
    Recibo = 2
}