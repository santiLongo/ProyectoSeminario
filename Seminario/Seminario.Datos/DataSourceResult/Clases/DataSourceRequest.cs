namespace Seminario.Datos.DataSourceResult.Clases;

public class DataSourceRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public SortDto? Sort { get; set; }
    public Dictionary<string, string>? Filters { get; set; }
}

public class SortDto
{
    public string Field { get; set; } = default!;
    public string Direction { get; set; } = "asc"; // asc | desc
}