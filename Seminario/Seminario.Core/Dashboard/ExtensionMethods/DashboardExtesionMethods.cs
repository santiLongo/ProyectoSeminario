using Seminario.Core.Dashboard.Models;

namespace Seminario.Core.Dashboard.ExtensionMethods;

public static class DashboardExtesionMethods
{
    /// <summary>
    /// Busca recursivamente un dashboard por su dashboard-name.
    /// Devuelve null si no lo encuentra.
    /// </summary>
    public static Models.Dashboard? FindDashboard(this Models.Dashboard root, string dashboardName)
    {
        if (root is null || string.IsNullOrWhiteSpace(dashboardName))
            return null;

        if (string.Equals(root.DashboardName, dashboardName, StringComparison.OrdinalIgnoreCase))
            return root;

        foreach (var card in root.Cards)
        {
            if (card.Children is null)
                continue;

            var found = card.Children.FindDashboard(dashboardName);
            if (found is not null)
                return found;
        }

        return null;
    }
    
    /// <summary>
    /// Construye el menú de la sidenav a partir del dashboard base,
    /// recorriendo recursivamente los children. Ignora las cards hidden.
    /// </summary>
    public static MenuModel[] BuildMenu(this Models.Dashboard root)
    {
        if (root is null)
            return Array.Empty<MenuModel>();

        return BuildItems(root.Cards, parentRoute: "");
    }

    private static MenuModel[] BuildItems(IEnumerable<Card> cards, string parentRoute)
    {
        var items = new List<MenuModel>();

        foreach (var card in cards)
        {
            if (card.Hidden.GetValueOrDefault())
                continue;

            var route = CombineRoute(parentRoute, CleanSegment(card.Href));

            var children = card.Children is null
                ? null
                : BuildItems(card.Children.Cards, route);

            // si todos los hijos estaban hidden, no dejo un array vacío
            if (children is { Length: 0 })
                children = null;

            items.Add(new MenuModel
            {
                Key = route.Trim('/').Replace('/', '-'),
                Label = BuildLabel(card),
                Icon = card.Icon,
                Route = route,
                Children = children
            });
        }

        return items.ToArray();
    }

    private static string CleanSegment(string href)
        => string.IsNullOrWhiteSpace(href)
            ? string.Empty
            : href.Replace("./", "").Trim('/').Trim();

    private static string CombineRoute(string parent, string segment)
    {
        if (string.IsNullOrEmpty(segment)) return parent;
        return string.IsNullOrEmpty(parent) ? $"/{segment}" : $"{parent}/{segment}";
    }

    private static string BuildLabel(Card card)
    {
        var parts = new[] { card.Title, card.Subtitle }
            .Where(s => !string.IsNullOrWhiteSpace(s));
        return string.Join(" ", parts).Trim();
    }
}