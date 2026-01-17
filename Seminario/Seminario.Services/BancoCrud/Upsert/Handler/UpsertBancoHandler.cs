using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.BancoCrud.Upsert.Command;

namespace Seminario.Services.BancoCrud.Upsert.Handler
{
    public class UpsertBancoHandler
    {
        private readonly IAppDbContext _ctx;

        public UpsertBancoHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task Handle(UpsertBancoCommand command)
        {
            var banco = await _ctx.BancoRepo.GetByIdAsync(command.Id.GetValueOrDefault());

            if (banco == null)
            {
                banco = new Banco();
                _ctx.BancoRepo.Add(banco);
            }
            banco.Descripcion = command.Descripcion;
            await _ctx.SaveChangesAsync();

        }
    }
}
