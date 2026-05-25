using System.Globalization;
using System.Xml;
using EasyAfip.Core.Modelos;
using EasyAfip.Core.SoapClientService;
using EasyAfip.Service.FacturaElectronica.UltimoComprobanteAutorizado;
using EasyAfip.Service.LoginWSAA;
using Microsoft.Extensions.Options;

namespace EasyAfip.Service.FacturaElectronica;

public interface IFeCAEService
{
    Task<RespuestaAfip> EjecutarAsync(SolicitudFactura solicitud);
}

public class FeCAEService : IFeCAEService
{
    private readonly IWsaaService _wsaa;
    private readonly ISoapClientService _soap;
    private readonly IUltimoComprobanteService _ultimoComprobante;
    private readonly AfipSettings _settings;

    public FeCAEService(
        IWsaaService wsaa,
        ISoapClientService soap,
        IUltimoComprobanteService ultimoComprobante,
        IOptions<AfipSettings> settings)
    {
        _wsaa               = wsaa;
        _soap               = soap;
        _ultimoComprobante  = ultimoComprobante;
        _settings           = settings.Value;
    }

    public async Task<RespuestaAfip> EjecutarAsync(SolicitudFactura s)
    {
        var ticket  = await _wsaa.ObtenerTicketAsync();
        var proxNro = await _ultimoComprobante.EjecutarAsync(s.TipoComprobante) + 1;

        var soapBody = BuildSoapBody(ticket, s, proxNro);
        var doc      = await _soap.PostAsync(soapBody, "FECAESolicitar");

        return ParseRespuesta(doc);
    }

    // ─── Build XML ───────────────────────────────────────────────────────────

    private string BuildSoapBody(TicketAcceso ticket, SolicitudFactura s, long nro)
    {
        string D(decimal v)  => v.ToString("F2", CultureInfo.InvariantCulture);
        string Df(double v)  => v.ToString("G",  CultureInfo.InvariantCulture);

        var fecha = s.FechaComprobante ?? DateTime.Today.ToString("yyyyMMdd");

        var camposServicio = (s.Concepto == 2 || s.Concepto == 3)
            ? $"""
                  <FchServDesde>{s.FechaServicioDesde}</FchServDesde>
                  <FchServHasta>{s.FechaServicioHasta}</FchServHasta>
                  <FchVtoPago>{s.FechaVencimientoPago}</FchVtoPago>
              """
            : "";

        var condIva = s.CondicionIVAReceptorId.HasValue
            ? $"<CondicionIVAReceptorId>{s.CondicionIVAReceptorId}</CondicionIVAReceptorId>"
            : "";

        var bloqueIva = s.Ivas.Count > 0
            ? "<Iva>" + string.Concat(s.Ivas.Select(i =>
                $"<AlicIva><Id>{i.Id}</Id><BaseImp>{D(i.BaseImponible)}</BaseImp><Importe>{D(i.Importe)}</Importe></AlicIva>"))
              + "</Iva>"
            : "";

        return $"""
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <FECAESolicitar xmlns="http://ar.gov.afip.dif.FEV1/">
                  <Auth>
                    <Token>{Escape(ticket.Token)}</Token>
                    <Sign>{Escape(ticket.Sign)}</Sign>
                    <Cuit>{_settings.Cuit}</Cuit>
                  </Auth>
                  <FeCAEReq>
                    <FeCabReq>
                      <CantReg>1</CantReg>
                      <PtoVta>{_settings.PuntoVenta}</PtoVta>
                      <CbteTipo>{s.TipoComprobante}</CbteTipo>
                    </FeCabReq>
                    <FeDetReq>
                      <FECAEDetRequest>
                        <Concepto>{s.Concepto}</Concepto>
                        <DocTipo>{s.DocTipo}</DocTipo>
                        <DocNro>{s.DocNro}</DocNro>
                        <CbteDesde>{nro}</CbteDesde>
                        <CbteHasta>{nro}</CbteHasta>
                        <CbteFch>{fecha}</CbteFch>
                        <ImpTotal>{D(s.ImporteTotal)}</ImpTotal>
                        <ImpTotConc>{D(s.ImporteNoGravado)}</ImpTotConc>
                        <ImpNeto>{D(s.ImporteNeto)}</ImpNeto>
                        <ImpOpEx>{D(s.ImporteExento)}</ImpOpEx>
                        <ImpIVA>{D(s.ImporteIVA)}</ImpIVA>
                        <ImpTrib>{D(s.ImporteTributos)}</ImpTrib>
                        {camposServicio}
                        <MonId>{s.Moneda}</MonId>
                        <MonCotiz>{Df(s.Cotizacion)}</MonCotiz>
                        {condIva}
                        {bloqueIva}
                      </FECAEDetRequest>
                    </FeDetReq>
                  </FeCAEReq>
                </FECAESolicitar>
              </soap:Body>
            </soap:Envelope>
            """;
    }

    // ─── Parse respuesta ─────────────────────────────────────────────────────

    private static RespuestaAfip ParseRespuesta(XmlDocument doc)
    {
        var resultado = doc.SelectSingleNode("//FECAEDetResponse/Resultado")?.InnerText
            ?? doc.SelectSingleNode("//FeCabResp/Resultado")?.InnerText
            ?? "R";

        var cae      = doc.SelectSingleNode("//CAE")?.InnerText      ?? "";
        var caeFch   = doc.SelectSingleNode("//CAEFchVto")?.InnerText ?? "";
        var reproceso = doc.SelectSingleNode("//Reproceso")?.InnerText == "S";

        var obs = doc.SelectNodes("//Obs/Obs")
            ?.Cast<XmlNode>()
            .Select(n => $"[{n.SelectSingleNode("Code")?.InnerText}] {n.SelectSingleNode("Msg")?.InnerText}")
            .ToList() ?? new List<string>();

        var errores = doc.SelectNodes("//Errors/Err")
            ?.Cast<XmlNode>()
            .Select(n => $"[{n.SelectSingleNode("Code")?.InnerText}] {n.SelectSingleNode("Msg")?.InnerText}")
            .ToList() ?? new List<string>();

        return new RespuestaAfip
        {
            Resultado     = resultado,
            CAE           = cae,
            CAEFchVto     = caeFch,
            Reproceso     = reproceso,
            Observaciones = obs,
            Errores       = errores
        };
    }

    private static string Escape(string v) =>
        System.Security.SecurityElement.Escape(v) ?? v;
}
