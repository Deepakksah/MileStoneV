using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class DashboardViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private ListView _lvTasks;
        private Label _lblTotalCams;
        private Label _lblActiveServers;
        private Label _lblCpuAvg;
        private Label _lblStorageTotal;
        private System.Windows.Forms.Timer _refreshTimer;

        public DashboardViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshDashboard();

            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 2000;
            _refreshTimer.Tick += (s, e) => RefreshDashboard();
            _refreshTimer.Start();
        }

        private void BuildUI()
        {
            var pnlContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(16) };
            this.Controls.Add(pnlContainer);

            // KPI Cards Panel
            var pnlCards = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(0, 0, 0, 12)
            };
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            // Card 1: Cameras
            var c1 = CreateCard("TOTAL CAMERAS", "26 Channels Active", Color.FromArgb(0, 102, 204), out _lblTotalCams);
            pnlCards.Controls.Add(c1, 0, 0);

            // Card 2: Recording Servers
            var c2 = CreateCard("RECORDING SERVERS", "4 Online (100%)", Color.FromArgb(16, 185, 129), out _lblActiveServers);
            pnlCards.Controls.Add(c2, 1, 0);

            // Card 3: CPU & RAM
            var c3 = CreateCard("SYSTEM CPU / RAM", "CPU: 28% | RAM: 42%", Color.FromArgb(124, 58, 237), out _lblCpuAvg);
            pnlCards.Controls.Add(c3, 2, 0);

            // Card 4: Total Storage
            var c4 = CreateCard("STORAGE OCCUPIED", "24.5 TB / 88.0 TB", Color.FromArgb(217, 119, 6), out _lblStorageTotal);
            pnlCards.Controls.Add(c4, 3, 0);

            pnlContainer.Controls.Add(pnlCards);

            // Tasks List
            var gbTasks = new GroupBox { Text = "📋 Active System Tasks & Storage Jobs", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(10) };

            _lvTasks = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F)
            };
            _lvTasks.Columns.Add("Task Description", 280);
            _lvTasks.Columns.Add("Target Node", 130);
            _lvTasks.Columns.Add("Progress", 120);
            _lvTasks.Columns.Add("Execution Status", 130);
            _lvTasks.Columns.Add("Started At", 160);

            gbTasks.Controls.Add(_lvTasks);
            pnlContainer.Controls.Add(gbTasks);
            gbTasks.BringToFront();
        }

        private Panel CreateCard(string title, string value, Color accentColor, out Label lblValue)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(12),
                Margin = new Padding(4)
            };

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Top,
                AutoSize = true
            };

            lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = accentColor,
                Dock = DockStyle.Bottom,
                AutoSize = true
            };

            pnl.Controls.Add(lblT);
            pnl.Controls.Add(lblValue);
            return pnl;
        }

        public void RefreshDashboard()
        {
            int camCount = _vmoService.GetAllCameras().Count;
            _lblTotalCams.Text = camCount + " Channels Active";

            int srvCount = _vmoService.GetServers().Count;
            _lblActiveServers.Text = srvCount + " Online (100%)";

            _lvTasks.Items.Clear();
            foreach (var t in _vmoService.GetTasks())
            {
                var item = new ListViewItem(t.Name);
                item.SubItems.Add(t.TargetServer);
                item.SubItems.Add(t.ProgressPercent + "%");
                item.SubItems.Add(t.Status);
                item.SubItems.Add(t.StartedAt.ToString("HH:mm:ss dd-MMM"));
                item.Tag = t;
                _lvTasks.Items.Add(item);
            }
        }
    }
}
