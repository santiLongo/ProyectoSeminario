namespace Seminario.Services.FacturasServices.FacturasEmitidas.UpdateEmitida;

public class UpdateEmitidaCommand
{
    public DateTime? FechaVencimiento { get; set; }
    public int IdMoneda { get; set; }
    public decimal? TipoCambio { get; set; }
    public string? Observaciones { get; set; }
}
