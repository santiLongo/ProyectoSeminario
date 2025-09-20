using System.ComponentModel.DataAnnotations; // [Key]
using System.ComponentModel.DataAnnotations.Schema; // [Table], [Column]
using Microsoft.EntityFrameworkCore;

namespace ProyectoSeminario.Models.Vehiculo
{

    //Clase de acceso a la Base de Datos
    [Table("vehiculo")]
    public class VehiculoDAO
    {
        //Id de la tabla, se autocompleta al ingresar un nuevo registro de en la Base
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Patente del vehiculo, puede no ser unico
        [Required]
        [StringLength(10)]
        [Column("patente")]
        public string Patente { get; set; }

        //Marca del vehiculo
        [Required]
        [StringLength(20)]
        [Column("marca")]
        public string Marca { get; set; }

        //Modelo del vehiculo
        [Required]
        [StringLength(20)]
        [Column("modelo")]
        public string Modelo { get; set; }

        //Id del usuario, no puede ser nulo
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        //Id del localizador asociado, no puede ser nulo y debe ser unico
        [Column("id_localizador")]
        public int IdLocalizador { get; set; }
    }

}