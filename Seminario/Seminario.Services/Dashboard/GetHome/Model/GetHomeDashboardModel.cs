namespace Seminario.Services.Dashboard.GetHome.Model;
public class GetHomeDashboardModel
{
    public Cabezera Cabezera { get; set; }
}

public class Cabezera
{
    public int ViajesActivos { get; set; }
    public int ViajesSuspendidos { get; set; }
    public int CamionesEnMantenimiento { get; set; }
    public decimal SaldoMensual { get; set; }
}

public class Alertas
{
    public int ChequesAVencer { get; set; }
    public int ChequesACobrar { get; set; }
    public int CobrosPendientes { get; set; }
}

public class Cards
{
    public List<ViajeDashboard> Viajes { get; set; }
    public List<MantenimientoDashboard> MantenimientosActivos { get; set; }
    public FinanzasDashboard Finanazas { get; set; }
}

public class ViajeDashboard
{
    public int IdViaje { get; set; }
    public string Destino { get; set; }
    public string Camion { get; set; }
}

public class MantenimientoDashboard
{
    public int IdMantenimiento { get; set; }
    public string Camion { get; set; }
    public string Taller { get; set; }
}

public class FinanzasDashboard
{
    public decimal CobrosPendientes { get; set; }
    public decimal PagosProximos { get; set; }
}