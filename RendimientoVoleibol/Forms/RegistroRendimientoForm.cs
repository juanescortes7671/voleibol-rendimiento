using RendimientoVoleibol.Models;

namespace RendimientoVoleibol.Forms
{
    public class RegistroRendimientoForm : Form
    {
        private ComboBox cmbJugador = null!;
        private DateTimePicker dtpFecha = null!;
        private NumericUpDown nudSaltos = null!, nudFatiga = null!, nudPuntos = null!, nudBloqueos = null!, nudRecepciones = null!;
        private TextBox txtAlturaPromSalto = null!, txtTiempoReaccion = null!, txtVelocidad = null!, txtObservaciones = null!;
        private Label lblError = null!;
        private DataGridView dgv = null!;

        public RegistroRendimientoForm()
        {
            this.Text = "Registro de Rendimiento";
            this.BackColor = AppTheme.BgLight;
            this.Font = AppTheme.FontBody;
            BuildUI();
            CargarGrid();
        }

        private void BuildUI()
        {
            var header = AppTheme.CreateHeaderPanel("📋 Registro de Rendimiento IoT", "Ingrese los datos simulados de sensores IoT para cada sesión de entrenamiento");
            this.Controls.Add(header);

            // Panel formulario
            var panForm = new Panel { Left = 20, Top = 90, Width = 400, BackColor = Color.White, Padding = new Padding(20), AutoSize = false };
            panForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            this.Controls.Add(panForm);

            var lblTitle = new Label { Text = "Nueva Sesión de Entrenamiento", Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(20, 14) };
            panForm.Controls.Add(lblTitle);

            int y = 50;

            // Jugador
            panForm.Controls.Add(new Label { Text = "Jugador *", Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) });
            y += 20;
            cmbJugador = new ComboBox { Location = new Point(20, y), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleComboBox(cmbJugador);
            foreach (var j in DataStore.Jugadores) cmbJugador.Items.Add(j.NombreCompleto);
            cmbJugador.Tag = DataStore.Jugadores;
            panForm.Controls.Add(cmbJugador);
            y += 42;

            // Fecha
            panForm.Controls.Add(new Label { Text = "Fecha de sesión *", Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) });
            y += 20;
            dtpFecha = new DateTimePicker { Location = new Point(20, y), Width = 350, Format = DateTimePickerFormat.Short, Font = AppTheme.FontBody };
            panForm.Controls.Add(dtpFecha);
            y += 42;

            // Métricas IoT en 2 columnas
            var lblMetricas = new Label { Text = "— Métricas del Sensor IoT —", Font = AppTheme.FontBold, ForeColor = AppTheme.Accent, AutoSize = true, Location = new Point(20, y) };
            panForm.Controls.Add(lblMetricas);
            y += 28;

            nudSaltos = CrearNumeric(panForm, "Cantidad de Saltos *", y, 0, 500, 60); y += 62;
            txtAlturaPromSalto = CrearTexto(panForm, "Altura Promedio del Salto (cm) *", y); y += 62;
            txtTiempoReaccion = CrearTexto(panForm, "Tiempo de Reacción (ms) *", y); y += 62;
            nudFatiga = CrearNumeric(panForm, "Nivel de Fatiga (1-10) *", y, 1, 10, 5); y += 62;
            txtVelocidad = CrearTexto(panForm, "Velocidad de Desplazamiento (m/s)", y); y += 62;
            nudPuntos = CrearNumeric(panForm, "Puntos Anotados", y, 0, 100, 0); y += 62;
            nudBloqueos = CrearNumeric(panForm, "Bloqueos", y, 0, 50, 0); y += 62;
            nudRecepciones = CrearNumeric(panForm, "Recepciones", y, 0, 100, 0); y += 62;

            panForm.Controls.Add(new Label { Text = "Observaciones", Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) });
            y += 20;
            txtObservaciones = new TextBox { Location = new Point(20, y), Width = 350, Height = 60, Multiline = true, Font = AppTheme.FontBody };
            panForm.Controls.Add(txtObservaciones);
            y += 70;

