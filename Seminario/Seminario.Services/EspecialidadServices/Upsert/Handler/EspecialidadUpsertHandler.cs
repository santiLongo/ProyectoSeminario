using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.EspecialidadServices.Upsert.Command;

namespace Seminario.Services.EspecialidadServices.Upsert.Handler;

public class EspecialidadUpsertHandler
{
    private readonly IAppDbContext _ctx;

    public EspecialidadUpsertHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(EspecialidadUpsertCommand command)
    {
        var especialidad = await _ctx.EspecialidadRepo.GetByIdAsync(command.IdEspecialidad.GetValueOrDefault());

        if (especialidad == null)
        {
            especialidad = Especialidad.Create();
            _ctx.EspecialidadRepo.Add(especialidad);
        }
        
        especialidad.Descripcion = command.Descripcion;
        await _ctx.SaveChangesAsync();
    } 
}