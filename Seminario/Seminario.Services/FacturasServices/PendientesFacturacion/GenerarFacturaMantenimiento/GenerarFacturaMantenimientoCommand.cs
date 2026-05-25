using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaMantenimiento;

public class GenerarFacturaMantenimientoCommand
{
    [Required(ErrorMessage = "El punto de venta es requerido")]
    public int? PuntoVenta { get; set; }

    [Required(ErrorMessage = "El número de comprobante es requerido")]
    public int? Numero { get; set; }

    public int IdMoneda { get; set; } = 1;
    public decimal PorcentajeIva { get; set; } = 21;
    public DateTime? FechaVencimiento { get; set; }
    public string? Observaciones { get; set; }
}
