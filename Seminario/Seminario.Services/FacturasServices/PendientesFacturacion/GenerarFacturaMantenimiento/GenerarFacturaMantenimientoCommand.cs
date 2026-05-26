namespace Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaMantenimiento;

public class GenerarFacturaMantenimientoCommand
{
    public int PtoVenta  { get; set; }
    public int NumeroFactura { get; set; }
    public int IdMoneda { get; set; } = 1;
    public decimal PorcentajeIva { get; set; } = 21;
    public DateTime? FechaVencimiento { get; set; }
    public string? Observaciones { get; set; }
}
