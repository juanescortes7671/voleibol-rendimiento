namespace RendimientoVoleibol.Forms
{
    public class ReportesForm : Form
    {
        private ComboBox cmbJugador = null!;
        private Panel panStats = null!;
        private DataGridView dgvSesiones = null!;

        public ReportesForm()
        {
            this.Text = "Reportes";
            this.BackColor = AppTheme.BgLight;
            this.Font = AppTheme.FontBody;
            BuildUI();
        }

        private void BuildUI()
        {
            var header = AppTheme.CreateHeaderPanel("📈 Reportes de Rendimiento", "Análisis estadístico individual por jugador — identificación de patrones IoT");
            this.Controls.Add(header);

            var panSelector = new Panel { Left = 20, Top = 90, Height = 60, BackColor = Color.White, Padding = new Padding(16, 10, 16, 10) };
            panSelector.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(panSelector);

            panSelector.Controls.Add(new Label { Text = "Seleccionar Jugador:", Font = AppTheme.FontBold, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(16, 18) });
            cmbJugador = new ComboBox { Location = new Point(180, 14), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleComboBox(cmbJugador);
            foreach (var j in DataStore.Jugadores) cmbJugador.Items.Add(j.NombreCompleto);
            panSelector.Controls.Add(cmbJugador);

            var btnGenerar = new Button { Text = "📊 Generar Reporte", Width = 160, Location = new Point(415, 12) };
            AppTheme.StyleButton(btnGenerar);
            btnGenerar.Click += BtnGenerar_Click;
            panSelector.Controls.Add(btnGenerar);

            // Panel estadísticas
            panStats = new Panel { Left = 20, Top = 162, Height = 180, BackColor = Color.White, Padding = new Padding(20) };
            panStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(panStats);

            var lblInstruccion = new Label { Text = "Seleccione un jugador y presione 'Generar Reporte' para ver su análisis estadístico.", Font = AppTheme.FontBody, ForeColor = AppTheme.TextGray, BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 20) };
            panStats.Controls.Add(lblInstruccion);

            // Grid de sesiones
            var lblSesiones = new Label { Text = "Detalle de sesiones", Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Left = 20, Top = 358 };
            this.Controls.Add(lblSesiones);

            dgvSesiones = new DataGridView { Left = 20, Top = 390 };
            dgvSesiones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            AppTheme.StyleDataGrid(dgvSesiones);
            dgvSesiones.Columns.Add("Fecha", "Fecha");
            dgvSesiones.Columns.Add("Saltos", "Saltos");
            dgvSesiones.Columns.Add("Altura", "Altura (cm)");
            dgvSesiones.Columns.Add("Reaccion", "Reacción (ms)");
            dgvSesiones.Columns.Add("Fatiga", "Fatiga");
            dgvSesiones.Columns.Add("Velocidad", "Vel. (m/s)");
            dgvSesiones.Columns.Add("Puntos", "Puntos");
            dgvSesiones.Columns.Add("Bloqueos", "Bloqueos");
            dgvSesiones.Columns.Add("Recepciones", "Recepciones");
            foreach (DataGridViewColumn col in dgvSesiones.Columns) col.Width = 105;
            this.Controls.Add(dgvSesiones);

            this.Resize += (s, e) =>
            {
                panSelector.Width = this.ClientSize.Width - 40;
                panStats.Width = this.ClientSize.Width - 40;
                dgvSesiones.Width = this.ClientSize.Width - 40;
                dgvSesiones.Height = this.ClientSize.Height - 410;
            };
        }

        private void BtnGenerar_Click(object? sender, EventArgs e)
        {
            if (cmbJugador.SelectedIndex < 0) { MessageBox.Show("Seleccione un jugador.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var jug = DataStore.Jugadores[cmbJugador.SelectedIndex];
            var sesiones = DataStore.RegistrosPorJugador(jug.Id);

            panStats.Controls.Clear();

            if (!sesiones.Any())
            {
                panStats.Controls.Add(new Label { Text = $"El jugador {jug.NombreCompleto} no tiene sesiones registradas.", Font = AppTheme.FontBody, ForeColor = AppTheme.TextGray, AutoSize = true, Location = new Point(20, 20) });
                return;
            }

            var (promSaltos, promAltura, promFatiga, promReaccion) = DataStore.EstadisticasJugador(jug.Id);

            // Encabezado del reporte
            var lblNombre = new Label { Text = $"📋 Reporte: {jug.NombreCompleto}  |  Posición: {jug.Posicion}  |  Equipo: {jug.Equipo}", Font = AppTheme.FontBold, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(20, 14) };
            panStats.Controls.Add(lblNombre);

            // KPI cards
            var flow = new FlowLayoutPanel { Location = new Point(20, 44), AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            panStats.Controls.Add(flow);

            flow.Controls.Add(CrearMini("Sesiones totales", sesiones.Count.ToString(), AppTheme.PrimaryMid));
            flow.Controls.Add(CrearMini("Prom. Saltos", $"{promSaltos:F1}", AppTheme.Accent));
            flow.Controls.Add(CrearMini("Prom. Altura Salto", $"{promAltura:F1} cm", AppTheme.Success));
            flow.Controls.Add(CrearMini("Prom. Fatiga", $"{promFatiga:F1} / 10", AppTheme.Warning));
            flow.Controls.Add(CrearMini("Prom. Reacción", $"{promReaccion:F1} ms", AppTheme.PrimaryLight));
            flow.Controls.Add(CrearMini("Max. Saltos", sesiones.Max(r => r.CantidadSaltos).ToString(), AppTheme.Danger));

            // Interpretación automática
            string interpretacion = "";
            if (promFatiga > 7) interpretacion += "⚠️ Nivel de fatiga alto — considerar reducir carga de entrenamiento. ";
            if (promReaccion > 350) interpretacion += "⚠️ Tiempo de reacción por encima del promedio — ejercicios de velocidad recomendados. ";
            if (promSaltos > 80) interpretacion += "✅ Alto volumen de saltos — rendimiento positivo. ";
            if (promAltura > 65) interpretacion += "✅ Buena altura de salto promedio. ";

            if (!string.IsNullOrEmpty(interpretacion))
            {
                var lblInterp = new Label { Text = interpretacion, Font = AppTheme.FontSmall, ForeColor = AppTheme.PrimaryDark, BackColor = Color.FromArgb(235, 245, 255), AutoSize = false, Width = panStats.Width - 50, Height = 36, Location = new Point(20, 134), TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(8, 0, 8, 0) };
                panStats.Controls.Add(lblInterp);
            }

            // Cargar sesiones en grid
            dgvSesiones.Rows.Clear();
            foreach (var r in sesiones)
                dgvSesiones.Rows.Add(r.FechaSesion.ToString("dd/MM/yyyy"), r.CantidadSaltos, r.AlturaPromSalto, r.TiempoReaccion, r.NivelFatiga, r.VelocidadDesplazamiento, r.PuntosAnotados, r.Bloqueos, r.Recepciones);
        }

        private Panel CrearMini(string titulo, string valor, Color color)
        {
            var p = new Panel { Width = 130, Height = 80, Margin = new Padding(0, 0, 12, 0), BackColor = Color.White };
            p.Paint += (s, e) => { using var pen = new Pen(color, 3); e.Graphics.DrawLine(pen, 0, 0, 130, 0); };
            p.Controls.Add(new Label { Text = titulo, Font = AppTheme.FontSmall, ForeColor = AppTheme.TextGray, AutoSize = false, Width = 120, Height = 28, Location = new Point(6, 10) });
            p.Controls.Add(new Label { Text = valor, Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = color, AutoSize = false, Width = 120, Height = 36, Location = new Point(6, 36) });
            return p;
        }
    }
}
