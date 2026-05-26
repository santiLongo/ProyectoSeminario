namespace Seminario.Services.FacturasServices.FacturasRecibidas.ConfirmarRecibida;

public class ConfirmarRecibidaCommand
{
    public int? TipoComprobante { get; set; }
    public string? CAE { get; set; }
    public string? CAEFchVto { get; set; } // formato YYYYMMDD
}
