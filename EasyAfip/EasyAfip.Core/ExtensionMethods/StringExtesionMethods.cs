namespace Seminario.Datos.ExtensionMethods;

public static class StringExtesionMethods
{
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
    
    public static int ToIntOrDefault(this string value)
    {
        if (!value.IsNullOrEmpty())
        {
            return Convert.ToInt32(value);
        }
        
        return 0;
    }
    
    public static long ToLongOrDefault(this string value)
    {
        if (!value.IsNullOrEmpty())
        {
            return Convert.ToInt64(value);
        }
        
        return 0;
    }
}