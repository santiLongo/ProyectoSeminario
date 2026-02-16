namespace Seminario.Datos.DataSourceResult.Clases;

public class DataSourceResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Total { get; set; }
}
