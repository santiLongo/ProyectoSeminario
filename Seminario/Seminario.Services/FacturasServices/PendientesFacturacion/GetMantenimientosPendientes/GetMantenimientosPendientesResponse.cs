namespace Seminario.Services.FacturasServices.PendientesFacturacion.GetMantenimientosPendientes;

public class GetMantenimientosPendientesResponse
{
    public int IdMantenimiento { get; set; }
    public string Titulo { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public decimal? Importe { get; set; }
    public int IdTaller { get; set; }
    public string? NombreTaller { get; set; }
    public string? PatenteCamion { get; set; }
}
