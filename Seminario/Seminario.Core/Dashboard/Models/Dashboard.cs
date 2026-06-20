using System.Text.Json.Serialization;

namespace Seminario.Core.Dashboard.Models;

public class Dashboard
{
    [JsonPropertyName("dashboard-name")]
    public string DashboardName { get; set; } = string.Empty;

    [JsonPropertyName("cards")]
    public List<Card> Cards { get; set; } = new();
}

public class Card
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("subtitle")]
    public string? Subtitle { get; set; }

    [JsonPropertyName("value")]
    public object? Value { get; set; }   // string | number

    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("iconBg")]
    public string? IconBg { get; set; }

    [JsonPropertyName("iconColor")]
    public string? IconColor { get; set; }

    [JsonPropertyName("trend")]
    public string? Trend { get; set; }

    [JsonPropertyName("trendUp")]
    public bool? TrendUp { get; set; }

    [JsonPropertyName("hidden")]
    public bool? Hidden { get; set; }

    [JsonPropertyName("children")]
    public Dashboard? Children { get; set; }
}