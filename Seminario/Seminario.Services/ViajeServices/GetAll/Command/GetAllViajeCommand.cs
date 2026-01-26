namespace Seminario.Services.ViajeServices.GetAll.Command;

public class GetAllViajeCommand
{
    public int? IdCamion { get; set; }
    public int? IdCliente  { get; set; }
    public int? IdChofer { get; set; }
    public int? IdLocalidadDestino { get; set; }
    public int? IdLocalidadProcedencia { get; set; }
    public string? NroViaje { get; set; }
    public DateTime? FechaAltaDesde { get; set; }
    public DateTime? FechaAltaHasta { get; set; }
    public int? Estado { get; set; }
}