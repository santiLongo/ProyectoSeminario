using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.FacturasServices.GetFactura;

public class GetFacturaCommand
{
    [Required]
    public int? IdFactura { get; set; }
}
