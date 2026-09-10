using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;
using MilestoneManagementClient.UI.Dialogs;
using MilestoneManagementClient.UI.Views;

namespace MilestoneManagementClient.UI.Forms
{
    public class ManagementClientForm : Form
    {
        private readonly MilestoneVmoService _vmoService;
        private TreeView _treeNav;
        private Panel _pnlContentHost;
        private SplitContainer _splitMain;
        private ToolStripStatusLabel _lblStatusHost;
        private ToolStripStatusLabel _lblStatusUser;
        private ToolStripStatusLabel _lblStatusCams;
        private ToolStripStatusLabel _lblStatusAi;
        private System.Windows.Forms.Timer _clockTimer;

        // View Controls
        private CamerasViewControl _viewCameras;
        private ServersViewControl _viewServers;
        private LicenseViewControl _viewLicense;
        private RulesViewControl _viewRules;
        private SecurityViewControl _viewSecurity;
        private DashboardViewControl _viewDashboard;
        private LogsViewControl _viewLogs;
        private AlarmsViewControl _viewAlarms;
        private AnalyticsViewControl _viewAnalytics;
        private MobileViewControl _viewMobile;
        private SiteViewControl _viewSite;
        private AccessControlViewControl _viewAccessControl;
        private IncidentManagerViewControl _viewIncidentManager;
        private SmartMapViewControl _viewSmartMap;
        private FailoverClusterViewControl _viewFailover;

        public ManagementClientForm()
        {
            _vmoService = new MilestoneVmoService();

            this.Text = "LycheeVMS — Intelligent Video Management Client 2025 • [lychee-cluster.local]";
            this.Size = new Size(1366, 800);
            this.MinimumSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.DoubleBuffered = true;

            string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lychee.ico");
            if (!File.Exists(icoPath)) icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (File.Exists(icoPath))
            {
                try { this.Icon = new Icon(icoPath); } catch { }
            }

            this.ShowIcon = true;
            this.ShowInTaskbar = true;

            InitializeViews();
            BuildUI();

            this.Load += (s, e) =>
            {
                try { if (_splitMain != null && _splitMain.Width > 300) _splitMain.SplitterDistance = 240; } catch { }
            };
            this.Shown += (s, e) =>
            {
                try
                {
                    if (_splitMain != null && _splitMain.Width > 300) _splitMain.SplitterDistance = 240;
                    this.BringToFront();
                    this.Activate();
                }
                catch { }
            };
            this.Resize += (s, e) =>
            {
                try
                {
                    if (_splitMain != null && this.ClientSize.Width > 400)
                    {
                        int navW = Math.Max(200, Math.Min(260, (int)(this.ClientSize.Width * 0.18)));
                        _splitMain.SplitterDistance = navW;
                    }
                }
                catch { }
            };

            _clockTimer = new System.Windows.Forms.Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += (s, e) => UpdateStatus();
            _clockTimer.Start();
            UpdateStatus();

            // Default to Cameras view
            SwitchView(_viewCameras);
        }

        public void SetLoginContext(string serverAddress, string username, int authMode)
        {
            string userDisplay = (authMode == 0)
                ? Environment.UserName + " (Windows)"
                : (!string.IsNullOrWhiteSpace(username) ? username : "Administrator");

            if (_lblStatusHost != null) _lblStatusHost.Text = "🌐 " + serverAddress;
            if (_lblStatusUser != null) _lblStatusUser.Text = "👤 " + userDisplay;
            this.Text = "LycheeVMS — Management Client 2025 • [" + serverAddress + "]";
        }

        private void InitializeViews()
        {
            _viewCameras = new CamerasViewControl(_vmoService);
            _viewServers = new ServersViewControl(_vmoService);
            _viewLicense = new LicenseViewControl(_vmoService);
            _viewRules = new RulesViewControl(_vmoService);
            _viewSecurity = new SecurityViewControl(_vmoService);
            _viewDashboard = new DashboardViewControl(_vmoService);
            _viewLogs = new LogsViewControl(_vmoService);
            _viewAlarms = new AlarmsViewControl(_vmoService);
            _viewAnalytics = new AnalyticsViewControl(_vmoService);
            _viewMobile = new MobileViewControl(_vmoService);
            _viewSite = new SiteViewControl(_vmoService);
            _viewAccessControl = new AccessControlViewControl(_vmoService);
            _viewIncidentManager = new IncidentManagerViewControl(_vmoService);
            _viewSmartMap = new SmartMapViewControl(_vmoService);
            _viewFailover = new FailoverClusterViewControl(_vmoService);
        }

