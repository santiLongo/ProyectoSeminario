using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.ViajeServices.Update.Command;

public class UpdateViajeCommand
{
    [Required(ErrorMessage = "El id del viaje es obligatorio" , ErrorMessageResourceType = typeof(InvalidOperationException))]
    public int IdViaje { get; set; }
    
    public decimal? Kilometros { get; set; }
    
    [Required(ErrorMessage = "Se requiere aunque sea un destino para dar de alta un viaje", ErrorMessageResourceType = typeof(InvalidOperationException))]
    public List<int> Destinos { get; set; }
    
    [Required(ErrorMessage = "Se requiere aunque sea una procedencia para dar de alta un viaje", ErrorMessageResourceType = typeof(InvalidOperationException))]
    public List<int> Procendecias { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar el monto del viaje", ErrorMessageResourceType = typeof(InvalidOperationException))]
    public decimal MontoTotal { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar el chofer", ErrorMessageResourceType = typeof(InvalidOperationException))]
    public int Chofer { get; set; }
    
    [Required(ErrorMessage = "Se requiere informar el chofer", ErrorMessageResourceType = typeof(InvalidOperationException))]
    public int Camion { get; set; }
    
    public string? Carga { get; set; }
    
    public float? Kilos { get; set; }
}