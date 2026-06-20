using Microsoft.AspNetCore.Mvc;
using Seminario.Core.ControlGroupSingleton;
using Seminario.Core.ControlGroupSingleton.Models;
using Seminario.Core.FilterResponse;

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