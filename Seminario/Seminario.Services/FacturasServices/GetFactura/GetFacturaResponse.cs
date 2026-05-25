namespace Seminario.Services.FacturasServices.GetFactura;

public class GetFacturaResponse
{
    public int IdFactura { get; set; }
    public int Tipo { get; set; }
    public int PuntoVenta { get; set; }
    public int Numero { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public decimal Subtotal { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal Total { get; set; }
    public int IdMoneda { get; set; }
    public decimal? TipoCambio { get; set; }
    public int Estado { get; set; }
    public string? Observaciones { get; set; }
    public bool Anulada { get; set; }
    public bool Confirmada { get; set; }
    public int? TipoComprobante { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("cae")]
    public string? CAE { get; set; }
    [System.Text.Json.Serialization.JsonPropertyName("caeFchVto")]
    public string? CAEFchVto { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }
    public string? NombreContraparte { get; set; }
    public decimal SaldoPendiente { get; set; }

    public List<GetFacturaDetalleResponse> Detalles { get; set; } = new();
    public List<GetFacturaViajeResponse> Viajes { get; set; } = new();
    public List<GetFacturaMantenimientoResponse> Mantenimientos { get; set; } = new();
    public List<GetFacturaCompraResponse> CompraRepuestos { get; set; } = new();
    public List<GetFacturaReciboResponse> RecibosImputados { get; set; } = new();
}

public class GetFacturaDetalleResponse
{
    public int IdFacturaDetalle { get; set; }
    public int Orden { get; set; }
    public string Descripcion { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
}

public class GetFacturaViajeResponse
{
    public int IdFacturaViaje { get; set; }
    public int IdViaje { get; set; }
    public string? NroViaje { get; set; }
    public string? Cliente { get; set; }
    public decimal MontoViaje { get; set; }
}

public class GetFacturaMantenimientoResponse
{
    public int IdFacturaMantenimiento { get; set; }
    public int IdMantenimiento { get; set; }
    public string? Titulo { get; set; }
    public decimal ImporteMantenimiento { get; set; }
}

public class GetFacturaCompraResponse
{
    public int IdFacturaCompraRepuesto { get; set; }
    public int IdCompraRepuesto { get; set; }
    public decimal ImporteCompra { get; set; }
}

public class GetFacturaReciboResponse
{
    public int IdReciboFactura { get; set; }
    public int IdRecibo { get; set; }
    public DateTime FechaRecibo { get; set; }
    public decimal ImporteAplicado { get; set; }
    public bool ReciboAnulado { get; set; }
}
