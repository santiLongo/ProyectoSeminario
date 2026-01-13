using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades
{
    //Clase de acceso a la Base de Datos
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("mail")]
        public string? Mail { get; set; }

        [Required]
        [StringLength(30)]
        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [StringLength(10)]
        [Column("role")]
        public string? Role { get; set; }
    }
}
