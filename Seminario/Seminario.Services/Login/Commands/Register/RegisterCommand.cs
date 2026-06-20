using System.ComponentModel.DataAnnotations;

namespace Seminario.Services.Login.Commands.Register;

public class RegisterCommand
{
    [Required(ErrorMessage = "El usuario es requerido")]
    [StringLength(100,ErrorMessage = "El usuario no debe superar los 100 caracteres")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(30,ErrorMessage = "La contraseña no debe superar los 30 caracteres")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(30,ErrorMessage = "La contraseña no debe superar los 30 caracteres")]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "El email es requerido")]
    [StringLength(50,ErrorMessage = "El email no debe superar los 50 caracteres")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "El numero es requerido")]
    [StringLength(20,ErrorMessage = "El numero no debe superar los 20 caracteres")]
    public string PhoneNumber { get; set; }
}