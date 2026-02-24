namespace Seminario.Datos.ControlGroupSingleton.Models;

public class PosicionUnidad
{
        public string Tipo { get; set; }          // A
        public string Nombre { get; set; }        // B
        public string Entidad { get; set; }       // D
        public DateTime? FechaPosicion { get; set; } // F
        public int? Velocidad { get; set; }        // G
        public string HojaDeRuta { get; set; }    // H
        public string Ubicacion { get; set; }     // I
        public double? Latitud { get; set; }       // N
        public double? Longitud { get; set; }      // O
        public int? IdRastreable { get; set; }     // R
}