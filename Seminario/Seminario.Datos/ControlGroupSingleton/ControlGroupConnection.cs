using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Seminario.Datos.ControlGroupSingleton.Models;

namespace Seminario.Datos.ControlGroupSingleton;

public interface IControlConnection
{
    Task LoginAsync();
    Task<List<PosicionUnidad>> GetPosicionUnidadesAsync();
}

public class ControlGroupConnection : IControlConnection
{
    private readonly HttpClient _client;
    private readonly CookieContainer _cookies = new();
    private readonly SemaphoreSlim _loginLock = new(1, 1);

    private const string BaseUrl = "https://browser.control-group.com.ar";
    private const string LoginUrl = "/inc/fc.asp?q=logon&uname=SGV&psw=2901&remember=on";
    private const string GrillaUrl = "/inc/fc.asp?q=grilla";

    private bool _isLogged = false;

    public ControlGroupConnection()
    {
        var handler = new HttpClientHandler
        {
            CookieContainer = _cookies,
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All
        };

        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };

        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("*/*"));
        _client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    }

    // ================= LOGIN =================

    public async Task LoginAsync()
    {
        await _loginLock.WaitAsync();
        try
        {
            if (_isLogged) return;

            var response = await _client.GetAsync(LoginUrl);
            var result = await response.Content.ReadAsStringAsync();

            if (!result.Contains("true"))
                throw new Exception("Login ControlGroup falló");

            _isLogged = true;
        }
        finally
        {
            _loginLock.Release();
        }
    }

    // ================= POSICIONES =================

    public async Task<List<PosicionUnidad>> GetPosicionUnidadesAsync()
    {
        int intentos = 0;

        while (intentos < 3)
        {
            intentos++;

            if (!_isLogged)
                await LoginAsync();

            try
            {
                var response = await _client.PostAsync(
                    GrillaUrl,
                    new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")
                );

                // Si el server mata la sesión
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ResetLoginAsync();
                    continue;
                }

                var xml = await response.Content.ReadAsStringAsync();

                // Algunos servers ASP devuelven HTML si se pierde sesión
                if (!xml.Contains("<grilla"))
                {
                    await ResetLoginAsync();
                    continue;
                }

                return ParseUnidades(xml);
            }
            catch
            {
                await ResetLoginAsync();
            }
        }

        throw new Exception("No se pudo obtener posiciones después de 3 intentos");
    }

    // ================= PARSER XML =================

    private List<PosicionUnidad> ParseUnidades(string xml)
    {
        var doc = XDocument.Parse(xml);

        return doc
            .Descendants("grilla")
            .Descendants("filas")
            .Descendants("i")
            .Select(x => new PosicionUnidad
            {
                Nombre = (string)x.Attribute("B"),
                Entidad = (string)x.Attribute("D"),
                FechaPosicion = DateTime.Parse((string)x.Attribute("F")),
                Velocidad = int.Parse((string)x.Attribute("G") ?? "0"),
                Ubicacion = (string)x.Attribute("I"),
                Latitud = double.Parse((string)x.Attribute("N"), CultureInfo.InvariantCulture),
                Longitud = double.Parse((string)x.Attribute("O"), CultureInfo.InvariantCulture),
                IdRastreable = int.Parse((string)x.Attribute("R"))
            })
            .ToList();
    }

    // ================= RESET SESSION =================

    private async Task ResetLoginAsync()
    {
        _isLogged = false;

        // limpiar cookies ASPSESSION
        var uri = new Uri(BaseUrl);
        _cookies.GetCookies(uri)
            .Cast<Cookie>()
            .Where(c => c.Name.StartsWith("ASPSESSION"))
            .ToList()
            .ForEach(c => c.Expired = true);

        await Task.Delay(300); // pequeña pausa anti flood
    }
}