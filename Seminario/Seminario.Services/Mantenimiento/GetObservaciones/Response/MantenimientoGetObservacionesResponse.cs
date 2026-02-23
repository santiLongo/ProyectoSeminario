namespace Seminario.Services.Mantenimiento.GetObservaciones.Response;

public class MantenimientoGetObservacionesResponse
{
    public int IdObservacion { get; set; }
    public int IdMantenimiento { get; set; }
    public string Descripcion { get; set; }
    public DateTime UserDateTime { get; set; }
    public string UserName { get; set; }
}