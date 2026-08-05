using Seminario.Core.ArchivoManager;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Core.ExtensionMethods;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.CamionCrud.Archivos.Subir;

public class SubirArchivosCamionesHandler
{
    private readonly IAppDbContext _ctx;
    private readonly IArchivosManager _archivosManager;

    public SubirArchivosCamionesHandler(IAppDbContext ctx, IArchivosManager archivosManager)
    {
        _ctx = ctx;
        _archivosManager = archivosManager;
    }

    public async Task HandleAsync(SubirArchivosCamionesCommand command)
    {
        var idCamion = command.IdCamion;
        
        var camion = await _ctx.CamionRepo.GetCamionByIdAsync(idCamion);
        if (camion.IsNull())
        {
            throw new SeminarioException("No se encontro el camion");
        }

        var directory = _ctx.ConfiguracionRepo.GetCamionesDirectory();

        var fileName = command.NombreArchivo;
        var bytes = command.Bytes;

        var fileId = await _archivosManager.GuardarAsync(directory, fileName, bytes);

        var archivo = _ctx.CamionRepo.GetArchivo(idCamion, fileName);
        if (archivo.IsNull())
        {
            archivo = new ArchivosCamiones
            {
                Archivo = fileName,
                IdCamion = idCamion
            };
            
            _ctx.CamionRepo.Add(archivo);
        }
        
        archivo.Fecha = DateTime.Now;
        archivo.FileId = fileId;

        await _ctx.SaveChangesAsync();
    }
}