namespace Seminario.Services.Ubicacion.GetAll.Response;

public class UbicacionGetAllResponse
{
    public int IdLocalidad { get; set; }
    public string? Localidad { get; set; }
    public string? Provincia { get; set; }
    public string? Pais { get; set; }
    public string? UserName { get; set; }
    public DateTime? UserDateTime { get; set; }
}