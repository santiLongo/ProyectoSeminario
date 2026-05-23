using System.ComponentModel.DataAnnotations;
using Seminario.Datos.Entidades.Interfaces;

namespace Seminario.Datos.Entidades;

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
    public double? TipoCambio { get; set; }

    public EstadoFactura Estado { get; set; } // Pendiente, PagoParcial, Cancelada, Anulada

    public string? Observaciones { get; set; }
    public bool Anulada { get; set; }

    public string? UserName { get; set; }
    public DateTime? UserDateTime { get; set; }
    
    public string? UserAlta { get; set; }
    public DateTime? FechaAlta { get; set; }

    public virtual Moneda Moneda { get; set; }

    public virtual ICollection<FacturaDetalle> Detalles { get; set; }

    public void CreatedAt(DateTime date, string user)
    {
        UserName = user;
        FechaEmision = date;
    }

    public void ModifiedAt(DateTime date, string user)
    {
        UserName = user;
        FechaEmision = date;
    }
    
    public enum TipoFactura  { Emitida = 1, Recibida = 2 }
    public enum EstadoFactura { Pendiente = 1, PagoParcial = 2, Cancelada = 3, Anulada = 4 }
}