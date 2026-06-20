using Microsoft.AspNetCore.Mvc;
using Seminario.Core.Dashboard;
using Seminario.Core.Dashboard.ExtensionMethods;
using Seminario.Core.Dashboard.Models;
using Seminario.Core.FilterResponse;

namespace Seminario.Api.Controllers.Home.v1;

[ApiController]
[Route("api/v1/home")]
public class HomeController : ControllerBase
{
    private readonly DashboardStore _store;
    
    public HomeController()
    {
        _store = DashboardStore.Instance;
    }

    [HttpGet("getDashboard")]
    [SeminarioResponse]
    public List<CardModel> GetDashboard([FromQuery] string dashboardName)
    {
        var dashboard = _store.Root.FindDashboard(dashboardName);
        
        return dashboard?.Cards
            .Where(card => !card.Hidden.GetValueOrDefault())
            .Select(card => new CardModel
                    {
                        Title = card.Title,
                        Subtitle = card.Subtitle,
                        Href = card.Href,
                        Icon = card.Icon,
                    }).ToList();
    }

    [HttpGet("getMenu")]
    [SeminarioResponse]
    public List<MenuModel> GetMenu()
    {
        return _store.Root.BuildMenu().ToList();
    }
}