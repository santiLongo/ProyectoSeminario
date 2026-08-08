using Microsoft.AspNetCore.StaticFiles;
using Seminario.Core.ArchivoManager;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Core.ExtensionMethods;
using Seminario.Core.Type.DownloadFileResult;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.CamionCrud.Archivos.Download;

public class DownloadArchivosCamionesHandler
{
    private readonly IAppDbContext _ctx;
    private readonly IArchivosManager _archivosManager;

    public DownloadArchivosCamionesHandler(IAppDbContext ctx, IArchivosManager archivosManager)
    {
        _ctx = ctx;
        _archivosManager = archivosManager;
    }

    public async Task<DownloadFileResult> HandleAsync(DownloadArchivosCamionesCommand command)
    {
        var id = command.Id;
        var archivo = _ctx.CamionRepo.GetArchivo(id);
        if (archivo.IsNull())
        {
            throw new SeminarioException("No existe el archivo");
        }

        var fileName = archivo.Archivo;

        var directorio = _ctx.ConfiguracionRepo.GetCamionesDirectory();
        
        var bytes = await _archivosManager.ObtenerAsync(directorio,  fileName);
        
        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return new DownloadFileResult
        {
            Bytes = bytes,
            FileName = fileName,
            ContentType = contentType
        };
    }
}