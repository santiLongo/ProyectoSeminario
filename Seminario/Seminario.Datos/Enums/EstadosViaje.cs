namespace Seminario.Datos;

public enum EstadosViaje
{
    EnViaje = 1,
    Finalizado = 2,
    Suspendido = 3,
    Cobrado = 4,
}

public static class EstadosViajeExtensions
{
    public static int ToInt(this EstadosViaje estado) => Convert.ToInt32(estado);
    
    public static bool ExisteEstadoViaje(this int estado) 
    {
        return Enum.IsDefined(typeof(EstadosViaje), estado);
    }
}