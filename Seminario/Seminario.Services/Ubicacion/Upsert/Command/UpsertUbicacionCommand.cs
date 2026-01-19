namespace Seminario.Services.Ubicacion.Upsert.Command;

public class UpsertUbicacionCommand
{
    public int? Id { get; set; }
    public string? Descripcion { get; set; }
    public int IdProvincia { get; set; }
}