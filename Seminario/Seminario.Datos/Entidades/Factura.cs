using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Seminario.Datos.Entidades.Interfaces;

namespace Seminario.Datos.Entidades;

[Table("factura")]
public class Factura : IAuditable
{
    [Key]
    public int IdFactura { get; set; }

    public TipoFactura Tipo { get; set; } // Emitida = 1, Recibida = 2

    public int PuntoVenta { get; set; }
    public int Numero { get; set; }

    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }

    public decimal Subtotal { get; set; }
    public decimal ImporteIva { get; set; }
    public decimal Total { get; set; } // Subtotal + IVA calculado

    public int IdMoneda { get; set; }
    public decimal? TipoCambio { get; set; }

    public EstadoFactura Estado { get; set; } = EstadoFactura.Pendiente;

    public string Observaciones { get; set; }
    public bool Anulada { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }

    public string UserName { get; set; }
    public DateTime? UserDateTime { get; set; }

    public string? UserAlta { get; set; }
    public DateTime? FechaAlta { get; set; }
    
    public int? PuntoVentaReal { get; set; }
    public int? NumeroReal { get; set; }

    public virtual ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();

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

    public void RecalcularEstado(IEnumerable<ReciboFactura> recibos)
    {
        if (Anulada)
        {
            Estado = EstadoFactura.Anulada;
            return;
        }

        var totalAplicado = recibos
            .Where(r => r.Recibo != null && !r.Recibo.Anulado)
            .Sum(r => r.ImporteAplicado);

        if (totalAplicado <= 0)
            Estado = EstadoFactura.Pendiente;
        else if (totalAplicado >= Total)
            Estado = EstadoFactura.Cancelada;
        else
            Estado = EstadoFactura.PagoParcial;
    }

    public enum TipoFactura { Emitida, Recibida }
    public enum EstadoFactura { Pendiente = 1, PagoParcial = 2, Cancelada = 3, Anulada = 4 }

    public void RecalculoCabecera()
    {
        Subtotal = Detalles.Select(o => o.Subtotal).Sum();
        ImporteIva = Detalles.Select(o => o.PrecioIva).Sum();
        Total = Detalles.Select(o => o.Total).Sum();
    }
}
