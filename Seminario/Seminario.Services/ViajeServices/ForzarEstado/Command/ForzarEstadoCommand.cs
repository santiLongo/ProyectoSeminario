using Seminario.Datos;

namespace Seminario.Services.ViajeServices.ForzarEstado.Command;

public class ForzarEstadoCommand
{
    public int Estado { get; set; }
    public int IdViaje { get; set; }
    public bool Seguridad { get; set; }
    public string? Observacion { get; set; }
}