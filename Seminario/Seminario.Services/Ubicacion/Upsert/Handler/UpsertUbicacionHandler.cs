using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.Ubicacion.Upsert.Command;

namespace Seminario.Services.Ubicacion.Upsert.Handler;

public class UpsertUbicacionHandler
{
    private readonly IAppDbContext _ctx;

    public UpsertUbicacionHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(UpsertUbicacionCommand command)
    {
        var localidad = await _ctx.UbicacionRepo.GetLocalidadByIdAsync(command.Id.GetValueOrDefault());

        if (localidad == null)
        {
            localidad = new Localidad();
            _ctx.UbicacionRepo.Add(localidad);
        }
        
        localidad.Descripcion = command.Descripcion;
        localidad.IdProvincia = command.IdProvincia;
        await _ctx.SaveChangesAsync();
    }
}