using System.ComponentModel.DataAnnotations;

namespace ProyectoSeminario.Models.Usuario.ModelsDtos
{
    public class CrearUsuarioDTO
    {
        [Required]
        [StringLength(100)]
        public string Mail { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string Role { get; set; }
    }
}
