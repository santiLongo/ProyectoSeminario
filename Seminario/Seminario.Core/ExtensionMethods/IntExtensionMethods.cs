namespace Seminario.Core.ExtensionMethods
{
    public static class IntExtensionMethods
    {
        public static bool IsNullOrZero(this int? number)
        {
            if(number == null || number == 0)
            {
                return true;
            }

            return false;
        }
    }
}
