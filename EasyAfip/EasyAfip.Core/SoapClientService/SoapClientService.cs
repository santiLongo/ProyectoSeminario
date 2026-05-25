using System.Text;
using System.Xml;
using EasyAfip.Core.Modelos;
using Microsoft.Extensions.Options;

namespace EasyAfip.Core.SoapClientService;

public interface ISoapClientService
{
    Task<XmlDocument> PostAsync(string soapBody, string actionName);
}

public class SoapClientService : ISoapClientService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly AfipSettings _settings;

    private const string NsAfip = "http://ar.gov.afip.dif.FEV1/";

    public SoapClientService(IHttpClientFactory httpFactory, IOptions<AfipSettings> settings)
    {
        _httpFactory = httpFactory;
        _settings    = settings.Value;
    }

    public async Task<XmlDocument> PostAsync(string soapBody, string actionName)
    {
        var client  = _httpFactory.CreateClient("afip");
        var request = new HttpRequestMessage(HttpMethod.Post, _settings.WsfEv1Url)
        {
            Content = new StringContent(soapBody, Encoding.UTF8, "text/xml")
        };
        request.Headers.Add("SOAPAction", $"\"{NsAfip}{actionName}\"");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var xml = await response.Content.ReadAsStringAsync();
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return doc;
    }
}
