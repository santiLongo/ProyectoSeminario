using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Seminario.Datos.Entidades.Interfaces;

namespace Seminario.Datos.Entidades;

[Table("ArchivosCamiones")]
public class ArchivosCamiones : IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column("Archivo",  TypeName = "varchar(100)")]
    public string Archivo { get; set; }
    
    [Column("IdCamion", TypeName = "int")]
    public int IdCamion { get; set; }
    
    [Column("FileId", TypeName = "varchar(255)")]
    public string FileId { get; set; }
    
    [Column("Fecha", TypeName = "datetime")]
    public DateTime Fecha { get; set; }
    
    [Column("UserName", TypeName = "varchar(20)")]
    public string UserName { get; set; }
    public void CreatedAt(DateTime date, string user)
    {
        UserName =  user;
        Fecha = date;
    }

    public void ModifiedAt(DateTime date, string user)
    {
        UserName =  user;
        Fecha = date;
    }
}