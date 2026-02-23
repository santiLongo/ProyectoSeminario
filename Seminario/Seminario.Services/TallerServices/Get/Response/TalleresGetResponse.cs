namespace Seminario.Services.TallerServices.Get.Response;

public class TalleresGetResponse
{
    public int? IdTaller { get; set; }
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public long? Cuit { get; set; }
    public int? Telefono { get; set; }
    public string Responsable { get; set; }
    public string Mail { get; set; }
    public string CodigoPostal { get; set; }
    public int IdLocalidad { get; set; }

    public List<GetEspecialidadTaller> Especialidades { get; set; } = new();
}

public class GetEspecialidadTaller
{
    public int IdEspecialidad { get; set; }
    public string Descripcion { get; set; }
}