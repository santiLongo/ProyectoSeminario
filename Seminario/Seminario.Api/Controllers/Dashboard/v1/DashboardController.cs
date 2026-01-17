using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;

namespace Seminario.Api.Controllers.Dashboard.v1
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController(IAppDbContext ctx, IDbSession unit) : ControllerBase
    {
        private readonly IAppDbContext _ctx = ctx;
        private readonly IDbSession _unit = unit;

        [Route("home")]
        [HttpGet]
        public async Task GetHome()
        {

        }
    }
}
