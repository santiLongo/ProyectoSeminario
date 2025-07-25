using System.ComponentModel.DataAnnotations; // [Key]
using System.ComponentModel.DataAnnotations.Schema; // [Table], [Column]
using Microsoft.EntityFrameworkCore;

namespace ProyectoSeminario.Models
{
    //Clase de acceso a la Base de Datos
    [Table("localizador")]
    public class SistemaGpsDAO
    {

        //Id de la tabla, se autocompleta al ingresar un nuevo registro de en la Base
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Clave de indentificacion unica para los localizadores. Como si fuese una MAC
        [Column("nro_localizador")]
        public string? NroLocalizador { get; set; }

        //Historial de coordenadas de los vehiculos
        //public List<Coordenada> Historial { get; set; } = new List<Coordenada
    }
}