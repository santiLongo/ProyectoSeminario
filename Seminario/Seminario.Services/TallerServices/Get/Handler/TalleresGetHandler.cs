using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.TallerServices.Get.Command;
using Seminario.Services.TallerServices.Get.Response;

namespace Seminario.Services.TallerServices.Get.Handler;

public class TalleresGetHandler
{
    private readonly IAppDbContext _ctx;
    
    public TalleresGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<TalleresGetResponse> HandleAsync(TalleresGetCommand command)
    {
        var taller = await _ctx.TallerRepo.FindByIdAsync(command.IdTaller, includeEspecilidades: true);

        if (taller == null)
            throw new SeminarioException("No se encotro el taller", HttpStatusCode.NotFound);

        var ids = taller.TallerEspecialidades.Select(e => e.IdEspecialidad).ToList();
        
        var especialidades = await _ctx.EspecialidadRepo.GetRange(ids);

        return new TalleresGetResponse
        {
            IdTaller = taller.IdTaller,
            Nombre = taller.Nombre,
            Direccion = taller.Direccion,
            Cuit = taller.Cuit,
            Telefono = taller.Telefono,
            Responsable = taller.Responsable,
            Mail = taller.Mail,
            CodigoPostal = taller.CodigoPostal,
            IdLocalidad = taller.IdLocalidad,
            Especialidades = especialidades.Select(e => new GetEspecialidadTaller
            {
                IdEspecialidad = e.IdEspecialidad,
                Descripcion = e.Descripcion
            }).ToList()
        };
    }
}