namespace Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaViaje;

public class GenerarFacturaViajeCommand
{
    public int IdMoneda { get; set; } = 1;
    public decimal PorcentajeIva { get; set; } = 21;
    public DateTime? FechaVencimiento { get; set; }
    public string? Observaciones { get; set; }
}
