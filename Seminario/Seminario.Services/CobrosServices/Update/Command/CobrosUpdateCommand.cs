using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.CobrosServices.Update.Command;

public class CobrosUpdateCommand
{
    public int IdCobro { get; set; }
    
    [Required(ErrorMessage = "El monto es obligatorio")]
    public decimal? Monto { get; set; }
    
    [Required(ErrorMessage = "La moneda es obligatoria")]
    public int? IdMoneda { get; set; }
    public double? TipoCambio { get; set; }
    
    [Required(ErrorMessage = "La forma de pago es obligatoria")]
    public int? IdFormaPago { get; set; }
    
    public CobroChequeCommand? DatosCheque { get; set; }
    
    public bool Seguridad { get; set; }
}

public class CobroChequeCommand
{
    public int? IdCheque { get; set; }
    public int? NroCheque { get; set; }
    public long? CuitEmisor { get; set; }
    public int? IdBanco { get; set; }
    public DateTime? FechaCobro { get; set; }
}