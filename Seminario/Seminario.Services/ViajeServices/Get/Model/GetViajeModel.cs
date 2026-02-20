namespace Seminario.Services.ViajeServices.Get.Model;

public class GetViajeModel
{
    public DatosPrincipales DatosPrincipales { get; set; }
    public DatosChofer DatosChofer { get; set; }
    public DatosCliente DatosCliente { get; set; }
    public DatosCamion DatosCamion { get; set; }
    public List<DatosDestino> DatosDestino { get; set; }
    public List<DatosProcedencias> DatosProcedencias { get; set; }
}

public class DatosPrincipales
{
    public int IdViaje { get; set; }
    public decimal Kilometros { get; set; }
    public decimal MontoTotal { get; set; }
    public float PrecioKm { get; set; }
    public string Moneda { get; set; }
    public int IdMoneda { get; set; }
    public DateTime FechaPartida { get; set; }
    public DateTime? FechaDescarga { get; set; }
    public string Carga { get; set; }
    public float Kilos { get; set; }
    public string UserName { get; set; }
    public string UserAlta { get; set; }
    public DateTime UserDateTime { get; set; }
    public DateTime FechaAlta { get; set; }
}

public class DatosChofer
{
    public int IdChofer { get; set; }
    public string NombreCompleto { get; set; }
    public string NroRegistro { get; set; }
    public int Dni { get; set; }
}

public class DatosCliente 
{
    public int IdCliente { get; set; }
    public string Cuit  { get; set; }
    public string RazonSocial { get; set; }
}

public class DatosCamion
{
    public int IdCamion { get; set; }
    public string Patente { get; set; }
    public DateTime? UltimoMantenimiento { get; set; }
    public string TipoCamion { get; set; }
}

public class DatosDestino
{
    public int IdDestino { get; set; }
    public string Localidad { get; set; }
}

public class DatosProcedencias
{
    public int IdProcedencia { get; set; }
    public string Localidad { get; set; }
}