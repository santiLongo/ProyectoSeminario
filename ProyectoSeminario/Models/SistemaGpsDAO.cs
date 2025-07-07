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
        public List<Coordenada> Historial { get; set; } = new List<Coordenada>();

        //Agrega una coordenada al Historial.
        public void agregarCoordenada(Coordenada coordenada)
        {
            Historial.Add(coordenada);
        }

        //Toma todas las coordenadas del Historial y calcula cual fue la distancia que recorrio
        public double distanciaRecorrida()
        {
            return 1.1;
        }

        //Devuelva la ultima coordenada del Historial, que se asume que es la poscion actual
        public Coordenada ubicacionActual()
        {
            return this.Historial.Last();
        }
    }
}