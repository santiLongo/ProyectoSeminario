using System.Xml;
using EasyAfip.Core.Modelos;
using EasyAfip.Core.SoapClientService;
using EasyAfip.Service.LoginWSAA;
using Microsoft.Extensions.Options;

namespace EasyAfip.Service.FacturaElectronica.UltimoComprobanteAutorizado;

public interface IUltimoComprobanteService
{
    Task<long> EjecutarAsync(int tipoComprobante, int? puntoVenta = null);
}

public class UltimoComprobanteService : IUltimoComprobanteService
{
    private readonly IWsaaService _wsaa;
    private readonly ISoapClientService _soap;
    private readonly AfipSettings _settings;

    public UltimoComprobanteService(
        IWsaaService wsaa,
        ISoapClientService soap,
        IOptions<AfipSettings> settings)
    {
        _wsaa     = wsaa;
        _soap     = soap;
        _settings = settings.Value;
    }

    public async Task<long> EjecutarAsync(int tipoComprobante, int? puntoVenta = null)
    {
        var ticket = await _wsaa.ObtenerTicketAsync();
        var pv     = puntoVenta ?? _settings.PuntoVenta;

        var soapBody = $"""
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <FECompUltimoAutorizado xmlns="http://ar.gov.afip.dif.FEV1/">
                  <Auth>
                    <Token>{Escape(ticket.Token)}</Token>
                    <Sign>{Escape(ticket.Sign)}</Sign>
                    <Cuit>{_settings.Cuit}</Cuit>
                  </Auth>
                  <PtoVta>{pv}</PtoVta>
                  <CbteTipo>{tipoComprobante}</CbteTipo>
                </FECompUltimoAutorizado>
              </soap:Body>
            </soap:Envelope>
            """;

        var doc = await _soap.PostAsync(soapBody, "FECompUltimoAutorizado");

        VerificarErrores(doc);

        var nroText = doc.SelectSingleNode("//CbteNro")?.InnerText ?? "0";
        return long.TryParse(nroText, out var nro) ? nro : 0;
    }

    private static void VerificarErrores(XmlDocument doc)
    {
        var errores = doc.SelectNodes("//Errors/Err")
            ?.Cast<XmlNode>()
            .Select(n => $"[{n.SelectSingleNode("Code")?.InnerText}] {n.SelectSingleNode("Msg")?.InnerText}")
            .ToList();

        if (errores != null && errores.Count > 0)
            throw new InvalidOperationException("AFIP: " + string.Join(" | ", errores));
    }

    private static string Escape(string v) =>
        System.Security.SecurityElement.Escape(v) ?? v;
}
