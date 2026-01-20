namespace Seminario.Services.ChoferesCrud.Upsert.Command;

public class UpsertChoferCommand
{
    public int? IdChofer { get; set; }
    
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public int Telefono { get; set; }
    public long NroRegistro { get; set; }
    public int Dni { get; set; }
}