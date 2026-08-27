using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class LogsViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private ListView _lvLogs;
        private TextBox _txtSearch;
        private ComboBox _cbLevelFilter;

        public LogsViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshLogs();
        }

        private void BuildUI()
        {
            var pnlToolbar = new Panel { Dock = DockStyle.Top, Height = 38, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 6, 8, 6) };

            pnlToolbar.Controls.Add(new Label { Text = "Filter Level:", Location = new Point(8, 10), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbLevelFilter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(85, 6), Size = new Size(110, 24) };
            _cbLevelFilter.Items.AddRange(new object[] { "ALL LEVELS", "INFO", "WARN", "ERROR" });
            _cbLevelFilter.SelectedIndex = 0;
            _cbLevelFilter.SelectedIndexChanged += (s, e) => RefreshLogs();
            pnlToolbar.Controls.Add(_cbLevelFilter);

            pnlToolbar.Controls.Add(new Label { Text = "Search Logs:", Location = new Point(210, 10), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtSearch = new TextBox { Location = new Point(290, 6), Size = new Size(220, 24) };
            _txtSearch.TextChanged += (s, e) => RefreshLogs();
            pnlToolbar.Controls.Add(_txtSearch);

            var btnRefresh = new Button
            {
                Text = "🔄 Refresh (F5)",
                Location = new Point(520, 5),
                Size = new Size(110, 26),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => RefreshLogs();
            pnlToolbar.Controls.Add(btnRefresh);

            this.Controls.Add(pnlToolbar);

            _lvLogs = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F)
            };
            _lvLogs.Columns.Add("Timestamp", 160);
            _lvLogs.Columns.Add("Level", 80);
            _lvLogs.Columns.Add("Source Subsystem", 150);
            _lvLogs.Columns.Add("Event Message", 420);
            _lvLogs.Columns.Add("User Identity", 120);

            this.Controls.Add(_lvLogs);
            _lvLogs.BringToFront();
        }

        public void RefreshLogs()
        {
            _lvLogs.Items.Clear();
            string lvl = _cbLevelFilter != null && _cbLevelFilter.SelectedItem != null ? _cbLevelFilter.SelectedItem.ToString() : "ALL LEVELS";
            string q = _txtSearch != null ? _txtSearch.Text.Trim().ToLower() : "";

            var logs = _vmoService.GetLogs();
            foreach (var log in logs)
            {
                if (lvl != "ALL LEVELS" && log.Level != lvl) continue;
                if (!string.IsNullOrEmpty(q) && !log.Message.ToLower().Contains(q) && !log.Source.ToLower().Contains(q)) continue;

                var item = new ListViewItem(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(log.Level);
                item.SubItems.Add(log.Source);
                item.SubItems.Add(log.Message);
                item.SubItems.Add(log.User);

                if (log.Level == "ERROR") item.ForeColor = Color.Red;
                else if (log.Level == "WARN") item.ForeColor = Color.DarkOrange;
                else item.ForeColor = Color.FromArgb(30, 41, 59);

                _lvLogs.Items.Add(item);
            }
        }
    }
}
