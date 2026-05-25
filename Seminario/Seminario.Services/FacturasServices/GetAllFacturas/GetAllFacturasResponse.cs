using System.Text.Json.Serialization;

namespace Seminario.Services.FacturasServices.GetAllFacturas;

public class GetAllFacturasResponse
{
    public int IdFactura { get; set; }
    public int Tipo { get; set; }
    public int PuntoVenta { get; set; }
    public int Numero { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public decimal Total { get; set; }
    public int Estado { get; set; }
    public bool Anulada { get; set; }
    public bool Confirmada { get; set; }
    [JsonPropertyName("cae")]
    public string? CAE { get; set; }
    public string? NombreContraparte { get; set; }
    public decimal SaldoPendiente { get; set; }
}