            lblError = new Label { Text = "", Font = AppTheme.FontSmall, ForeColor = AppTheme.Danger, AutoSize = false, Width = 350, Height = 40, Location = new Point(20, y) };
            panForm.Controls.Add(lblError);
            y += 44;

            var panBtns = new FlowLayoutPanel { Location = new Point(20, y), AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            var btnGuardar = new Button { Text = "💾 Guardar Sesión", Width = 160 };
            AppTheme.StyleButton(btnGuardar);
            btnGuardar.Click += BtnGuardar_Click;
            var btnLimpiar = new Button { Text = "🔄 Limpiar", Width = 100 };
            AppTheme.StyleSecondaryButton(btnLimpiar);
            btnLimpiar.Click += (s, e) => LimpiarFormulario();
            var btnSimular = new Button { Text = "🤖 Simular IoT", Width = 120 };
            AppTheme.StyleButton(btnSimular, false);
            btnSimular.Click += BtnSimular_Click;
            panBtns.Controls.AddRange(new Control[] { btnGuardar, new Panel { Width = 8 }, btnLimpiar, new Panel { Width = 8 }, btnSimular });
            panForm.Controls.Add(panBtns);

            panForm.Height = y + 80;

            // Panel derecho — Historial
            var panList = new Panel { Left = 440, Top = 90, BackColor = Color.White, Padding = new Padding(20) };
            panList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Controls.Add(panList);

            var lblHist = new Label { Text = "Historial de Sesiones", Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(20, 14) };
            panList.Controls.Add(lblHist);

            dgv = new DataGridView { Location = new Point(20, 50), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            AppTheme.StyleDataGrid(dgv);
            dgv.Columns.Add("Jugador", "Jugador");
            dgv.Columns.Add("Fecha", "Fecha");
            dgv.Columns.Add("Saltos", "Saltos");
            dgv.Columns.Add("Altura", "Altura (cm)");
            dgv.Columns.Add("Reaccion", "Reacción (ms)");
            dgv.Columns.Add("Fatiga", "Fatiga");
            dgv.Columns.Add("Velocidad", "Vel. (m/s)");
            dgv.Columns.Add("Puntos", "Puntos");
            dgv.Columns.Add("Fuente", "Fuente");
            foreach (DataGridViewColumn col in dgv.Columns) col.Width = 95;
            dgv.Columns["Jugador"].Width = 140;
            panList.Controls.Add(dgv);

            var btnEliminar = new Button { Text = "🗑️ Eliminar sesión", Width = 160 };
            AppTheme.StyleDangerButton(btnEliminar);
            btnEliminar.Click += BtnEliminar_Click;
            panList.Controls.Add(btnEliminar);

            this.Resize += (s, e) =>
            {
                panForm.Height = this.ClientSize.Height - 100;
                panList.Width = this.ClientSize.Width - 470;
                panList.Height = this.ClientSize.Height - 100;
                dgv.Width = panList.ClientSize.Width - 40;
                dgv.Height = panList.ClientSize.Height - 110;
                btnEliminar.Top = dgv.Bottom + 10;
                btnEliminar.Left = 20;
            };
        }

        private NumericUpDown CrearNumeric(Panel parent, string label, int y, int min, int max, int valor)
        {
            parent.Controls.Add(new Label { Text = label, Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) });
            var nud = new NumericUpDown { Location = new Point(20, y + 20), Width = 350, Minimum = min, Maximum = max, Value = valor, Font = AppTheme.FontBody };
            parent.Controls.Add(nud);
            return nud;
        }

