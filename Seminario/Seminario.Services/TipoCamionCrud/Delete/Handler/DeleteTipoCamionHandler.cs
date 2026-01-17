using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.TipoCamionCrud.Delete.Command;

namespace Seminario.Services.TipoCamionCrud.Delete.Handler;

public class DeleteTipoCamionHandler
{
    private readonly IAppDbContext _ctx;

    public DeleteTipoCamionHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(DeleteTipoCamionCommand command)
    {
        var tipo = await _ctx.TipoCamionRepo.GetByIdAsync(command.Id);

        if (tipo == null)
        {
            throw new InvalidOperationException("Ups, parece que no se encontro el banco que desea eliminar");
        }

        _ctx.TipoCamionRepo.Remove(tipo);
        await _ctx.SaveChangesAsync();
    }
}