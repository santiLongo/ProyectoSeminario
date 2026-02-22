namespace Seminario.Services.CobrosServices.GetAll.Command;

public class CobrosGetAllCommand
{
    public string NroViaje { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? FormaPago { get; set; }
    public int? Estado { get; set; }
}