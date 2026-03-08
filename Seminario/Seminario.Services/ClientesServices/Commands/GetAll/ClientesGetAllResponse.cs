namespace Seminario.Services.ClientesServices.Commands.GetAll;

public class ClientesGetAllResponse
{
    public int IdCliente { get; set; }
    public string Cuit { get; set; }
    public string RazonSocial { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public DateTime? FechaBaja { get; set; }
    public string UserName { get; set; }
    public DateTime? UserDateTime { get; set; }
}