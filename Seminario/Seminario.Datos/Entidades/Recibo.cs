using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Seminario.Datos.Entidades.Interfaces;

namespace Seminario.Datos.Entidades;

[Table("recibo")]
public class Recibo : IAuditable
{
    [Key]
    public int IdRecibo { get; set; }

    public TipoRecibo Tipo { get; set; }

    public DateTime FechaRecibo { get; set; }

    public decimal MontoTotal { get; set; }

    public int IdMoneda { get; set; }
    public double? TipoCambio { get; set; }

    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }

    public string? Observaciones { get; set; }
    public bool Anulado { get; set; }

    // Auditoría
    public string? UserName { get; set; }
    public DateTime? UserDateTime { get; set; }
    
    public string? UserAlta { get; set; }
    public DateTime? FechaAlta { get; set; }

    // Navegación
    public virtual Moneda Moneda { get; set; }
    public virtual Cliente? Cliente { get; set; }
    public virtual Proveedor? Proveedor { get; set; }
    public virtual Taller? Taller { get; set; }

    public virtual ICollection<ReciboFormaPago> FormasDePago { get; set; }
    public virtual ICollection<ReciboFactura> ReciboFacturas { get; set; }
    
    public enum TipoRecibo { Cobro = 1, Pago = 2 }

    public void CreatedAt(DateTime date, string user)
    {
        UserAlta = user;
        FechaAlta = date;
    }

    public void ModifiedAt(DateTime date, string user)
    {
        UserName = user;
        UserDateTime = date;
    }
}

