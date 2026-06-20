namespace Seminario.Core.DataSourceResult.Clases;

public class DataSourceResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Total { get; set; }
}
