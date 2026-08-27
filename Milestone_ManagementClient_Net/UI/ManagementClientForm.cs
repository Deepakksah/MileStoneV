using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;
using MilestoneManagementClient.UI.Views;

namespace MilestoneManagementClient.UI
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
                ? System.Security.Principal.WindowsIdentity.GetCurrent().Name
                : username;

            this.Text = string.Format("LycheeVMS — Intelligent Video Management Client 2025 • [{0}]", serverAddress);
            if (_lblStatusHost != null) _lblStatusHost.Text = "LycheeVMS Active: " + serverAddress;
            if (_lblStatusUser != null) _lblStatusUser.Text = "👤 " + userDisplay;
        }

        private void InitializeViews()
        {
            _viewCameras   = new CamerasViewControl(_vmoService);
            _viewServers   = new ServersViewControl(_vmoService);
            _viewLicense   = new LicenseViewControl(_vmoService);
            _viewRules     = new RulesViewControl(_vmoService);
            _viewSecurity  = new SecurityViewControl(_vmoService);
            _viewDashboard = new DashboardViewControl(_vmoService);
            _viewLogs      = new LogsViewControl(_vmoService);
            _viewAlarms    = new AlarmsViewControl(_vmoService);
            _viewAnalytics = new AnalyticsViewControl(_vmoService);
            _viewMobile    = new MobileViewControl(_vmoService);
            _viewSite      = new SiteViewControl(_vmoService);
        }

        private void BuildUI()
        {
            string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lychee.ico");
            if (!File.Exists(icoPath)) icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");

            // 1. Top Crimson / Ruby Branding Header Bar
            var pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 42;
            pnlHeader.BackColor = Color.FromArgb(190, 18, 60); // Crimson / Ruby Red
            pnlHeader.Padding = new Padding(12, 0, 12, 0);

            var pbLogo = new PictureBox
            {
                Size = new Size(26, 26),
                Location = new Point(12, 8),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            if (File.Exists(icoPath)) try { pbLogo.Image = new Icon(icoPath, 26, 26).ToBitmap(); } catch { }
            pnlHeader.Controls.Add(pbLogo);

            var lblLogo = new Label();
            lblLogo.Text = "LycheeVMS";
            lblLogo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(44, 8);

            var lblSubTitle = new Label();
            lblSubTitle.Text = "Intelligent VMS Management Client 2025 R3";
            lblSubTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblSubTitle.ForeColor = Color.FromArgb(254, 205, 211); // Rose tint
            lblSubTitle.AutoSize = true;
            lblSubTitle.Location = new Point(165, 11);

            var pnlBadge = new Panel();
            pnlBadge.Dock = DockStyle.Right;
            pnlBadge.Width = 200;
            pnlBadge.BackColor = Color.Transparent;

            var lblBadge = new Label();
            lblBadge.Text = "● Enterprise Cluster Online";
            lblBadge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblBadge.ForeColor = Color.White;
            lblBadge.BackColor = Color.FromArgb(159, 18, 57);
            lblBadge.Padding = new Padding(8, 4, 8, 4);
            lblBadge.AutoSize = true;
            lblBadge.Location = new Point(20, 9);
            pnlBadge.Controls.Add(lblBadge);

            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblSubTitle);
            pnlHeader.Controls.Add(pnlBadge);
            this.Controls.Add(pnlHeader);

            // 2. Action Ribbon Toolbar
            var pnlToolbar = new Panel();
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Height = 40;
            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Padding = new Padding(8, 5, 8, 5);

            var btnSaveConfig = CreateRibbonButton("💾 Save Configuration", true);
            btnSaveConfig.Location = new Point(8, 5);
            btnSaveConfig.Click += (s, e) => _viewCameras.SaveCurrentCamera();

            var btnRefreshHw = CreateRibbonButton("🔄 Refresh Hardware", false);
            btnRefreshHw.Location = new Point(160, 5);
            btnRefreshHw.Click += (s, e) => RefreshActiveView();

            var btnAddCam = CreateRibbonButton("➕ Add Lychee Camera", true);
            btnAddCam.Location = new Point(310, 5);
            btnAddCam.Click += (s, e) => _viewCameras.PromptAddDeviceGroup();

            var btnAddSrv = CreateRibbonButton("🖧 Add Recording Server", false);
            btnAddSrv.Location = new Point(485, 5);
            btnAddSrv.Click += (s, e) => SwitchView(_viewServers);

            pnlToolbar.Controls.Add(btnSaveConfig);
            pnlToolbar.Controls.Add(btnRefreshHw);
            pnlToolbar.Controls.Add(btnAddCam);
            pnlToolbar.Controls.Add(btnAddSrv);
            this.Controls.Add(pnlToolbar);

            // 3. Status Bar
            var sb = new StatusStrip();
            sb.BackColor = Color.FromArgb(248, 250, 252);
            sb.Padding = new Padding(6, 2, 6, 2);

            _lblStatusHost = new ToolStripStatusLabel("LycheeVMS Active: lychee-cluster.local");
            _lblStatusHost.ForeColor = Color.FromArgb(71, 85, 105);
            _lblStatusHost.Font = new Font("Segoe UI", 9F);

            _lblStatusUser = new ToolStripStatusLabel("👤 Administrator");
            _lblStatusUser.ForeColor = Color.FromArgb(71, 85, 105);

            _lblStatusCams = new ToolStripStatusLabel("📹 Active Cameras: 2");
            _lblStatusCams.ForeColor = Color.FromArgb(190, 18, 60);
            _lblStatusCams.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            _lblStatusAi = new ToolStripStatusLabel("Real-time AI Video Analytics Pipeline: [Running]");
            _lblStatusAi.ForeColor = Color.FromArgb(5, 150, 105);
            _lblStatusAi.Spring = true;
            _lblStatusAi.TextAlign = ContentAlignment.MiddleRight;

            sb.Items.Add(_lblStatusHost);
            sb.Items.Add(new ToolStripSeparator());
            sb.Items.Add(_lblStatusUser);
            sb.Items.Add(new ToolStripSeparator());
            sb.Items.Add(_lblStatusCams);
            sb.Items.Add(new ToolStripSeparator());
            sb.Items.Add(_lblStatusAi);
            this.Controls.Add(sb);

            // 4. Main Splitter Layout (Visible Draggable Divider Line)
            _splitMain = new SplitContainer();
            _splitMain.Dock = DockStyle.Fill;
            _splitMain.Orientation = Orientation.Vertical;
            _splitMain.FixedPanel = FixedPanel.Panel1;
            _splitMain.Panel1MinSize = 25;
            _splitMain.Panel2MinSize = 25;
            _splitMain.SplitterWidth = 6;
            _splitMain.BackColor = Color.FromArgb(203, 213, 225); // Visible Slate 300 Divider Line
            _splitMain.Panel1.BackColor = Color.White;
            _splitMain.Panel2.BackColor = Color.White;
            this.Controls.Add(_splitMain);
            _splitMain.BringToFront();
            try { if (_splitMain.Width > 300) _splitMain.SplitterDistance = 240; } catch { }

            // 4.1 Left Panel: Site Navigation
            var pnlNav = new Panel();
            pnlNav.Dock = DockStyle.Fill;
            pnlNav.BackColor = Color.White;
            var pnlNavHeader = new Panel();
            pnlNavHeader.Dock = DockStyle.Top;
            pnlNavHeader.Height = 28;
            pnlNavHeader.BackColor = Color.FromArgb(255, 241, 242); // Soft Rose Header
            var lblNavTitle = new Label();
            lblNavTitle.Text = "Site Navigation";
            lblNavTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNavTitle.ForeColor = Color.FromArgb(190, 18, 60); // Crimson text
            lblNavTitle.Dock = DockStyle.Left;
            lblNavTitle.AutoSize = true;
            lblNavTitle.Padding = new Padding(8, 5, 0, 0);
            pnlNavHeader.Controls.Add(lblNavTitle);
            pnlNav.Controls.Add(pnlNavHeader);

            _treeNav = new TreeView();
            _treeNav.Dock = DockStyle.Fill;
            _treeNav.BorderStyle = BorderStyle.None;
            _treeNav.Font = new Font("Segoe UI", 9F);
            _treeNav.AfterSelect += TreeNav_AfterSelect;
            PopulateSiteNavTree();
            pnlNav.Controls.Add(_treeNav);
            _treeNav.BringToFront();
            _splitMain.Panel1.Controls.Add(pnlNav);

            // 4.2 Right Panel: Dynamic Content Host
            _pnlContentHost = new Panel();
            _pnlContentHost.Dock = DockStyle.Fill;
            _pnlContentHost.BackColor = Color.White;
            _splitMain.Panel2.Controls.Add(_pnlContentHost);
        }

        private Button CreateRibbonButton(string text, bool isPrimary)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Height = 30;
            btn.AutoSize = true;
            btn.Padding = new Padding(12, 2, 12, 2);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            if (isPrimary)
            {
                btn.BackColor = Color.FromArgb(190, 18, 60); // Crimson
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderSize = 0;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(190, 18, 60);
                btn.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
                btn.FlatAppearance.BorderSize = 1;
            }
            return btn;
        }

        private void PopulateSiteNavTree()
        {
            _treeNav.Nodes.Clear();
            var root = new TreeNode("🖧 MXM-PS-01 - (25.2a)");
            root.ForeColor = Color.FromArgb(30, 41, 59);
            root.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);

            // 1. Basics
            var nBasics = root.Nodes.Add("📁 Basics");
            nBasics.Nodes.Add("📜 License Information");
            nBasics.Nodes.Add("ℹ️ Site Information");

            // 2. Servers
            var nServers = root.Nodes.Add("🛡️ Servers");
            nServers.Nodes.Add("🖧 Recording Servers");
            nServers.Nodes.Add("🔄 Failover Servers");
            nServers.Nodes.Add("📱 Mobile Servers");

            // 3. Devices
            var nDevices = root.Nodes.Add("📹 Devices");
            var nCams = nDevices.Nodes.Add("📹 Cameras");
            nCams.ForeColor = Color.FromArgb(190, 18, 60);
            nCams.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            nDevices.Nodes.Add("🎤 Microphones");
            nDevices.Nodes.Add("🔊 Speakers");
            nDevices.Nodes.Add("🏷️ Metadata");
            nDevices.Nodes.Add("🔌 Input");
            nDevices.Nodes.Add("💡 Output");

            // 4. Client
            var nClient = root.Nodes.Add("🖥️ Client");
            nClient.Nodes.Add("📁 View Groups");
            nClient.Nodes.Add("📋 Smart Client Profiles");
            nClient.Nodes.Add("▦ Matrix");

            // 5. Rules and Events
            var nRules = root.Nodes.Add("⚙️ Rules and Events");
            nRules.Nodes.Add("📜 Rules");
            nRules.Nodes.Add("🕒 Time Profiles");
            nRules.Nodes.Add("✉️ Notification Profiles");
            nRules.Nodes.Add("⚡ User-defined Events");
            nRules.Nodes.Add("⚡ Analytics Events");
            nRules.Nodes.Add("⚡ Generic Events");
            nRules.Nodes.Add("🔗 Webhooks");

            // 6. Security
            var nSec = root.Nodes.Add("🔑 Security");
            nSec.Nodes.Add("🔑 Roles");
            nSec.Nodes.Add("👤 Basic Users");

            // 7. System Dashboard
            var nDash = root.Nodes.Add("⏱️ System Dashboard");
            nDash.Nodes.Add("📋 Current Tasks");
            nDash.Nodes.Add("🟢 System Monitor");
            nDash.Nodes.Add("🎛️ System Monitor Thresholds");
            nDash.Nodes.Add("📊 Configuration Reports");

            // 8. Server Logs
            root.Nodes.Add("📋 Server Logs");

            // 9. Metadata Use
            var nMeta = root.Nodes.Add("🗄️ Metadata Use");
            nMeta.Nodes.Add("🔍 Metadata Search");

            // 10. Access Control
            root.Nodes.Add("🪪 Access Control");

            // 11. Alarms
            var nAlarms = root.Nodes.Add("🔔 Alarms");
            nAlarms.Nodes.Add("🔔 Alarm Definitions");
            nAlarms.Nodes.Add("⚙️ Alarm Data Settings");
            nAlarms.Nodes.Add("🔊 Sound Settings");

            _treeNav.Nodes.Add(root);
            root.Expand();
            nBasics.Expand();
            nServers.Expand();
            nDevices.Expand();
            nClient.Expand();
            nRules.Expand();
            nSec.Expand();
            nDash.Expand();
            nAlarms.Expand();
        }

        private void TreeNav_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            string txt = e.Node.Text;

            if (txt.Contains("IP Cameras") || txt.Contains("Cameras") || txt.Contains("Video & Audio"))
                SwitchView(_viewCameras);
            else if (txt.Contains("Recording Servers") || txt.Contains("Lychee Servers") || txt.Contains("Failover"))
                SwitchView(_viewServers);
            else if (txt.Contains("License"))
                SwitchView(_viewLicense);
            else if (txt.Contains("Rules") || txt.Contains("Alarm"))
                SwitchView(_viewRules);
            else if (txt.Contains("Security") || txt.Contains("User") || txt.Contains("Roles"))
                SwitchView(_viewSecurity);
            else if (txt.Contains("Site Info"))
                SwitchView(_viewSite);
            else if (txt.Contains("Mobile"))
                SwitchView(_viewMobile);
            else
                SwitchView(_viewCameras);
        }

        private void SwitchView(UserControl view)
        {
            _pnlContentHost.Controls.Clear();
            if (view != null)
            {
                view.Dock = DockStyle.Fill;
                _pnlContentHost.Controls.Add(view);
                view.BringToFront();
            }
        }

        private void RefreshActiveView()
        {
            if (_pnlContentHost.Controls.Count > 0)
            {
                var ctrl = _pnlContentHost.Controls[0];
                var cvc = ctrl as CamerasViewControl;
                if (cvc != null) { cvc.PopulateDevicesTree(); return; }
                var svc = ctrl as ServersViewControl;
                if (svc != null) { svc.RefreshList(); return; }
                var lvc = ctrl as LicenseViewControl;
                if (lvc != null) { lvc.RefreshLicenseInfo(); return; }
            }
        }

        private void UpdateStatus()
        {
            try
            {
                int count = _vmoService.GetAllCameras().Count;
                if (_lblStatusCams != null) _lblStatusCams.Text = "📹 Active Cameras: " + count;
            }
            catch { }
        }
    }
}
