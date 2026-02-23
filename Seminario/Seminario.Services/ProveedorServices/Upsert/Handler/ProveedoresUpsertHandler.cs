using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.ProveedorServices.Upsert.Command;

namespace Seminario.Services.ProveedorServices.Upsert.Handler;

public class ProveedoresUpsertHandler
{
    private readonly IAppDbContext _ctx;

    public ProveedoresUpsertHandler(IAppDbContext context)
    {
        _ctx = context;
    }

    public async Task HandleAsync(ProveedoresUpsertCommand command)
    {
        var proveedor = await _ctx.ProveedorRepo.FindByIdAsync(command.IdProveedor.GetValueOrDefault(), includeEspecilidades: true);

        if (proveedor == null)
        {
            proveedor = new Proveedor();
            _ctx.ProveedorRepo.Add(proveedor);
        }
        
        proveedor.RazonSocial =  command.RazonSocial;
        proveedor.Direccion = command.Direccion;
        proveedor.Cuit = command.Cuit.GetValueOrDefault();
        proveedor.Responsable = command.Responsable;
        proveedor.Mail = command.Mail;
        proveedor.CodigoPostal = command.CodigoPostal;
        proveedor.IdLocalidad = command.IdLocalidad;

        if (proveedor.ProveedorEspecialidades.Any())
        {
            var remove = proveedor.ProveedorEspecialidades
                .Where(e => !command.Especialidades.Exists(c => c.IdEspecialidad == e.IdEspecialidad));
            
            _ctx.ProveedorRepo.RemoveRangeEspecialidades(remove.ToList());
        }
        
        var especialidades = proveedor.ProveedorEspecialidades.ToList();

        foreach (var item in command.Especialidades
                     .Where(e => !especialidades
                         .Exists(c => c.IdEspecialidad == e.IdEspecialidad)))
        {
            var especialidad = new ProveedorEspecialidad();
            especialidad.IdEspecialidad =  item.IdEspecialidad;
            
            proveedor.ProveedorEspecialidades.Add(especialidad);
        }

        await _ctx.SaveChangesAsync();
    }
}