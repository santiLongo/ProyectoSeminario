using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.ClientesServices.Models;

public class ClienteFormModel
{
    public int? IdCliente { get; set; }
    [Required(ErrorMessage = "El cuit es obligatorio")]
    public long? Cuit { get; set; }
    [Required(ErrorMessage = "La Razon Social es obligatoria")]
    [MaxLength(50, ErrorMessage = "La razon social debe tener 50 como maximo")]
    public string RazonSocial { get; set; }
    [MaxLength(30, ErrorMessage = "La direccion no puede superar los 30 caracteres")]
    public string Direccion { get; set; }
    public int? Telefono { get; set; }
    [MaxLength(50, ErrorMessage = "El mail debe tener 50 como maximo")]
    public string Email { get; set; }
}