using System.ComponentModel.DataAnnotations;
using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasRecibidas.CrearRecibida;

public class CrearRecibidaCommand
{
    // Contraparte: proveedor o taller (al menos uno requerido)
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }

    [Required(ErrorMessage = "La fecha de emisión es requerida")]
    public DateTime? FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    [Required(ErrorMessage = "El porcentaje de IVA es requerido")]
    public decimal? PorcentajeIva { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    public int? IdMoneda { get; set; }

    public decimal? TipoCambio { get; set; }
    public string? Observaciones { get; set; }

    // Asociaciones opcionales
    public List<int> IdsMantenimiento { get; set; } = new();
    public List<int> IdsCompraRepuesto { get; set; } = new();

    [Required(ErrorMessage = "Debe informar al menos un detalle")]
    public List<CrearFacturaDetalleCommand> Detalles { get; set; } = new();
}
