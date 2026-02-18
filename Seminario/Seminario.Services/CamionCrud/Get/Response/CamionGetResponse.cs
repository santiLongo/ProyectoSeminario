namespace Seminario.Services.CamionCrud.Get.Response;

public class CamionGetResponse
{
    public int IdCamion { get; set; }
    public int IdTipoCamion { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? NroChasis { get; set; }
    public string? NroMotor { get; set; }
    public string? Patente { get; set; }
}