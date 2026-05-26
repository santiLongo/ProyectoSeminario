using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasRecibidas.UpdateRecibida;

public class UpdateRecibidaCommand
{
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public decimal PorcentajeIva { get; set; }
    public int IdMoneda { get; set; }
    public decimal? TipoCambio { get; set; }
    public string? Observaciones { get; set; }
    public List<CrearFacturaDetalleCommand> Detalles { get; set; } = new();
}
