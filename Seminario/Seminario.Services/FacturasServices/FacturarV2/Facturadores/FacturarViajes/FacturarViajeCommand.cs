using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.FacturasServices.FacturarV2.Facturadores.FacturarViajes;

public class FacturarViajeCommand
{
    public int? PuntoVentaReal { get; set; }
    public int? NumeroReal { get; set; }
    
    [Required(ErrorMessage = "La fecha de emision es requerida")]
    public DateTime FechaEmision { get; set; }
    
    [Required(ErrorMessage = "La fecha de vencimiento es requerida")]
    public DateTime FechaVencimiento { get; set; }
    
    public int Moneda { get; set; }
    public decimal TipoCambio { get; set; }
    public bool Facturada { get; set; }
    
    public int? IdCliente { get; set; }   // Viajes
    
    public List<FacturarViajeDetalle> Detalles { get; set; }
}

public class FacturarViajeDetalle
{
    public decimal Precio { get; set; }
    public decimal PorcentajeIva { get; set; }
    public bool CalcularIva { get; set; }
    public bool CalcularTotal { get; set; }
    
    public int? IdViaje { get; set; }
}