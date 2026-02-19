using System.Reflection;
using Seminario.Datos.DataSourceResult.Clases;

namespace Seminario.Datos.DataSourceResult.ExtesionMethods;

public static class QueryExtensions
{
    public static DataSourceResult<T> ToDataSourceResult<T>(
        this IEnumerable<T> data,
        DataSourceRequest request)
    {
        var list = data as IList<T> ?? data.ToList();

        if (request.Filters != null && request.Filters.Any())
        {
            list = ApplyFiltering(list, request.Filters).ToList();
        }
        
        var total = list.Count;

        if (request.Sort != null)
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
    
    private static IEnumerable<T> ApplySorting<T>(
        IEnumerable<T> data,
        SortDto sort)
    {
        var prop = typeof(T).GetProperty(
            sort.Field,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
        );

        if (prop == null)
            return data;

        var desc = sort.Direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return desc
            ? data.OrderByDescending(x => prop.GetValue(x))
            : data.OrderBy(x => prop.GetValue(x));
    }
    
    private static IEnumerable<T> ApplyFiltering<T>(
        IEnumerable<T> data,
        Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            var field = filter.Key;
            var value = filter.Value?.ToString();

            if (string.IsNullOrWhiteSpace(value))
                continue;

            var prop = typeof(T).GetProperty(field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
                continue;

            data = data.Where(item =>
            {
                var propValue = prop.GetValue(item);

                if (propValue == null)
                    return false;

                var str = propValue.ToString();

                return str != null &&
                       str.Contains(value, StringComparison.OrdinalIgnoreCase);
            });
        }

        return data;
    }
}
