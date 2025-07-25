using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoSeminario.Models.ModelsDtos
{
    public class CrearVehiculoDTO
    {
        [Required]
        [StringLength(10)]
        public string Patente { get; set; }
        //Marca del vehiculo
        [Required]
        [StringLength(20)]
        public string Marca { get; set; }
        //Modelo del vehiculo
        [Required]
        [StringLength(20)]
        public string Modelo { get; set; }
        //Id del usuario, no puede ser nulo
        [Required]
        public int IdUsuario { get; set; }
        //Id del localizador asociado, no puede ser nulo y debe ser unico
        [Required]
        public int IdLocalizador { get; set; }
    }
}
