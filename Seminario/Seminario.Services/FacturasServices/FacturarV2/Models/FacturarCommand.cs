using System.ComponentModel.DataAnnotations;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.FacturarV2.Models;

public class FacturarCommand
{
    public TipoFacturaV2 Tipo { get; set; }
    
    public FacturaModel Cabecera { get; set; }
}

public class FacturaModel
{
    public Factura.TipoFactura Tipo { get; set; }
    public int? PuntoVentaReal { get; set; }
    public int? NumeroReal { get; set; }
    
    [Required(ErrorMessage = "La fecha de emision es requerida")]
    public DateTime FechaEmision { get; set; }
    
    [Required(ErrorMessage = "La fecha de vencimiento es requerida")]
    public DateTime FechaVencimiento { get; set; }
    
    public int Moneda { get; set; }
    public decimal TipoCambio { get; set; }
    
    public int? IdCliente { get; set; }   // Viajes
    public int? IdProveedor { get; set; } // Repuestos / Gasto
    public int? IdTaller { get; set; }    // Mantenimientos
    
    public List<FacturaDetalleModel> Detalles { get; set; }
}

public class FacturaDetalleModel
{
    public Concepto Concepto { get; set; }
    public string Descripcion { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal PorcentajeIva { get; set; }
    public bool CalcularIva { get; set; }
    public bool CalcularTotal { get; set; }
    
    public int? IdViaje { get; set; }
    public int? IdMantenimiento { get; set; }
    public int? IdCompra { get; set; }
}

public enum TipoFacturaV2
{
    Viajes,
    Mantenimientos,
    Provedores,
    Gasto
}

public enum Concepto
{
    Producto = 1,
    Servicio = 2,
    ProductosSevicio = 3
}