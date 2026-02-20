using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ChoferesCrud.Get.Command;
using Seminario.Services.ChoferesCrud.Get.Response;

namespace Seminario.Services.ChoferesCrud.Get.Handler;

public class ChoferesGetHandler
{
    private readonly IAppDbContext _ctx;
    
    public ChoferesGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ChoferesGetResponse> HandleAsync(ChoferesGetCommand command)
    {
        var chofer = await _ctx.ChoferRepo.GetAsync(command.IdChofer);

        if (chofer == null)
            throw new SeminarioException("No se encontro el chofer", HttpStatusCode.NotFound);

        var response = new ChoferesGetResponse
        {
            IdChofher = chofer.IdChofer,
            Nombre = chofer.Nombre,
            Apellido = chofer.Apellido,
            Direccion = chofer.Direccion,
            Telefono = chofer.Telefono.GetValueOrDefault(),
            NroRegistro = chofer.NroRegistro,
            Dni = chofer.Dni
        };
        
        return response;
    }
}