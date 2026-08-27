using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class AlarmsViewControl : UserControl
    {
        private readonly MilestoneVmoService _svc;
        private DataGridView _grid;
        private Label _lblTitle;
        private Button _btnAdd;
        private Button _btnRefresh;

        public AlarmsViewControl(MilestoneVmoService svc)
        {
            _svc = svc;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
            RefreshAlarms();
        }

        private void BuildUI()
        {
            // ── Header ──────────────────────────────────────────────────────
            Panel header = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White };
            header.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 235, 245), 1), 0, header.Height - 1, header.Width, header.Height - 1);
            };

            _lblTitle = new Label
            {
                Text = "🚨  Alarm Definitions",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 30, 50),
                Location = new Point(20, 14),
                AutoSize = true
            };
            header.Controls.Add(_lblTitle);

            _btnAdd = new Button
            {
                Text = "+ Add Alarm",
                Size = new Size(110, 30),
                Location = new Point(header.Width - 250, 13),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(14, 165, 233),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnAdd.FlatAppearance.BorderSize = 0;
            _btnAdd.Click += (s, e) => AddAlarmWizard();
            header.Controls.Add(_btnAdd);

            _btnRefresh = new Button
            {
                Text = "↺ Refresh",
                Size = new Size(90, 30),
                Location = new Point(header.Width - 130, 13),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 244, 248),
                ForeColor = Color.FromArgb(50, 60, 80),
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            _btnRefresh.FlatAppearance.BorderSize = 0;
            _btnRefresh.Click += (s, e) => RefreshAlarms();
            header.Controls.Add(_btnRefresh);
            this.Controls.Add(header);

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

            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AlarmId",     HeaderText = "Alarm ID",              FillWeight = 15 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name",        HeaderText = "Alarm Name",            FillWeight = 25 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TriggeredBy", HeaderText = "Triggered By Event",    FillWeight = 25 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AutoClose",   HeaderText = "Auto Close",            FillWeight = 15 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority",    HeaderText = "Priority",              FillWeight = 10 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status",      HeaderText = "Status",                FillWeight = 10 });

            _grid.CellFormatting += Grid_CellFormatting;
            this.Controls.Add(_grid);
        }

        public void RefreshAlarms()
        {
            _grid.Rows.Clear();
            string[][] data = new string[][] {
                new[] { "ALM-001", "Motion in Server Room",      "Motion Detection",      "5 min",    "High",   "🟢 Enabled"  },
                new[] { "ALM-002", "Unauthorized Access – Gate", "Line Crossing",          "10 min",   "High",   "🟢 Enabled"  },
                new[] { "ALM-003", "Camera Tampering",           "Tamper Detection",       "Never",    "Medium", "🟢 Enabled"  },
                new[] { "ALM-004", "Parking Zone Overflow",      "Analytics – Object Count","Never",   "Low",    "🟢 Enabled"  },
                new[] { "ALM-005", "Night Movement Alert",       "Time Profile: Night",    "30 min",   "Medium", "⚫ Disabled"  },
                new[] { "ALM-006", "Fire Alarm Integration",     "I/O Input Rising Edge",  "Immediate","Critical","🟢 Enabled" },
                new[] { "ALM-007", "Speed Violation",            "LPR Speed > 50 km/h",    "Never",    "Medium", "🟢 Enabled"  },
                new[] { "ALM-008", "Loitering Detected",         "Analytics – Dwell Time > 60s","Never","Low",   "🟢 Enabled"  },
            };
            foreach (var r in data) _grid.Rows.Add(r);
            _lblTitle.Text = string.Format("🚨  Alarm Definitions  ({0})", _grid.Rows.Count);
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string v = e.Value.ToString();
                e.CellStyle.ForeColor = v.Contains("Enabled") ? Color.FromArgb(5, 150, 105) : Color.Gray;
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }
            if (_grid.Columns[e.ColumnIndex].Name == "Priority" && e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "Critical": e.CellStyle.ForeColor = Color.FromArgb(180, 30, 30); break;
                    case "High":     e.CellStyle.ForeColor = Color.FromArgb(200, 80, 0);  break;
                    case "Medium":   e.CellStyle.ForeColor = Color.FromArgb(130, 100, 0); break;
                    case "Low":      e.CellStyle.ForeColor = Color.FromArgb(70, 120, 70); break;
                }
            }
        }

        private void AddAlarmWizard()
        {
            Form dlg = new Form
            {
                Text = "Add Alarm Definition",
                Size = new Size(440, 320),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false,
                BackColor = Color.White
            };
            Label lName = new Label { Text = "Alarm Name:", Location = new Point(20,30), AutoSize = true };
            TextBox tName = new TextBox { Location = new Point(20,50), Width = 380, Font = new Font("Segoe UI", 10f) };
            Label lEvt  = new Label { Text = "Triggered By Event:", Location = new Point(20,90), AutoSize = true };
            ComboBox cEvt = new ComboBox { Location = new Point(20,110), Width = 380, DropDownStyle = ComboBoxStyle.DropDownList };
            cEvt.Items.AddRange(new object[]{"Motion Detection","Line Crossing","Tamper Detection","I/O Input","Analytics Event","Time Profile"});
            cEvt.SelectedIndex = 0;
            Label lPri = new Label { Text = "Priority:", Location = new Point(20,150), AutoSize = true };
            ComboBox cPri = new ComboBox { Location = new Point(20,170), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cPri.Items.AddRange(new object[]{"Critical","High","Medium","Low"}); cPri.SelectedIndex = 1;
            Button btnOk = new Button { Text = "Add", Location = new Point(250,230), Size = new Size(80,30),
                FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(14,165,233), ForeColor = Color.White, DialogResult = DialogResult.OK };
            btnOk.FlatAppearance.BorderSize = 0;
            Button btnCx = new Button { Text = "Cancel", Location = new Point(340,230), Size = new Size(70,30),
                FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(230,234,240), DialogResult = DialogResult.Cancel };
            btnCx.FlatAppearance.BorderSize = 0;
            dlg.Controls.AddRange(new Control[]{lName,tName,lEvt,cEvt,lPri,cPri,btnOk,btnCx});
            dlg.AcceptButton = btnOk; dlg.CancelButton = btnCx;
            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(tName.Text))
            {
                int id = _grid.Rows.Count + 1;
                _grid.Rows.Add(string.Format("ALM-{0:D3}", id), tName.Text, cEvt.SelectedItem.ToString(), "Never", cPri.SelectedItem.ToString(), "🟢 Enabled");
                _lblTitle.Text = string.Format("🚨  Alarm Definitions  ({0})", _grid.Rows.Count);
            }
        }
    }
}
