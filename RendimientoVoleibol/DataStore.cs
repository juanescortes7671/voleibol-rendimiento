using RendimientoVoleibol.Models;

namespace RendimientoVoleibol
{
    /// <summary>
    /// Almacén de datos en memoria que simula la base de datos SQL Server.
    /// En producción, estos métodos realizarían consultas reales a SQL Server.
    /// </summary>
    public static class DataStore
    {
        private static int _nextJugadorId = 1;
        private static int _nextRegistroId = 1;

        public static List<Jugador> Jugadores { get; private set; } = new();
        public static List<RegistroRendimiento> Registros { get; private set; } = new();

        static DataStore()
        {
            SeedData();
        }

        private static void SeedData()
        {
            // Jugadores de muestra
            var jugadores = new[]
            {
                new Jugador { Id = _nextJugadorId++, Nombre = "Carlos", Apellido = "Ramírez", Edad = 22, Posicion = "Atacante", Correo = "carlos.ramirez@voleibol.com", Telefono = "3101234567", Equipo = "Selección A", FechaRegistro = DateTime.Now.AddMonths(-6) },
                new Jugador { Id = _nextJugadorId++, Nombre = "Andrés", Apellido = "López", Edad = 25, Posicion = "Líbero", Correo = "andres.lopez@voleibol.com", Telefono = "3209876543", Equipo = "Selección A", FechaRegistro = DateTime.Now.AddMonths(-4) },
                new Jugador { Id = _nextJugadorId++, Nombre = "Miguel", Apellido = "Torres", Edad = 20, Posicion = "Bloqueador", Correo = "miguel.torres@voleibol.com", Telefono = "3154567890", Equipo = "Selección B", FechaRegistro = DateTime.Now.AddMonths(-3) },
                new Jugador { Id = _nextJugadorId++, Nombre = "Sebastián", Apellido = "García", Edad = 24, Posicion = "Armador", Correo = "sebastian.garcia@voleibol.com", Telefono = "3185551234", Equipo = "Selección B", FechaRegistro = DateTime.Now.AddMonths(-5) },
                new Jugador { Id = _nextJugadorId++, Nombre = "David", Apellido = "Martínez", Edad = 21, Posicion = "Opuesto", Correo = "david.martinez@voleibol.com", Telefono = "3002223344", Equipo = "Selección A", FechaRegistro = DateTime.Now.AddMonths(-2) },
            };
            Jugadores.AddRange(jugadores);

            // Registros IoT simulados
            var rand = new Random(42);
            var posiciones = new[] { "Atacante", "Líbero", "Bloqueador", "Armador", "Opuesto" };
            for (int j = 1; j <= 5; j++)
            {
                for (int s = 0; s < 6; s++)
                {
                    var jug = Jugadores[j - 1];
                    Registros.Add(new RegistroRendimiento
                    {
                        Id = _nextRegistroId++,
                        JugadorId = j,
                        NombreJugador = jug.NombreCompleto,
                        FechaSesion = DateTime.Now.AddDays(-(30 - s * 5)),
                        CantidadSaltos = rand.Next(40, 120),
                        AlturaPromSalto = Math.Round(rand.NextDouble() * 30 + 50, 1),
                        TiempoReaccion = Math.Round(rand.NextDouble() * 100 + 200, 1),
                        NivelFatiga = rand.Next(2, 9),
                        VelocidadDesplazamiento = Math.Round(rand.NextDouble() * 2 + 3, 2),
                        PuntosAnotados = rand.Next(5, 25),
                        Bloqueos = rand.Next(1, 10),
                        Recepciones = rand.Next(5, 20),
                        Observaciones = "Datos simulados por sensor IoT",
                        FuenteDatos = "IoT Simulado"
                    });
                }
            }
        }

        // --- JUGADORES ---
        public static void AgregarJugador(Jugador j) { j.Id = _nextJugadorId++; Jugadores.Add(j); }
        public static void ActualizarJugador(Jugador j) { var idx = Jugadores.FindIndex(x => x.Id == j.Id); if (idx >= 0) Jugadores[idx] = j; }
        public static void EliminarJugador(int id) { Jugadores.RemoveAll(x => x.Id == id); Registros.RemoveAll(x => x.JugadorId == id); }
        public static Jugador? BuscarJugador(string texto) => Jugadores.FirstOrDefault(j => j.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase) || j.Equipo.Contains(texto, StringComparison.OrdinalIgnoreCase));

        // --- REGISTROS ---
        public static void AgregarRegistro(RegistroRendimiento r) { r.Id = _nextRegistroId++; Registros.Add(r); }
        public static void EliminarRegistro(int id) { Registros.RemoveAll(x => x.Id == id); }
        public static List<RegistroRendimiento> RegistrosPorJugador(int jugadorId) => Registros.Where(r => r.JugadorId == jugadorId).OrderByDescending(r => r.FechaSesion).ToList();

        // --- ESTADÍSTICAS ---
        public static (double PromSaltos, double PromAltura, double PromFatiga, double PromReaccion) EstadisticasJugador(int jugadorId)
        {
            var regs = RegistrosPorJugador(jugadorId);
            if (!regs.Any()) return (0, 0, 0, 0);
            return (
                regs.Average(r => r.CantidadSaltos),
                regs.Average(r => r.AlturaPromSalto),
                regs.Average(r => r.NivelFatiga),
                regs.Average(r => r.TiempoReaccion)
            );
        }
    }
}
