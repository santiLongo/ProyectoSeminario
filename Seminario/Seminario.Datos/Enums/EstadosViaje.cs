namespace Seminario.Datos;

public enum EstadosViaje
{
    EnViaje = 1,
    Finalizado = 2,
    Suspendido = 3,
    Cobrado = 4,
}

public static class EstadosViajeDiccionary
{
    public static Dictionary<int, string> Estados = new Dictionary<int, string>
    {
        { EstadosViaje.EnViaje.ToInt(), "En Viaje" },
        { EstadosViaje.Finalizado.ToInt(), "Finalizado" },
        { EstadosViaje.Suspendido.ToInt(), "Suspendido" },
        { EstadosViaje.Cobrado.ToInt(), "Cobrado" }
    };
}

public static class EstadosViajeExtensions
{
    public static int ToInt(this EstadosViaje estado) => Convert.ToInt32(estado);
    
    public static bool ExisteEstadoViaje(this int estado) 
    {
        return Enum.IsDefined(typeof(EstadosViaje), estado);
    }
}