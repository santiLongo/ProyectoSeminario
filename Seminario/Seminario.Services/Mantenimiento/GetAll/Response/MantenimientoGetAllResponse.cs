namespace Seminario.Services.Mantenimiento.GetAll.Response;

public class MantenimientoGetAllResponse
{
    public int IdMantenimiento { get; set; }
    public string Titulo { get; set; }
    public string Camion  { get; set; }
    public DateTime? FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public string Taller { get; set; }
    public decimal? Importe { get; set; }
    public string Estado { get; set; }
}