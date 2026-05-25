using System.Xml;
using EasyAfip.Core.Modelos;
using EasyAfip.Core.SoapClientService;

namespace EasyAfip.Service.FacturaElectronica.Dummy;

public interface IDummyService
{
    Task<DummyResponse> EjecutarAsync();
}

public class DummyService : IDummyService
{
    private readonly ISoapClientService _soap;

    public DummyService(ISoapClientService soap)
    {
        _soap = soap;
    }

    public async Task<DummyResponse> EjecutarAsync()
    {
        const string soapBody = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <FEDummy xmlns="http://ar.gov.afip.dif.FEV1/"/>
              </soap:Body>
            </soap:Envelope>
            """;

        var doc = await _soap.PostAsync(soapBody, "FEDummy");

        return new DummyResponse
        {
            AppServer  = doc.SelectSingleNode("//AppServer")?.InnerText  ?? "?",
            DbServer   = doc.SelectSingleNode("//DbServer")?.InnerText   ?? "?",
            AuthServer = doc.SelectSingleNode("//AuthServer")?.InnerText ?? "?"
        };
    }
}
