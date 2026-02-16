using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Handler;

namespace Seminario.Api.Controllers.Combo.v1;

[ApiController]
[Route("api/v1/combo")]
public class ComboController : ControllerBase
{
    
    [HttpGet("get")]
    [SeminarioResponse]
    public List<ICombo> Get([FromQuery] string type,
        [FromServices] IAppDbContext context, [FromServices] IDbSession session)
    {
        var extraParams = HttpContext.Request.Query
            .ToDictionary(x => x.Key, x => x.Value.ToString());
        
        var handler = new ComboHandler(context, session);
        var response = handler.Handle(type, extraParams);
        return response.ToList();
    }
}