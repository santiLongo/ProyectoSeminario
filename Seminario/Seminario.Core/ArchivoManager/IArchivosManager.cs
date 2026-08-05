namespace Seminario.Core.ArchivoManager;

public interface IArchivosManager
{
    Task<string> GuardarAsync(string directorio, string nombreArchivo, byte[] bytes);
    Task<byte[]> ObtenerAsync(string directorio, string nombreArchivo);
    Task<bool> ExisteAsync(string directorio, string nombreArchivo);
    Task EliminarAsync(string directorio, string nombreArchivo);
}