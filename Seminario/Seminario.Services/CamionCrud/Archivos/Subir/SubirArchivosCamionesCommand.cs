using Microsoft.AspNetCore.Http;

namespace Seminario.Services.CamionCrud.Archivos.Subir;

public class SubirArchivosCamionesCommand
{
    public int IdCamion { get; set; }
    public IFormFile File { get; set; }
}