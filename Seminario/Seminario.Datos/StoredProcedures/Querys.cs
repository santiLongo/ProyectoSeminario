using System.Reflection;

namespace Seminario.Datos.StoredProcedures
{
    public static class Querys
    {
        public static string GetAllCheques => GetQuery("GetAllCheques");

        private static string GetQuery(string query)
        {
            var assembly = typeof(Querys).Assembly;

            var resourceName = $"Seminario.Datos.StoredProcedures.Querys.{query}.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new Exception($"No se encontró el recurso: {resourceName}");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
