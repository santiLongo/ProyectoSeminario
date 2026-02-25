using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table(("tiposevento"))]
public class TipoEvento
{
    [Key]
    [Column("IdTipo")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int  IdTipo { get; set; }
    
    [Column("Nombre")]
    [MaxLength(50)]
    public string  Nombre { get; set; }
    
    [Column("Descripcion")]
    [MaxLength(200)]
    public string  Descripcion { get; set; }
    
    public ICollection<Evento> Eventos { get; set; }
}