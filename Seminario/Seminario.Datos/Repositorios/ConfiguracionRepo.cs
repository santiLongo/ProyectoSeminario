using Seminario.Core.ExtensionMethods;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IConfiguracionRepo
{
    string GetCamionesDirectory();
}

public class ConfiguracionRepo : IConfiguracionRepo
{
    private readonly AppDbContext _ctx;

    public ConfiguracionRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public string GetCamionesDirectory()
    {
        const string modulo = "CONFIGURACION";
        const string nombre = "CAMIONES";
        const string clave = "PATHARCHIVOS";
        const string defaultValue = "Camiones";
        return GetValue(modulo, nombre, clave, defaultValue);
    }

    private int GetIntValue(string modulo, string nombre, string clave, int defaultValue)
    {
        var value = defaultValue.ToString();
        
        value = GetValue(modulo, nombre, clave, value);

        return value.ToIntOrDefault();
    }
    
    private string GetValue(string modulo, string nombre, string clave, string defaultValue)
    {
        var config = _ctx.Configuraciones.FirstOrDefault(c => c.Modulo.Trim() == modulo.Trim() && c.Clave.Trim() == clave.Trim() && c.Nombre.Trim() == nombre.Trim());
        if (config == null)
        {
            config = new Configuracion
            {
                Modulo = modulo,
                Nombre = nombre,
                Clave = clave,
                Valor = defaultValue
            };
            
            _ctx.Configuraciones.Add(config);
            _ctx.SaveChanges();
        }
        
        return config.Nombre;
    }
    
    private bool GetBooleanValue(string modulo, string nombre, string clave, bool defaultValue)
    {
        var value = "FALSE";
        
        if (defaultValue)
        {
            value = "TRUE";
        }
        
        value = GetValue(modulo, nombre, clave, value);
        
        return value == "TRUE";
    }
}