using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.ViajeServices.CargarDescarga.Command;

public class CargarDescargaViajeCommand
{
    [Required(ErrorMessage = "Falta el id del viaje")]
    public int IdViaje { get; set; }
    
    public DateTime? FechaDescarga { get; set; }
}