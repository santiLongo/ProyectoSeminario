using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Login.Command;
using Seminario.Services.Login.Handler;
using Seminario.Services.Login.Response;

namespace Seminario.Api.Controllers.Login.v1
{
    [Route("api/v1/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAppDbContext _ctx;
        private readonly IConfiguration _config;

        public LoginController(IAppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        [HttpPost("auth")]
        [SeminarioResponse]
        [AllowAnonymous]
        public async Task<AuthResponse> Auth([FromBody] AuthCommand command)
        {
            var handler = new AuthHandler(_ctx, _config);
            return await handler.Handle(command);
        }
    }
}
