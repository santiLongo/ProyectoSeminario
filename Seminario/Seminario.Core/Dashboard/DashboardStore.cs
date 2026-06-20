using System.Reflection;
using System.Text.Json;

namespace Seminario.Core.Dashboard;

public sealed class DashboardStore
{
    private const string ResourceName = "Seminario.Core.Dashboard.json.dashboard.json";
    
    private static readonly Lazy<DashboardStore> _instance =
        new(() => new DashboardStore());

    public static DashboardStore Instance => _instance.Value;

    public Models.Dashboard Root { get; }

    private DashboardStore()
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName) ?? 
                     throw new InvalidOperationException("No se encontro el recurso incrustado: " + ResourceName);

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        Root = JsonSerializer.Deserialize<Models.Dashboard>(json, options)
               ?? throw new InvalidOperationException("No se pudo deserializar dashboards.json");
    }
}