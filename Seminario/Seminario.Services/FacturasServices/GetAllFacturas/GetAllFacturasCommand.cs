namespace Seminario.Services.FacturasServices.GetAllFacturas;

public class GetAllFacturasCommand
{
    public int? TipoFactura { get; set; }
    public int? IdCliente { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdTaller { get; set; }
    public int? Estado { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool? Confirmada { get; set; }
    public bool? Anulada { get; set; }
}
