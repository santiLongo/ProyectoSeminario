namespace Seminario.Services.Mantenimiento.InformarSalida.Command;

public class MantenimientoInformarSalidaCommand
{
    public int IdMantenimiento { get; set; }
    public DateTime FechaSalida { get; set; }
    public string Observacion { get; set; }
}