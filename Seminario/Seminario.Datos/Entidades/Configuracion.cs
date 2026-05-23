using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("Configuraciones")]
public class Configuracion
{
    [MaxLength(20)]
    [Column("Modulo")]
    public string Modulo { get; set; }
    
    [MaxLength(20)]
    [Column("Nombre")]
    public string Nombre { get; set; }
    
    [MaxLength(20)]
    [Column("Clave")]
    public string Clave { get; set; }
    
    [MaxLength(20)]
    [Column("Valor")]
    public string Valor { get; set; }
}