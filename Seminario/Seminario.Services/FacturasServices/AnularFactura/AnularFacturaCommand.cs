using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.FacturasServices.AnularFactura;

public class AnularFacturaCommand
{
    [Required]
    public int? IdFactura { get; set; }
}
