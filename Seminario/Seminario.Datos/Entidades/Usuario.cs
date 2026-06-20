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
        [Column("name")]
        public string? Name { get; set; }

        [Required]
        [StringLength(30)]
        [Column("password")]
        public string? Password { get; set; }

        [StringLength(10)]
        [Column("role")]
        public string? Role { get; set; }
        
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; }
        
        [MaxLength(20)]
        [Column("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
