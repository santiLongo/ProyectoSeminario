using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.TipoCamionCrud.GetAll.Command;
using Seminario.Services.TipoCamionCrud.GetAll.Model;

namespace Seminario.Services.TipoCamionCrud.GetAll.Handler;

public class GetAllTipoCamionHandler
{
    private readonly IAppDbContext _ctx;

    public GetAllTipoCamionHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<GetAllTipoCamionModel>> Handle(GetAllTipoCamionCommand command)
    {
        var data = await _ctx.BancoRepo.GetAllAsync(command.Tipo);

        return data.Select(b => new GetAllTipoCamionModel
        {
            Id = b.IdBanco,
            Tipo = b.Descripcion
        }).ToList();
    }
}