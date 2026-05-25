using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.FacturasServices.EnviarAfip;

public class EnviarFacturaAfipCommand
{
    [Required]
    public int IdFactura { get; set; }

    /// <summary>Tipo AFIP: 1=Factura A, 6=Factura B, 11=Factura C</summary>
    [Required]
    public int TipoComprobante { get; set; }

    /// <summary>1=Productos, 2=Servicios, 3=Productos y Servicios</summary>
    [Required]
    public int Concepto { get; set; }

    /// <summary>Tipo doc del receptor: 80=CUIT, 86=CUIL, 96=DNI, 99=Consumidor Final</summary>
    public int DocTipoReceptor { get; set; } = 80;

    /// <summary>Obligatorio si Concepto = 2 o 3. Formato YYYYMMDD.</summary>
    public string FechaServicioDesde { get; set; }

    public string FechaServicioHasta { get; set; }

    public string FechaVencimientoPago { get; set; }

    /// <summary>Condición IVA del receptor según RG 5616.</summary>
    public int? CondicionIVAReceptorId { get; set; }
}
