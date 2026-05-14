namespace RendimientoVoleibol.Forms
{
    public class DashboardForm : Form
    {
        public DashboardForm()
        {
            this.Text = "Dashboard";
            this.BackColor = AppTheme.BgLight;
            this.Font = AppTheme.FontBody;
            BuildUI();
        }

        private void BuildUI()
        {
            var header = AppTheme.CreateHeaderPanel("📊 Dashboard", "Resumen general del sistema de análisis de rendimiento");
            this.Controls.Add(header);

            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = AppTheme.BgLight, Padding = new Padding(20, 20, 20, 20) };
            this.Controls.Add(scroll);
            scroll.BringToFront();

            int totalJugadores = DataStore.Jugadores.Count;
            int totalRegistros = DataStore.Registros.Count;
            double promFatiga = DataStore.Registros.Any() ? DataStore.Registros.Average(r => r.NivelFatiga) : 0;
            double promSaltos = DataStore.Registros.Any() ? DataStore.Registros.Average(r => r.CantidadSaltos) : 0;

            // Tarjetas KPI
            var kpiPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                Location = new Point(10, 10),
                Width = 900
            };

            kpiPanel.Controls.Add(CrearKpi("👤 Jugadores Registrados", totalJugadores.ToString(), AppTheme.PrimaryMid));
            kpiPanel.Controls.Add(CrearKpi("📋 Sesiones IoT Registradas", totalRegistros.ToString(), AppTheme.Accent));
            kpiPanel.Controls.Add(CrearKpi("🏃 Promedio de Saltos/Sesión", $"{promSaltos:F1}", AppTheme.Success));
            kpiPanel.Controls.Add(CrearKpi("💤 Nivel de Fatiga Promedio", $"{promFatiga:F1} / 10", AppTheme.Warning));

            scroll.Controls.Add(kpiPanel);

            // Últimas sesiones
            var lblUlt = new Label
            {
                Text = "Últimas sesiones registradas",
                Font = AppTheme.FontSubtitle,
                ForeColor = AppTheme.PrimaryDark,
                AutoSize = true,
                Location = new Point(10, 200)
            };
            scroll.Controls.Add(lblUlt);

            var dgv = new DataGridView { Location = new Point(10, 230), Width = 900, Height = 240 };
            AppTheme.StyleDataGrid(dgv);
            dgv.Columns.Add("Jugador", "Jugador");
            dgv.Columns.Add("Fecha", "Fecha Sesión");
            dgv.Columns.Add("Saltos", "Cant. Saltos");
            dgv.Columns.Add("AlturaSalto", "Altura Prom. (cm)");
            dgv.Columns.Add("Fatiga", "Nivel Fatiga");
            dgv.Columns.Add("Reaccion", "Tiempo Reacción (ms)");
            dgv.Columns.Add("Fuente", "Fuente");
            foreach (DataGridViewColumn col in dgv.Columns) col.Width = 128;

            foreach (var r in DataStore.Registros.OrderByDescending(x => x.FechaSesion).Take(8))
            {
                dgv.Rows.Add(r.NombreJugador, r.FechaSesion.ToString("dd/MM/yyyy"), r.CantidadSaltos,
                    $"{r.AlturaPromSalto} cm", $"{r.NivelFatiga}/10", $"{r.TiempoReaccion} ms", r.FuenteDatos);
            }
            scroll.Controls.Add(dgv);

            // Info del proyecto
            var infoPan = new Panel { Location = new Point(10, 490), Width = 900, Height = 100, BackColor = Color.FromArgb(232, 240, 254), Padding = new Padding(15) };
            var lblInfo = new Label
            {
                Text = "ℹ️  Sistema desarrollado para el análisis del rendimiento deportivo de jugadores de voleibol\n" +
                       "     utilizando datos simulados de sensores IoT almacenados en SQL Server Management Studio.\n" +
                       "     Universidad Unicervantes — Arquitectura Big Data e Internet de las Cosas — 2026",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.PrimaryDark,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            infoPan.Controls.Add(lblInfo);
            scroll.Controls.Add(infoPan);
        }

        private Panel CrearKpi(string titulo, string valor, Color color)
        {
            var pan = new Panel { Width = 200, Height = 120, Margin = new Padding(0, 0, 15, 15), BackColor = Color.White };
            pan.Paint += (s, e) => { using var pen = new Pen(color, 3); e.Graphics.DrawLine(pen, 0, 0, 0, pan.Height); };

            var lblTitulo = new Label { Text = titulo, Font = AppTheme.FontSmall, ForeColor = AppTheme.TextGray, BackColor = Color.Transparent, AutoSize = false, Width = 180, Height = 35, Location = new Point(14, 12), TextAlign = ContentAlignment.TopLeft };
            var lblValor = new Label { Text = valor, Font = new Font("Segoe UI", 24f, FontStyle.Bold), ForeColor = color, BackColor = Color.Transparent, AutoSize = false, Width = 180, Height = 60, Location = new Point(14, 48), TextAlign = ContentAlignment.MiddleLeft };
            pan.Controls.Add(lblTitulo);
            pan.Controls.Add(lblValor);
            return pan;
        }
    }
}
