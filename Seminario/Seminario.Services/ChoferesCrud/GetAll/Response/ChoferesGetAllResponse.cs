namespace Seminario.Services.ChoferesCrud.GetAll.Response;

public class ChoferesGetAllResponse
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Dni { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? NroRegistro { get; set; }
    public DateTime? UltimoViaje { get; set; }
    public DateTime? FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }
}