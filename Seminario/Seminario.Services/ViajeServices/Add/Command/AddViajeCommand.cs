using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.ViajeServices.Add.Command;

public class AddViajeCommand
{
    [Required(ErrorMessage = "Se requiere informar el cliente del viaje")]
    public int? Cliente { get; set; }
    
    public decimal? Kilometros { get; set; }
    
    [Required(ErrorMessage = "Se requiere aunque sea un destino para dar de alta un viaje")]
    public List<int>? Destinos { get; set; }
    
    [Required(ErrorMessage = "Se requiere aunque sea una procedencia para dar de alta un viaje")]
    public List<int>? Procendecias { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar el monto del viaje")]
    public decimal? MontoTotal { get; set; }
    
    [Required(ErrorMessage = "La moneda es requerida")]
    public int? IdMoneda { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar la fecha de partida")]
    public DateTime? FechaPartida { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar el chofer")]
    public int? Chofer { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar el chofer")]
    public int? Camion { get; set; }
    
    public int? Semirremolque { get; set; }
    
    public string? Carga { get; set; }
    
    public float? Kilos { get; set; }
}

