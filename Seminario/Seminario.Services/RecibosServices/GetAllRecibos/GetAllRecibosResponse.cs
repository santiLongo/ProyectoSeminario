namespace Seminario.Services.RecibosServices.GetAllRecibos;

public class GetAllRecibosResponse
{
    public int IdRecibo { get; set; }
    public int Tipo { get; set; }
    public DateTime FechaRecibo { get; set; }
    public decimal MontoTotal { get; set; }
    public bool Anulado { get; set; }
    public string? NombreContraparte { get; set; }
    public int CantFacturasImputadas { get; set; }
}
