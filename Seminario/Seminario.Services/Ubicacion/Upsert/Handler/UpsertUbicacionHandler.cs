using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Ubicacion.Upsert.Command;

namespace Seminario.Services.Ubicacion.Upsert.Handler;

public class UpsertUbicacionHandler
{
    private readonly IAppDbContext _ctx;

    public UpsertUbicacionHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handler(UpsertUbicacionCommand command)
    {
        var localidad = 
    }
}