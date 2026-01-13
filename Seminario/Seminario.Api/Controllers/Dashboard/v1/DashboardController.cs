using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Api.Controllers.Dashboard.v1
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController(IAppDbContext ctx) : ControllerBase
    {
        private readonly IAppDbContext _ctx = ctx;

        [Route("home")]
        [HttpGet]
        public async Task GetHome(){}
    }
}
