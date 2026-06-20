namespace Seminario.Core.Dashboard.Models;

public class MenuModel
{
    public string Key { get; set; }
    public string Label {get; set;}
    public string Icon { get; set; }
    public string Route { get; set; }
    public MenuModel[] Children { get; set; }
}