using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class AnalyticsViewControl : UserControl
    {
        private readonly MilestoneVmoService _svc;
        private DataGridView _grid;
        private Label _lblTitle;
        private Panel _summaryPanel;

        public AnalyticsViewControl(MilestoneVmoService svc)
        {
            _svc = svc;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
            RefreshAnalytics();
        }

        private void BuildUI()
        {
            // ── Header ──────────────────────────────────────────────────────
            Panel header = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White };
            header.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 235, 245), 1), 0, header.Height - 1, header.Width, header.Height - 1);

            _lblTitle = new Label
            {
                Text = "🔍  License Plate Recognition (ANPR) & Analytics",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 30, 50),
                Location = new Point(20, 14),
                AutoSize = true
            };
            header.Controls.Add(_lblTitle);

            Button btnRefresh = new Button
            {
                Text = "↺ Refresh",
                Size = new Size(90, 30),
                Location = new Point(940, 13),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 244, 248),
                ForeColor = Color.FromArgb(50, 60, 80),
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => RefreshAnalytics();
            header.Controls.Add(btnRefresh);
            this.Controls.Add(header);

            // ── KPI Summary Cards ────────────────────────────────────────────
            _summaryPanel = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(14, 10, 14, 6) };
            string[] kpiLabels = { "Total Plates Read", "Blacklist Hits", "Avg Confidence", "Cameras w/ LPR" };
            string[] kpiVals   = { "1,847",             "3",             "97.4%",          "8" };
            Color[]  kpiColors = { Color.FromArgb(14,165,233), Color.FromArgb(239,68,68), Color.FromArgb(16,185,129), Color.FromArgb(99,102,241) };
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                Panel card = new Panel
                {
                    Size = new Size(168, 58),
                    Location = new Point(14 + idx * 185, 10),
                    BackColor = Color.White,
                    Cursor = Cursors.Default
                };
                card.Paint += (s, e) =>
                {
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 225, 235)), 0, 0, card.Width - 1, card.Height - 1);
                    e.Graphics.FillRectangle(new SolidBrush(kpiColors[idx]), 0, 0, 4, card.Height);
                    using (Font vf = new Font("Segoe UI", 18f, FontStyle.Bold))
                        e.Graphics.DrawString(kpiVals[idx], vf, new SolidBrush(kpiColors[idx]), 12, 6);
                    using (Font lf = new Font("Segoe UI", 7.5f))
                        e.Graphics.DrawString(kpiLabels[idx], lf, new SolidBrush(Color.FromArgb(100, 110, 130)), 12, 36);
                };
                _summaryPanel.Controls.Add(card);
            }
            this.Controls.Add(_summaryPanel);

            // ── LPR Records Grid ─────────────────────────────────────────────
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

            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Time",        HeaderText = "Timestamp",             FillWeight = 18 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Camera",      HeaderText = "Camera",                FillWeight = 20 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Plate",       HeaderText = "Plate Number",          FillWeight = 15 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Confidence",  HeaderText = "AI Confidence",         FillWeight = 12 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Vehicle",     HeaderText = "Vehicle Classification", FillWeight = 15 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Speed",       HeaderText = "Speed",                 FillWeight = 10 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Blacklist",   HeaderText = "Blacklist",             FillWeight = 10 });

            _grid.CellFormatting += Grid_CellFormatting;
            this.Controls.Add(_grid);
        }

        public void RefreshAnalytics()
        {
            _grid.Rows.Clear();
            Random rnd = new Random(42);
            string[] cams = { "CAM-01 North Gate", "CAM-02 South Exit", "CAM-03 Parking A", "CAM-04 Main Entrance", "CAM-06 Loading Bay" };
            string[] plates = { "MH12AB1234", "DL01XY9988", "KA03ZZ4567", "TN09PQ7777", "GJ05MN3322", "HR26AB8800", "UP32CD5501", "RJ14FG2293" };
            string[] vehicles = { "Sedan", "SUV", "Truck", "Motorcycle", "Bus", "Van" };
            DateTime now = DateTime.Now;
            for (int i = 0; i < 18; i++)
            {
                string plate = plates[rnd.Next(plates.Length)];
                bool blacklist = plate == "DL01XY9988" || plate == "UP32CD5501" || plate == "RJ14FG2293";
                _grid.Rows.Add(
                    now.AddMinutes(-i * 7).ToString("HH:mm:ss  dd-MMM"),
                    cams[rnd.Next(cams.Length)],
                    plate,
                    string.Format("{0:F1}%", 92 + rnd.NextDouble() * 7.9),
                    vehicles[rnd.Next(vehicles.Length)],
                    string.Format("{0} km/h", 20 + rnd.Next(60)),
                    blacklist ? "⚠ HIT" : "—"
                );
            }
            _lblTitle.Text = string.Format("🔍  ANPR & Analytics  ({0} records)", _grid.Rows.Count);
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name == "Plate" && e.Value != null)
            {
                e.CellStyle.Font = new Font("Consolas", 10f, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(14, 100, 200);
            }
            if (_grid.Columns[e.ColumnIndex].Name == "Blacklist" && e.Value != null)
            {
                string v = e.Value.ToString();
                if (v.Contains("HIT"))
                {
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.BackColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
            }
        }
    }
}
