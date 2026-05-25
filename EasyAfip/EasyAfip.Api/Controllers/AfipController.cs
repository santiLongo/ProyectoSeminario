using EasyAfip.Core.Modelos;
using EasyAfip.Service.FacturaElectronica;
using EasyAfip.Service.FacturaElectronica.Dummy;
using EasyAfip.Service.FacturaElectronica.UltimoComprobanteAutorizado;
using Microsoft.AspNetCore.Mvc;

namespace EasyAfip.Api.Controllers;

[ApiController]
[Route("api/afip")]
public class AfipController : ControllerBase
{
    private readonly IDummyService _dummy;
    private readonly IUltimoComprobanteService _ultimoComprobante;
    private readonly IFeCAEService _feCAE;

    public AfipController(
        IDummyService dummy,
        IUltimoComprobanteService ultimoComprobante,
        IFeCAEService feCAE)
    {
        _dummy             = dummy;
        _ultimoComprobante = ultimoComprobante;
        _feCAE             = feCAE;
    }

    /// <summary>Verifica que los servidores de AFIP estén operativos. No requiere autenticación.</summary>
    [HttpGet("dummy")]
    public async Task<DummyResponse> Dummy()
        => await _dummy.EjecutarAsync();

    /// <summary>Devuelve el último número de comprobante autorizado para el punto de venta y tipo dados.</summary>
    [HttpGet("ultimo-comprobante")]
    public async Task<long> UltimoComprobante(
        [FromQuery] int tipoComprobante,
        [FromQuery] int? puntoVenta = null)
        => await _ultimoComprobante.EjecutarAsync(tipoComprobante, puntoVenta);

    /// <summary>
    /// Solicita el CAE para un comprobante electrónico.
    /// El número se obtiene automáticamente de AFIP antes de enviar.
    /// </summary>
    [HttpPost("factura")]
    public async Task<RespuestaAfip> SolicitarCAE([FromBody] SolicitudFactura solicitud)
        => await _feCAE.EjecutarAsync(solicitud);
}
