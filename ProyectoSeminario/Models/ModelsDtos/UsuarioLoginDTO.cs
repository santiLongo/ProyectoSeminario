using System.ComponentModel.DataAnnotations;

namespace ProyectoSeminario.Models.ModelsDtos
{
    public class UsuarioLoginDTO
    {
        [Required]
        [StringLength(100)]
        public string Mail { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }
    }
}
