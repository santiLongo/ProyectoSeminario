namespace Seminario.Services.ProveedorServices.GetAll.Response;

public class ProveedoresGetAllResponse
{
    public int IdProveedor { get; set; }
    public string RazonSocial { get; set; }
    public long? Cuit  { get; set; }
    public string Responsable { get; set; }
    public string Direccion { get; set; }
    public string Localidad { get; set; }
    public string Provincia { get; set; }
}