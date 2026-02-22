namespace Seminario.Services.CobrosServices.GetAll.Response;

public class CobrosGetAllResponse
{
    public int IdCobro { get; set; }
    public string NroViaje { get; set; }
    public DateTime FechaRecibo { get; set; }
    public decimal Monto  { get; set; }
    public string Moneda  { get; set; }
    public float? TipoCambio { get; set; }
    public string FormaPago { get; set; }
    public string Estado { get; set; }
}