        private TextBox CrearTexto(Panel parent, string label, int y)
        {
            parent.Controls.Add(new Label { Text = label, Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) });
            var txt = new TextBox { Location = new Point(20, y + 20), Width = 350 };
            AppTheme.StyleTextBox(txt);
            parent.Controls.Add(txt);
            return txt;
        }

        private void CargarGrid()
        {
            dgv.Rows.Clear();
            foreach (var r in DataStore.Registros.OrderByDescending(x => x.FechaSesion))
                dgv.Rows.Add(r.NombreJugador, r.FechaSesion.ToString("dd/MM/yyyy"), r.CantidadSaltos,
                    r.AlturaPromSalto, r.TiempoReaccion, r.NivelFatiga, r.VelocidadDesplazamiento,
                    r.PuntosAnotados, r.FuenteDatos);
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            if (!Validar()) return;

            int idx = cmbJugador.SelectedIndex;
            var jug = DataStore.Jugadores[idx];

            var reg = new RegistroRendimiento
            {
                JugadorId = jug.Id,
                NombreJugador = jug.NombreCompleto,
                FechaSesion = dtpFecha.Value,
                CantidadSaltos = (int)nudSaltos.Value,
                AlturaPromSalto = double.Parse(txtAlturaPromSalto.Text.Trim()),
                TiempoReaccion = double.Parse(txtTiempoReaccion.Text.Trim()),
                NivelFatiga = (int)nudFatiga.Value,
                VelocidadDesplazamiento = string.IsNullOrEmpty(txtVelocidad.Text) ? 0 : double.Parse(txtVelocidad.Text.Trim()),
                PuntosAnotados = (int)nudPuntos.Value,
                Bloqueos = (int)nudBloqueos.Value,
                Recepciones = (int)nudRecepciones.Value,
                Observaciones = txtObservaciones.Text,
                FuenteDatos = "IoT Simulado"
            };

            DataStore.AgregarRegistro(reg);
            LimpiarFormulario();
            CargarGrid();
            MessageBox.Show("Sesión registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool Validar()
        {
            var errores = new List<string>();
            if (cmbJugador.SelectedIndex < 0) errores.Add("• Seleccione un jugador.");
            if (!double.TryParse(txtAlturaPromSalto.Text.Trim(), out double h) || h < 10 || h > 200) errores.Add("• La altura del salto debe ser un número válido (10–200 cm).");
            if (!double.TryParse(txtTiempoReaccion.Text.Trim(), out double t) || t < 50 || t > 2000) errores.Add("• El tiempo de reacción debe ser un número válido (50–2000 ms).");
            if (!string.IsNullOrWhiteSpace(txtVelocidad.Text) && !double.TryParse(txtVelocidad.Text.Trim(), out _)) errores.Add("• La velocidad debe ser un número decimal.");
            if (errores.Any()) { lblError.Text = string.Join("\n", errores); return false; }
            return true;
        }

        private void LimpiarFormulario()
        {
            cmbJugador.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now;
            nudSaltos.Value = 60; nudFatiga.Value = 5; nudPuntos.Value = 0; nudBloqueos.Value = 0; nudRecepciones.Value = 0;
            txtAlturaPromSalto.Text = txtTiempoReaccion.Text = txtVelocidad.Text = txtObservaciones.Text = "";
            lblError.Text = "";
        }

        private void BtnSimular_Click(object? sender, EventArgs e)
        {
            if (cmbJugador.SelectedIndex < 0) { MessageBox.Show("Seleccione un jugador primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var rand = new Random();
            nudSaltos.Value = rand.Next(40, 130);
            txtAlturaPromSalto.Text = Math.Round(rand.NextDouble() * 30 + 50, 1).ToString();
            txtTiempoReaccion.Text = Math.Round(rand.NextDouble() * 150 + 200, 1).ToString();
            nudFatiga.Value = rand.Next(2, 10);
            txtVelocidad.Text = Math.Round(rand.NextDouble() * 2 + 3, 2).ToString();
            nudPuntos.Value = rand.Next(3, 25);
            nudBloqueos.Value = rand.Next(0, 12);
            nudRecepciones.Value = rand.Next(4, 20);
            txtObservaciones.Text = "Datos generados automáticamente por simulación de sensor IoT";
            MessageBox.Show("Datos IoT simulados generados correctamente.", "Simulación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Seleccione una sesión para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("¿Eliminar esta sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int idx = dgv.SelectedRows[0].Index;
                if (idx < DataStore.Registros.Count) { DataStore.EliminarRegistro(DataStore.Registros.OrderByDescending(x => x.FechaSesion).ToList()[idx].Id); CargarGrid(); }
            }
        }
    }
}
