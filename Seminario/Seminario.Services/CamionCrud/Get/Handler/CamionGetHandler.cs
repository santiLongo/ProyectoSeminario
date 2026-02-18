using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CamionCrud.Get.Command;
using Seminario.Services.CamionCrud.Get.Response;

namespace Seminario.Services.CamionCrud.Get.Handler;

public class CamionGetHandler
{
    private readonly IAppDbContext _ctx;
    
    public CamionGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<CamionGetResponse> HandleAsync(CamionGetCommand command)
    {
        var camion = await _ctx.CamionRepo.GetCamionByIdAsync(command.Id);
        //
        if (camion == null)
            throw new SeminarioException("No se encontro el camion buscado", HttpStatusCode.NotFound);
        //
        var response = new CamionGetResponse
        {
            IdCamion = camion.IdCamion,
            IdTipoCamion = camion.IdTipoCamion.GetValueOrDefault(),
            Marca = camion.Marca,
            Modelo = camion.Modelo,
            NroChasis = camion.NroChasis,
            NroMotor = camion.NroMotor,
            Patente = camion.Patente
        };
        //
        return response;
    }
}