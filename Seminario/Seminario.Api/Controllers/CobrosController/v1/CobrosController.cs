using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CobrosServices.Add.Command;
using Seminario.Services.CobrosServices.Add.Handler;

namespace Seminario.Api.Controllers.CobrosController.v1;

[ApiController]
[Route("api/v1/cobros")]
public class CobrosController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public CobrosController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost("add")]
    public async Task Add([FromBody] AddCobroCommand command)
    {
        var handler = new AddCobroHandler(_ctx);
        await handler.Handle(command);
    }
}