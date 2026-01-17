using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.BancoCrud.GetAll.Command;
using Seminario.Services.BancoCrud.GetAll.Model;

namespace Seminario.Services.BancoCrud.GetAll.Handler
{
    public class GetAllBancoHandler
    {
        private readonly IAppDbContext _ctx;

        public GetAllBancoHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<GetAllBancoModel>> Handle(GetAllBancoCommand command)
        {
            var data = await _ctx.BancoRepo.GetAllAsync(command.Banco, true);

            return data.Select(b => new GetAllBancoModel
            {
                Id = b.IdBanco,
                Banco = b.Descripcion
            }).ToList();
        }
    }
}
