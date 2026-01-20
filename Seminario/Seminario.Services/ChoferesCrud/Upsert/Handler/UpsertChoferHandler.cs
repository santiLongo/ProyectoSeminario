using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.ChoferesCrud.Upsert.Command;

namespace Seminario.Services.ChoferesCrud.Upsert.Handler;

public class UpsertChoferHandler
{
    private readonly IAppDbContext _ctx;

    public UpsertChoferHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(UpsertChoferCommand command)
    {
        var chofer = await _ctx.ChoferRepo.GetAsync(q => 
                q.WhereEqualIdChofer(command.IdChofer.GetValueOrDefault())
                );

        if (chofer == null)
        {
            chofer = Chofer.Create();
            _ctx.ChoferRepo.Add(chofer);
            chofer.FechaAlta = DateTime.Today;
        }
        
        chofer.Nombre = command.Nombre;
        chofer.Apellido = command.Apellido;
        chofer.Direccion = command.Direccion;
        chofer.Dni = command.Dni;
        chofer.Telefono = command.Telefono;
        chofer.NroRegistro = command.NroRegistro;

        await _ctx.SaveChangesAsync();
    }
}