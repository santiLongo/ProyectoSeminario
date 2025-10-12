
namespace ProyectoSeminario.Commands.MaestroCliente.Models
{
    public class ClienteGridModel
    {
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string Provnicia { get; set; }
        public string Localidad { get; set; }
        public string UserAlta { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
    }
}
