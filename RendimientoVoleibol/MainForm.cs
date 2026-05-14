using RendimientoVoleibol.Forms;

namespace RendimientoVoleibol
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ConfigurarVentana();
        }

        private void ConfigurarVentana()
        {
            this.Text = "Sistema de Análisis de Rendimiento Deportivo — Voleibol";
            this.BackColor = AppTheme.BgLight;
            this.MinimumSize = new Size(1050, 680);
            this.Size = new Size(1200, 780);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.Font = AppTheme.FontBody;
            this.WindowState = FormWindowState.Maximized;

            BuildSidebar();
            BuildTopBar();
        }

        private Panel? _sidebar;

        private void BuildTopBar()
        {
            var topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = AppTheme.PrimaryDark,
                Padding = new Padding(10, 0, 10, 0)
            };

            var lblApp = new Label
            {
                Text = "🏐  RENDIMIENTO VOLEIBOL  |  Sistema IoT + Big Data",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Dock = DockStyle.Left
            };
            lblApp.Padding = new Padding(10, 14, 0, 0);

            var lblFecha = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                Font = AppTheme.FontSmall,
                ForeColor = Color.FromArgb(170, 200, 230),
                BackColor = Color.Transparent,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Width = 220
            };

            topBar.Controls.Add(lblFecha);
            topBar.Controls.Add(lblApp);
            this.Controls.Add(topBar);
            topBar.BringToFront();
        }

        private void BuildSidebar()
        {
            _sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 210,
                BackColor = AppTheme.PrimaryDark,
                Padding = new Padding(0, 60, 0, 0)
            };

            var menuItems = new[]
            {
                ("🏠", "Inicio / Dashboard", (Action)AbrirDashboard),
                ("👤", "Gestión de Jugadores", (Action)AbrirJugadores),
                ("📊", "Registro de Rendimiento", (Action)AbrirRegistroRendimiento),
                ("🔍", "Búsqueda y Análisis", (Action)AbrirBusqueda),
                ("📈", "Reportes", (Action)AbrirReportes),
                ("ℹ️", "Acerca del Sistema", (Action)AbrirAcerca),
            };

            int yPos = 10;
            foreach (var (icon, label, action) in menuItems)
            {
                var btn = new Button
                {
                    Text = $"  {icon}  {label}",
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.FromArgb(200, 220, 240),
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 9.5f),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Width = 210,
                    Height = 46,
                    Left = 0,
                    Top = yPos,
                    Cursor = Cursors.Hand,
                    Tag = action
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = AppTheme.PrimaryMid;
                btn.Click += (s, e) => ((Action)((Button)s!).Tag!)();
                _sidebar.Controls.Add(btn);
                yPos += 47;

                // Separador fino
                var sep = new Panel { BackColor = Color.FromArgb(40, 80, 120), Height = 1, Width = 180, Left = 15, Top = yPos - 1 };
                _sidebar.Controls.Add(sep);
            }

            this.Controls.Add(_sidebar);
        }

        // --- Navegación ---
        private void AbrirDashboard() => AbrirFormulario(new DashboardForm());
        private void AbrirJugadores() => AbrirFormulario(new JugadoresForm());
        private void AbrirRegistroRendimiento() => AbrirFormulario(new RegistroRendimientoForm());
        private void AbrirBusqueda() => AbrirFormulario(new BusquedaForm());
        private void AbrirReportes() => AbrirFormulario(new ReportesForm());
        private void AbrirAcerca() => AbrirFormulario(new AcercaForm());

        private void AbrirFormulario(Form form)
        {
            // Cerrar formularios hijos del mismo tipo si ya están abiertos
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == form.GetType())
                {
                    f.Activate();
                    form.Dispose();
                    return;
                }
            }
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AbrirDashboard();
        }
    }

    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent() { this.SuspendLayout(); this.AutoScaleMode = AutoScaleMode.Font; this.ResumeLayout(false); }
    }
}
