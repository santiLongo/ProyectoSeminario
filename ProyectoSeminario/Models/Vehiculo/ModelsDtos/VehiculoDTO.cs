using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoSeminario.Models.Vehiculo.ModelsDtos
{
    //Clase destinada a enviar la informacion necesaria de los vehiculos
    public class VehiculoDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Patente { get; set; }

        [Required]
        [StringLength(20)]
        public string Marca { get; set; }

        [Required]
        [StringLength(20)]
        public string Modelo { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdLocalizador { get; set; }
    }
}