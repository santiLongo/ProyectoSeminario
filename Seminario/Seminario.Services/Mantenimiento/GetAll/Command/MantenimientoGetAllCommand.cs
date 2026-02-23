namespace Seminario.Services.Mantenimiento.GetAll.Command;

public class MantenimientoGetAllCommand
{
    public int? Estado { get; set; }
    public int? Camion { get; set; }
    public int? Taller { get; set; }
    public DateTime? FechaEntradaDesde { get; set; }
    public DateTime? FechaEntradaHasta { get; set; }
}