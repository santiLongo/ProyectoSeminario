namespace Seminario.Datos.ExtensionMethods;

public static class GenericExtensionMethods
{
    public static bool IsNull<T>(this T generic) => generic == null;
    public static bool IsNotNull<T>(this T generic) => generic != null;
}