        private void BuildUI()
        {
            // Menu Strip
            var menu = new MenuStrip { BackColor = Color.White, Font = new Font("Segoe UI", 9F) };
            var mFile = new ToolStripMenuItem("File");
            mFile.DropDownItems.Add("Connect to Server...", null, (s, e) => ShowLogin());
            mFile.DropDownItems.Add("Disconnect", null, (s, e) => { });
            mFile.DropDownItems.Add(new ToolStripSeparator());
            mFile.DropDownItems.Add("Exit", null, (s, e) => Application.Exit());

            var mView = new ToolStripMenuItem("View");
            mView.DropDownItems.Add("Cameras & Storage", null, (s, e) => SwitchView(_viewCameras));
            mView.DropDownItems.Add("Recording Servers", null, (s, e) => SwitchView(_viewServers));
            mView.DropDownItems.Add("GIS Smart Map", null, (s, e) => SwitchView(_viewSmartMap));
            mView.DropDownItems.Add("System Dashboard", null, (s, e) => SwitchView(_viewDashboard));
            mView.DropDownItems.Add("License Information", null, (s, e) => SwitchView(_viewLicense));

            var mTools = new ToolStripMenuItem("Tools");
            mTools.DropDownItems.Add("Add Hardware Wizard...", null, (s, e) => {
                var wiz = new AddHardwareWizardDialog(_vmoService, () => {
                    _viewCameras.PopulateDevicesTree();
                    _viewServers.RefreshList();
                });
                wiz.ShowDialog(this);
            });
            mTools.DropDownItems.Add("Server Configurator (Certificates)...", null, (s, e) => {
                using (var certDlg = new CertificateConfiguratorDialog())
                {
                    certDlg.ShowDialog(this);
                }
            });
            mTools.DropDownItems.Add("Backup & Restore VMS Configuration...", null, (s, e) => {
                using (var bakDlg = new BackupRestoreDialog())
                {
                    bakDlg.ShowDialog(this);
                }
            });
            mTools.DropDownItems.Add(new ToolStripSeparator());
            mTools.DropDownItems.Add("Options...", null, (s, e) => { });

            var mHelp = new ToolStripMenuItem("Help");
            mHelp.DropDownItems.Add("About LycheeVMS 2025", null, (s, e) =>
                MessageBox.Show("LycheeVMS — Intelligent Video Management Client\nVersion: 2025 R3 (Build 25.3.109)\nIntegration: VideoOS Platform SDK & MSSQL", "About", MessageBoxButtons.OK, MessageBoxIcon.Information));

            menu.Items.AddRange(new ToolStripItem[] { mFile, mView, mTools, mHelp });
            this.MainMenuStrip = menu;
            this.Controls.Add(menu);

            // Main Splitter (Nav Tree on Left, Views on Right)
            _splitMain = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 240,
                SplitterWidth = 4,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Left Navigation Panel
            var pnlNav = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlNavHeader = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(12, 8, 12, 8) };
            var lblNav = new Label { Text = "Site Navigation", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Left, AutoSize = true };
            pnlNavHeader.Controls.Add(lblNav);

