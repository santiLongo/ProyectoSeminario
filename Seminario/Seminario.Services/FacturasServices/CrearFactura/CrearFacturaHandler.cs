using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.FacturasServices.CrearFactura;

public class CrearFacturaHandler
{
    private readonly IAppDbContext _ctx;

    public CrearFacturaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(CrearFacturaCommand command)
    {
        var creador = FacturaCreadorResolver.Resolver(command.Tipo!.Value);
        return await creador.CrearAsync(command, _ctx);
    }
}
