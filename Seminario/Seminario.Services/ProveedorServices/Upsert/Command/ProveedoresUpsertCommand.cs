namespace Seminario.Services.ProveedorServices.Upsert.Command;

public class ProveedoresUpsertCommand
{
    public int? IdProveedor { get; set; }
    public string RazonSocial { get; set; }
    public string Direccion { get; set; }
    public long? Cuit { get; set; }
    public string Responsable { get; set; }
    public string Mail { get; set; }
    public string CodigoPostal { get; set; }
    public int IdLocalidad { get; set; }
    public List<UpsertEspecialidadProveedor> Especialidades { get; set; } = new();
}
public class UpsertEspecialidadProveedor
{
    public int IdEspecialidad { get; set; }
    public string Descripcion { get; set; } = null!;
}