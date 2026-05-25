using System.ComponentModel.DataAnnotations;
using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasEmitidas.CrearEmitidaConViaje;

public class CrearEmitidaConViajeCommand
{
    [Required(ErrorMessage = "El cliente es requerido")]
    public int? IdCliente { get; set; }

    [Required(ErrorMessage = "Debe asociar al menos un viaje")]
    public List<CrearFacturaViajeCommand> Viajes { get; set; } = new();

    [Required(ErrorMessage = "La fecha de emisión es requerida")]
    public DateTime? FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    [Required(ErrorMessage = "El porcentaje de IVA es requerido")]
    public decimal? PorcentajeIva { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    public int? IdMoneda { get; set; }

    public decimal? TipoCambio { get; set; }
    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "Debe informar al menos un detalle")]
    public List<CrearFacturaDetalleCommand> Detalles { get; set; } = new();
}
