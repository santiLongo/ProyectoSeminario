using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ClientesServices.Helpers;
using Seminario.Services.ClientesServices.Models;

namespace Seminario.Services.ClientesServices.Commands.Get;

public class ClienteGetHandler
{
    private readonly IAppDbContext _ctx;

    public ClienteGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ClienteFormModel> HandleAsync(int IdCliente)
    {
        var cliente = await _ctx.ClienteRepo.FindByIdAsync(IdCliente);
        //
        if (cliente == null)
            throw new SeminarioException("No se encontro el cliente", HttpStatusCode.NotFound);
        //
        return cliente.MapToForm();
    }
}