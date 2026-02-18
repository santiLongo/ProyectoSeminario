namespace Seminario.Services.CamionCrud.GetAll.Response;

public class CamionGetAllResponse
{
    public int Id { get; set; }
    public string? Patente { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? NroChasis { get; set; }
    public string? NroMotor { get; set; }
    public string? TipoCamion { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }
    public DateTime? UltimoMantenimiento { get; set; }
    public DateTime? UltimoViaje { get; set; }
}