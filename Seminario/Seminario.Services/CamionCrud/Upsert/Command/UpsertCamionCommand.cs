using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.CamionCrud.Upsert.Command;

public class UpsertCamionCommand
{
    public int idCamion  { get; set; }
    public int IdTipoCamion  { get; set; }
    [MaxLength(20, ErrorMessage = "La marca no debe superar los 20 caracteres")]
    public string Marca { get; set; }
    
    [MaxLength(20, ErrorMessage = "El model no debe superar los 20 caracteres")]
    public string Modelo { get; set; }
    
    [MaxLength(50, ErrorMessage = "El Nro. Chasis no debe superar los 50 caracteres")]
    public string NroChasis { get; set; }
    
    [MaxLength(50, ErrorMessage = "El Nro. Motor no debe superar los 50 caracteres")]
    public string NroMotor { get; set; }
    
    [MaxLength(10, ErrorMessage = "La pantente no debe superar los 10 caracteres")]
    public string Patente { get; set; }
}