using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades.Cliente
{
    [Table("cliente")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Column("cuit")]
        public string? Cuit { get; set; }

        [Column("razonSocial")]
        public string? RazonSocial { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("telefono")]
        public int? Telefono { get; set; }

        [Column("mail")]
        public string? Mail { get; set; }

        [Column("userAlta")]
        public string? UserAlta { get; set; }

        [Column("userAltaDate")]
        public DateTime? UserAltaDate { get; set; }
    }
}
