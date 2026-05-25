using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.FacturasServices.EnviarAfip;

public class EnviarFacturaAfipHandler
{
    private readonly IAppDbContext _ctx;
    private readonly HttpClient _http;

    public EnviarFacturaAfipHandler(IAppDbContext ctx, HttpClient http)
    {
        _ctx  = ctx;
        _http = http;
    }

    public async Task<RespuestaAfipDto> HandleAsync(EnviarFacturaAfipCommand command)
    {
        var factura = await _ctx.FacturaRepo.Query()
            .IncludeDetalles()
            .IncludeContraparte()
            .FirstOrDefaultAsync(f => f.IdFactura == command.IdFactura);

        if (factura == null)
            throw new InvalidOperationException($"No se encontró la factura {command.IdFactura}");

        if (factura.Anulada)
            throw new InvalidOperationException("No se puede enviar a AFIP una factura anulada");

        if (!string.IsNullOrEmpty(factura.CAE))
            throw new InvalidOperationException($"La factura ya tiene CAE: {factura.CAE}");

        var solicitud = ArmarSolicitud(factura, command);

        var response = await _http.PostAsJsonAsync("api/afip/factura", solicitud);
        response.EnsureSuccessStatusCode();

        var respuesta = await response.Content.ReadFromJsonAsync<RespuestaAfipDto>()
            ?? throw new InvalidOperationException("EasyAfip no devolvió respuesta");

        if (!respuesta.Aprobado)
            throw new InvalidOperationException(
                "AFIP rechazó el comprobante: " + string.Join(" | ", respuesta.Errores));

        factura.TipoComprobante = command.TipoComprobante;
        factura.CAE             = respuesta.CAE;
        factura.CAEFchVto       = respuesta.CAEFchVto;
        factura.Confirmada      = true;

        await _ctx.SaveChangesAsync();

        return respuesta;
    }

    private static SolicitudFacturaDto ArmarSolicitud(Factura factura, EnviarFacturaAfipCommand cmd)
    {
        var cuitReceptor = ObtenerCuit(factura);
        var ivas = CalcularIvas(factura);
        var impNeto = factura.Detalles.Sum(d => d.Subtotal);
        var impIva  = factura.Detalles.Sum(d => d.Total - d.Subtotal);

        return new SolicitudFacturaDto
        {
            TipoComprobante         = cmd.TipoComprobante,
            Concepto                = cmd.Concepto,
            DocTipo                 = cmd.DocTipoReceptor,
            DocNro                  = cuitReceptor,
            FechaComprobante        = factura.FechaEmision.ToString("yyyyMMdd"),
            FechaServicioDesde      = cmd.FechaServicioDesde,
            FechaServicioHasta      = cmd.FechaServicioHasta,
            FechaVencimientoPago    = cmd.FechaVencimientoPago
                ?? factura.FechaVencimiento?.ToString("yyyyMMdd"),
            ImporteNeto             = impNeto,
            ImporteIVA              = impIva,
            ImporteNoGravado        = 0,
            ImporteExento           = 0,
            ImporteTributos         = 0,
            ImporteTotal            = factura.Total,
            Moneda                  = "PES",
            Cotizacion              = 1.0,
            CondicionIVAReceptorId  = cmd.CondicionIVAReceptorId,
            Ivas                    = ivas
        };
    }

    private static long ObtenerCuit(Factura factura)
    {
        if (factura.Cliente?.Cuit != null)
        {
            var soloNumeros = new string(factura.Cliente.Cuit.Where(char.IsDigit).ToArray());
            if (long.TryParse(soloNumeros, out var c)) return c;
        }

        if (factura.Proveedor?.Cuit is long provCuit && provCuit > 0)
            return provCuit;

        if (factura.Taller != null && factura.Taller.Cuit > 0)
            return factura.Taller.Cuit;

        return 0; // Consumidor Final
    }

    private static List<AlicuotaIvaDto> CalcularIvas(Factura factura)
    {
        return factura.Detalles
            .GroupBy(d => d.PorcentajeIva)
            .Select(g => new AlicuotaIvaDto
            {
                Id            = MapearIdIva(g.Key),
                BaseImponible = g.Sum(d => d.Subtotal),
                Importe       = g.Sum(d => d.Total - d.Subtotal)
            })
            .Where(a => a.Id > 0)
            .ToList();
    }

    private static int MapearIdIva(decimal porcentaje) => (int)porcentaje switch
    {
        0   => 3,
        10  => 4, // 10.5% → Id 4
        21  => 5,
        27  => 6,
        _   => 5  // default 21%
    };
}

// ─── DTOs que replican el contrato de EasyAfip.Api ───

public class SolicitudFacturaDto
{
    public int TipoComprobante { get; set; }
    public int Concepto { get; set; }
    public int DocTipo { get; set; }
    public long DocNro { get; set; }
    public string FechaComprobante { get; set; }
    public string FechaServicioDesde { get; set; }
    public string FechaServicioHasta { get; set; }
    public string FechaVencimientoPago { get; set; }
    public decimal ImporteNeto { get; set; }
    public decimal ImporteIVA { get; set; }
    public decimal ImporteNoGravado { get; set; }
    public decimal ImporteExento { get; set; }
    public decimal ImporteTributos { get; set; }
    public decimal ImporteTotal { get; set; }
    public string Moneda { get; set; } = "PES";
    public double Cotizacion { get; set; } = 1.0;
    public int? CondicionIVAReceptorId { get; set; }
    public List<AlicuotaIvaDto> Ivas { get; set; } = new();
}

public class AlicuotaIvaDto
{
    public int Id { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Importe { get; set; }
}

public class RespuestaAfipDto
{
    public string Resultado { get; set; }
    public string CAE { get; set; }
    public string CAEFchVto { get; set; }
    public bool Reproceso { get; set; }
    public List<string> Observaciones { get; set; } = new();
    public List<string> Errores { get; set; } = new();
    public bool Aprobado => Resultado == "A" || Resultado == "P";
}
