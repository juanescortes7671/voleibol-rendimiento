namespace RendimientoVoleibol.Forms
{
    public class AcercaForm : Form
    {
        public AcercaForm()
        {
            this.Text = "Acerca del Sistema";
            this.BackColor = AppTheme.BgLight;
            this.Font = AppTheme.FontBody;
            BuildUI();
        }

        private void BuildUI()
        {
            var header = AppTheme.CreateHeaderPanel("ℹ️ Acerca del Sistema", "Información del proyecto académico");
            this.Controls.Add(header);

            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = AppTheme.BgLight, Padding = new Padding(40, 30, 40, 30) };
            this.Controls.Add(scroll);
            scroll.BringToFront();

            int y = 20;

            void AgregarTitulo(string texto) { var l = new Label { Text = texto, Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(0, y) }; scroll.Controls.Add(l); y += 30; }
            void AgregarTexto(string texto) { var l = new Label { Text = texto, Font = AppTheme.FontBody, ForeColor = AppTheme.TextDark, AutoSize = false, Width = 700, Height = 22, Location = new Point(12, y) }; scroll.Controls.Add(l); y += 24; }
            void AgregarSep() { var p = new Panel { BackColor = AppTheme.BorderColor, Height = 1, Width = 700, Location = new Point(0, y) }; scroll.Controls.Add(p); y += 20; }
            void AgregarEspacio() { y += 12; }

            // Logo / título
            var lblMain = new Label
            {
                Text = "🏐  SISTEMA PARA EL ANÁLISIS DEL RENDIMIENTO\n     DEPORTIVO DE JUGADORES DE VOLEIBOL",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = AppTheme.PrimaryDark,
                AutoSize = true,
                Location = new Point(0, y)
            };
            scroll.Controls.Add(lblMain);
            y += 70;
            AgregarSep();

            AgregarTitulo("Información del Proyecto");
            AgregarTexto("Autor:          Juan Esteban Cortés Sánchez");
            AgregarTexto("Institución:    Unicervantes");
            AgregarTexto("Asignatura:     Arquitectura Big Data e Internet de las Cosas");
            AgregarTexto("Docente:        Jamilton Fernando Benavides");
            AgregarTexto("Fecha:          22 de marzo de 2026");
            AgregarEspacio();
            AgregarSep();

            AgregarTitulo("Descripción");
            AgregarTexto("Sistema de análisis de rendimiento deportivo en jugadores de voleibol basado en");
            AgregarTexto("datos IoT simulados, utilizando SQL Server Management Studio como herramienta");
            AgregarTexto("principal para el almacenamiento, gestión y procesamiento de la información.");
            AgregarEspacio();
            AgregarSep();

            AgregarTitulo("Objetivos del Sistema");
            AgregarTexto("✔  Diseñar una base de datos SQL para almacenar información del rendimiento deportivo.");
            AgregarTexto("✔  Simular la recolección de datos como si provinieran de sensores IoT.");
            AgregarTexto("✔  Realizar consultas SQL para analizar el rendimiento de los jugadores.");
            AgregarTexto("✔  Identificar patrones y comportamientos en los datos recolectados.");
            AgregarEspacio();
            AgregarSep();

            AgregarTitulo("Tecnologías Utilizadas");
            AgregarTexto("Frontend:        C# Windows Forms (.NET 6)");
            AgregarTexto("Base de datos:   SQL Server Management Studio (SSMS)");
            AgregarTexto("Paradigma:       IoT Simulado + Big Data Relacional");
            AgregarTexto("IDE:             Microsoft Visual Studio 2022");
            AgregarEspacio();
            AgregarSep();

            AgregarTitulo("Variables IoT Monitoreadas");
            AgregarTexto("📡  Cantidad de saltos por sesión");
            AgregarTexto("📡  Altura promedio del salto (cm)");
            AgregarTexto("📡  Tiempo de reacción (ms)");
            AgregarTexto("📡  Nivel de fatiga (escala 1–10)");
            AgregarTexto("📡  Velocidad de desplazamiento (m/s)");
            AgregarTexto("📡  Puntos anotados, bloqueos y recepciones");
        }
    }
}
