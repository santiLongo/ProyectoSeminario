using System.ComponentModel.DataAnnotations; // [Key]
using System.ComponentModel.DataAnnotations.Schema; // [Table], [Column]
using Microsoft.EntityFrameworkCore;

namespace ProyectoSeminario.Models
{
    //Clase de acceso a la Base de Datos
    [Table("usuario")]
    public class UsuarioDAO
    {

        //Id de la tabla, se autocompleta al ingresar un nuevo registro de en la Base
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Mail del usuario, tiene que ser unico
        [Required]
        [StringLength(100)]
        [Column("mail")]
        public string Mail { get; set; }

        //Password del usuario
        [Required]
        [StringLength(30)]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        [Column("role")]
        public string Role { get; set; }

        // Relcaion Uno a Muchos. "usuario" es el nombre de la propiedad asociada
        //[InverseProperty("usuario")]
        //public List<VehiculoDAO> UserVehiculos { get; set; } = new
    }
}