            _treeNav = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9.5F),
                ShowLines = true,
                ShowPlusMinus = true,
                ItemHeight = 26,
                BackColor = Color.White
            };
            _treeNav.AfterSelect += TreeNav_AfterSelect;
            BuildNavTree();

            pnlNav.Controls.Add(_treeNav);
            pnlNav.Controls.Add(pnlNavHeader);
            _splitMain.Panel1.Controls.Add(pnlNav);

            // Right Host Panel
            _pnlContentHost = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            _splitMain.Panel2.Controls.Add(_pnlContentHost);

            this.Controls.Add(_splitMain);
            _splitMain.BringToFront();

            // Status Strip
            var status = new StatusStrip { BackColor = Color.FromArgb(241, 245, 249), Font = new Font("Segoe UI", 8.5F) };
            _lblStatusHost = new ToolStripStatusLabel { Text = "🌐 http://lychee-cluster.local:7563", ForeColor = Color.FromArgb(71, 85, 105), BorderSides = ToolStripStatusLabelBorderSides.Right };
            _lblStatusUser = new ToolStripStatusLabel { Text = "👤 Administrator (Superuser)", ForeColor = Color.FromArgb(71, 85, 105), BorderSides = ToolStripStatusLabelBorderSides.Right };
            _lblStatusCams = new ToolStripStatusLabel { Text = "📹 Cameras: 0 Online", ForeColor = Color.FromArgb(5, 150, 105), BorderSides = ToolStripStatusLabelBorderSides.Right };
            _lblStatusAi = new ToolStripStatusLabel { Text = "🧠 Multi-Brand MIP Framework: Active", ForeColor = Color.FromArgb(124, 58, 237), BorderSides = ToolStripStatusLabelBorderSides.Right };

            status.Items.AddRange(new ToolStripItem[] { _lblStatusHost, _lblStatusUser, _lblStatusCams, _lblStatusAi });
            this.Controls.Add(status);
        }

        private void BuildNavTree()
        {
            _treeNav.Nodes.Clear();

            var nCluster = new TreeNode("📁 lychee-cluster.local") { ForeColor = Color.FromArgb(15, 23, 42), NodeFont = new Font("Segoe UI", 9.5F, FontStyle.Bold) };

            var nDash = new TreeNode("📊 System Dashboard") { Tag = _viewDashboard };
            var nDev = new TreeNode("📹 Devices & Cameras") { Tag = _viewCameras };
            var nSrv = new TreeNode("🖥 Recording Servers") { Tag = _viewServers };
            var nFailover = new TreeNode("🛡️ Failover & High Availability") { Tag = _viewFailover };
            var nMap = new TreeNode("🗺️ GIS Smart Map") { Tag = _viewSmartMap };
            var nAccess = new TreeNode("🚪 Access Control & Doors") { Tag = _viewAccessControl };
            var nIncidents = new TreeNode("📁 Incident Cases & Evidence") { Tag = _viewIncidentManager };
            var nRules = new TreeNode("⚡ Rules & Events") { Tag = _viewRules };
            var nSec = new TreeNode("🔐 Security & Users") { Tag = _viewSecurity };
            var nAlarms = new TreeNode("🚨 Alarms & Incidents") { Tag = _viewAlarms };
            var nAi = new TreeNode("🧠 AI Video Analytics") { Tag = _viewAnalytics };
            var nMob = new TreeNode("📱 Mobile & Web Gateway") { Tag = _viewMobile };
            var nLic = new TreeNode("📜 Licenses & Device Packs") { Tag = _viewLicense };
            var nLogs = new TreeNode("📋 System & Audit Logs") { Tag = _viewLogs };
            var nSite = new TreeNode("🌐 Multi-Site Hierarchy") { Tag = _viewSite };

            nCluster.Nodes.AddRange(new[] { nDash, nDev, nSrv, nFailover, nMap, nAccess, nIncidents, nRules, nSec, nAlarms, nAi, nMob, nLic, nLogs, nSite });
            _treeNav.Nodes.Add(nCluster);
            nCluster.Expand();

            _treeNav.SelectedNode = nDev;
        }

        private void TreeNav_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                Control c = e.Node.Tag as Control;
                if (c != null)
                {
                    SwitchView(c);
                }
            }
        }

        private void SwitchView(Control view)
        {
            if (view == null || _pnlContentHost == null) return;
            _pnlContentHost.SuspendLayout();
            _pnlContentHost.Controls.Clear();
            view.Dock = DockStyle.Fill;
            _pnlContentHost.Controls.Add(view);
            _pnlContentHost.ResumeLayout();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_lblStatusCams != null && _vmoService != null)
            {
                int total = _vmoService.GetAllCameras().Count;
                _lblStatusCams.Text = string.Format("📹 Cameras: {0} Active Channels", total);
            }
        }

        private void ShowLogin()
        {
            using (var dlg = new LoginDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    SetLoginContext(dlg.ServerAddress, dlg.Username, (int)dlg.AuthenticationMode);
                }
            }
        }
    }
}
