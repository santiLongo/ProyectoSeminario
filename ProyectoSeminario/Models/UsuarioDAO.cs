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
        public string? Mail { get; set; }

        //Password del usuario
        [Required]
        [StringLength(30)]
        [Column("password")]
        public string? Password { get; set; }

        // Relcaion Uno a Muchos. "usuario" es el nombre de la propiedad asociada
        [InverseProperty("usuario")]
        public List<VehiculoDAO> UserVehiculos { get; set; } = new();

        //Constructor de la clase. Se utilizara a la hora de crear uno
        public UsuarioDAO(string mail, string password)
        {
            Mail = mail;
            Password = password;
        }

        //Metodo encargado de agregar un vehiculo a la lista de los usuarios
        public void agregarVehiculo(VehiculoDAO vehiculo)
        {
            UserVehiculos.Add(vehiculo);
        }

        //Metodo que permite eliminar un vehiculo de la lista de los usuarios
        public void eliminarVehiculo(VehiculoDAO vehiculo)
        {
            UserVehiculos.Remove(vehiculo);
        }
    }
}