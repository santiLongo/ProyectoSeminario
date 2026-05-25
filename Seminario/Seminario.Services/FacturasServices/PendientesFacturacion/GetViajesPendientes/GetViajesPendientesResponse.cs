namespace Seminario.Services.FacturasServices.PendientesFacturacion.GetViajesPendientes;

public class GetViajesPendientesResponse
{
    public int IdViaje { get; set; }
    public string? NroViaje { get; set; }
    public DateTime FechaPartida { get; set; }
    public DateTime? FechaDescarga { get; set; }
    public int Estado { get; set; }
    public decimal MontoTotal { get; set; }
    public int IdCliente { get; set; }
    public string? RazonSocialCliente { get; set; }
    public string? NombreChofer { get; set; }
    public string? PatenteCamion { get; set; }
}
