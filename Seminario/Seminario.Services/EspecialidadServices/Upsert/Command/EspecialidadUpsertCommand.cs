using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.EspecialidadServices.Upsert.Command;

public class EspecialidadUpsertCommand
{
    public int? IdEspecialidad { get; set; }
    [Required(ErrorMessage =  "La especialidad debe tener una descripcion", AllowEmptyStrings = false)]
    public string Descripcion { get; set; }
}