namespace Seminario.Services.ProveedorServices.Get.Response;

public class ProveedoresGetResponse
{
    public int? IdProveedor { get; set; }
    public string RazonSocial { get; set; } 
    public string Direccion { get; set; }
    public long? Cuit { get; set; }
    public string Responsable { get; set; }
    public string Mail { get; set; }
    public string CodigoPostal { get; set; }
    public int IdLocalidad { get; set; }
    public List<GetEspecialidadProveedores> Especialidades { get; set; } = new();
}

public class GetEspecialidadProveedores
{
    public int IdEspecialidad { get; set; }
    public string Descripcion { get; set; }
}