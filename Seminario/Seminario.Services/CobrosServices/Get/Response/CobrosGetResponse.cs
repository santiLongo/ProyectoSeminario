namespace Seminario.Services.CobrosServices.Get.Response;

public class CobrosGetResponse
{
    public int IdCobro { get; set; }
    public decimal? Monto { get; set; }
    public int? IdMoneda { get; set; }
    public double? TipoCambio { get; set; }
    public int? IdFormaPago { get; set; }
    
    public DatosCobroCheque? DatosCheque { get; set; }
}

public class DatosCobroCheque
{
    public int? IdCheque { get; set; }
    public int? NroCheque { get; set; }
    public long? CuitEmisor { get; set; }
    public int? IdBanco { get; set; }
    public DateTime? FechaCobro { get; set; }
}