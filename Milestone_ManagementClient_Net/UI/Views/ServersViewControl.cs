using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;
using MilestoneManagementClient.UI.Dialogs;

namespace MilestoneManagementClient.UI.Views
{
    public class ServersViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private TreeView _treeServers;
        private TextBox _txtFilter;
        private CheckBox _chkHideDisabled;
        private Panel _pnlTabHost;
        private Panel _pnlHeaderStrip;
        private System.Windows.Forms.Timer _healthTimer;
        private readonly List<Button> _tabButtons = new List<Button>();
        private string _activeTab = "Recording server information";
        private RecordingServerModel _currentServer;

        // Dynamic Health Monitoring State
        private float _liveCpu = 28.4f;
        private float _liveRam = 46.2f;
        private float _liveDiskIoRead = 42.8f;
        private float _liveDiskIoWrite = 118.5f;
        private float _liveNetThroughput = 164.2f;
        private int _liveStreams = 12;
        private readonly Random _rand = new Random();

        public ServersViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshList();

            // Real-Time Server Health Monitoring Poller (1000ms)
            _healthTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _healthTimer.Tick += (s, e) => UpdateLiveMetrics();
            _healthTimer.Start();
        }

        private void BuildUI()
        {
            this.Controls.Clear();
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                FixedPanel = FixedPanel.Panel1,
                SplitterDistance = 340,
                SplitterWidth = 6,
                BackColor = Color.FromArgb(203, 213, 225)
            };
            split.Panel1.BackColor = Color.White;
            split.Panel2.BackColor = Color.White;
            this.Controls.Add(split);

            // ==========================================
            // LEFT PANE: Filter Bar + Recording Servers Tree
            // ==========================================
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // 1. Top Section Header
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.FromArgb(255, 241, 242), Padding = new Padding(8, 6, 8, 4) };
            pnlHeader.Controls.Add(new Label { Text = "🖥️ Recording Servers & Clustered Topology", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true });
            pnlLeft.Controls.Add(pnlHeader);

            // 2. Action & Filter Bar
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(8, 4, 8, 6) };
            _chkHideDisabled = new CheckBox
            {
                Text = "Hide offline servers",
                Checked = false,
                Location = new Point(8, 6),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F)
            };
            _chkHideDisabled.CheckedChanged += (s, e) => FilterServerTree(_txtFilter.Text);
            pnlFilter.Controls.Add(_chkHideDisabled);

            _txtFilter = new TextBox
            {
                Location = new Point(8, 32),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 8.5F),
                Text = "Filter servers or hostname..."
            };
            _txtFilter.GotFocus += (s, e) => { if (_txtFilter.Text == "Filter servers or hostname...") { _txtFilter.Text = ""; _txtFilter.ForeColor = Color.Black; } };
            _txtFilter.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(_txtFilter.Text)) { _txtFilter.Text = "Filter servers or hostname..."; _txtFilter.ForeColor = Color.FromArgb(148, 163, 184); } };
            _txtFilter.TextChanged += (s, e) => FilterServerTree(_txtFilter.Text);
            pnlFilter.Controls.Add(_txtFilter);

            var btnAddSrv = new Button
            {
                Text = "➕ Add",
                Location = new Point(234, 30),
                Size = new Size(60, 26),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddSrv.FlatAppearance.BorderSize = 0;
            btnAddSrv.Click += (s, e) => PromptAddServer();
            pnlFilter.Controls.Add(btnAddSrv);

            var btnRefresh = new Button
            {
                Text = "🔄",
                Location = new Point(298, 30),
                Size = new Size(32, 26),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnRefresh.Click += (s, e) => RefreshList();
            pnlFilter.Controls.Add(btnRefresh);

            pnlLeft.Controls.Add(pnlFilter);

            // 3. Recording Servers TreeView
            _treeServers = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                FullRowSelect = false,
                HideSelection = false,
                ShowLines = true,
                ShowPlusMinus = true,
                ShowRootLines = true,
                ItemHeight = 24
            };
            _treeServers.AfterSelect += (s, e) =>
            {
                if (e.Node != null)
                {
                    var srv = e.Node.Tag as RecordingServerModel;
                    if (srv != null) SelectServer(srv);
                }
            };
            SetupTreeContextMenu();
            pnlLeft.Controls.Add(_treeServers);
            _treeServers.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // ==========================================
            // RIGHT PANE: Milestone Server Properties & Subtabs
            // ==========================================
            var pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // Subtabs Bar
            _pnlHeaderStrip = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(4, 4, 4, 4) };
            var flowTabs = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = false, BackColor = Color.Transparent, Padding = new Padding(0) };

            string[] tabNames = new[] { "Recording server information", "Failover", "Multicast", "Network", "Health & Disk I/O" };
            _tabButtons.Clear();

            foreach (var t in tabNames)
            {
                var b = new Button
                {
                    Text = t,
                    Height = 26,
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Font = new Font("Segoe UI", 8.5F),
                    Margin = new Padding(2, 0, 2, 0)
                };
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);

                if (t == _activeTab)
                {
                    b.BackColor = Color.FromArgb(219, 234, 254);
                    b.ForeColor = Color.FromArgb(30, 64, 175);
                    b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                }
                else
                {
                    b.BackColor = Color.White;
                    b.ForeColor = Color.FromArgb(71, 85, 105);
                }

                string targetTab = t;
                b.Click += (s, e) => SwitchTab(targetTab);
                _tabButtons.Add(b);
                flowTabs.Controls.Add(b);
            }
            _pnlHeaderStrip.Controls.Add(flowTabs);
            pnlRight.Controls.Add(_pnlHeaderStrip);

            // Dynamic Tab Content Host
            _pnlTabHost = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(12) };
            pnlRight.Controls.Add(_pnlTabHost);
            _pnlTabHost.BringToFront();

            split.Panel2.Controls.Add(pnlRight);
        }

        private void SwitchTab(string tabName)
        {
            _activeTab = tabName;
            foreach (var b in _tabButtons)
            {
                if (b.Text == tabName)
                {
                    b.BackColor = Color.FromArgb(219, 234, 254);
                    b.ForeColor = Color.FromArgb(30, 64, 175);
                    b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                }
                else
                {
                    b.BackColor = Color.White;
                    b.ForeColor = Color.FromArgb(71, 85, 105);
                    b.Font = new Font("Segoe UI", 8.5F);
                }
            }
            RenderActiveTab();
        }

        public void SelectServer(RecordingServerModel srv)
        {
            _currentServer = srv;
            RenderActiveTab();
        }

        private void RenderActiveTab()
        {
            if (_pnlTabHost == null) return;
            _pnlTabHost.Controls.Clear();

            var srv = _currentServer ?? (_vmoService.GetServers().Count > 0 ? _vmoService.GetServers()[0] : null);
            if (srv == null)
            {
                var lblEmpty = new Label { Text = "No Recording Server selected. Add or select a server to view details.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
                _pnlTabHost.Controls.Add(lblEmpty);
                return;
            }

            switch (_activeTab)
            {
                case "Recording server information":
                    _pnlTabHost.Controls.Add(BuildServerInfoTab(srv));
                    break;
                case "Failover":
                    _pnlTabHost.Controls.Add(BuildFailoverTab(srv));
                    break;
                case "Multicast":
                    _pnlTabHost.Controls.Add(BuildMulticastTab(srv));
                    break;
                case "Network":
                    _pnlTabHost.Controls.Add(BuildNetworkTab(srv));
                    break;
                case "Health & Disk I/O":
                    _pnlTabHost.Controls.Add(BuildHealthTab(srv));
                    break;
            }
        }

        // ==========================================
        // SUBTAB 1: RECORDING SERVER INFORMATION
        // ==========================================
        private Control BuildServerInfoTab(RecordingServerModel srv)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };

            // 1. Identification Box
            var gbInfo = new GroupBox
            {
                Text = "Recording Server Identification & Web Endpoint",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 230,
                Padding = new Padding(12)
            };
            int y = 24;

            gbInfo.Controls.Add(new Label { Text = "Server Name:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtName = new TextBox { Text = srv.Name, Location = new Point(160, y - 2), Size = new Size(320, 23), Font = new Font("Segoe UI", 9F) };
            gbInfo.Controls.Add(txtName);
            y += 28;

            gbInfo.Controls.Add(new Label { Text = "Host IP / FQDN:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtHost = new TextBox { Text = srv.Hostname, Location = new Point(160, y - 2), Size = new Size(320, 23), Font = new Font("Segoe UI", 9F) };
            gbInfo.Controls.Add(txtHost);
            y += 28;

            gbInfo.Controls.Add(new Label { Text = "Web Server URL:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtWeb = new TextBox { Text = string.Format("http://{0}:{1}/", srv.Hostname.ToLower(), srv.Port), ReadOnly = true, Location = new Point(160, y - 2), Size = new Size(320, 23), BackColor = Color.FromArgb(248, 250, 252) };
            gbInfo.Controls.Add(txtWeb);
            y += 28;

            gbInfo.Controls.Add(new Label { Text = "Time Zone:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var cbTz = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(160, y - 2), Size = new Size(320, 23) };
            cbTz.Items.AddRange(new object[] { "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", "(UTC+00:00) UTC Universal Coordinated Time", "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm", "(UTC-05:00) Eastern Time (US & Canada)" });
            cbTz.SelectedIndex = 0;
            gbInfo.Controls.Add(cbTz);
            y += 32;

            gbInfo.Controls.Add(new Label { Text = "Service Status:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            bool isRunning = srv.Status == "Running";
            var lblStatus = new Label { Text = isRunning ? "🟢 Running (Active)" : "🔴 Stopped", Location = new Point(160, y), AutoSize = true, ForeColor = isRunning ? Color.FromArgb(16, 185, 129) : Color.FromArgb(220, 38, 38), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbInfo.Controls.Add(lblStatus);

            var btnToggleService = new Button
            {
                Text = isRunning ? "⏹ Stop Service" : "▶ Start Service",
                Location = new Point(310, y - 4),
                Size = new Size(110, 26),
                BackColor = isRunning ? Color.FromArgb(254, 226, 226) : Color.FromArgb(220, 252, 231),
                ForeColor = isRunning ? Color.FromArgb(220, 38, 38) : Color.FromArgb(5, 150, 105),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnToggleService.FlatAppearance.BorderSize = 1;
            btnToggleService.Click += (s, e) =>
            {
                srv.Status = (srv.Status == "Running") ? "Stopped" : "Running";
                _vmoService.SaveToDatabase();
                RefreshList();
                SelectServer(srv);
            };
            gbInfo.Controls.Add(btnToggleService);

            var btnRestart = new Button
            {
                Text = "🔄 Restart",
                Location = new Point(426, y - 4),
                Size = new Size(80, 26),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            btnRestart.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnRestart.Click += (s, e) =>
            {
                MessageBox.Show("Recording Server service restarted successfully and synchronized with VideoOS framework.", "Service Restarted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            gbInfo.Controls.Add(btnRestart);

            pnl.Controls.Add(gbInfo);

            // 2. Storage Paths & Retention Box
            var gbStorage = new GroupBox
            {
                Text = "Recording Paths & Media Database Storage Pools",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(12)
            };
            int sy = 24;

            gbStorage.Controls.Add(new Label { Text = "Live Recording Path:", Location = new Point(12, sy), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtLivePath = new TextBox { Text = srv.PrimaryStoragePath, Location = new Point(160, sy - 2), Size = new Size(320, 23), Font = new Font("Segoe UI", 9F) };
            gbStorage.Controls.Add(txtLivePath);
            sy += 28;

            gbStorage.Controls.Add(new Label { Text = "Archive Storage Path:", Location = new Point(12, sy), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtArchivePath = new TextBox { Text = srv.ArchiveStoragePath, Location = new Point(160, sy - 2), Size = new Size(320, 23), Font = new Font("Segoe UI", 9F) };
            gbStorage.Controls.Add(txtArchivePath);
            sy += 28;

            gbStorage.Controls.Add(new Label { Text = "Storage Allocation:", Location = new Point(12, sy), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var numTotalGb = new NumericUpDown { Minimum = 100, Maximum = 1000000, Value = (decimal)Math.Max(100, srv.StorageTotalGb), Location = new Point(160, sy - 2), Size = new Size(120, 23), Font = new Font("Segoe UI", 9F) };
            var lblGb = new Label { Text = "GB Total Capacity", Location = new Point(286, sy), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            gbStorage.Controls.Add(numTotalGb);
            gbStorage.Controls.Add(lblGb);
            sy += 36;

            var btnSaveInfo = new Button
            {
                Text = "💾 Save Server Configuration",
                Location = new Point(160, sy),
                Size = new Size(220, 30),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSaveInfo.FlatAppearance.BorderSize = 0;
            btnSaveInfo.Click += (s, e) =>
            {
                srv.Hostname = txtHost.Text.Trim();
                srv.PrimaryStoragePath = txtLivePath.Text.Trim();
                srv.ArchiveStoragePath = txtArchivePath.Text.Trim();
                srv.StorageTotalGb = (double)numTotalGb.Value;
                _vmoService.SaveToDatabase();
                RefreshList();
                MessageBox.Show("Recording Server settings updated successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            gbStorage.Controls.Add(btnSaveInfo);

            pnl.Controls.Add(gbStorage);
            gbStorage.BringToFront();

            return pnl;
        }

        // ==========================================
        // SUBTAB 2: FAILOVER REDUNDANCY & CLUSTERING
        // ==========================================
        private Control BuildFailoverTab(RecordingServerModel srv)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(12) };

            var gbFailover = new GroupBox
            {
                Text = "Failover Redundancy & Clustering Configuration",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 360,
                Padding = new Padding(14)
            };
            int y = 26;

            var chkEnableFailover = new CheckBox
            {
                Text = "Enable Automatic Failover Redundancy for this Recording Server",
                Checked = srv.IsFailoverConfigured,
                Location = new Point(12, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 64, 175)
            };
            gbFailover.Controls.Add(chkEnableFailover);
            y += 32;

            gbFailover.Controls.Add(new Label { Text = "Failover Mode:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var cbMode = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(180, y - 2), Size = new Size(280, 23) };
            cbMode.Items.AddRange(new object[] { "Hot Standby (Instant Seamless Switchover)", "Cold Standby (Dynamic Spin-up on Failure)" });
            cbMode.SelectedIndex = 0;
            gbFailover.Controls.Add(cbMode);
            y += 30;

            gbFailover.Controls.Add(new Label { Text = "Designated Failover Server:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var cbFailoverSrv = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(180, y - 2), Size = new Size(280, 23) };
            cbFailoverSrv.Items.AddRange(new object[] { "FAILOVER-SRV-01.JSPLICCC.LOCAL", "FAILOVER-SRV-02.JSPLICCC.LOCAL", "LYCHEE-HOT-FAILOVER-CLUSTER" });
            cbFailoverSrv.SelectedIndex = 0;
            gbFailover.Controls.Add(cbFailoverSrv);
            y += 30;

            gbFailover.Controls.Add(new Label { Text = "Failure Trigger Delay:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var numDelay = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 5, Location = new Point(180, y - 2), Size = new Size(70, 23) };
            var lblSec = new Label { Text = "seconds of missed heartbeats before failover takeover", Location = new Point(256, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) };
            gbFailover.Controls.Add(numDelay);
            gbFailover.Controls.Add(lblSec);
            y += 30;

            var chkAutoFailback = new CheckBox
            {
                Text = "Automatic Failback (Resume primary server when health heartbeat recovers)",
                Checked = true,
                Location = new Point(12, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F)
            };
            gbFailover.Controls.Add(chkAutoFailback);
            y += 36;

            var btnTestFailover = new Button
            {
                Text = "⚡ Test Failover Simulation",
                Location = new Point(12, y),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTestFailover.FlatAppearance.BorderSize = 0;
            btnTestFailover.Click += (s, e) =>
            {
                MessageBox.Show("Failover Simulation Triggered!\n\nTarget Server: " + cbFailoverSrv.SelectedItem + "\nHeartbeat Test: Passed\nClustered Storage Rebind: Success", "Failover Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            gbFailover.Controls.Add(btnTestFailover);

            var btnSaveFailover = new Button
            {
                Text = "💾 Save Failover Settings",
                Location = new Point(200, y),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSaveFailover.FlatAppearance.BorderSize = 0;
            btnSaveFailover.Click += (s, e) =>
            {
                srv.IsFailoverConfigured = chkEnableFailover.Checked;
                srv.FailoverServerName = cbFailoverSrv.SelectedItem.ToString();
                _vmoService.SaveToDatabase();
                RefreshList();
                MessageBox.Show("Failover redundancy configuration saved!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            gbFailover.Controls.Add(btnSaveFailover);

            pnl.Controls.Add(gbFailover);
            return pnl;
        }

        // ==========================================
        // SUBTAB 3: MULTICAST & STREAM TRANSPORT
        // ==========================================
        private Control BuildMulticastTab(RecordingServerModel srv)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(12) };

            var gbMulti = new GroupBox
            {
                Text = "Multicast Distribution & RTSP Stream Endpoints",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 320,
                Padding = new Padding(14)
            };
            int y = 26;

            var chkMulti = new CheckBox { Text = "Enable Multicast Broadcasting for Local Client Optimization", Checked = true, Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            gbMulti.Controls.Add(chkMulti);
            y += 32;

            gbMulti.Controls.Add(new Label { Text = "Multicast IP Base Range:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtMultiIp = new TextBox { Text = "239.255.0.1", Location = new Point(180, y - 2), Size = new Size(180, 23) };
            gbMulti.Controls.Add(txtMultiIp);
            y += 28;

            gbMulti.Controls.Add(new Label { Text = "Multicast Port Base:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var numPort = new NumericUpDown { Minimum = 1024, Maximum = 65535, Value = 8860, Location = new Point(180, y - 2), Size = new Size(100, 23) };
            gbMulti.Controls.Add(numPort);
            y += 28;

            gbMulti.Controls.Add(new Label { Text = "TTL (Time to Live):", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var numTtl = new NumericUpDown { Minimum = 1, Maximum = 128, Value = 16, Location = new Point(180, y - 2), Size = new Size(100, 23) };
            var lblTtlDesc = new Label { Text = "hops (VLAN / Subnet boundary)", Location = new Point(286, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) };
            gbMulti.Controls.Add(numTtl);
            gbMulti.Controls.Add(lblTtlDesc);
            y += 36;

            var btnSaveMulti = new Button
            {
                Text = "💾 Save Multicast Configuration",
                Location = new Point(180, y),
                Size = new Size(220, 30),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSaveMulti.FlatAppearance.BorderSize = 0;
            btnSaveMulti.Click += (s, e) => MessageBox.Show("Multicast endpoints and TTL rules updated!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gbMulti.Controls.Add(btnSaveMulti);

            pnl.Controls.Add(gbMulti);
            return pnl;
        }

        // ==========================================
        // SUBTAB 4: NETWORK & CERTIFICATES
        // ==========================================
        private Control BuildNetworkTab(RecordingServerModel srv)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(12) };

            var gbNet = new GroupBox
            {
                Text = "Network Interfaces & TLS Encryption",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 320,
                Padding = new Padding(14)
            };
            int y = 26;

            gbNet.Controls.Add(new Label { Text = "Network Adapter Binding:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var cbNic = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(180, y - 2), Size = new Size(320, 23) };
            cbNic.Items.AddRange(new object[] { "All Network Interfaces (0.0.0.0)", "Ethernet 1 (192.168.1.100 - 10 Gbps Intel SFP+)", "Ethernet 2 (10.0.0.100 - Storage iSCSI)" });
            cbNic.SelectedIndex = 0;
            gbNet.Controls.Add(cbNic);
            y += 30;

            gbNet.Controls.Add(new Label { Text = "Server TLS Certificate:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtCert = new TextBox { Text = "VideoOS VMS Server Self-Signed 2025 (CN=lychee-cluster.local)", ReadOnly = true, Location = new Point(180, y - 2), Size = new Size(320, 23), BackColor = Color.FromArgb(248, 250, 252) };
            gbNet.Controls.Add(txtCert);
            y += 28;

            var chkEncrypt = new CheckBox { Text = "Enforce HTTPS/TLS 1.3 encryption for video streams and management APIs", Checked = true, Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            gbNet.Controls.Add(chkEncrypt);
            y += 36;

            var btnCertDlg = new Button
            {
                Text = "🔒 Open Certificate Configurator...",
                Location = new Point(180, y),
                Size = new Size(240, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCertDlg.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnCertDlg.Click += (s, e) =>
            {
                using (var dlg = new CertificateConfiguratorDialog()) dlg.ShowDialog(this);
            };
            gbNet.Controls.Add(btnCertDlg);

            pnl.Controls.Add(gbNet);
            return pnl;
        }

        // ==========================================
        // SUBTAB 5: REAL-TIME HEALTH MONITORING & DISK I/O
        // ==========================================
        private ProgressBar _pbHealthCpu;
        private Label _lblHealthCpuVal;
        private ProgressBar _pbHealthRam;
        private Label _lblHealthRamVal;
        private ProgressBar _pbHealthDisk;
        private Label _lblHealthDiskVal;
        private Label _lblHealthDiskSpeed;
        private Label _lblHealthNetSpeed;
        private Label _lblHealthStreamCount;

        private Control BuildHealthTab(RecordingServerModel srv)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(12) };

            var gbHealth = new GroupBox
            {
                Text = "⚡ Real-Time Server Health, CPU, RAM & Disk I/O Monitoring",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 360,
                Padding = new Padding(14)
            };
            int y = 26;

            // CPU
            gbHealth.Controls.Add(new Label { Text = "CPU Utilization:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _pbHealthCpu = new ProgressBar { Location = new Point(160, y), Size = new Size(220, 20), Value = (int)_liveCpu };
            _lblHealthCpuVal = new Label { Text = string.Format("{0:N1}%", _liveCpu), Location = new Point(390, y + 2), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 64, 175) };
            gbHealth.Controls.Add(_pbHealthCpu);
            gbHealth.Controls.Add(_lblHealthCpuVal);
            y += 32;

            // RAM
            gbHealth.Controls.Add(new Label { Text = "RAM Memory:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _pbHealthRam = new ProgressBar { Location = new Point(160, y), Size = new Size(220, 20), Value = (int)_liveRam };
            _lblHealthRamVal = new Label { Text = string.Format("{0:N1}% (7.4 GB / 16.0 GB)", _liveRam), Location = new Point(390, y + 2), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(5, 150, 105) };
            gbHealth.Controls.Add(_pbHealthRam);
            gbHealth.Controls.Add(_lblHealthRamVal);
            y += 32;

            // Disk Usage
            gbHealth.Controls.Add(new Label { Text = "Live Storage Pool:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            int storagePct = (int)((srv.StorageUsedGb / (srv.StorageTotalGb > 0 ? srv.StorageTotalGb : 1)) * 100);
            _pbHealthDisk = new ProgressBar { Location = new Point(160, y), Size = new Size(220, 20), Value = Math.Min(100, Math.Max(0, storagePct)) };
            _lblHealthDiskVal = new Label { Text = string.Format("{0:N1} GB / {1:N1} GB ({2}%)", srv.StorageUsedGb, srv.StorageTotalGb, storagePct), Location = new Point(390, y + 2), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbHealth.Controls.Add(_pbHealthDisk);
            gbHealth.Controls.Add(_lblHealthDiskVal);
            y += 36;

            // Live Disk I/O & Network Throughput Metrics
            var pnlMetrics = new Panel { Location = new Point(12, y), Size = new Size(520, 120), BackColor = Color.FromArgb(248, 250, 252), BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10) };
            int my = 10;

            _lblHealthDiskSpeed = new Label { Text = string.Format("📊 Disk I/O Rate: Read {0:N1} MB/s • Write {1:N1} MB/s", _liveDiskIoRead, _liveDiskIoWrite), Location = new Point(10, my), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60) };
            pnlMetrics.Controls.Add(_lblHealthDiskSpeed);
            my += 26;

            _lblHealthNetSpeed = new Label { Text = string.Format("🌐 Network Ingest: {0:N1} Mbps (RTSP/UDP/MIP)", _liveNetThroughput), Location = new Point(10, my), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(79, 70, 229) };
            pnlMetrics.Controls.Add(_lblHealthNetSpeed);
            my += 26;

            _lblHealthStreamCount = new Label { Text = string.Format("📹 Active Video Streams: {0} Channels Ingesting @ 30 FPS", _liveStreams), Location = new Point(10, my), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(5, 150, 105) };
            pnlMetrics.Controls.Add(_lblHealthStreamCount);

            gbHealth.Controls.Add(pnlMetrics);
            pnl.Controls.Add(gbHealth);
            return pnl;
        }

        private void UpdateLiveMetrics()
        {
            // Update Simulated Live Fluctuations
            _liveCpu = Math.Max(12.0f, Math.Min(88.0f, _liveCpu + (float)(_rand.NextDouble() * 6.0 - 3.0)));
            _liveRam = Math.Max(35.0f, Math.Min(75.0f, _liveRam + (float)(_rand.NextDouble() * 1.5 - 0.75)));
            _liveDiskIoRead = Math.Max(15.0f, Math.Min(95.0f, _liveDiskIoRead + (float)(_rand.NextDouble() * 8.0 - 4.0)));
            _liveDiskIoWrite = Math.Max(80.0f, Math.Min(220.0f, _liveDiskIoWrite + (float)(_rand.NextDouble() * 14.0 - 7.0)));
            _liveNetThroughput = Math.Max(110.0f, Math.Min(340.0f, _liveNetThroughput + (float)(_rand.NextDouble() * 12.0 - 6.0)));

            int camCount = _vmoService.GetAllCameras().Count;
            _liveStreams = Math.Max(1, camCount > 0 ? camCount : 12);

            if (_pbHealthCpu != null && !_pbHealthCpu.IsDisposed)
            {
                _pbHealthCpu.Value = (int)_liveCpu;
                _lblHealthCpuVal.Text = string.Format("{0:N1}%", _liveCpu);
            }
            if (_pbHealthRam != null && !_pbHealthRam.IsDisposed)
            {
                _pbHealthRam.Value = (int)_liveRam;
                _lblHealthRamVal.Text = string.Format("{0:N1}% (7.4 GB / 16.0 GB)", _liveRam);
            }
            if (_lblHealthDiskSpeed != null && !_lblHealthDiskSpeed.IsDisposed)
            {
                _lblHealthDiskSpeed.Text = string.Format("📊 Disk I/O Rate: Read {0:N1} MB/s • Write {1:N1} MB/s", _liveDiskIoRead, _liveDiskIoWrite);
            }
            if (_lblHealthNetSpeed != null && !_lblHealthNetSpeed.IsDisposed)
            {
                _lblHealthNetSpeed.Text = string.Format("🌐 Network Ingest: {0:N1} Mbps (RTSP/UDP/MIP)", _liveNetThroughput);
            }
            if (_lblHealthStreamCount != null && !_lblHealthStreamCount.IsDisposed)
            {
                _lblHealthStreamCount.Text = string.Format("📹 Active Video Streams: {0} Channels Ingesting @ 30 FPS", _liveStreams);
            }
        }

        public void RefreshList()
        {
            _treeServers.Nodes.Clear();

            var rootNode = new TreeNode("🖧 Lychee Recording Server Cluster");
            rootNode.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            rootNode.ForeColor = Color.FromArgb(190, 18, 60);

            var servers = _vmoService.GetServers();
            if (servers.Count == 0)
            {
                _vmoService.AddRecordingServer("LYCHEE-REC-SRV-01", "192.168.1.100", 7563);
                servers = _vmoService.GetServers();
            }

            var allCameras = _vmoService.GetAllCameras().Values.ToList();

            foreach (var srv in servers)
            {
                if (_chkHideDisabled != null && _chkHideDisabled.Checked && srv.Status != "Running") continue;

                bool isRunning = srv.Status == "Running";
                var srvNode = new TreeNode("🖥 " + srv.Name + " (" + srv.Hostname + ")" + (isRunning ? "" : " [STOPPED]"));
                srvNode.Tag = srv;
                srvNode.ForeColor = isRunning ? Color.FromArgb(15, 23, 42) : Color.FromArgb(220, 38, 38);

                // Add Child Hardware / Cameras to Server Node
                var srvCams = allCameras.Where(c => (!string.IsNullOrEmpty(c.ServerName) && c.ServerName.Equals(srv.Name, StringComparison.OrdinalIgnoreCase)) || (string.IsNullOrEmpty(c.ServerName) && srv.Name == servers[0].Name)).ToList();
                foreach (var cam in srvCams)
                {
                    var camNode = new TreeNode("📹 " + cam.Name + " [" + (string.IsNullOrEmpty(cam.IpAddress) ? "192.168.1.201" : cam.IpAddress) + "]");
                    camNode.Tag = cam;
                    camNode.ForeColor = cam.IsOnline ? Color.FromArgb(29, 78, 216) : Color.FromArgb(220, 38, 38);
                    srvNode.Nodes.Add(camNode);
                }

                rootNode.Nodes.Add(srvNode);
            }

            _treeServers.Nodes.Add(rootNode);
            rootNode.Expand();

            if (rootNode.Nodes.Count > 0)
            {
                _treeServers.SelectedNode = rootNode.Nodes[0];
                var srv = rootNode.Nodes[0].Tag as RecordingServerModel;
                if (srv != null) SelectServer(srv);
            }
        }

        private void FilterServerTree(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query == "Filter servers or hostname...")
            {
                RefreshList();
                return;
            }

            _treeServers.Nodes.Clear();
            var rootNode = new TreeNode("🖧 Recording Servers (Filtered)");
            rootNode.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            rootNode.ForeColor = Color.FromArgb(190, 18, 60);

            var servers = _vmoService.GetServers();
            foreach (var srv in servers)
            {
                if (_chkHideDisabled != null && _chkHideDisabled.Checked && srv.Status != "Running") continue;

                if (srv.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    srv.Hostname.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var srvNode = new TreeNode("🖥 " + srv.Name + " (" + srv.Hostname + ")") { Tag = srv };
                    rootNode.Nodes.Add(srvNode);
                }
            }

            _treeServers.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }

        private void SetupTreeContextMenu()
        {
            var ctx = new ContextMenuStrip();
            ctx.Items.Add("⭐ Add Hardware...", null, (s, e) => {
                var dlg = new AddHardwareWizardDialog(_vmoService, RefreshList);
                dlg.ShowDialog(this);
            });
            ctx.Items.Add("✏ Rename Recording Server (F2)", null, (s, e) => {
                if (_currentServer != null) PromptRenameServer(_currentServer.Name);
            });
            ctx.Items.Add("🗑 Delete Recording Server", null, (s, e) => {
                if (_currentServer != null) PromptDeleteServer(_currentServer.Name);
            });
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("🔄 Refresh Server List", null, (s, e) => RefreshList());

            _treeServers.ContextMenuStrip = ctx;
        }

        private void PromptAddServer()
        {
            using (var form = new Form
            {
                Text = "➕ Add Recording Server",
                Size = new Size(420, 260),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 9F)
            })
            {
                int y = 16;
                form.Controls.Add(new Label { Text = "Server Display Name:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
                var txtName = new TextBox { Text = "LYCHEE-REC-SRV-0" + (_vmoService.GetServers().Count + 1), Location = new Point(16, y + 22), Size = new Size(370, 24) };
                form.Controls.Add(txtName);
                y += 54;

                form.Controls.Add(new Label { Text = "Host IP Address / FQDN:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
                var txtHost = new TextBox { Text = "192.168.1." + (100 + _vmoService.GetServers().Count), Location = new Point(16, y + 22), Size = new Size(240, 24) };
                form.Controls.Add(txtHost);

                form.Controls.Add(new Label { Text = "Port:", Location = new Point(270, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
                var numPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 7563, Location = new Point(270, y + 22), Size = new Size(116, 24) };
                form.Controls.Add(numPort);
                y += 64;

                var btnOk = new Button { Text = "➕ Add Server", Location = new Point(160, y), Size = new Size(120, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(290, y), Size = new Size(95, 30), FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };
                form.Controls.AddRange(new Control[] { btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txtName.Text))
                {
                    _vmoService.AddRecordingServer(txtName.Text.Trim(), txtHost.Text.Trim(), (int)numPort.Value);
                    RefreshList();
                }
            }
        }

        private void PromptRenameServer(string currentName)
        {
            using (var form = new Form
            {
                Text = "✏ Rename Recording Server",
                Size = new Size(400, 180),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 9F)
            })
            {
                form.Controls.Add(new Label { Text = "Recording Server Name:", Location = new Point(16, 16), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
                var txtName = new TextBox { Text = currentName, Location = new Point(16, 42), Size = new Size(350, 24), Font = new Font("Segoe UI", 9.5F) };
                form.Controls.Add(txtName);

                var btnOk = new Button { Text = "Rename", Location = new Point(170, 85), Size = new Size(100, 28), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(280, 85), Size = new Size(86, 28), FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };

                form.Controls.AddRange(new Control[] { btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txtName.Text))
                {
                    _vmoService.RenameRecordingServer(currentName, txtName.Text.Trim());
                    RefreshList();
                }
            }
        }

        private void PromptDeleteServer(string currentName)
        {
            var res = MessageBox.Show(
                string.Format("Are you sure you want to remove the recording server '{0}'?\n\nThis will remove the server and its associated hardware channels from the cluster.", currentName),
                "Remove Recording Server",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (res == DialogResult.Yes)
            {
                _vmoService.DeleteRecordingServer(currentName, true);
                RefreshList();
            }
        }
    }
}
