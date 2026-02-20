using System.ComponentModel.DataAnnotations;
using Seminario.Api.Middleware.ExceptionMiddleware;

namespace Seminario.Services.ChoferesCrud.Upsert.Command;

public class UpsertChoferCommand
{
    public int? IdChofer { get; set; }
    
    [Required(ErrorMessage = "El chofer debe tener nombre", AllowEmptyStrings = false, ErrorMessageResourceType = typeof(SeminarioException))]
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "El chofer debe tener apellido", AllowEmptyStrings = false, ErrorMessageResourceType = typeof(SeminarioException))]
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public int Telefono { get; set; }
    
    [Required(ErrorMessage = "El chofer debe tener un Nro de Registro", AllowEmptyStrings = false, ErrorMessageResourceType = typeof(SeminarioException))]
    public string NroRegistro { get; set; }
    
    [Required(ErrorMessage = "El chofer debe tener un DNI", ErrorMessageResourceType = typeof(SeminarioException))]
    public int Dni { get; set; }
}