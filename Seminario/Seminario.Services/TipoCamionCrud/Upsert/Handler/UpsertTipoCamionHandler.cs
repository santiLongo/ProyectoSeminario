using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.TipoCamionCrud.Upsert.Command;

namespace Seminario.Services.TipoCamionCrud.Upsert.Handler;

public class UpsertTipoCamionHandler
{
    private readonly IAppDbContext _ctx;

    public UpsertTipoCamionHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(UpsertTipoCamionCommand command)
    {
        var tipo = await _ctx.TipoCamionRepo.GetByIdAsync(command.Id.GetValueOrDefault());

        if (tipo == null)
        {
            tipo = new TipoCamion();
            _ctx.TipoCamionRepo.Add(tipo);
        }
        tipo.Descripcion = command.Descripcion;
        await _ctx.SaveChangesAsync();

    }
}