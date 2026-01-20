using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seminario.Datos.Entidades;

[Table("procedencia")]
public class Procedencia
{
    [Column("idProcedencia", TypeName = "int(11)")]
    [Key]
    public int IdProcedencia { get; set; }
    
    [Column("idViaje", TypeName = "int(11)")]
    public int IdViaje { get; set; }
    
    [Column("idLocalidad", TypeName = "int(11)")]
    public int IdLocalidad { get; set; }
    
    public virtual Localidad Localidad { get; set; }
    public virtual Viaje Viaje { get; set; }

    public static Procedencia Create()
    {
        return new Procedencia
        {
            IdProcedencia = 0,
            IdViaje = 0,
            IdLocalidad = 0,
            Localidad = null,
            Viaje = null
        };
    }
}