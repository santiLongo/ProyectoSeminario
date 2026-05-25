using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.RecibosServices.AddRecibo;

public class AddReciboCommand
{
    /// <summary>1 = Cobro, 2 = Pago</summary>
    [Required(ErrorMessage = "El tipo de recibo es requerido")]
    public int? TipoRecibo { get; set; }

    [Required(ErrorMessage = "La fecha del recibo es requerida")]
    public DateTime? FechaRecibo { get; set; }

    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    public int? IdMoneda { get; set; }

    public decimal? TipoCambio { get; set; }

    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "Debe informar al menos una forma de pago")]
    public List<AddReciboFormaPagoCommand> FormasDePago { get; set; } = new();

    /// <summary>Lista de imputaciones a facturas. Puede estar vacía para anticipos.</summary>
    public List<AddReciboImputacionCommand> Imputaciones { get; set; } = new();
}

public class AddReciboFormaPagoCommand
{
    [Required]
    public int? IdFormaPago { get; set; }

    [Required]
    public decimal? Monto { get; set; }

    public AddReciboChequeCommand? DatosCheque { get; set; }
}

public class AddReciboChequeCommand
{
    public int? NroCheque { get; set; }
    public long? CuitEmisor { get; set; }
    public int? IdBanco { get; set; }
    public DateTime? FechaCobro { get; set; }
    public bool EsPropio { get; set; }
}

public class AddReciboImputacionCommand
{
    [Required]
    public int? IdFactura { get; set; }

    [Required]
    public decimal? ImporteAplicado { get; set; }
}
