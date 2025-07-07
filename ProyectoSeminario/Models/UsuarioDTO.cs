using System.ComponentModel.DataAnnotations;

namespace ProyectoSeminario.Models
{

    //Clase destinada a enviar la informacion necesaria para el usuario
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Mail { get; set; }

        public List<VehiculoDTO> Vehiculos { get; set; } = new();
    }
}