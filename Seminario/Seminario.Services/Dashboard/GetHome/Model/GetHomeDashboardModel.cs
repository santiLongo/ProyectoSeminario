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
    public List<Viaje> Viajes { get; set; }
    public List<Mantenimiento> MantenimientosActivos { get; set; }
    public Finanzas Finanazas { get; set; }
}

public class Viaje
{
    public int IdViaje { get; set; }
    public string Destino { get; set; }
    public string Camion { get; set; }
}

public class Mantenimiento
{
    public int IdMantenimiento { get; set; }
    public string Camion { get; set; }
    public string Taller { get; set; }
}

public class Finanzas
{
    public decimal CobrosPendientes { get; set; }
    public decimal PagosProximos { get; set; }
}