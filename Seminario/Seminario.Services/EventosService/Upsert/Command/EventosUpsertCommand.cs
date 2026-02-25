namespace Seminario.Services.EventosService.Upsert.Command;

public class EventosUpsertCommand
{
    public int  IdEvento { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaEvento { get; set; }
    public bool Inactivo { get; set; }
    public int IdTipoEvento { get; set; }
}