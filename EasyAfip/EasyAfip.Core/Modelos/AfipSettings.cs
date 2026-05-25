namespace EasyAfip.Core.Modelos;

public class AfipSettings
{
    public string CertificadoPath { get; set; }
    public string CertificadoPassword { get; set; }
    public string Cuit { get; set; }
    public int PuntoVenta { get; set; } = 1;
    public bool Produccion { get; set; } = false;

    public string WsaaUrl => Produccion
        ? "https://wsaa.afip.gov.ar/ws/services/LoginCms"
        : "https://wsaahomo.afip.gov.ar/ws/services/LoginCms";

    public string WsfEv1Url => Produccion
        ? "https://servicios1.afip.gov.ar/wsfev1/service.asmx"
        : "https://wswhomo.afip.gov.ar/wsfev1/service.asmx";
}
