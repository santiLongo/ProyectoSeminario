using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Api.Controllers.Ubicacion.v1;
[Route("api/v1/ubicacion")]
public class UbicacionController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public UbicacionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    
    public IActionResult Index()
    {
        return View();
    }
}