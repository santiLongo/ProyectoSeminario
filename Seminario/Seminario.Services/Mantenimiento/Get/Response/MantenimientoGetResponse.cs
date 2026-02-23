namespace Seminario.Services.Mantenimiento.Get.Response;

public class MantenimientoGetResponse
{
    public int IdMantenimiento { get; set; }
    public string Titulo { get; set; }
    public int? IdCamion { get; set; }
    public DateTime? FechaEntrada { get; set; }
    public int? IdTaller { get; set; }
    public List<MantenimientoGetTareas> Tareas { get; set; }
}

public class MantenimientoGetTareas
{
    public int? IdTarea { get; set; }
    public string Descripcion { get; set; }
    public decimal? Costo { get; set; }
}