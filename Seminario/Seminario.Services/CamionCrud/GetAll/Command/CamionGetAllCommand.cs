namespace Seminario.Services.CamionCrud.GetAll.Command;

public class CamionGetAllCommand
{
    public int? Camion  { get; set; }
    public int? TipoCamion { get; set; }
    public string? Marca  { get; set; }
    public string? Modelo { get; set; }
}