namespace Seminario.Services.ChequesServices.Commands.GetAll
{
    public class ChequesGetAllCommand
    {
        public DateTime? FechaCobroDesde {  get; set; }
        public DateTime? FechaCobroHasta {  get; set; }
        public bool? SoloPropios {  get; set; }
        public EstadosCheques Estado {  get; set; }
    }

    public enum EstadosCheques
    {
        ParaCobrar = 1,
        PorCobrar = 2,
        Cobrados = 3,
        Rechazados = 4,
        Todos = 5
    }

    public class ChequesGetAllResponse
    {
        public int Id { get; set; }
        public int NroCheque { get; set; }
        public bool? EsPropio { get; set; }
        public long? CuitEmisor { get; set; }
        public string Banco { get; set; }
        public DateTime? FechaDeCobro { get; set; }
        public DateTime? FechaDeposito { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? IdMantenimiento { get; set; }
        public string MantenimientoDesc { get; set; }
        public int? IdCompraRepuesto { get; set; }
        public string Pago { get; set; }
    }
}
