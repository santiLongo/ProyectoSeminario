using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ProveedorServices.Get.Command;
using Seminario.Services.ProveedorServices.Get.Response;

namespace Seminario.Services.ProveedorServices.Get.Handler;

public class ProveedoresGetHandler
{
    private readonly IAppDbContext _ctx;
    
    public ProveedoresGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<ProveedoresGetResponse> HandleAsync(ProveedoresGetCommand command)
    {
        var proveedor = await _ctx.ProveedorRepo.FindByIdAsync(command.IdProveedor, includeEspecilidades: true);

        if (proveedor == null)
            throw new SeminarioException("No se encotro el Proveedor", HttpStatusCode.NotFound);

        var ids = proveedor.ProveedorEspecialidades.Select(e => e.IdEspecialidad).ToList();
        
        var especialidades = await _ctx.EspecialidadRepo.GetRange(ids);

        return new ProveedoresGetResponse
        {
            IdProveedor = proveedor.IdProveedor,
            RazonSocial = proveedor.RazonSocial,
            Direccion = proveedor.Direccion,
            Cuit = proveedor.Cuit,
            Responsable = proveedor.Responsable,
            Mail = proveedor.Mail,
            CodigoPostal = proveedor.CodigoPostal,
            IdLocalidad = proveedor.IdLocalidad,
            Especialidades = especialidades.Select(e => new GetEspecialidadProveedores
            {
                IdEspecialidad = e.IdEspecialidad,
                Descripcion = e.Descripcion
            }).ToList()
        };
    }
}