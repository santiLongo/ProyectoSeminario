namespace Seminario.Services.CamionCrud.Upsert.Command;

public class UpsertCamionCommand
{
    public int idCamion  { get; set; }
    public int IdTipoCamion  { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string NroChasis { get; set; }
    public string NroMotor { get; set; }
    public string Patente { get; set; }
}