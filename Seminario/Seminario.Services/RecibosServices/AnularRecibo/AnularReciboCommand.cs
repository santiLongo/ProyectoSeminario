using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.RecibosServices.AnularRecibo;

public class AnularReciboCommand
{
    [Required]
    public int? IdRecibo { get; set; }
}
