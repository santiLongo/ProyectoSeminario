namespace EasyAfip.Core.Modelos;

public class TicketAcceso
{
    public string Token { get; set; }
    public string Sign { get; set; }
    public DateTime Vencimiento { get; set; }

    public bool EstaVigente() => Vencimiento > DateTime.UtcNow.AddMinutes(15);
}
