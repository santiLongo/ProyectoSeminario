namespace Seminario.Services.ViajeServices.GetAll.Model;

public class GetAllViajeModel
{
    public int IdViaje { get; set; }
    public string NroViaje { get; set; }
    public string Cliente { get; set; }
    public string Chofer { get; set; }
    public string Patente { get; set; }
    public string Carga { get; set; }
    public decimal Kilometros { get; set; }
    public decimal MontoTotal { get; set; }
    public string Moneda { get; set; }
    public string Estado { get; set; }
    public DateTime FechaPartida { get; set; }
    public DateTime? FechaDescarga { get; set; }
    public string UserName { get; set; } 
    public DateTime UserDateTime { get; set; } 
}