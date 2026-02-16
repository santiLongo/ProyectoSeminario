using Seminario.Datos.DataSourceResult.Clases;

namespace Seminario.Datos.DataSourceResult.ExtesionMethods;

public static class QueryExtensions
{
    public static DataSourceResult<T> ToDataSourceResult<T>(
        this IEnumerable<T> data,
        DataSourceRequest request)
    {
        var list = data as IList<T> ?? data.ToList();

        var total = list.Count;

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            list = ApplySorting(list, request.Sort).ToList();
        }

        var items = list
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new DataSourceResult<T>
        {
            Items = items,
            Total = total
        };
    }
    
    public static IEnumerable<T> ApplySorting<T>(IEnumerable<T> data, string sort)
    {
        var parts = sort.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var prop = typeof(T).GetProperty(parts[0]);

        if (prop == null) return data;

        var desc = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

        return desc
            ? data.OrderByDescending(x => prop.GetValue(x))
            : data.OrderBy(x => prop.GetValue(x));
    }
}
