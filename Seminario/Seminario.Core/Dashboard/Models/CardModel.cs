namespace Seminario.Core.Dashboard.Models;

public class CardModel
{
    public string Title { get; set; } = string.Empty;

    public string? Subtitle { get; set; }

    public string Href { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;
}