using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;
using Seminario.Services.CombosServices.Factory.Resolver;

namespace Seminario.Services.CombosServices.Handler;

public class ComboHandler
{
    private readonly IAppDbContext _ctx;
    private readonly IDbSession _session;

    public ComboHandler(IAppDbContext ctx, IDbSession session)
    {
        _ctx = ctx;
        _session = session;
    }

    public List<ICombo> Handle(string type)
    {
        var provider = ComboResolver.Resolve(type);
        
        if (provider is ISetSession setSession)
            setSession.SetSession(_session);

        if (provider is ISetContext setContext)
            setContext.SetContext(_ctx);
        
        return provider.GetCombo().ToList();
    }
}