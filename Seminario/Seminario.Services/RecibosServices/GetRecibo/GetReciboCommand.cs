using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.RecibosServices.GetRecibo;

public class GetReciboCommand
{
    [Required]
    public int? IdRecibo { get; set; }
}
