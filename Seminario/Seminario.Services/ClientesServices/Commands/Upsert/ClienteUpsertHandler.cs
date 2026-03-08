using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.ClientesServices.Helpers;
using Seminario.Services.ClientesServices.Models;

namespace Seminario.Services.ClientesServices.Commands.Upsert;

public class ClienteUpsertHandler
{
    private readonly IAppDbContext _ctx;

    public ClienteUpsertHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(ClienteFormModel form)
    {
        var cliente = await _ctx.ClienteRepo.FindByIdAsync(form.IdCliente.GetValueOrDefault());
        //
        if (cliente == null)
        {
            cliente = new Cliente();
            cliente.RazonSocial = form.RazonSocial;
            cliente.Cuit = form.Cuit.ToString();
            _ctx.ClienteRepo.Add(cliente);
        }
        //
        form.MapToEntity(cliente);
        await _ctx.SaveChangesAsync();
    }
}