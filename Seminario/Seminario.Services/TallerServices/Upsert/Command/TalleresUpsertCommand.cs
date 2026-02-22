namespace Seminario.Services.TallerServices.Upsert.Command;

public class TalleresUpsertCommand
{
    public int? IdTaller { get; set; }
    public string Nombre { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public long? Cuit { get; set; }
    public int? Telefono { get; set; }
    public string Responsable { get; set; } = null!;
    public string Mail { get; set; } = null!;
    public string CodigoPostal { get; set; } = null!;
    public int IdLocalidad { get; set; }

    public List<UpsertEspecialidadTaller> Especialidades { get; set; } = new();
}
public class UpsertEspecialidadTaller
{
    public int IdEspecialidad { get; set; }
    public string Descripcion { get; set; } = null!;
}