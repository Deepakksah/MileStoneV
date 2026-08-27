using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class MobileViewControl : UserControl
    {
        private readonly MilestoneVmoService _svc;
        private DataGridView _grid;
        private Label _lblTitle;
        private Label _lblPort;
        private System.Windows.Forms.Timer _refreshTimer;

        public MobileViewControl(MilestoneVmoService svc)
        {
            _svc = svc;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
            RefreshSessions();

            _refreshTimer = new System.Windows.Forms.Timer { Interval = 8000 };
            _refreshTimer.Tick += (s, e) => RefreshSessions();
            _refreshTimer.Start();
        }

        private void BuildUI()
        {
            // ── Header ──────────────────────────────────────────────────────
            Panel header = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White };
            header.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 235, 245), 1), 0, header.Height - 1, header.Width, header.Height - 1);

            _lblTitle = new Label
            {
                Text = "📱  Mobile Server Gateway & WebRTC Streaming",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 30, 50),
                Location = new Point(20, 14),
                AutoSize = true
            };
            header.Controls.Add(_lblTitle);
            this.Controls.Add(header);

            // ── Info bar ─────────────────────────────────────────────────────
            Panel infoBar = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(240, 249, 255), Padding = new Padding(16, 0, 0, 0) };
            infoBar.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(186, 230, 253), 1), 0, 0, infoBar.Width, 0);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(186, 230, 253), 1), 0, infoBar.Height - 1, infoBar.Width, infoBar.Height - 1);
            };

            Label lblPortLbl = new Label
            {
                Text = "📡  Mobile Server (HTTPS):",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(14, 100, 200),
                Location = new Point(16, 16), AutoSize = true
            };
            _lblPort = new Label
            {
                Text = "Port 443  |  WebRTC Port 9000  |  HLS Port 7563  |  Status: 🟢 Running",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(30, 50, 80),
                Location = new Point(210, 16), AutoSize = true
            };
            infoBar.Controls.Add(lblPortLbl);
            infoBar.Controls.Add(_lblPort);
            this.Controls.Add(infoBar);

            // ── Grid ────────────────────────────────────────────────────────
            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.FromArgb(245, 247, 250),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                GridColor = Color.FromArgb(230, 235, 245),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            _grid.ColumnHeadersHeight = 36;
            _grid.EnableHeadersVisualStyles = false;
            _grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            _grid.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            _grid.RowTemplate.Height = 32;

            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "SessionId",   HeaderText = "Session ID",             FillWeight = 20 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "UserId",      HeaderText = "User / Account",         FillWeight = 20 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Platform",    HeaderText = "Client Type",            FillWeight = 18 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Streams",     HeaderText = "Active Video Streams",   FillWeight = 20 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Bandwidth",   HeaderText = "Bandwidth",              FillWeight = 12 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Connected",   HeaderText = "Connected Since",        FillWeight = 10 });

            _grid.CellFormatting += Grid_CellFormatting;
            this.Controls.Add(_grid);
        }

        public void RefreshSessions()
        {
            if (_grid.InvokeRequired) { _grid.Invoke(new Action(RefreshSessions)); return; }
            _grid.Rows.Clear();
            Random rnd = new Random();
            string[][] data = new string[][] {
                new[] { "MSS-7A3F", "deepak.sharma@icomplylive.com",  "📱 iOS – Milestone Mobile",    "3 streams (WebRTC)", string.Format("{0} Mbps", (2.1 + rnd.NextDouble()*1.5).ToString("F1")), "08:42 AM" },
                new[] { "MSS-8B2E", "admin@milestone.corp",           "🖥️ Web Client (Chrome)",       "6 streams (HLS)",    string.Format("{0} Mbps", (4.5 + rnd.NextDouble()*2.0).ToString("F1")), "09:15 AM" },
                new[] { "MSS-9C1D", "security.ops@icomplylive.com",   "📱 Android – Milestone Mobile", "2 streams (WebRTC)", string.Format("{0} Mbps", (1.2 + rnd.NextDouble()*0.8).ToString("F1")), "10:03 AM" },
                new[] { "MSS-4D5A", "guard01@site",                   "📺 Smart TV – Kiosk Mode",     "8 streams (HLS)",    string.Format("{0} Mbps", (6.0 + rnd.NextDouble()*2.5).ToString("F1")), "11:00 AM" },
                new[] { "MSS-2E6B", "vms.viewer@corp.net",            "🖥️ Web Client (Edge)",         "4 streams (HLS)",    string.Format("{0} Mbps", (3.2 + rnd.NextDouble()*1.2).ToString("F1")), "11:22 AM" },
            };
            foreach (var r in data) _grid.Rows.Add(r);
            _lblTitle.Text = string.Format("📱  Mobile Server Gateway & WebRTC Streaming  ({0} active sessions)", _grid.Rows.Count);
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name == "Streams" && e.Value != null)
            {
                e.CellStyle.ForeColor = Color.FromArgb(14, 100, 200);
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _refreshTimer != null) _refreshTimer.Stop();
            base.Dispose(disposing);
        }
    }
}
