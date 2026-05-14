namespace RendimientoVoleibol
{
    /// <summary>
    /// Paleta de colores y estilos consistentes para toda la aplicación.
    /// </summary>
    public static class AppTheme
    {
        // Paleta principal: azul marino + naranja deportivo
        public static readonly Color PrimaryDark   = Color.FromArgb(15, 52, 96);    // Azul marino profundo
        public static readonly Color PrimaryMid    = Color.FromArgb(22, 96, 168);   // Azul medio
        public static readonly Color PrimaryLight  = Color.FromArgb(52, 152, 219);  // Azul claro
        public static readonly Color Accent        = Color.FromArgb(230, 126, 34);  // Naranja deportivo
        public static readonly Color AccentLight   = Color.FromArgb(255, 165, 80);  // Naranja claro
        public static readonly Color Success       = Color.FromArgb(39, 174, 96);   // Verde éxito
        public static readonly Color Danger        = Color.FromArgb(192, 57, 43);   // Rojo peligro
        public static readonly Color Warning       = Color.FromArgb(243, 156, 18);  // Amarillo advertencia
        public static readonly Color BgLight       = Color.FromArgb(236, 240, 245); // Fondo claro
        public static readonly Color BgWhite       = Color.White;
        public static readonly Color TextDark      = Color.FromArgb(30, 39, 46);    // Texto oscuro
        public static readonly Color TextGray      = Color.FromArgb(120, 140, 160); // Texto secundario
        public static readonly Color BorderColor   = Color.FromArgb(189, 205, 220); // Bordes

        // Fuentes
        public static readonly Font FontTitle    = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static readonly Font FontSubtitle = new Font("Segoe UI", 11f, FontStyle.Bold);
        public static readonly Font FontBody     = new Font("Segoe UI", 9.5f);
        public static readonly Font FontSmall    = new Font("Segoe UI", 8.5f);
        public static readonly Font FontBold     = new Font("Segoe UI", 9.5f, FontStyle.Bold);

        // Helpers para aplicar estilo a controles
        public static void StyleButton(Button btn, bool isPrimary = true)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = isPrimary ? PrimaryMid : Accent;
            btn.ForeColor = Color.White;
            btn.Font = FontBold;
            btn.Cursor = Cursors.Hand;
            btn.Height = 36;
            btn.FlatAppearance.MouseOverBackColor = isPrimary ? PrimaryLight : AccentLight;
        }

        public static void StyleDangerButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Danger;
            btn.ForeColor = Color.White;
            btn.Font = FontBold;
            btn.Cursor = Cursors.Hand;
            btn.Height = 36;
        }

        public static void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = BorderColor;
            btn.FlatAppearance.BorderSize = 1;
            btn.BackColor = BgWhite;
            btn.ForeColor = TextDark;
            btn.Font = FontBold;
            btn.Cursor = Cursors.Hand;
            btn.Height = 36;
        }

        public static void StyleTextBox(TextBox txt)
        {
            txt.Font = FontBody;
            txt.BackColor = BgWhite;
            txt.ForeColor = TextDark;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Height = 28;
        }

        public static void StyleComboBox(ComboBox cmb)
        {
            cmb.Font = FontBody;
            cmb.BackColor = BgWhite;
            cmb.ForeColor = TextDark;
            cmb.FlatStyle = FlatStyle.Flat;
        }

        public static void StyleLabel(Label lbl, bool isTitle = false)
        {
            lbl.Font = isTitle ? FontSubtitle : FontBody;
            lbl.ForeColor = isTitle ? PrimaryDark : TextDark;
            lbl.BackColor = Color.Transparent;
        }

        public static void StyleDataGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = BgWhite;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = BorderColor;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.Font = FontBody;
            dgv.DefaultCellStyle.BackColor = BgWhite;
            dgv.DefaultCellStyle.ForeColor = TextDark;
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 253);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryDark;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = FontBold;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 4, 4, 4);
            dgv.ColumnHeadersHeight = 38;
            dgv.RowTemplate.Height = 32;
            dgv.EnableHeadersVisualStyles = false;
        }

        public static Panel CreateHeaderPanel(string titulo, string subtitulo = "")
        {
            var panel = new Panel
            {
                BackColor = PrimaryDark,
                Height = 70,
                Dock = DockStyle.Top,
                Padding = new Padding(20, 10, 20, 10)
            };
            var lblTitle = new Label
            {
                Text = titulo,
                Font = FontTitle,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, 10)
            };
            panel.Controls.Add(lblTitle);
            if (!string.IsNullOrEmpty(subtitulo))
            {
                var lblSub = new Label
                {
                    Text = subtitulo,
                    Font = FontSmall,
                    ForeColor = Color.FromArgb(170, 200, 230),
                    BackColor = Color.Transparent,
                    AutoSize = true,
                    Location = new Point(22, 42)
                };
                panel.Controls.Add(lblSub);
            }
            return panel;
        }
    }
}
