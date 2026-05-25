using System.ComponentModel.DataAnnotations;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.CrearFactura;

public class CrearFacturaCommand
{
    [Required(ErrorMessage = "El tipo de factura es requerido")]
    public Factura.TipoFactura? Tipo { get; set; }

    // Emitida
    public int? IdCliente { get; set; }
    public List<CrearFacturaViajeCommand> Viajes { get; set; } = new();

    // Recibida
    public int? TipoOrigen { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }
    public int? PuntoVenta { get; set; }
    public int? Numero { get; set; }
    public List<int> IdsMantenimiento { get; set; } = new();
    public List<int> IdsCompraRepuesto { get; set; } = new();

    // Comunes
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

public class CrearFacturaDetalleCommand
{
    [Required]
    public string Descripcion { get; set; }

    [Required]
    public decimal? Cantidad { get; set; }

    [Required]
    public decimal? PrecioUnitario { get; set; }

    public decimal PorcentajeIva { get; set; }
}

public class CrearFacturaViajeCommand
{
    [Required]
    public int? IdViaje { get; set; }

    [Required]
    public decimal? MontoViaje { get; set; }
}
