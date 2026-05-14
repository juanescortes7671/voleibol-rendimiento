namespace RendimientoVoleibol.Forms
{
    public class BusquedaForm : Form
    {
        private ComboBox cmbJugador = null!;
        private DateTimePicker dtpDesde = null!, dtpHasta = null!;
        private ComboBox cmbCriterio = null!;
        private DataGridView dgv = null!;
        private Label lblResumen = null!;

        public BusquedaForm()
        {
            this.Text = "Búsqueda y Análisis";
            this.BackColor = AppTheme.BgLight;
            this.Font = AppTheme.FontBody;
            BuildUI();
        }

        private void BuildUI()
        {
            var header = AppTheme.CreateHeaderPanel("🔍 Búsqueda y Análisis", "Consulte y filtre los datos de rendimiento — simula consultas SQL sobre la base de datos");
            this.Controls.Add(header);

            // Panel de filtros
            var panFiltros = new Panel { Left = 20, Top = 90, Width = 800, Height = 130, BackColor = Color.White, Padding = new Padding(16) };
            panFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(panFiltros);

            var lblFiltros = new Label { Text = "Filtros de búsqueda", Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(16, 14) };
            panFiltros.Controls.Add(lblFiltros);

            // Jugador
            panFiltros.Controls.Add(new Label { Text = "Jugador:", Font = AppTheme.FontBold, AutoSize = true, Location = new Point(16, 48) });
            cmbJugador = new ComboBox { Location = new Point(80, 44), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleComboBox(cmbJugador);
            cmbJugador.Items.Add("Todos");
            foreach (var j in DataStore.Jugadores) cmbJugador.Items.Add(j.NombreCompleto);
            cmbJugador.SelectedIndex = 0;
            panFiltros.Controls.Add(cmbJugador);

            // Desde
            panFiltros.Controls.Add(new Label { Text = "Desde:", Font = AppTheme.FontBold, AutoSize = true, Location = new Point(276, 48) });
            dtpDesde = new DateTimePicker { Location = new Point(328, 44), Width = 130, Format = DateTimePickerFormat.Short, Font = AppTheme.FontBody, Value = DateTime.Now.AddMonths(-3) };
            panFiltros.Controls.Add(dtpDesde);

            // Hasta
            panFiltros.Controls.Add(new Label { Text = "Hasta:", Font = AppTheme.FontBold, AutoSize = true, Location = new Point(470, 48) });
            dtpHasta = new DateTimePicker { Location = new Point(520, 44), Width = 130, Format = DateTimePickerFormat.Short, Font = AppTheme.FontBody };
            panFiltros.Controls.Add(dtpHasta);

            // Criterio
            panFiltros.Controls.Add(new Label { Text = "Ordenar por:", Font = AppTheme.FontBold, AutoSize = true, Location = new Point(16, 90) });
            cmbCriterio = new ComboBox { Location = new Point(104, 86), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleComboBox(cmbCriterio);
            cmbCriterio.Items.AddRange(new[] { "Fecha (más reciente)", "Mayor cantidad de saltos", "Mayor altura de salto", "Menor tiempo de reacción", "Mayor nivel de fatiga" });
            cmbCriterio.SelectedIndex = 0;
            panFiltros.Controls.Add(cmbCriterio);

            var btnBuscar = new Button { Text = "🔍 Buscar", Width = 110, Location = new Point(300, 84) };
            AppTheme.StyleButton(btnBuscar);
            btnBuscar.Click += BtnBuscar_Click;
            panFiltros.Controls.Add(btnBuscar);

            var btnExportar = new Button { Text = "📋 Ver SQL", Width = 110, Location = new Point(424, 84) };
            AppTheme.StyleSecondaryButton(btnExportar);
            btnExportar.Click += BtnVerSQL_Click;
            panFiltros.Controls.Add(btnExportar);

            // Resumen estadístico
            lblResumen = new Label { Left = 20, Top = 232, AutoSize = false, Width = 800, Height = 30, Font = AppTheme.FontBold, ForeColor = AppTheme.PrimaryMid, BackColor = Color.Transparent };
            lblResumen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(lblResumen);

            // Grid de resultados
            dgv = new DataGridView { Left = 20, Top = 268 };
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            AppTheme.StyleDataGrid(dgv);
            dgv.Columns.Add("Jugador", "Jugador");
            dgv.Columns.Add("Fecha", "Fecha");
            dgv.Columns.Add("Saltos", "Saltos");
            dgv.Columns.Add("Altura", "Altura (cm)");
            dgv.Columns.Add("Reaccion", "Reacción (ms)");
            dgv.Columns.Add("Fatiga", "Fatiga (1-10)");
            dgv.Columns.Add("Velocidad", "Vel. (m/s)");
            dgv.Columns.Add("Puntos", "Puntos");
            dgv.Columns.Add("Bloqueos", "Bloqueos");
            dgv.Columns.Add("Recepciones", "Recepciones");
            foreach (DataGridViewColumn col in dgv.Columns) col.Width = 100;
            dgv.Columns["Jugador"].Width = 150;
            this.Controls.Add(dgv);

            this.Resize += (s, e) =>
            {
                panFiltros.Width = this.ClientSize.Width - 40;
                lblResumen.Width = this.ClientSize.Width - 40;
                dgv.Width = this.ClientSize.Width - 40;
                dgv.Height = this.ClientSize.Height - 290;
            };

            // Cargar todos al inicio
            BtnBuscar_Click(null, EventArgs.Empty);
        }

        private void BtnBuscar_Click(object? sender, EventArgs e)
        {
            var resultados = DataStore.Registros.AsEnumerable();

            if (cmbJugador.SelectedIndex > 0)
            {
                var nombre = cmbJugador.Text;
                resultados = resultados.Where(r => r.NombreJugador == nombre);
            }

            resultados = resultados.Where(r => r.FechaSesion.Date >= dtpDesde.Value.Date && r.FechaSesion.Date <= dtpHasta.Value.Date);

            resultados = cmbCriterio.SelectedIndex switch
            {
                1 => resultados.OrderByDescending(r => r.CantidadSaltos),
                2 => resultados.OrderByDescending(r => r.AlturaPromSalto),
                3 => resultados.OrderBy(r => r.TiempoReaccion),
                4 => resultados.OrderByDescending(r => r.NivelFatiga),
                _ => resultados.OrderByDescending(r => r.FechaSesion)
            };

            var lista = resultados.ToList();
            dgv.Rows.Clear();
            foreach (var r in lista)
                dgv.Rows.Add(r.NombreJugador, r.FechaSesion.ToString("dd/MM/yyyy"),
                    r.CantidadSaltos, r.AlturaPromSalto, r.TiempoReaccion, r.NivelFatiga,
                    r.VelocidadDesplazamiento, r.PuntosAnotados, r.Bloqueos, r.Recepciones);

            string resumen = lista.Any()
                ? $"📊  {lista.Count} registros encontrados   |   Promedio saltos: {lista.Average(r => r.CantidadSaltos):F1}   |   Prom. fatiga: {lista.Average(r => r.NivelFatiga):F1}   |   Prom. reacción: {lista.Average(r => r.TiempoReaccion):F1} ms"
                : "⚠️  No se encontraron registros con los filtros aplicados.";
            lblResumen.Text = resumen;
        }

        private void BtnVerSQL_Click(object? sender, EventArgs e)
        {
            string jugadorFilter = cmbJugador.SelectedIndex == 0 ? "-- (Sin filtro de jugador)" : $"AND j.NombreCompleto = '{cmbJugador.Text}'";
            string orderBy = cmbCriterio.SelectedIndex switch
            {
                1 => "r.CantidadSaltos DESC",
                2 => "r.AlturaPromSalto DESC",
                3 => "r.TiempoReaccion ASC",
                4 => "r.NivelFatiga DESC",
                _ => "r.FechaSesion DESC"
            };

            string sql = $@"-- ============================================================
--  Consulta generada por el sistema de Análisis de Rendimiento
--  Base de datos: SQL Server Management Studio
-- ============================================================
SELECT
    j.Nombre + ' ' + j.Apellido   AS NombreJugador,
    r.FechaSesion,
    r.CantidadSaltos,
    r.AlturaPromSalto,
    r.TiempoReaccion,
    r.NivelFatiga,
    r.VelocidadDesplazamiento,
    r.PuntosAnotados,
    r.Bloqueos,
    r.Recepciones,
    r.FuenteDatos
FROM RegistroRendimiento r
INNER JOIN Jugadores j ON j.Id = r.JugadorId
WHERE r.FechaSesion BETWEEN '{dtpDesde.Value:yyyy-MM-dd}' AND '{dtpHasta.Value:yyyy-MM-dd}'
{jugadorFilter}
ORDER BY {orderBy};

-- Estadísticas agregadas:
SELECT
    COUNT(*)              AS TotalSesiones,
    AVG(CantidadSaltos)   AS PromSaltos,
    AVG(AlturaPromSalto)  AS PromAlturaSalto,
    AVG(TiempoReaccion)   AS PromTiempoReaccion,
    AVG(NivelFatiga)      AS PromFatiga
FROM RegistroRendimiento r
INNER JOIN Jugadores j ON j.Id = r.JugadorId
WHERE r.FechaSesion BETWEEN '{dtpDesde.Value:yyyy-MM-dd}' AND '{dtpHasta.Value:yyyy-MM-dd}'
{jugadorFilter};";

            var frm = new Form
            {
                Text = "Consulta SQL Equivalente",
                Size = new Size(700, 500),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = AppTheme.BgLight
            };
            var rtb = new RichTextBox { Dock = DockStyle.Fill, Text = sql, Font = new Font("Consolas", 9.5f), BackColor = Color.FromArgb(30, 36, 50), ForeColor = Color.FromArgb(200, 220, 255), ReadOnly = true, BorderStyle = BorderStyle.None };
            var btnCerrar = new Button { Text = "Cerrar", Dock = DockStyle.Bottom, Height = 36 };
            AppTheme.StyleSecondaryButton(btnCerrar);
            btnCerrar.Click += (s, e2) => frm.Close();
            frm.Controls.Add(rtb);
            frm.Controls.Add(btnCerrar);
            frm.ShowDialog(this);
        }
    }
}
