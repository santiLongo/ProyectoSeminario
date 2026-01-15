using Microsoft.AspNetCore.Mvc;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;

namespace Seminario.Api.Controllers.Dashboard.v1
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController(IAppDbContext ctx, IUnitOfWork unit) : ControllerBase
    {
        private readonly IAppDbContext _ctx = ctx;
        private readonly IUnitOfWork _unit = unit;

        [Route("home")]
        [HttpGet]
        public async Task GetHome()
        {

        }
    }
}
