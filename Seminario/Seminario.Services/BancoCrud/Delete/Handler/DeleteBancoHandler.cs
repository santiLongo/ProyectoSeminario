using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.BancoCrud.Delete.Command;

namespace Seminario.Services.BancoCrud.Delete.Handler
{
    public class DeleteBancoHandler
    {
        private readonly IAppDbContext _ctx;

        public DeleteBancoHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task Handle(DeleteBancoCommand command)
        {
            var banco = await _ctx.BancoRepo.GetByIdAsync(command.Id);

            if (banco == null)
            {
                throw new InvalidOperationException("Ups, parece que no se encontro el banco que desea eliminar");
            }

            _ctx.BancoRepo.Remove(banco);
            await _ctx.SaveChangesAsync();
        }
    }
}
