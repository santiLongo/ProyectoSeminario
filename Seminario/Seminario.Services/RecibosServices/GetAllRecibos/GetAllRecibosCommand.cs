namespace Seminario.Services.RecibosServices.GetAllRecibos;

public class GetAllRecibosCommand
{
    public int? TipoRecibo { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }
    public bool? Anulado { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}
