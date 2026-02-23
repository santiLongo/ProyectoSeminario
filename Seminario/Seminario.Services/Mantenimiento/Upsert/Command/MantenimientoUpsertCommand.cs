using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.Mantenimiento.Upsert.Command;

public class MantenimientoUpsertCommand
{
    public int? IdMantenimiento { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Es obligatorio ponerle un titulo al mantenimiento")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "Se tiene que informar el camion")]
    public int IdCamion { get; set; }
    [Required(ErrorMessage = "Se tiene que informar la fecha de ingreso/entrada")]
    public DateTime FechaEntrada { get; set; }
    [Required(ErrorMessage = "Se tiene que informar el taller")]
    public int IdTaller { get; set; }
    public List<MantenimientoUpsertTareas> Tareas { get; set; }
}

public class MantenimientoUpsertTareas
{
    public int IdTarea { get; set; }
    public string Descripcion { get; set; }
    public decimal? Costo { get; set; }
}