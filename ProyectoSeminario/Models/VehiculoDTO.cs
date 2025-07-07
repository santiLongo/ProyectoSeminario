using System.ComponentModel.DataAnnotations;

namespace ProyectoSeminario.Models
{
    //Clase destinada a enviar la informacion necesaria de los vehiculos
    public class VehiculoDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string? Patente { get; set; }

        [Required]
        [StringLength(20)]
        public string? Marca { get; set; }

        [Required]
        [StringLength(20)]
        public string? Modelo { get; set; }
    }
}