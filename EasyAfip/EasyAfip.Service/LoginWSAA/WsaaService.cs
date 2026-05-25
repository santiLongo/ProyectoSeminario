using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using EasyAfip.Core.Modelos;
using Microsoft.Extensions.Options;

namespace EasyAfip.Service.LoginWSAA;

public interface IWsaaService
{
    Task<TicketAcceso> ObtenerTicketAsync();
}

public class WsaaService : IWsaaService
{
    private readonly AfipSettings _settings;
    private readonly IHttpClientFactory _httpFactory;
    private TicketAcceso? _cache;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public WsaaService(IOptions<AfipSettings> settings, IHttpClientFactory httpFactory)
    {
        _settings = settings.Value;
        _httpFactory = httpFactory;
    }

    public async Task<TicketAcceso> ObtenerTicketAsync()
    {
        if (_cache != null && _cache.EstaVigente())
            return _cache;

        await _lock.WaitAsync();
        try
        {
            if (_cache != null && _cache.EstaVigente())
                return _cache;

            _cache = await RenovarAsync();
            return _cache;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<TicketAcceso> RenovarAsync()
    {
        var traXml = BuildTra();
        var cmsBase64 = FirmarTra(traXml);
        return await LlamarWsaaAsync(cmsBase64);
    }

    private string BuildTra()
    {
        var now = DateTime.UtcNow.AddHours(-3); // Argentina UTC-3
        var gen = now.ToString("yyyy-MM-ddTHH:mm:ss");
        var exp = now.AddHours(12).ToString("yyyy-MM-ddTHH:mm:ss");
        var uniqueId = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <loginTicketRequest version="1.0">
              <header>
                <uniqueId>{uniqueId}</uniqueId>
                <generationTime>{gen}</generationTime>
                <expirationTime>{exp}</expirationTime>
              </header>
              <service>wsfe</service>
            </loginTicketRequest>
            """;
    }

    private string FirmarTra(string traXml)
    {
        var cert = new X509Certificate2(
            _settings.CertificadoPath,
            _settings.CertificadoPassword,
            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);

        var traBytes = Encoding.UTF8.GetBytes(traXml);
        var contentInfo = new ContentInfo(traBytes);
        var signedCms = new SignedCms(contentInfo, detached: false);
        var signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, cert)
        {
            DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1") // SHA-256
        };
        signedCms.ComputeSignature(signer, silent: false);

        return Convert.ToBase64String(signedCms.Encode());
    }

    private async Task<TicketAcceso> LlamarWsaaAsync(string cmsBase64)
    {
        var soap = $"""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:wsaa="http://wsaa.view.sua.dvadac.desein.afip.gov">
               <soapenv:Header/>
               <soapenv:Body>
                  <wsaa:loginCms>
                     <wsaa:in0>{cmsBase64}</wsaa:in0>
                  </wsaa:loginCms>
               </soapenv:Body>
            </soapenv:Envelope>
            """;

        var client = _httpFactory.CreateClient("afip");
        var request = new HttpRequestMessage(HttpMethod.Post, _settings.WsaaUrl)
        {
            Content = new StringContent(soap, Encoding.UTF8, "text/xml")
        };
        request.Headers.Add("SOAPAction", "\"\"");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var xml = await response.Content.ReadAsStringAsync();

        return ParseRespuesta(xml);
    }

    private TicketAcceso ParseRespuesta(string xml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var token = doc.SelectSingleNode("//token")?.InnerText
            ?? throw new InvalidOperationException("El WSAA no devolvió un token válido");

        var sign = doc.SelectSingleNode("//sign")?.InnerText
            ?? throw new InvalidOperationException("El WSAA no devolvió un sign válido");

        var expText = doc.SelectSingleNode("//expirationTime")?.InnerText;

        DateTime vencimiento;
        if (expText != null && DateTime.TryParse(expText, out var parsed))
            vencimiento = parsed.ToUniversalTime();
        else
            vencimiento = DateTime.UtcNow.AddHours(12);

        return new TicketAcceso { Token = token, Sign = sign, Vencimiento = vencimiento };
    }
}
