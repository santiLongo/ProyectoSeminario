using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Seminario.Datos.Entidades.Interfaces;

namespace Seminario.Datos.Entidades;

[Table("viajes/observaciones")]
public class ViajeObservacion : IAuditable
{
    [Column("idViaje", TypeName = "int")]
    public int IdViaje { get; set; }
    
    [Column("observacion", TypeName = "varchar")]
    [MaxLength(200)]
    public string Observacion { get; set; }
    
    [Column("userDateTime", TypeName = "datetime")]
    public DateTime UserDateTime { get; set; }
    
    [Column("userName", TypeName = "varchar")]
    [MaxLength(15)]
    public string UserName { get; set; }
    
    public virtual Viaje Viaje { get; set; }
    
    public void CreatedAt(DateTime date, string user)
    {
        UserDateTime = date;
        UserName = user;
    }

    public void ModifiedAt(DateTime date, string user)
    {
        UserDateTime = date;
        UserName = user;
    }
}