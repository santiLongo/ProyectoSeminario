
namespace Seminario.Datos.ExtensionMethods
{
    public static class DecimalExtensionMethods
    {
        public static bool IsNullOrZero(this decimal? number)
        {
            if(number == null || number == 0)
            {
                return true;
            }

            return false;
        }
    }
}
