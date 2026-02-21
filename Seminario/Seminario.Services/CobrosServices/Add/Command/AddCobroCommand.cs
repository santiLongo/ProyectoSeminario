using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.CobrosServices.Add.Command;

public class AddCobroCommand
{
    [Required(ErrorMessage = "El viaje es obligatorio")]
    public int? IdViaje { get; set; }
    [Required(ErrorMessage = "La fecha de cuando recibio el cobro es obligatorio")]
    public DateTime? FechaRecibido { get; set; }
    [Required(ErrorMessage = "El monto es obligatorio")]
    public decimal? Monto { get; set; }
    [Required(ErrorMessage = "La moneda es obligatoria")]
    public int? IdMoneda { get; set; }
    public double? TipoCambio { get; set; }
    [Required(ErrorMessage = "La forma de pago es obligatoria")]
    public int? IdFormaPago { get; set; }
    public CobroChequeCommand? DatosCheque { get; set; }
}

public class CobroChequeCommand
{
    public int? NroCheque { get; set; }
    public long? CuitEmisor { get; set; }
    public int? IdBanco { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaDeposito { get; set; }
}