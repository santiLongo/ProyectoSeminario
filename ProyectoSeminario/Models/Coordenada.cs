using System.ComponentModel.DataAnnotations; // [Key]
using System.ComponentModel.DataAnnotations.Schema; // [Table], [Column]
using Microsoft.EntityFrameworkCore;

namespace ProyectoSeminario.Models
{

    //Clase de acceso a la base de datos. Tadvia falta hacer la base de datos no relacional
    [Table("coordenada")]
    public class Coordenada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Column("latitud")]
        public string? Latitud { get; set; }
        [Required]
        [StringLength(50)]
        [Column("longitud")]
        public string? Longitud { get; set; }
        [Column("fecha")]
        public DateTime FechaYHora { get; set; } = DateTime.Now;
        [Column("movimiento")]
        public bool Movimiento { get; set; } = false;
        [ForeignKey("id_localizador")]
        public int IdLocalizador { get; set; }

        private bool enMovimiento()
        {
            return true;
        }
    }
}
