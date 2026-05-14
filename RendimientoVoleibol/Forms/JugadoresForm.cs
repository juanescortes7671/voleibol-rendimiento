using RendimientoVoleibol.Models;

namespace RendimientoVoleibol.Forms
{
    public class JugadoresForm : Form
    {
        private DataGridView dgv = null!;
        private TextBox txtNombre = null!, txtApellido = null!, txtEdad = null!, txtCorreo = null!, txtTelefono = null!, txtEquipo = null!, txtBuscar = null!;
        private ComboBox cmbPosicion = null!;
        private Button btnGuardar = null!, btnLimpiar = null!, btnEliminar = null!, btnBuscar = null!;
        private Label lblError = null!;
        private int _editandoId = -1;

        public JugadoresForm()
        {
            this.Text = "Gestión de Jugadores";
            this.BackColor = AppTheme.BgLight;
            this.Font = AppTheme.FontBody;
            BuildUI();
            CargarGrid();
        }

        private void BuildUI()
        {
            var header = AppTheme.CreateHeaderPanel("👤 Gestión de Jugadores", "Registre, edite y consulte los jugadores del sistema");
            this.Controls.Add(header);

            // Panel izquierdo — Formulario
            var panForm = new Panel { Left = 20, Top = 90, Width = 360, BackColor = Color.White, Padding = new Padding(20) };
            panForm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            this.Controls.Add(panForm);

            var lblFormTitle = new Label { Text = "Datos del Jugador", Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(20, 16) };
            panForm.Controls.Add(lblFormTitle);

            int y = 52;
            txtNombre = CrearCampo(panForm, "Nombre *", y); y += 62;
            txtApellido = CrearCampo(panForm, "Apellido *", y); y += 62;
            txtEdad = CrearCampo(panForm, "Edad *", y); y += 62;

            // Posición ComboBox
            var lblPos = new Label { Text = "Posición *", Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) };
            panForm.Controls.Add(lblPos);
            y += 20;
            cmbPosicion = new ComboBox { Location = new Point(20, y), Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleComboBox(cmbPosicion);
            cmbPosicion.Items.AddRange(new[] { "Atacante", "Líbero", "Bloqueador", "Armador", "Opuesto", "Receptor" });
            panForm.Controls.Add(cmbPosicion);
            y += 42;

            txtEquipo = CrearCampo(panForm, "Equipo *", y); y += 62;
            txtCorreo = CrearCampo(panForm, "Correo electrónico *", y); y += 62;
            txtTelefono = CrearCampo(panForm, "Teléfono", y); y += 62;

            lblError = new Label { Text = "", Font = AppTheme.FontSmall, ForeColor = AppTheme.Danger, AutoSize = false, Width = 310, Height = 34, Location = new Point(20, y), TextAlign = ContentAlignment.TopLeft };
            panForm.Controls.Add(lblError);
            y += 38;

            var panBtns = new FlowLayoutPanel { Location = new Point(20, y), AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
            panForm.Controls.Add(panBtns);

            btnGuardar = new Button { Text = "💾 Guardar", Width = 130 };
            AppTheme.StyleButton(btnGuardar);
            btnGuardar.Click += BtnGuardar_Click;

            btnLimpiar = new Button { Text = "🔄 Limpiar", Width = 110 };
            AppTheme.StyleSecondaryButton(btnLimpiar);
            btnLimpiar.Click += (s, e) => LimpiarFormulario();

            panBtns.Controls.Add(btnGuardar);
            panBtns.Controls.Add(new Panel { Width = 8, Height = 1 });
            panBtns.Controls.Add(btnLimpiar);

            panForm.Height = y + 80;

            // Panel derecho — Lista
            var panList = new Panel { Left = 400, Top = 90, BackColor = Color.White, Padding = new Padding(20) };
            panList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Controls.Add(panList);

            var lblListTitle = new Label { Text = "Jugadores Registrados", Font = AppTheme.FontSubtitle, ForeColor = AppTheme.PrimaryDark, AutoSize = true, Location = new Point(20, 16) };
            panList.Controls.Add(lblListTitle);

            // Búsqueda
            var panSearch = new FlowLayoutPanel { Location = new Point(20, 46), AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            txtBuscar = new TextBox { Width = 220, PlaceholderText = "Buscar por nombre o equipo..." };
            AppTheme.StyleTextBox(txtBuscar);
            btnBuscar = new Button { Text = "🔍 Buscar", Width = 100 };
            AppTheme.StyleButton(btnBuscar);
            btnBuscar.Click += BtnBuscar_Click;
            var btnVerTodos = new Button { Text = "Ver todos", Width = 90 };
            AppTheme.StyleSecondaryButton(btnVerTodos);
            btnVerTodos.Click += (s, e) => { txtBuscar.Text = ""; CargarGrid(); };
            panSearch.Controls.AddRange(new Control[] { txtBuscar, new Panel { Width = 8 }, btnBuscar, new Panel { Width = 8 }, btnVerTodos });
            panList.Controls.Add(panSearch);

            dgv = new DataGridView { Location = new Point(20, 90), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            AppTheme.StyleDataGrid(dgv);
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Nombre", "Nombre Completo");
            dgv.Columns.Add("Edad", "Edad");
            dgv.Columns.Add("Posicion", "Posición");
            dgv.Columns.Add("Equipo", "Equipo");
            dgv.Columns.Add("Correo", "Correo");
            dgv.Columns.Add("Telefono", "Teléfono");
            dgv.Columns["Id"].Width = 40;
            dgv.Columns["Nombre"].Width = 160;
            dgv.Columns["Edad"].Width = 50;
            dgv.Columns["Posicion"].Width = 100;
            dgv.Columns["Equipo"].Width = 110;
            dgv.Columns["Correo"].Width = 190;
            dgv.Columns["Telefono"].Width = 110;
            dgv.CellDoubleClick += Dgv_CellDoubleClick;
            panList.Controls.Add(dgv);

            // Botón eliminar
            btnEliminar = new Button { Text = "🗑️ Eliminar seleccionado", Width = 180 };
            AppTheme.StyleDangerButton(btnEliminar);
            btnEliminar.Click += BtnEliminar_Click;
            panList.Controls.Add(btnEliminar);

            // Ajuste dinámico al cambiar tamaño
            this.Resize += (s, e) =>
            {
                panForm.Height = this.ClientSize.Height - 100;
                panList.Width = this.ClientSize.Width - 430;
                panList.Height = this.ClientSize.Height - 100;
                dgv.Width = panList.ClientSize.Width - 40;
                dgv.Height = panList.ClientSize.Height - 150;
                btnEliminar.Top = dgv.Bottom + 10;
                btnEliminar.Left = 20;
            };
        }

        private TextBox CrearCampo(Panel parent, string label, int y)
        {
            var lbl = new Label { Text = label, Font = AppTheme.FontBold, ForeColor = AppTheme.TextDark, AutoSize = true, Location = new Point(20, y) };
            parent.Controls.Add(lbl);
            var txt = new TextBox { Location = new Point(20, y + 20), Width = 310 };
            AppTheme.StyleTextBox(txt);
            parent.Controls.Add(txt);
            return txt;
        }

        private void CargarGrid(List<Jugador>? lista = null)
        {
            dgv.Rows.Clear();
            var fuente = lista ?? DataStore.Jugadores;
            foreach (var j in fuente)
                dgv.Rows.Add(j.Id, j.NombreCompleto, j.Edad, j.Posicion, j.Equipo, j.Correo, j.Telefono);
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            if (!Validar()) return;

            var jug = new Jugador
            {
                Id = _editandoId,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Edad = int.Parse(txtEdad.Text.Trim()),
                Posicion = cmbPosicion.Text,
                Equipo = txtEquipo.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                FechaRegistro = DateTime.Now
            };

            if (_editandoId < 0)
                DataStore.AgregarJugador(jug);
            else
                DataStore.ActualizarJugador(jug);

            LimpiarFormulario();
            CargarGrid();
            MessageBox.Show("Jugador guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool Validar()
        {
            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) errores.Add("• El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(txtApellido.Text)) errores.Add("• El apellido es obligatorio.");
            if (!int.TryParse(txtEdad.Text.Trim(), out int edad) || edad < 10 || edad > 60) errores.Add("• La edad debe ser un número entre 10 y 60.");
            if (cmbPosicion.SelectedIndex < 0) errores.Add("• Seleccione una posición.");
            if (string.IsNullOrWhiteSpace(txtEquipo.Text)) errores.Add("• El equipo es obligatorio.");
            if (!EsCorreoValido(txtCorreo.Text.Trim())) errores.Add("• El correo electrónico no tiene un formato válido.");
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text) && txtTelefono.Text.Trim().Any(c => !char.IsDigit(c)))
                errores.Add("• El teléfono solo debe contener dígitos.");

            if (errores.Any()) { lblError.Text = string.Join("\n", errores); return false; }
            return true;
        }

        private bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return false;
            try { var addr = new System.Net.Mail.MailAddress(correo); return addr.Address == correo; }
            catch { return false; }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = txtApellido.Text = txtEdad.Text = txtEquipo.Text = txtCorreo.Text = txtTelefono.Text = "";
            cmbPosicion.SelectedIndex = -1;
            lblError.Text = "";
            _editandoId = -1;
            btnGuardar.Text = "💾 Guardar";
        }

        private void Dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value;
            var jug = DataStore.Jugadores.FirstOrDefault(j => j.Id == id);
            if (jug == null) return;
            _editandoId = jug.Id;
            txtNombre.Text = jug.Nombre;
            txtApellido.Text = jug.Apellido;
            txtEdad.Text = jug.Edad.ToString();
            cmbPosicion.Text = jug.Posicion;
            txtEquipo.Text = jug.Equipo;
            txtCorreo.Text = jug.Correo;
            txtTelefono.Text = jug.Telefono;
            btnGuardar.Text = "✏️ Actualizar";
        }

        private void BtnBuscar_Click(object? sender, EventArgs e)
        {
            var texto = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(texto)) { CargarGrid(); return; }
            var resultados = DataStore.Jugadores.Where(j =>
                j.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                j.Equipo.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                j.Posicion.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToList();
            CargarGrid(resultados);
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Seleccione un jugador para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            int id = (int)dgv.SelectedRows[0].Cells["Id"].Value;
            var confirmacion = MessageBox.Show($"¿Está seguro de eliminar al jugador ID {id}?\nSe eliminarán también sus registros de rendimiento.", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmacion == DialogResult.Yes) { DataStore.EliminarJugador(id); CargarGrid(); LimpiarFormulario(); }
        }
    }
}
