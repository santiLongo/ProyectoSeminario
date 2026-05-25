namespace Seminario.Services.RecibosServices.GetRecibo;

public class GetReciboResponse
{
    public int IdRecibo { get; set; }
    public int Tipo { get; set; }
    public DateTime FechaRecibo { get; set; }
    public decimal MontoTotal { get; set; }
    public int IdMoneda { get; set; }
    public decimal? TipoCambio { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }
    public string? NombreContraparte { get; set; }
    public string? Observaciones { get; set; }
    public bool Anulado { get; set; }

    public List<GetReciboFormaPagoResponse> FormasDePago { get; set; } = new();
    public List<GetReciboFacturaResponse> FacturasImputadas { get; set; } = new();
}

public class GetReciboFormaPagoResponse
{
    public int IdReciboFormaPago { get; set; }
    public int IdFormaPago { get; set; }
    public string? DescripcionFormaPago { get; set; }
    public decimal Monto { get; set; }
    public int? IdPagoCheque { get; set; }
    public int? NroCheque { get; set; }
    public long? CuitEmisor { get; set; }
    public int? IdBanco { get; set; }
    public DateTime? FechaCobro { get; set; }
    public bool? EsPropio { get; set; }
}

public class GetReciboFacturaResponse
{
    public int IdReciboFactura { get; set; }
    public int IdFactura { get; set; }
    public int PuntoVenta { get; set; }
    public int Numero { get; set; }
    public DateTime FechaEmision { get; set; }
    public decimal ImporteAplicado { get; set; }
}
