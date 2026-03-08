using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.ClientesServices.Commands.BajaAlta;

public class ClienteBajaAltaHandler
{
    private readonly IAppDbContext _ctx;

    public ClienteBajaAltaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(int IdCliente)
    {
        var cliente = await _ctx.ClienteRepo.FindByIdAsync(IdCliente);
        //
        if (cliente == null)
            throw new SeminarioException("No se encontro el cliente", HttpStatusCode.NotFound);
        //
        if (cliente.FechaBaja.HasValue)
        {
            cliente.FechaBaja = null;
        }
        else
        {
            cliente.FechaBaja = DateTime.Now;
        }
        //
        await _ctx.SaveChangesAsync();
    }
}