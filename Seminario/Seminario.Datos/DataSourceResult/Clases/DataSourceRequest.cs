namespace Seminario.Datos.DataSourceResult.Clases;

public class DataSourceRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Sort { get; set; }
}