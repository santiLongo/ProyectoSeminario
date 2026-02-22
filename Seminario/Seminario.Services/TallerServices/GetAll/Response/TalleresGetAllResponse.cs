namespace Seminario.Services.TallerServices.GetAll.Response;

public class TalleresGetAllResponse
{
    public int IdTaller { get; set; }
    public string Nombre { get; set; }
    public long? Cuit  { get; set; }
    public int? Telefono { get; set; }
    public string Localidad { get; set; }
    public string Provincia { get; set; }
}