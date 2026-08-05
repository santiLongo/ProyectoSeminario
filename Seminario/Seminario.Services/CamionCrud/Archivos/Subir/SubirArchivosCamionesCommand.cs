namespace Seminario.Services.CamionCrud.Archivos.Subir;

public class SubirArchivosCamionesCommand
{
    public int IdCamion { get; set; }
    public Byte[] Bytes { get; set; }
    public string NombreArchivo { get; set; }
}