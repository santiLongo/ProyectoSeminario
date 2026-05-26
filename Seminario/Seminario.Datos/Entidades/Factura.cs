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
    public decimal PorcentajeIva { get; set; } // 0, 10.5, 21, etc.
    public decimal Total { get; set; } // Subtotal + IVA calculado

    public int IdMoneda { get; set; }
    public decimal? TipoCambio { get; set; }

    public EstadoFactura Estado { get; set; } = EstadoFactura.Pendiente;

    public string? Observaciones { get; set; }
    public bool Anulada { get; set; }

    // Campos AFIP — se completan cuando se autoriza el comprobante
    public int? TipoComprobante { get; set; } // 1=FA, 6=FB, 11=FC
    public string? CAE { get; set; }
    public string? CAEFchVto { get; set; } // formato YYYYMMDD

    // Para emitidas: true cuando tiene CAE de AFIP o fue confirmada manualmente
    public bool Confirmada { get; set; } = false;

    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }

    public string? UserName { get; set; }
    public DateTime? UserDateTime { get; set; }

    public string? UserAlta { get; set; }
    public DateTime? FechaAlta { get; set; }
    
    public int? PuntoVentaReal { get; set; }
    public int? NumeroReal { get; set; }

    // Navigation Properties
    public virtual Moneda Moneda { get; set; }
    public virtual Cliente? Cliente { get; set; }
    public virtual Proveedor? Proveedor { get; set; }
    public virtual Taller? Taller { get; set; }

    public virtual ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    public virtual ICollection<FacturaViaje> FacturasViaje { get; set; } = new List<FacturaViaje>();
    public virtual ICollection<FacturaMantenimiento> FacturasMantenimiento { get; set; } = new List<FacturaMantenimiento>();
    public virtual ICollection<FacturaCompraRepuesto> FacturasCompraRepuesto { get; set; } = new List<FacturaCompraRepuesto>();
    public virtual ICollection<ReciboFactura> ReciboFacturas { get; set; } = new List<ReciboFactura>();

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

    public enum TipoFactura { Emitida = 1, Recibida = 2 }
    public enum EstadoFactura { Pendiente = 1, PagoParcial = 2, Cancelada = 3, Anulada = 4 }
}
