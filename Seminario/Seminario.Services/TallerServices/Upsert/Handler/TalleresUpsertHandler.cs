using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.TallerServices.Upsert.Command;

namespace Seminario.Services.TallerServices.Upsert.Handler;

public class TalleresUpsertHandler
{
    private readonly IAppDbContext _ctx;

    public TalleresUpsertHandler(IAppDbContext context)
    {
        _ctx = context;
    }

    public async Task HandleAsync(TalleresUpsertCommand command)
    {
        var taller = await _ctx.TallerRepo.FindByIdAsync(command.IdTaller.GetValueOrDefault(), includeEspecilidades: true);

        if (taller == null)
        {
            taller = new Taller();
            _ctx.TallerRepo.Add(taller);
        }
        
        taller.Nombre =  command.Nombre;
        taller.Direccion = command.Direccion;
        taller.Cuit = command.Cuit.GetValueOrDefault();
        taller.Telefono = command.Telefono.GetValueOrDefault();
        taller.Responsable = command.Responsable;
        taller.Mail = command.Mail;
        taller.CodigoPostal = command.CodigoPostal;
        taller.IdLocalidad = command.IdLocalidad;

        if (taller.TallerEspecialidades.Any())
        {
            var remove = taller.TallerEspecialidades
                .Where(e => !command.Especialidades.Exists(c => c.IdEspecialidad == e.IdEspecialidad));
            
            _ctx.TallerRepo.RemoveRangeEspecialidades(remove.ToList());
        }
        
        var especialidades = taller.TallerEspecialidades.ToList();

        foreach (var item in command.Especialidades
                     .Where(e => !especialidades
                         .Exists(c => c.IdEspecialidad == e.IdEspecialidad)))
        {
            var especialidad = new TallerEspecialidad();
            especialidad.IdEspecialidad =  item.IdEspecialidad;
            
            taller.TallerEspecialidades.Add(especialidad);
        }

        await _ctx.SaveChangesAsync();
    }
}