using Seminario.Core.ArchivoManager;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Core.ExtensionMethods;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.CamionCrud.Archivos.Borrar;

public class DeleteArchivoCamionHandler
{
    private readonly IAppDbContext _ctx;
    private readonly IArchivosManager _archivosManager;

    public DeleteArchivoCamionHandler(IAppDbContext ctx, IArchivosManager archivosManager)
    {
        _ctx = ctx;
        _archivosManager = archivosManager;
    }

    public async Task HandleAsync(DeleteArchivoCamionCommand command)
    {
        var id = command.IdArchivoCamion;
        var archivo = _ctx.CamionRepo.GetArchivo(id);
        if (archivo.IsNull())
        {
            throw new SeminarioException("No existe el archivo");
        }

        var fileName = archivo.Archivo;

        var directorio = _ctx.ConfiguracionRepo.GetCamionesDirectory();
        
        await _archivosManager.EliminarAsync(directorio,  fileName);
        
        _ctx.CamionRepo.Remove(archivo);
        
        await _ctx.SaveChangesAsync();
    }
}