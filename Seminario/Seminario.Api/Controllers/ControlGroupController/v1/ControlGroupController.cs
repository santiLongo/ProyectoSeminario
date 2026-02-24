using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.ControlGroupSingleton;
using Seminario.Datos.ControlGroupSingleton.Models;

namespace Seminario.Api.Controllers.ControlGroupController.v1;

[ApiController]
[Route("api/v1/control-group")]
public class ControlGroupController
{
    private readonly IControlConnection _controlConnection;
    
    public ControlGroupController(IControlConnection controlConnection)
    {
        _controlConnection = controlConnection;
    }

    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<List<PosicionUnidad>> Get()
    {
        return await _controlConnection.GetPosicionUnidadesAsync();
    }
    
}