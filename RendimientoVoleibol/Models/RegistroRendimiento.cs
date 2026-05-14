namespace RendimientoVoleibol.Models
{
    public class RegistroRendimiento
    {
        public int Id { get; set; }
        public int JugadorId { get; set; }
        public string NombreJugador { get; set; } = string.Empty;
        public DateTime FechaSesion { get; set; } = DateTime.Now;
        public int CantidadSaltos { get; set; }
        public double AlturaPromSalto { get; set; }   // en cm
        public double TiempoReaccion { get; set; }    // en ms
        public int NivelFatiga { get; set; }           // 1-10
        public double VelocidadDesplazamiento { get; set; } // m/s
        public int PuntosAnotados { get; set; }
        public int Bloqueos { get; set; }
        public int Recepciones { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public string FuenteDatos { get; set; } = "IoT Simulado";
    }
}
