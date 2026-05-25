namespace Seminario.Services.FacturasServices.FacturasEmitidas.ConfirmarEmitida;

public class ConfirmarEmitidaCommand
{
    // Campos AFIP opcionales para confirmación manual sin pasar por AFIP
    public int? TipoComprobante { get; set; }
    public string? CAE { get; set; }
    public string? CAEFchVto { get; set; } // formato YYYYMMDD
}
