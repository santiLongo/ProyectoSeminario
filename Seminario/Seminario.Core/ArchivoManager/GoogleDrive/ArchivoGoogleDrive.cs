using Google.Apis.Drive.v3;

namespace Seminario.Core.ArchivoManager.GoogleDrive;

public class ArchivoGoogleDrive : IArchivosManager
{
    private readonly DriveService _driveService;

    public ArchivoGoogleDrive(DriveService driveService)
    {
        _driveService = driveService;
    }

    public async Task<string> GuardarAsync(string directorio, string nombreArchivo, byte[] bytes)
    {
        var folderId = await ObtenerOCrearCarpetaAsync(directorio);

        var file = new Google.Apis.Drive.v3.Data.File
        {
            Name = nombreArchivo,
            Parents = new[] { folderId }
        };

        using var stream = new MemoryStream(bytes);

        var request = _driveService.Files.Create(file, stream, "application/octet-stream");
        request.Fields = "id";

        await request.UploadAsync();

        if (request.ResponseBody == null)
            throw new Exception("No se pudo subir el archivo.");

        return request.ResponseBody.Id;
    }

    public async Task<byte[]> ObtenerAsync(string directorio, string nombreArchivo)
    {
        var fileId = await BuscarArchivoAsync(directorio, nombreArchivo);

        if (fileId == null)
            throw new FileNotFoundException(nombreArchivo);

        using var stream = new MemoryStream();

        var request = _driveService.Files.Get(fileId);
        await request.DownloadAsync(stream);

        return stream.ToArray();
    }

    public async Task<bool> ExisteAsync(string directorio, string nombreArchivo)
    {
        return await BuscarArchivoAsync(directorio, nombreArchivo) != null;
    }

    public async Task EliminarAsync(string directorio, string nombreArchivo)
    {
        var fileId = await BuscarArchivoAsync(directorio, nombreArchivo);

        if (fileId == null)
            return;

        await _driveService.Files.Delete(fileId).ExecuteAsync();
    }

    #region Privados

    private async Task<string?> BuscarArchivoAsync(string directorio, string nombreArchivo)
    {
        var folderId = await ObtenerCarpetaAsync(directorio);

        if (folderId == null)
            return null;

        var request = _driveService.Files.List();
        request.Q =
            $"name='{nombreArchivo.Replace("'", "\\'")}' and '{folderId}' in parents and trashed=false";
        request.Fields = "files(id)";
        request.PageSize = 1;

        var response = await request.ExecuteAsync();

        return response.Files.FirstOrDefault()?.Id;
    }

    private async Task<string?> ObtenerCarpetaAsync(string nombre)
    {
        var request = _driveService.Files.List();
        request.Q =
            $"mimeType='application/vnd.google-apps.folder' and name='{nombre.Replace("'", "\\'")}' and trashed=false";
        request.Fields = "files(id)";
        request.PageSize = 1;

        var response = await request.ExecuteAsync();

        return response.Files.FirstOrDefault()?.Id;
    }

    private async Task<string> ObtenerOCrearCarpetaAsync(string nombre)
    {
        var id = await ObtenerCarpetaAsync(nombre);

        if (id != null)
            return id;

        var folder = new Google.Apis.Drive.v3.Data.File
        {
            Name = nombre,
            MimeType = "application/vnd.google-apps.folder"
        };

        var request = _driveService.Files.Create(folder);
        request.Fields = "id";

        var response = await request.ExecuteAsync();

        return response.Id;
    }

    #endregion
}