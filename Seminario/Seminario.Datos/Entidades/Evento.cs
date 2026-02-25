using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Seminario.Datos.Entidades.Interfaces;

namespace Seminario.Datos.Entidades;

[Table("eventos")]
public class Evento : IAuditable
{
    [Key]
    [Column("IdEvento")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdEvento { get; set; }
    
    [Column("FechaEvento", TypeName =  "datetime")]
    public DateTime? FechaEvento { get; set; }
    
    [Column("FechaCreacion", TypeName =  "datetime")]
    public DateTime? FechaCreacion { get; set; }
    
    [Column("Titulo", TypeName =  "varchar")]
    [MaxLength(100)]
    public string Titulo { get; set; }
    
    [Column("Descripcion", TypeName =  "varchar")]
    [MaxLength(200)]
    public string Descripcion { get; set; }
    
    [Column("Color", TypeName =  "varchar")]
    [MaxLength(7)]
    public string Color { get; set; }

    [Column("Inactivo", TypeName = "bit")]
    public bool Inactivo { get; set; } = false;
    
    [Column("UserName", TypeName =  "varchar")]
    [MaxLength(15)]
    public string UserName { get; set; }
    
    [Column("UserDateTime", TypeName =  "datetime")]
    public DateTime UserDateTime { get; set; }
    
    [Column("IdTipoEvento")]
    public int IdTipoEvento { get; set; }
    
    public TipoEvento TipoEvento { get; set; }
    public void CreatedAt(DateTime date, string user)
    {
        UserName = user;
        UserDateTime = date;
    }

    public void ModifiedAt(DateTime date, string user)
    {
        UserName = user;
        UserDateTime = date;
    }
}