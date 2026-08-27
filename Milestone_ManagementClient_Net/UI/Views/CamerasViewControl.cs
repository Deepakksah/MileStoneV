using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class CamerasViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private SplitContainer _splitRoot;
        private SplitContainer _splitCenter;
        private TreeView _treeDevices;
        private Panel _pnlProperties;
        private Panel _pnlTabContainer;
        private PictureBox _pbVideoFeed;
        private Label _lblPreviewBottomName;
        private Label _lblPropDesc;
        private TextBox _txtSearch;
        private Button _btnToggleHidden;
        private System.Windows.Forms.Timer _clockTimer;

        // Active Tab & Tab Buttons
        private string _activeTab = "⚙ Settings";
        private readonly List<Button> _tabButtons = new List<Button>();

        // Hidden Groups Tracking
        private readonly HashSet<string> _hiddenGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private bool _showHiddenGroups = false;

        // Camera & Media
        private CameraModel _currentCam;
        private Image _cctvBaseImage;

        public CamerasViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            LoadBaseImage();
            BuildUI();

            SelectCamera("LYCHEE MAIN ENTRANCE CAM 01");

            _clockTimer = new System.Windows.Forms.Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += (s, e) => { if (_pbVideoFeed != null) _pbVideoFeed.Invalidate(); };
            _clockTimer.Start();
        }

        private void LoadBaseImage()
        {
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cctv_frame.png");
            if (File.Exists(imgPath))
            {
                try { _cctvBaseImage = Image.FromFile(imgPath); } catch { _cctvBaseImage = null; }
            }
        }

        private void BuildUI()
        {
            // 1. Root Split (Vertical): Left = Center (Explorer + Preview), Right = Full Height Properties
            _splitRoot = new SplitContainer();
            _splitRoot.Dock = DockStyle.Fill;
            _splitRoot.Orientation = Orientation.Vertical;
            _splitRoot.FixedPanel = FixedPanel.Panel2;
            _splitRoot.Panel1MinSize = 25;
            _splitRoot.Panel2MinSize = 25;
            _splitRoot.SplitterWidth = 6;
            _splitRoot.BackColor = Color.FromArgb(203, 213, 225);
            _splitRoot.Panel1.BackColor = Color.White;
            _splitRoot.Panel2.BackColor = Color.White;
            this.Controls.Add(_splitRoot);

            // 2. Center Split (Horizontal): Top = Devices Explorer, Bottom = Live CCTV Preview & PTZ
            _splitCenter = new SplitContainer();
            _splitCenter.Dock = DockStyle.Fill;
            _splitCenter.Orientation = Orientation.Horizontal;
            _splitCenter.FixedPanel = FixedPanel.Panel2;
            _splitCenter.Panel1MinSize = 25;
            _splitCenter.Panel2MinSize = 25;
            _splitCenter.SplitterWidth = 6;
            _splitCenter.BackColor = Color.FromArgb(203, 213, 225);
            _splitCenter.Panel1.BackColor = Color.White;
            _splitCenter.Panel2.BackColor = Color.White;
            _splitRoot.Panel1.Controls.Add(_splitCenter);

            this.Load += (s, e) => AdjustLayout();
            this.SizeChanged += (s, e) => AdjustLayout();

            // 3. Middle: Recording Servers & Cameras Explorer
            var pnlDevices = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var pnlDevHeader = new Panel { Dock = DockStyle.Top, Height = 28, BackColor = Color.FromArgb(255, 241, 242) };
            var lblDevTitle = new Label { Text = "Recording Servers & Cameras Explorer", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(8, 5, 0, 0) };
            pnlDevHeader.Controls.Add(lblDevTitle);

            // Action Toolbar (+ Add Camera, + Add Server, Delete, Toggle Hidden Groups)
            var pnlDevBar = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.White, Padding = new Padding(8, 5, 8, 5) };

            var btnAddCam = new Button { Text = "➕ Add Camera", BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Size = new Size(130, 32), Location = new Point(8, 4), Cursor = Cursors.Hand };
            btnAddCam.FlatAppearance.BorderSize = 0;
            btnAddCam.Click += (s, e) => PromptAddDeviceGroup();

            var btnAddSrv = new Button { Text = "➕ Add Server", BackColor = Color.White, ForeColor = Color.FromArgb(190, 18, 60), Font = new Font("Segoe UI", 9F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Size = new Size(120, 32), Location = new Point(144, 4), Cursor = Cursors.Hand };
            btnAddSrv.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnAddSrv.Click += (s, e) => PromptAddServer();

            var btnDel = new Button { Text = "🗑 Delete", BackColor = Color.White, ForeColor = Color.FromArgb(190, 18, 60), Font = new Font("Segoe UI", 9F), FlatStyle = FlatStyle.Flat, Size = new Size(90, 32), Location = new Point(270, 4), Cursor = Cursors.Hand };
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnDel.Click += (s, e) =>
            {
                if (_currentCam != null && MessageBox.Show("Delete camera '" + _currentCam.Name + "'?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _vmoService.DeleteCamera(_currentCam.Name);
                    PopulateDevicesTree();
                }
            };

            _btnToggleHidden = new Button { Text = "👁 Show Hidden (0)", BackColor = Color.White, ForeColor = Color.FromArgb(71, 85, 105), Font = new Font("Segoe UI", 9F), FlatStyle = FlatStyle.Flat, Size = new Size(140, 32), Location = new Point(366, 4), Cursor = Cursors.Hand };
            _btnToggleHidden.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            _btnToggleHidden.Click += (s, e) =>
            {
                _showHiddenGroups = !_showHiddenGroups;
                _btnToggleHidden.BackColor = _showHiddenGroups ? Color.FromArgb(219, 234, 254) : Color.White;
                _btnToggleHidden.ForeColor = _showHiddenGroups ? Color.FromArgb(30, 64, 175) : Color.FromArgb(71, 85, 105);
                PopulateDevicesTree();
            };

            pnlDevBar.Controls.Add(btnAddCam);
            pnlDevBar.Controls.Add(btnAddSrv);
            pnlDevBar.Controls.Add(btnDel);
            pnlDevBar.Controls.Add(_btnToggleHidden);

            // Search Bar
            var pnlSearch = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = Color.White, Padding = new Padding(8, 2, 8, 4) };
            _txtSearch = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(100, 116, 139), Text = "Search cameras or IP..." };
            _txtSearch.GotFocus += (s, e) => { if (_txtSearch.Text == "Search cameras or IP...") { _txtSearch.Text = ""; _txtSearch.ForeColor = Color.Black; } };
            _txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(_txtSearch.Text)) { _txtSearch.Text = "Search cameras or IP..."; _txtSearch.ForeColor = Color.FromArgb(100, 116, 139); } };
            _txtSearch.TextChanged += (s, e) => FilterTree(_txtSearch.Text);
            pnlSearch.Controls.Add(_txtSearch);

            _treeDevices = new TreeView { Dock = DockStyle.Fill, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 9F) };
            _treeDevices.AfterSelect += (s, e) =>
            {
                if (e.Node != null)
                {
                    var cam = e.Node.Tag as CameraModel;
                    if (cam != null) SelectCamera(cam.Name);
                }
            };

            PopulateDevicesTree();

            pnlDevices.Controls.Add(_treeDevices);
            pnlDevices.Controls.Add(pnlSearch);
            pnlDevices.Controls.Add(pnlDevBar);
            pnlDevices.Controls.Add(pnlDevHeader);
            _treeDevices.BringToFront();
            _splitCenter.Panel1.Controls.Add(pnlDevices);

            // 4. Bottom of Center: Lychee Live Stream & PTZ Control
            var pnlPreviewContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            BuildPreviewPane(pnlPreviewContainer);
            _splitCenter.Panel2.Controls.Add(pnlPreviewContainer);

            // 5. Right: Authentic Milestone Properties Panel (100% Full Height!)
            _pnlProperties = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            BuildPropertiesPanel();
            _splitRoot.Panel2.Controls.Add(_pnlProperties);
        }

        public void AdjustLayout()
        {
            try
            {
                if (_splitRoot != null && this.Width > 500)
                {
                    int propW = Math.Max(380, Math.Min(520, (int)(this.Width * 0.35)));
                    int centerW = this.Width - propW;
                    if (centerW > 200)
                        _splitRoot.SplitterDistance = centerW;
                }

                if (_splitCenter != null && _splitCenter.Height > 320)
                {
                    int previewH = Math.Max(220, Math.Min(340, (int)(_splitCenter.Height * 0.40)));
                    int explorerH = _splitCenter.Height - previewH;
                    if (explorerH > 150)
                        _splitCenter.SplitterDistance = explorerH;
                }
            }
            catch { }
        }

        public void PopulateDevicesTree()
        {
            _treeDevices.Nodes.Clear();
            var rootCluster = new TreeNode("📁 Lychee Recording Cluster");
            rootCluster.ForeColor = Color.FromArgb(190, 18, 60);
            rootCluster.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);

            var servers = _vmoService.GetServers();
            if (servers.Count == 0)
            {
                _vmoService.AddRecordingServer("LYCHEE-REC-SRV-01", "192.168.1.100", 7563);
                servers = _vmoService.GetServers();
            }

            var cams = _vmoService.GetAllCameras().Values.ToList();

            foreach (var srv in servers)
            {
                bool isHidden = _hiddenGroups.Contains(srv.Name);
                if (isHidden && !_showHiddenGroups) continue;

                string srvTitle = "📁 " + srv.Name + " (" + srv.Hostname + ")" + (isHidden ? " [HIDDEN]" : "");
                var srvNode = new TreeNode(srvTitle);
                srvNode.ForeColor = isHidden ? Color.FromArgb(156, 163, 175) : Color.FromArgb(79, 70, 229);
                srvNode.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
                srvNode.Tag = srv;

                // Server Context Menu (Rename, Hide/Unhide, Add Camera, Refresh)
                var menuSrv = new ContextMenuStrip();
                var miRename = new ToolStripMenuItem("✏ Rename Server (F2)", null, (s, e) => PromptRenameServer(srv.Name));
                miRename.ForeColor = Color.FromArgb(190, 18, 60);
                miRename.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                menuSrv.Items.Add(miRename);

                if (isHidden)
                {
                    menuSrv.Items.Add("👁 Unhide Group/Server", null, (s, e) => { _hiddenGroups.Remove(srv.Name); PopulateDevicesTree(); });
                }
                else
                {
                    menuSrv.Items.Add("👁 Hide Group/Server", null, (s, e) => { _hiddenGroups.Add(srv.Name); PopulateDevicesTree(); });
                }

                menuSrv.Items.Add(new ToolStripSeparator());
                menuSrv.Items.Add("➕ Add Camera to Server...", null, (s, e) => PromptAddDeviceGroup());
                menuSrv.Items.Add("🔄 Refresh Devices", null, (s, e) => PopulateDevicesTree());
                srvNode.ContextMenuStrip = menuSrv;

                foreach (var cam in cams)
                {
                    bool isOnline = cam.IsOnline;
                    var camNode = new TreeNode("📹 " + cam.Name + " [" + (string.IsNullOrEmpty(cam.MulticastIp) ? "192.168.1.201" : cam.MulticastIp) + "]" + (isOnline ? "" : " [OFFLINE]"));
                    
                    if (isHidden)
                    {
                        camNode.ForeColor = Color.FromArgb(156, 163, 175);
                    }
                    else
                    {
                        camNode.ForeColor = isOnline ? Color.FromArgb(29, 78, 216) : Color.FromArgb(220, 38, 38); // Blue for Online, Red for Offline
                    }
                    camNode.Tag = cam;

                    var menuCam = new ContextMenuStrip();
                    var miRefresh = new ToolStripMenuItem("🔄 Refresh Camera Feed & Status", null, (s, e) =>
                    {
                        SelectCamera(cam.Name);
                        RenderActiveTabContent();
                        MessageBox.Show("Camera '" + cam.Name + "' feed & hardware status refreshed successfully!", "Camera Refreshed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                    miRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    miRefresh.ForeColor = Color.FromArgb(5, 150, 105);

                    var miToggleOnline = new ToolStripMenuItem(isOnline ? "🔴 Mark Camera as Offline (Red)" : "🟢 Mark Camera as Online (Blue)", null, (s, e) =>
                    {
                        cam.IsOnline = !cam.IsOnline;
                        _vmoService.SaveCameraSettings(cam);
                        PopulateDevicesTree();
                        SelectCamera(cam.Name);
                    });
                    miToggleOnline.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    miToggleOnline.ForeColor = isOnline ? Color.FromArgb(220, 38, 38) : Color.FromArgb(29, 78, 216);

                    menuCam.Items.Add(miRefresh);
                    menuCam.Items.Add(miToggleOnline);
                    menuCam.Items.Add(new ToolStripSeparator());
                    menuCam.Items.Add("✏ Rename Camera...", null, (s, e) => PromptRenameDevice(cam));
                    menuCam.Items.Add("🗑 Delete Camera", null, (s, e) =>
                    {
                        if (MessageBox.Show("Delete camera '" + cam.Name + "'?", "Delete Camera", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            _vmoService.DeleteCamera(cam.Name);
                            PopulateDevicesTree();
                        }
                    });
                    camNode.ContextMenuStrip = menuCam;

                    srvNode.Nodes.Add(camNode);
                }

                rootCluster.Nodes.Add(srvNode);
            }

            _treeDevices.Nodes.Add(rootCluster);
            rootCluster.ExpandAll();

            if (_btnToggleHidden != null)
            {
                _btnToggleHidden.Text = _showHiddenGroups
                    ? "👁 Hide Hidden (" + _hiddenGroups.Count + ")"
                    : "👁 Show Hidden (" + _hiddenGroups.Count + ")";
            }
        }

        private void FilterTree(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term == "Search cameras or IP...")
            {
                PopulateDevicesTree();
                return;
            }
            term = term.Trim().ToLowerInvariant();
            _treeDevices.Nodes.Clear();
            var rootCluster = new TreeNode("📁 Filtered Search Results");
            rootCluster.ForeColor = Color.FromArgb(190, 18, 60);

            var cams = _vmoService.GetAllCameras().Values.Where(c => c.Name.ToLowerInvariant().Contains(term) || (c.MulticastIp != null && c.MulticastIp.Contains(term))).ToList();
            foreach (var cam in cams)
            {
                bool isOnline = cam.IsOnline;
                var camNode = new TreeNode("📹 " + cam.Name + " [" + cam.MulticastIp + "]" + (isOnline ? "" : " [OFFLINE]"));
                camNode.ForeColor = isOnline ? Color.FromArgb(29, 78, 216) : Color.FromArgb(220, 38, 38); // Blue for Online, Red for Offline
                camNode.Tag = cam;
                rootCluster.Nodes.Add(camNode);
            }
            _treeDevices.Nodes.Add(rootCluster);
            rootCluster.ExpandAll();
        }

        private void BuildPropertiesPanel()
        {
            _pnlProperties.Controls.Clear();

            // 1. Top Header: "Properties" with Driver Dropdown
            var pnlTopHeader = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.FromArgb(238, 242, 246) };
            
            var pnlTitleRow = new Panel { Dock = DockStyle.Top, Height = 26, BackColor = Color.FromArgb(219, 234, 254), Padding = new Padding(6, 4, 6, 4) };
            var lblTitle = new Label { Text = "Properties", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Left, AutoSize = true };
            var lblIcons = new Label { Text = "📌  ▾", Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(71, 85, 105), Dock = DockStyle.Right, AutoSize = true };
            pnlTitleRow.Controls.Add(lblTitle);
            pnlTitleRow.Controls.Add(lblIcons);

            var pnlDriverRow = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(8, 3, 8, 3) };
            var cbDriver = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cbDriver.Items.AddRange(new object[] { "HikVisionGeneric", "ONVIFGenericProfileS", "AxisNetworkVideo", "BoschGenericIP", "UniversalDriver" });
            cbDriver.SelectedIndex = 0;
            pnlDriverRow.Controls.Add(cbDriver);

            pnlTopHeader.Controls.Add(pnlDriverRow);
            pnlTopHeader.Controls.Add(pnlTitleRow);
            _pnlProperties.Controls.Add(pnlTopHeader);

            // 2. Top Tabs Bar (2 Clean Rows, All 9 Tabs 100% Visible with larger comfortable buttons)
            var pnlTabsStrip = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(4, 5, 4, 5) };
            var flowTabs = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = false, WrapContents = true, BackColor = Color.Transparent, Padding = new Padding(0) };

            string[] tabs = new[] { "ℹ Info", "⚙ Settings", "📹 Streams", "🔴 Record", "🏃 Motion", "👁 Fisheye", "⚡ Events", "🖥 Client", "▦ Privacy" };
            _tabButtons.Clear();

            foreach (var t in tabs)
            {
                var btn = new Button
                {
                    Text = t,
                    AutoSize = true,
                    Height = 28,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Font = new Font("Segoe UI", 9F),
                    Margin = new Padding(2, 2, 2, 2)
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);

                if (t == _activeTab)
                {
                    btn.BackColor = Color.FromArgb(219, 234, 254);
                    btn.ForeColor = Color.FromArgb(30, 64, 175);
                    btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else
                {
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.FromArgb(71, 85, 105);
                }

                btn.Click += (s, e) => SwitchTab(t);
                _tabButtons.Add(btn);
                flowTabs.Controls.Add(btn);
            }
            pnlTabsStrip.Controls.Add(flowTabs);

            // 3. Bottom Description Panel (Only description at bottom)
            var pnlBottomDesc = new Panel { Dock = DockStyle.Bottom, Height = 26, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 4, 8, 4) };
            _lblPropDesc = new Label { Text = "Video stream 1", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Fill };
            pnlBottomDesc.Controls.Add(_lblPropDesc);

            // 4. Center Dynamic Tab Host
            _pnlTabContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // Assemble Properties Panel Controls
            _pnlProperties.Controls.Add(_pnlTabContainer);
            _pnlProperties.Controls.Add(pnlBottomDesc);
            _pnlProperties.Controls.Add(pnlTabsStrip);
            _pnlProperties.Controls.Add(pnlTopHeader);
            _pnlTabContainer.BringToFront();

            RenderActiveTabContent();
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
            RenderActiveTabContent();
        }

        private void RenderActiveTabContent()
        {
            if (_pnlTabContainer == null) return;
            _pnlTabContainer.Controls.Clear();

            switch (_activeTab)
            {
                case "ℹ Info":
                    _pnlTabContainer.Controls.Add(BuildInfoTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Hardware Information & Device Metadata";
                    break;

                case "⚙ Settings":
                    _pnlTabContainer.Controls.Add(BuildSettingsGridTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Video stream 1";
                    break;

                case "📹 Streams":
                    _pnlTabContainer.Controls.Add(BuildStreamsTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "RTSP & Multicast Stream Transport Endpoints";
                    break;

                case "🔴 Record":
                    _pnlTabContainer.Controls.Add(BuildRecordTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Recording Tracks, Keyframe Buffers & Retention";
                    break;

                case "🏃 Motion":
                    _pnlTabContainer.Controls.Add(BuildMotionTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Hardware & Software Motion Detection Grid";
                    break;

                case "👁 Fisheye":
                case "👁 Fisheye Lens":
                    _pnlTabContainer.Controls.Add(BuildFisheyeTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "360° / 180° Immervision Panomorph Lens Dewarping";
                    break;

                case "⚡ Events":
                    _pnlTabContainer.Controls.Add(BuildEventsTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Hardware Event Subscriptions & Trigger Rules";
                    break;

                case "🖥 Client":
                    _pnlTabContainer.Controls.Add(BuildClientTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Smart Client Display & PTZ Priority Configuration";
                    break;

                case "▦ Privacy":
                case "▦ Privacy masking":
                    _pnlTabContainer.Controls.Add(BuildPrivacyTab());
                    if (_lblPropDesc != null) _lblPropDesc.Text = "Privacy Masking Grid & Redaction Rules";
                    break;
            }
        }

        // List to hold togglers for all accordion sections
        private readonly List<Action<bool>> _accordionTogglers = new List<Action<bool>>();

        // ==========================================
        // 1. SETTINGS TAB (Accordion Sections with Full Inline Editors & Save Button)
        // ==========================================
        private Control BuildSettingsGridTab()
        {
            _accordionTogglers.Clear();
            var pnlOuter = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // Top Action Bar with Save, Collapse All, and Expand All
            var pnlSaveBar = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(6, 4, 6, 4) };
            
            var btnSaveAll = new Button
            {
                Text = "💾 Save Settings",
                Size = new Size(115, 26),
                Location = new Point(6, 4),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSaveAll.FlatAppearance.BorderSize = 0;

            var btnCollapseAll = new Button
            {
                Text = "▼ Collapse All",
                Size = new Size(95, 26),
                Location = new Point(125, 4),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                Font = new Font("Segoe UI", 8.5F),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCollapseAll.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnCollapseAll.Click += (s, e) => { foreach (var t in _accordionTogglers) t(false); };

            var btnExpandAll = new Button
            {
                Text = "► Expand All",
                Size = new Size(90, 26),
                Location = new Point(224, 4),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                Font = new Font("Segoe UI", 8.5F),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExpandAll.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExpandAll.Click += (s, e) => { foreach (var t in _accordionTogglers) t(true); };

            var lblSaved = new Label
            {
                Text = "✓ Saved!",
                ForeColor = Color.FromArgb(5, 150, 105),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Location = new Point(320, 8),
                AutoSize = true,
                Visible = false
            };

            pnlSaveBar.Controls.Add(btnSaveAll);
            pnlSaveBar.Controls.Add(btnCollapseAll);
            pnlSaveBar.Controls.Add(btnExpandAll);
            pnlSaveBar.Controls.Add(lblSaved);
            pnlOuter.Controls.Add(pnlSaveBar);

            var pnlScroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(0) };
            pnlOuter.Controls.Add(pnlScroll);
            pnlScroll.BringToFront();

            // 1. General Section (Default: Collapsed)
            var pnlGenBody = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White };
            var cbKeepDefault = AddInlineDropdownRow(pnlGenBody, "Keep default settings", new[] { "Yes", "No" }, "Yes");
            var txtMulticastAddr = AddInlineTextRow(pnlGenBody, "Multicast address", _currentCam != null && !string.IsNullOrEmpty(_currentCam.MulticastIp) ? _currentCam.MulticastIp : "0.0.0.0");
            var txtBrightness = AddInlineTextRow(pnlGenBody, "Brightness", _currentCam != null ? _currentCam.Brightness.ToString() : "50");
            var txtContrast = AddInlineTextRow(pnlGenBody, "Contrast", _currentCam != null ? _currentCam.Contrast.ToString() : "50");
            var txtSaturation = AddInlineTextRow(pnlGenBody, "Saturation", _currentCam != null ? _currentCam.Saturation.ToString() : "50");
            var txtSharpness = AddInlineTextRow(pnlGenBody, "Sharpness", _currentCam != null ? _currentCam.Sharpness.ToString() : "50");
            AddAccordionSection(pnlScroll, "General", pnlGenBody, false);

            // 2. Video stream 1 Section (Default: Collapsed)
            var pnlS1Body = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White };
            var cbCodec1 = AddInlineDropdownRow(pnlS1Body, "Codec", new[] { "H.264", "H.265", "H.265+", "MJPEG", "AV1" }, _currentCam != null && !string.IsNullOrEmpty(_currentCam.Codec) ? _currentCam.Codec : "H.264");
            var cbControlMode1 = AddInlineDropdownRow(pnlS1Body, "Control mode", new[] { "Variable bit rate", "Constant bit rate" }, "Variable bit rate");
            var cbFps1 = AddInlineDropdownRow(pnlS1Body, "Frames per second", new[] { "30", "25", "20", "15", "10", "5" }, _currentCam != null ? _currentCam.Fps.ToString() : "20");
            var cbPlus1 = AddInlineDropdownRow(pnlS1Body, "H.264/H.265+ (will reboot the device)", new[] { "OFF", "ON" }, "OFF");
            var txtBitrate1 = AddInlineTextRow(pnlS1Body, "Max. Bitrate (kbit/s)", "2500");
            var txtKeyframes1 = AddInlineTextRow(pnlS1Body, "Max. frames between keyframes", "8");
            var cbKeyMode1 = AddInlineDropdownRow(pnlS1Body, "Max. frames between keyframes mode", new[] { "Default (determined by driver)", "Custom" }, "Default (determined by driver)");
            var txtPort1 = AddInlineTextRow(pnlS1Body, "Multicast port", _currentCam != null ? _currentCam.Port1.ToString() : "8860");
            var txtQuality1 = AddInlineTextRow(pnlS1Body, "Quality", _currentCam != null ? _currentCam.Quality.ToString() : "60");
            var cbRes1 = AddInlineDropdownRow(pnlS1Body, "Resolution", new[] { "1920x1080", "3840x2160 (4K UHD)", "2560x1440 (2K QHD)", "1280x720 (720p HD)", "640x360" }, _currentCam != null && !string.IsNullOrEmpty(_currentCam.Resolution) ? _currentCam.Resolution.Split(' ')[0] : "1920x1080");
            var cbStreamMethod1 = AddInlineDropdownRow(pnlS1Body, "Streaming method", new[] { "RTP/RTSP/TCP", "RTP/RTSP/HTTP/TCP", "RTP/UDP Multicast" }, "RTP/RTSP/TCP");
            AddAccordionSection(pnlScroll, "Video stream 1", pnlS1Body, false);

            // 3. Video stream 2 Section (Default: Collapsed)
            var pnlS2Body = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White };
            var cbCodec2 = AddInlineDropdownRow(pnlS2Body, "Codec", new[] { "H.265", "H.264", "MJPEG" }, "H.265");
            var cbControlMode2 = AddInlineDropdownRow(pnlS2Body, "Control mode", new[] { "Variable bit rate", "Constant bit rate" }, "Variable bit rate");
            var cbFps2 = AddInlineDropdownRow(pnlS2Body, "Frames per second", new[] { "25", "20", "15", "10", "5" }, "25");
            var cbPlus2 = AddInlineDropdownRow(pnlS2Body, "H.264/H.265+ (will reboot the device)", new[] { "OFF", "ON" }, "OFF");
            var txtBitrate2 = AddInlineTextRow(pnlS2Body, "Max. Bitrate (kbit/s)", "512");
            var txtKeyframes2 = AddInlineTextRow(pnlS2Body, "Max. frames between keyframes", "8");
            var cbKeyMode2 = AddInlineDropdownRow(pnlS2Body, "Max. frames between keyframes mode", new[] { "Default (determined by driver)", "Custom" }, "Default (determined by driver)");
            var txtPort2 = AddInlineTextRow(pnlS2Body, "Multicast port", "8866");
            var txtQuality2 = AddInlineTextRow(pnlS2Body, "Quality", "60");
            var cbRes2 = AddInlineDropdownRow(pnlS2Body, "Resolution", new[] { "640x360", "704x576 (D1)", "1280x720 (720p HD)" }, "640x360");
            var cbStreamMethod2 = AddInlineDropdownRow(pnlS2Body, "Streaming method", new[] { "RTP/RTSP/TCP", "RTP/RTSP/HTTP/TCP", "RTP/UDP Multicast" }, "RTP/RTSP/TCP");
            AddAccordionSection(pnlScroll, "Video stream 2", pnlS2Body, false);

            // 4. Audio Section (Default: Collapsed)
            var pnlAudioBody = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White };
            AddInlineDropdownRow(pnlAudioBody, "Audio Codec", new[] { "G.711u (PCM)", "AAC-LC 128 kbps", "G.726" }, "G.711u (PCM)");
            AddInlineDropdownRow(pnlAudioBody, "Sampling Rate", new[] { "8000 Hz / 64 kbps", "16000 Hz", "44100 Hz" }, "8000 Hz / 64 kbps");
            AddInlineDropdownRow(pnlAudioBody, "Two-Way Audio", new[] { "Enabled", "Disabled" }, "Enabled");
            AddAccordionSection(pnlScroll, "Audio", pnlAudioBody, false);

            // 5. PTZ Section (Default: Collapsed)
            var pnlPtzBody = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White };
            AddInlineDropdownRow(pnlPtzBody, "PTZ Protocol", new[] { "ONVIF PTZ Profile S", "Pelco-D", "Pelco-P" }, "ONVIF PTZ Profile S");
            AddInlineDropdownRow(pnlPtzBody, "Home Preset Return", new[] { "Enabled (30s timeout)", "Disabled" }, "Enabled (30s timeout)");
            AddAccordionSection(pnlScroll, "PTZ", pnlPtzBody, false);

            btnSaveAll.Click += (s, e) =>
            {
                if (_currentCam != null)
                {
                    _currentCam.Codec = cbCodec1.SelectedItem != null ? cbCodec1.SelectedItem.ToString() : "H.264";
                    _currentCam.Resolution = cbRes1.SelectedItem != null ? cbRes1.SelectedItem.ToString() : "1920x1080";
                    int parsedInt;
                    if (int.TryParse(txtBrightness.Text, out parsedInt)) _currentCam.Brightness = parsedInt;
                    if (int.TryParse(txtContrast.Text, out parsedInt)) _currentCam.Contrast = parsedInt;
                    if (int.TryParse(txtSaturation.Text, out parsedInt)) _currentCam.Saturation = parsedInt;
                    if (int.TryParse(txtSharpness.Text, out parsedInt)) _currentCam.Sharpness = parsedInt;
                    if (int.TryParse(cbFps1.SelectedItem != null ? cbFps1.SelectedItem.ToString() : "20", out parsedInt)) _currentCam.Fps = parsedInt;
                    _vmoService.SaveCameraSettings(_currentCam);
                }
                lblSaved.Visible = true;
                var timer = new System.Windows.Forms.Timer { Interval = 2500 };
                timer.Tick += (st, et) => { lblSaved.Visible = false; timer.Stop(); };
                timer.Start();
            };

            return pnlOuter;
        }

        private void AddAccordionSection(Panel parent, string title, Panel body, bool startExpanded)
        {
            var pnlSection = new Panel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White };

            var btnHeader = new Button
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = (startExpanded ? "▼ " : "► ") + title,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(15, 23, 42),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Padding = new Padding(4, 0, 0, 0)
            };
            btnHeader.FlatAppearance.BorderSize = 1;
            btnHeader.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);

            body.Dock = DockStyle.Top;
            body.Visible = startExpanded;

            btnHeader.Click += (s, e) =>
            {
                body.Visible = !body.Visible;
                btnHeader.Text = (body.Visible ? "▼ " : "► ") + title;
                if (_lblPropDesc != null) _lblPropDesc.Text = title;
            };

            _accordionTogglers.Add((expand) =>
            {
                body.Visible = expand;
                btnHeader.Text = (expand ? "▼ " : "► ") + title;
            });

            pnlSection.Controls.Add(body);
            pnlSection.Controls.Add(btnHeader);
            btnHeader.BringToFront();

            parent.Controls.Add(pnlSection);
            pnlSection.BringToFront();
        }

        private ComboBox AddInlineDropdownRow(Panel body, string propName, string[] options, string selectedVal)
        {
            var row = new Panel { Dock = DockStyle.Top, Height = 25, BackColor = Color.White, Padding = new Padding(0) };

            var lbl = new Label
            {
                Text = propName,
                Location = new Point(14, 4),
                Size = new Size(205, 18),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(30, 41, 59),
                Cursor = Cursors.Hand
            };
            lbl.Click += (s, e) => { if (_lblPropDesc != null) _lblPropDesc.Text = propName; };

            var cb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(222, 1),
                Size = new Size(225, 22),
                Font = new Font("Segoe UI", 8.5F),
                FlatStyle = FlatStyle.System
            };
            cb.Items.AddRange(options);
            int idx = Array.IndexOf(options, selectedVal);
            cb.SelectedIndex = idx >= 0 ? idx : 0;
            cb.GotFocus += (s, e) => { if (_lblPropDesc != null) _lblPropDesc.Text = propName; };

            row.Controls.Add(lbl);
            row.Controls.Add(cb);
            body.Controls.Add(row);
            row.BringToFront();

            return cb;
        }

        private TextBox AddInlineTextRow(Panel body, string propName, string textVal)
        {
            var row = new Panel { Dock = DockStyle.Top, Height = 25, BackColor = Color.White, Padding = new Padding(0) };

            var lbl = new Label
            {
                Text = propName,
                Location = new Point(14, 4),
                Size = new Size(205, 18),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(30, 41, 59),
                Cursor = Cursors.Hand
            };
            lbl.Click += (s, e) => { if (_lblPropDesc != null) _lblPropDesc.Text = propName; };

            var txt = new TextBox
            {
                Text = textVal,
                Location = new Point(222, 1),
                Size = new Size(225, 22),
                Font = new Font("Segoe UI", 8.5F),
                BorderStyle = BorderStyle.FixedSingle
            };
            txt.GotFocus += (s, e) => { if (_lblPropDesc != null) _lblPropDesc.Text = propName; };

            row.Controls.Add(lbl);
            row.Controls.Add(txt);
            body.Controls.Add(row);
            row.BringToFront();

            return txt;
        }

        // ==========================================
        // 2. INFO TAB (Editable Form)
        // ==========================================
        private Control BuildInfoTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), BackColor = Color.White };
            int y = 10;

            pnl.Controls.Add(new Label { Text = "Device Identification & Metadata", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Location = new Point(10, y), AutoSize = true });
            y += 28;

            var txtName = AddEditableRow(pnl, ref y, "Camera Name:", _currentCam != null ? _currentCam.Name : "LYCHEE MAIN ENTRANCE CAM 01");
            var txtModel = AddEditableRow(pnl, ref y, "Hardware Model:", "HikVision DS-2CD2043G2-I (UltraHD)");
            var txtMac = AddEditableRow(pnl, ref y, "MAC Address:", "00:40:8C:92:A4:1F");
            var txtFw = AddEditableRow(pnl, ref y, "Firmware Version:", "V5.7.3 build 220625");
            var txtSerial = AddEditableRow(pnl, ref y, "Serial Number:", "DS-2CD2043G220220625AAWR89104");
            var txtIp = AddEditableRow(pnl, ref y, "IP Address:", _currentCam != null && !string.IsNullOrEmpty(_currentCam.MulticastIp) ? _currentCam.MulticastIp : "192.168.1.201");
            var chkEnabled = new CheckBox { Text = "Device Enabled & Online in VMS", Checked = true, Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(5, 150, 105) };
            pnl.Controls.Add(chkEnabled);
            y += 32;

            var btnSave = new Button { Text = "💾 Apply Info Changes", Location = new Point(10, y), Size = new Size(200, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                if (_currentCam != null && !string.IsNullOrWhiteSpace(txtName.Text))
                {
                    _vmoService.RenameCamera(_currentCam.Name, txtName.Text.Trim());
                    _currentCam.MulticastIp = txtIp.Text.Trim();
                    _vmoService.SaveCameraSettings(_currentCam);
                    PopulateDevicesTree();
                    SelectCamera(txtName.Text.Trim());
                    MessageBox.Show("Hardware info saved successfully!", "Info Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            pnl.Controls.Add(btnSave);

            return pnl;
        }

        // ==========================================
        // 3. STREAMS TAB (Editable Form)
        // ==========================================
        private Control BuildStreamsTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), BackColor = Color.White };
            int y = 10;

            pnl.Controls.Add(new Label { Text = "Live Video Stream Transport Endpoints", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Location = new Point(10, y), AutoSize = true });
            y += 28;

            var txtS1 = AddEditableRow(pnl, ref y, "Stream 1 (Main):", "rtsp://192.168.1.201:554/Streaming/Channels/101");
            var txtS2 = AddEditableRow(pnl, ref y, "Stream 2 (Sub):", "rtsp://192.168.1.201:554/Streaming/Channels/102");
            var txtRtspPort = AddEditableRow(pnl, ref y, "RTSP Port:", "554");
            var txtHttpPort = AddEditableRow(pnl, ref y, "HTTP Port:", "80");
            var txtHttpsPort = AddEditableRow(pnl, ref y, "HTTPS Port:", "443");
            var txtMulticast = AddEditableRow(pnl, ref y, "Multicast Address:", "239.255.0.1:8860");

            var btnSave = new Button { Text = "💾 Save Stream Settings", Location = new Point(10, y), Size = new Size(200, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Stream transport endpoints updated!", "Streams Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnl.Controls.Add(btnSave);

            return pnl;
        }

        // ==========================================
        // 4. RECORD TAB (Exact Milestone Recording Settings)
        // ==========================================
        private Control BuildRecordTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10), BackColor = Color.FromArgb(248, 250, 252) };
            int y = 8;

            pnl.Controls.Add(new Label { Text = "Recording settings", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(8, y), AutoSize = true });
            y += 24;

            var chkRecording = new CheckBox { Text = "Recording", Checked = true, Location = new Point(8, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            pnl.Controls.Add(chkRecording);
            y += 24;

            var chkRelated = new CheckBox { Text = "Record on related devices", Checked = true, Location = new Point(32, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnl.Controls.Add(chkRelated);
            y += 24;

            var chkStopManual = new CheckBox { Text = "Stop manual recording after:", Checked = true, Location = new Point(32, y + 2), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            var numStopMin = new NumericUpDown { Minimum = 1, Maximum = 1440, Value = 5, Location = new Point(240, y), Size = new Size(55, 23), Font = new Font("Segoe UI", 8.5F) };
            var lblMin = new Label { Text = "minutes", Location = new Point(300, y + 3), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnl.Controls.Add(chkStopManual);
            pnl.Controls.Add(numStopMin);
            pnl.Controls.Add(lblMin);
            y += 32;

            // GroupBox: Pre-buffer
            var grpPreBuffer = new GroupBox { Text = "Pre-buffer", Location = new Point(8, y), Size = new Size(425, 75), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            grpPreBuffer.Controls.Add(new Label { Text = "Location:", Location = new Point(14, 22), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var cbPreLoc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(110, 19), Size = new Size(160, 22), Font = new Font("Segoe UI", 8.5F) };
            cbPreLoc.Items.AddRange(new object[] { "Memory", "Disk Buffer" });
            cbPreLoc.SelectedIndex = 0;
            grpPreBuffer.Controls.Add(cbPreLoc);

            grpPreBuffer.Controls.Add(new Label { Text = "Time:", Location = new Point(14, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var numPreTime = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 3, Location = new Point(110, 45), Size = new Size(160, 22), Font = new Font("Segoe UI", 8.5F) };
            var lblSec = new Label { Text = "seconds", Location = new Point(275, 47), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) };
            grpPreBuffer.Controls.Add(numPreTime);
            grpPreBuffer.Controls.Add(lblSec);
            pnl.Controls.Add(grpPreBuffer);
            y += 82;

            // GroupBox: Recording frame rate
            var grpFrameRate = new GroupBox { Text = "Recording frame rate", Location = new Point(8, y), Size = new Size(425, 75), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            grpFrameRate.Controls.Add(new Label { Text = "JPEG:", Location = new Point(14, 22), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var numJpegFps = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 5, Location = new Point(220, 19), Size = new Size(50, 22), Font = new Font("Segoe UI", 8.5F) };
            var lblFpsTag = new Label { Text = "FPS", Location = new Point(275, 21), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) };
            grpFrameRate.Controls.Add(numJpegFps);
            grpFrameRate.Controls.Add(lblFpsTag);

            grpFrameRate.Controls.Add(new Label { Text = "MPEG-4/H.264/H.265:", Location = new Point(14, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var chkKeyframesOnly = new CheckBox { Text = "Record keyframes only", Checked = false, Location = new Point(220, 47), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(30, 41, 59) };
            grpFrameRate.Controls.Add(chkKeyframesOnly);
            pnl.Controls.Add(grpFrameRate);
            y += 82;

            // GroupBox: Storage
            var grpStorage = new GroupBox { Text = "Storage & Retention", Location = new Point(8, y), Size = new Size(425, 275), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            
            // Retention Selector Row (30 - 60 - 90 Days)
            grpStorage.Controls.Add(new Label { Text = "Retention:", Location = new Point(10, 22), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var cbRetention = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(80, 19), Size = new Size(250, 23), Font = new Font("Segoe UI", 8.5F) };
            cbRetention.Items.AddRange(new object[] { "Live-30days (30 Days Retention)", "Live-60days (60 Days Retention)", "Live-90days (90 Days Retention)", "Live-180days (6 Months Archive)", "Live-365days (1 Year Archive)" });
            cbRetention.SelectedIndex = 1; // Default: 60 Days
            
            var btnSelectStorage = new Button { Text = "Select...", Location = new Point(340, 18), Size = new Size(75, 25), Font = new Font("Segoe UI", 8.5F), FlatStyle = FlatStyle.Flat };
            grpStorage.Controls.Add(cbRetention);
            grpStorage.Controls.Add(btnSelectStorage);

            // Status Row with Green (Active) / Red (Not Active) Toggle
            grpStorage.Controls.Add(new Label { Text = "Status:", Location = new Point(10, 52), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(80, 49), Size = new Size(130, 23), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            cbStatus.Items.AddRange(new object[] { "Active", "Not Active" });
            cbStatus.SelectedIndex = 0;

            var lblStatusBadge = new Label
            {
                Text = "🟢 Active (Recording)",
                Location = new Point(220, 51),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 185, 129) // Green
            };
            grpStorage.Controls.Add(cbStatus);
            grpStorage.Controls.Add(lblStatusBadge);

            // Storage Table
            var lvStorage = new ListView { Location = new Point(10, 80), Size = new Size(405, 120), View = View.Details, FullRowSelect = true, GridLines = true, Font = new Font("Segoe UI", 8.5F) };
            lvStorage.Columns.Add("Status", 80);
            lvStorage.Columns.Add("Database", 100);
            lvStorage.Columns.Add("Location", 130);
            lvStorage.Columns.Add("Used space", 85);

            Action updateStorageRows = () =>
            {
                lvStorage.Items.Clear();
                bool isActive = cbStatus.SelectedIndex == 0;
                string retName = cbRetention.SelectedItem != null ? cbRetention.SelectedItem.ToString().Split(' ')[0] : "Live-60days";
                string drivePath = retName.Contains("30") ? "E:\\Live-30Days" : retName.Contains("90") ? "E:\\Live-90Days" : "E:\\Live-60Days";
                string usedSize = retName.Contains("30") ? "28.6 GB" : retName.Contains("90") ? "84.2 GB" : "49.3 GB";
                string totalUsed = retName.Contains("30") ? "0.98 TB" : retName.Contains("90") ? "1.82 TB" : "1.41 TB";

                var item1 = new ListViewItem(isActive ? "🟢 Active" : "🔴 Not Active");
                item1.ForeColor = isActive ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38);
                item1.SubItems.AddRange(new[] { retName, drivePath, usedSize });

                var item2 = new ListViewItem(isActive ? "🟢 Active" : "🔴 Not Active");
                item2.ForeColor = isActive ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38);
                item2.SubItems.AddRange(new[] { "DATA-Archive", "D:\\DATA-Vault", "1.36 TB" });

                lvStorage.Items.AddRange(new[] { item1, item2 });
            };

            cbStatus.SelectedIndexChanged += (s, e) =>
            {
                bool isActive = cbStatus.SelectedIndex == 0;
                if (isActive)
                {
                    lblStatusBadge.Text = "🟢 Active (Recording)";
                    lblStatusBadge.ForeColor = Color.FromArgb(16, 185, 129); // Vibrant Green
                }
                else
                {
                    lblStatusBadge.Text = "🔴 Not Active (Offline)";
                    lblStatusBadge.ForeColor = Color.FromArgb(220, 38, 38); // Vibrant Red
                }
                updateStorageRows();
            };

            cbRetention.SelectedIndexChanged += (s, e) => updateStorageRows();
            updateStorageRows();
            grpStorage.Controls.Add(lvStorage);

            // Bottom of Storage Box: Total used space & Delete recordings
            grpStorage.Controls.Add(new Label { Text = "Total used space:", Location = new Point(50, 210), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) });
            var txtTotalUsed = new TextBox { Text = "1.41 TB", ReadOnly = true, Location = new Point(155, 207), Size = new Size(65, 23), Font = new Font("Segoe UI", 8.5F), BackColor = Color.White };
            var btnDeleteAll = new Button { Text = "Delete all recordings", Location = new Point(230, 206), Size = new Size(130, 25), Font = new Font("Segoe UI", 8.5F), FlatStyle = FlatStyle.Flat };
            var btnRefreshStorage = new Button { Text = "🔄", Location = new Point(365, 206), Size = new Size(26, 25), Font = new Font("Segoe UI", 8.5F), FlatStyle = FlatStyle.Flat };
            btnRefreshStorage.Click += (s, e) => updateStorageRows();

            grpStorage.Controls.Add(txtTotalUsed);
            grpStorage.Controls.Add(btnDeleteAll);
            grpStorage.Controls.Add(btnRefreshStorage);

            pnl.Controls.Add(grpStorage);
            y += 285;

            // GroupBox: Remote recordings
            var grpRemote = new GroupBox { Text = "Remote recordings", Location = new Point(8, y), Size = new Size(425, 45), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            pnl.Controls.Add(grpRemote);

            return pnl;
        }

        // ==========================================
        // 5. MOTION TAB (Exact Milestone Motion Detection)
        // ==========================================
        private Control BuildMotionTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10), BackColor = Color.FromArgb(248, 250, 252) };

            var chkMotion = new CheckBox { Text = "Motion detection", Checked = true, Location = new Point(12, 10), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            pnl.Controls.Add(chkMotion);

            // Left Side: Motion Preview Box
            var grpPreview = new GroupBox { Text = "Motion preview", Location = new Point(10, 36), Size = new Size(270, 390), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            var lblHint = new Label { Text = "Use left and right mouse buttons to select/clear", Location = new Point(10, 18), Size = new Size(250, 28), Font = new Font("Segoe UI", 7.5F), ForeColor = Color.FromArgb(100, 116, 139) };
            grpPreview.Controls.Add(lblHint);

            var pbMotionCctv = new PictureBox { Location = new Point(10, 50), Size = new Size(248, 160), BackColor = Color.FromArgb(15, 23, 42), SizeMode = PictureBoxSizeMode.Zoom };
            if (_cctvBaseImage != null) pbMotionCctv.Image = _cctvBaseImage;
            grpPreview.Controls.Add(pbMotionCctv);

            var chkShowPrivacy = new CheckBox { Text = "Show privacy masks", Checked = false, Location = new Point(10, 360), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(100, 116, 139) };
            grpPreview.Controls.Add(chkShowPrivacy);
            pnl.Controls.Add(grpPreview);

            // Right Side: Hardware Acceleration, Sensitivity Meter, Smart Search
            var pnlRight = new Panel { Location = new Point(290, 36), Size = new Size(280, 400), BackColor = Color.Transparent };

            pnlRight.Controls.Add(new Label { Text = "Hardware acceleration:", Location = new Point(4, 4), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) });
            var rbHwAuto = new RadioButton { Text = "Automatic", Checked = true, Location = new Point(10, 24), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            var rbHwOff = new RadioButton { Text = "Off", Checked = false, Location = new Point(10, 44), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(rbHwAuto);
            pnlRight.Controls.Add(rbHwOff);

            // Sensitivity Gradient Meter
            var pnlMeter = new Panel { Location = new Point(4, 72), Size = new Size(260, 14), BackColor = Color.FromArgb(226, 232, 240), BorderStyle = BorderStyle.FixedSingle };
            pnlMeter.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(new Rectangle(0, 0, 80, 14), Color.FromArgb(16, 185, 129), Color.FromArgb(52, 211, 153), LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, 70, 14);
                }
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawLine(pen, 70, 0, 70, 14);
                }
            };
            pnlRight.Controls.Add(pnlMeter);

            var chkManualSens = new CheckBox { Text = "Manual sensitivity", Checked = false, Location = new Point(6, 92), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(chkManualSens);

            var lblThreshold = new Label { Text = "Threshold: ───────", Location = new Point(6, 116), AutoSize = true, Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(71, 85, 105) };
            pnlRight.Controls.Add(lblThreshold);

            var chkKeyframes = new CheckBox { Text = "Keyframes only (MPEG-4/H.264/H.265)", Checked = true, Location = new Point(6, 144), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(chkKeyframes);

            pnlRight.Controls.Add(new Label { Text = "Process image every (msec):", Location = new Point(6, 172), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            var txtProcessMsec = new TextBox { Text = "500", Location = new Point(190, 169), Size = new Size(65, 23), Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(txtProcessMsec);

            pnlRight.Controls.Add(new Label { Text = "Detection resolution:", Location = new Point(6, 198), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            var txtResolution = new TextBox { Text = "12%", Location = new Point(190, 195), Size = new Size(65, 23), Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(txtResolution);

            var chkSmartSearch = new CheckBox { Text = "Generate motion data for smart search", Checked = true, Location = new Point(6, 226), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(chkSmartSearch);

            var chkExclude = new CheckBox { Text = "Use exclude regions", Checked = false, Location = new Point(10, 250), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(chkExclude);

            var cbGridSize = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(10, 274), Size = new Size(110, 23), Font = new Font("Segoe UI", 8.5F) };
            cbGridSize.Items.AddRange(new object[] { "16 x 16", "32 x 32", "8 x 8" });
            cbGridSize.SelectedIndex = 0;
            pnlRight.Controls.Add(cbGridSize);

            var chkShowGrid = new CheckBox { Text = "Show grid", Checked = true, Location = new Point(135, 274), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            var chkShowRegions = new CheckBox { Text = "Show regions", Checked = true, Location = new Point(135, 298), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnlRight.Controls.Add(chkShowGrid);
            pnlRight.Controls.Add(chkShowRegions);

            var btnClearGrid = new Button { Text = "Clear", Location = new Point(10, 302), Size = new Size(110, 25), Font = new Font("Segoe UI", 8.5F), FlatStyle = FlatStyle.Flat };
            pnlRight.Controls.Add(btnClearGrid);

            pnlRight.Controls.Add(new Label { Text = "Pen size:", Location = new Point(10, 334), AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(100, 116, 139) });
            var tbPenSize = new TrackBar { Minimum = 1, Maximum = 10, Value = 2, Location = new Point(10, 350), Size = new Size(245, 25), TickStyle = TickStyle.None };
            pnlRight.Controls.Add(tbPenSize);
            pnlRight.Controls.Add(new Label { Text = "Small", Location = new Point(10, 375), AutoSize = true, Font = new Font("Segoe UI", 7.5F), ForeColor = Color.FromArgb(100, 116, 139) });

            pnl.Controls.Add(pnlRight);
            return pnl;
        }

        // ==========================================
        // 6. FISHEYE TAB (Editable Form)
        // ==========================================
        private Control BuildFisheyeTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), BackColor = Color.White };
            int y = 10;

            pnl.Controls.Add(new Label { Text = "Fisheye Lens Dewarping Engine", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Location = new Point(10, y), AutoSize = true });
            y += 28;

            var chkDewarp = new CheckBox { Text = "Enable Fisheye Lens Dewarping", Checked = false, Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            pnl.Controls.Add(chkDewarp);
            y += 28;

            var cbMount = AddDropdownRow(pnl, ref y, "Mounting Type:", new[] { "Wall Mounted 180°", "Ceiling Mounted 360°", "Ground / Desk Mounted" });
            var cbLens = AddDropdownRow(pnl, ref y, "Lens Calibration Profile:", new[] { "Standard Panomorph Lens", "ImmerVision Enables RCA-01", "Generic Hemispheric 360" });

            var btnSave = new Button { Text = "💾 Save Lens Calibration", Location = new Point(10, y), Size = new Size(200, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Fisheye lens dewarping profile updated!", "Lens Calibration Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnl.Controls.Add(btnSave);

            return pnl;
        }

        // ==========================================
        // 7. EVENTS TAB (Exact Milestone Event Subscriptions & Select Driver Event Dialog)
        // ==========================================
        private Control BuildEventsTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10), BackColor = Color.FromArgb(248, 250, 252) };
            int y = 8;

            pnl.Controls.Add(new Label { Text = "Camera Event Subscriptions", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(8, y), AutoSize = true });
            y += 24;

            // Toolbar: Add Event, Delete, Save
            var pnlEventBar = new Panel { Location = new Point(8, y), Size = new Size(425, 30), BackColor = Color.Transparent };
            var btnAddEvent = new Button { Text = "➕ Add Event...", Location = new Point(0, 2), Size = new Size(110, 25), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnAddEvent.FlatAppearance.BorderSize = 0;

            var btnDeleteEvent = new Button { Text = "🗑 Delete", Location = new Point(116, 2), Size = new Size(80, 25), BackColor = Color.White, ForeColor = Color.FromArgb(190, 18, 60), Font = new Font("Segoe UI", 8.5F), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnDeleteEvent.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);

            var btnSaveEvents = new Button { Text = "💾 Save Subscriptions", Location = new Point(202, 2), Size = new Size(140, 25), BackColor = Color.White, ForeColor = Color.FromArgb(71, 85, 105), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSaveEvents.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);

            pnlEventBar.Controls.Add(btnAddEvent);
            pnlEventBar.Controls.Add(btnDeleteEvent);
            pnlEventBar.Controls.Add(btnSaveEvents);
            pnl.Controls.Add(pnlEventBar);
            y += 34;

            var lvEvents = new ListView { Location = new Point(8, y), Size = new Size(425, 260), View = View.Details, FullRowSelect = true, GridLines = true, Font = new Font("Segoe UI", 8.5F), BackColor = Color.White };
            lvEvents.Columns.Add("Driver Event Name", 170);
            lvEvents.Columns.Add("Source Driver", 120);
            lvEvents.Columns.Add("Subscription State", 125);

            Action<string, string, string> addEventRow = (name, driver, state) =>
            {
                var itm = new ListViewItem(name);
                itm.ForeColor = Color.FromArgb(190, 18, 60);
                itm.SubItems.Add(driver);
                itm.SubItems.Add(state);
                lvEvents.Items.Add(itm);
            };

            addEventRow("⚡ Motion Started (HW)", "HikVisionGeneric", "🟢 Active (Subscribed)");
            addEventRow("⚡ Motion Stopped (HW)", "HikVisionGeneric", "🟢 Active (Subscribed)");
            addEventRow("⚡ Tampering Started", "HikVisionGeneric", "🟡 Active (Subscribed)");
            addEventRow("⚡ Video Loss Started", "UniversalDriver", "🔴 Alarm Triggered");

            btnAddEvent.Click += (s, e) => PromptSelectDriverEvent(lvEvents);
            btnDeleteEvent.Click += (s, e) =>
            {
                if (lvEvents.SelectedItems.Count > 0)
                {
                    lvEvents.Items.Remove(lvEvents.SelectedItems[0]);
                }
            };
            btnSaveEvents.Click += (s, e) => MessageBox.Show("Driver event subscriptions saved successfully!", "Events Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            pnl.Controls.Add(lvEvents);
            y += 270;

            var grpActions = new GroupBox { Text = "Event Actions & Rules", Location = new Point(8, y), Size = new Size(425, 95), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            grpActions.Controls.Add(new CheckBox { Text = "Start Recording on Event Trigger", Checked = true, Location = new Point(14, 22), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            grpActions.Controls.Add(new CheckBox { Text = "Send Email & Push Notification to Administrators", Checked = true, Location = new Point(14, 44), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            grpActions.Controls.Add(new CheckBox { Text = "Move PTZ Camera to Preset 1 (Home) on Alarm", Checked = false, Location = new Point(14, 66), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            pnl.Controls.Add(grpActions);

            return pnl;
        }

        private void PromptSelectDriverEvent(ListView targetListView)
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Select Driver Event";
                dlg.Size = new Size(520, 430);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(248, 250, 252);
                dlg.Font = new Font("Segoe UI", 9F);

                var lvSelect = new ListView
                {
                    Location = new Point(12, 12),
                    Size = new Size(480, 320),
                    View = View.Details,
                    FullRowSelect = true,
                    MultiSelect = true,
                    HeaderStyle = ColumnHeaderStyle.None,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 9F)
                };
                lvSelect.Columns.Add("Event", 460);

                string[] driverEvents = new[]
                {
                    "⚡ Intrusion started",
                    "⚡ Intrusion stopped",
                    "⚡ Motion Started (HW)",
                    "⚡ Motion Stopped (HW)",
                    "⚡ Object Entering",
                    "⚡ Object Exiting",
                    "⚡ PIR Sensor",
                    "⚡ Scene change",
                    "⚡ Scene Change Stop",
                    "⚡ SD Card Error",
                    "⚡ Tampering Started",
                    "⚡ Tampering Stopped",
                    "⚡ Tripwire",
                    "⚡ Video Loss Started",
                    "⚡ Video Loss Stopped"
                };

                foreach (var ev in driverEvents)
                {
                    var itm = new ListViewItem(ev);
                    itm.ForeColor = Color.FromArgb(15, 23, 42);
                    lvSelect.Items.Add(itm);
                }

                if (lvSelect.Items.Count > 0) lvSelect.Items[0].Selected = true;

                var chkShowAdded = new CheckBox
                {
                    Text = "Show already added events",
                    Checked = false,
                    Location = new Point(12, 345),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5F)
                };

                var btnOk = new Button
                {
                    Text = "OK",
                    Location = new Point(310, 342),
                    Size = new Size(85, 28),
                    DialogResult = DialogResult.OK,
                    BackColor = Color.FromArgb(190, 18, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Location = new Point(405, 342),
                    Size = new Size(85, 28),
                    DialogResult = DialogResult.Cancel,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F)
                };

                dlg.Controls.AddRange(new Control[] { lvSelect, chkShowAdded, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (ListViewItem sel in lvSelect.SelectedItems)
                    {
                        var itm = new ListViewItem(sel.Text);
                        itm.ForeColor = Color.FromArgb(190, 18, 60);
                        itm.SubItems.Add("HikVisionGeneric");
                        itm.SubItems.Add("🟢 Active (Subscribed)");
                        targetListView.Items.Add(itm);
                    }
                }
            }
        }

        // ==========================================
        // 8. CLIENT TAB (Editable Form)
        // ==========================================
        private Control BuildClientTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), BackColor = Color.White };
            int y = 10;

            pnl.Controls.Add(new Label { Text = "Smart Client Display Preferences", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Location = new Point(10, y), AutoSize = true });
            y += 28;

            var cbStream = AddDropdownRow(pnl, ref y, "Live Display Stream:", new[] { "Adaptive (Stream 1 for single / Stream 2 for grid)", "Always Stream 1 (Full UHD)", "Always Stream 2 (Low Bandwidth)" });
            var cbHw = AddDropdownRow(pnl, ref y, "Hardware Acceleration:", new[] { "Auto Detect (NVIDIA CUDA / Intel QuickSync)", "DirectX 11 D3D11VA", "Off (Software CPU Only)" });
            var numPtz = AddNumericRow(pnl, ref y, "PTZ Priority Level:", 1, 1, 10);

            var btnSave = new Button { Text = "💾 Save Client Preferences", Location = new Point(10, y), Size = new Size(200, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Smart Client preferences saved!", "Client Preferences Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnl.Controls.Add(btnSave);

            return pnl;
        }

        // ==========================================
        // 9. PRIVACY MASKING TAB (Editable Form)
        // ==========================================
        private Control BuildPrivacyTab()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), BackColor = Color.White };
            int y = 10;

            pnl.Controls.Add(new Label { Text = "Privacy Masking & Video Redaction", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Location = new Point(10, y), AutoSize = true });
            y += 28;

            var chkMask = new CheckBox { Text = "Enable Permanent Privacy Masking", Checked = true, Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            pnl.Controls.Add(chkMask);
            y += 28;

            var cbColor = AddDropdownRow(pnl, ref y, "Mask Color / Texture:", new[] { "Solid Black", "Mosaic / Pixelate Blur", "Solid Dark Gray" });
            var chkLift = new CheckBox { Text = "Allow Liftable Privacy Mask with Supervisor PIN", Checked = true, Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            pnl.Controls.Add(chkLift);
            y += 32;

            var btnSave = new Button { Text = "💾 Apply Privacy Masking", Location = new Point(10, y), Size = new Size(200, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Privacy masking redaction grid applied!", "Privacy Masking Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnl.Controls.Add(btnSave);

            return pnl;
        }

        // --- Helper Form Builders ---
        private TextBox AddEditableRow(Panel pnl, ref int y, string label, string defaultValue)
        {
            var lblTag = new Label { Text = label, Location = new Point(10, y + 3), Size = new Size(130, 20), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            var txt = new TextBox { Text = defaultValue, Location = new Point(145, y), Size = new Size(260, 24), Font = new Font("Segoe UI", 9F) };
            pnl.Controls.Add(lblTag);
            pnl.Controls.Add(txt);
            y += 28;
            return txt;
        }

        private ComboBox AddDropdownRow(Panel pnl, ref int y, string label, string[] options)
        {
            var lblTag = new Label { Text = label, Location = new Point(10, y + 3), Size = new Size(140, 20), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            var cb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(155, y), Size = new Size(250, 24), Font = new Font("Segoe UI", 9F) };
            cb.Items.AddRange(options);
            cb.SelectedIndex = 0;
            pnl.Controls.Add(lblTag);
            pnl.Controls.Add(cb);
            y += 28;
            return cb;
        }

        private NumericUpDown AddNumericRow(Panel pnl, ref int y, string label, decimal val, decimal min, decimal max)
        {
            var lblTag = new Label { Text = label, Location = new Point(10, y + 3), Size = new Size(140, 20), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            var num = new NumericUpDown { Minimum = min, Maximum = max, Value = val, Location = new Point(155, y), Size = new Size(250, 24), Font = new Font("Segoe UI", 9F) };
            pnl.Controls.Add(lblTag);
            pnl.Controls.Add(num);
            y += 28;
            return num;
        }

        // ==========================================
        // BOTTOM LIVE STREAM & PTZ
        // ==========================================
        private void BuildPreviewPane(Panel pnlPreviewContainer)
        {
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 30, BackColor = Color.FromArgb(255, 241, 242), Padding = new Padding(8, 2, 8, 2) };

            var lblTitle = new Label { Text = "📹 Lychee Live Stream & PTZ Control", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 4, 0, 0) };
            pnlHeader.Controls.Add(lblTitle);

            // PTZ Toolbar
            var pnlPtz = new Panel { Dock = DockStyle.Right, Width = 460, BackColor = Color.Transparent };
            var ptzBtns = new[] { "◀ Pan Left", "▲ Tilt Up", "▼ Tilt Down", "▶ Pan Right", "🔍 Zoom +", "🔍 Zoom -" };
            int bx = 0;
            foreach (var t in ptzBtns)
            {
                var b = new Button
                {
                    Text = t,
                    Location = new Point(bx, 2),
                    Size = new Size(72, 24),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(190, 18, 60),
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                b.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
                pnlPtz.Controls.Add(b);
                bx += 76;
            }
            pnlHeader.Controls.Add(pnlPtz);
            pnlPreviewContainer.Controls.Add(pnlHeader);

            var pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(11, 15, 25) };

            _lblPreviewBottomName = new Label
            {
                Text = "SELECTED CAMERA: LYCHEE MAIN ENTRANCE CAM 01 • [192.168.1.201] • PROTOCOL: ONVIF Generic Profile S/G/T",
                ForeColor = Color.FromArgb(190, 18, 60),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 22
            };

            _pbVideoFeed = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(11, 15, 25)
            };
            _pbVideoFeed.Paint += PbVideoFeed_Paint;
            _pbVideoFeed.Resize += (s, e) => _pbVideoFeed.Invalidate();

            pnlContent.Controls.Add(_pbVideoFeed);
            pnlContent.Controls.Add(_lblPreviewBottomName);

            pnlPreviewContainer.Controls.Add(pnlContent);
            pnlContent.BringToFront();
        }

        private void PbVideoFeed_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = _pbVideoFeed.Width;
            int h = _pbVideoFeed.Height;
            if (w <= 10 || h <= 10) return;

            using (var bgBrush = new SolidBrush(Color.FromArgb(11, 15, 25)))
            {
                g.FillRectangle(bgBrush, 0, 0, w, h);
            }

            if (_cctvBaseImage != null)
            {
                int frameW = w - 40;
                int frameH = (int)(frameW * 9.0 / 16.0);
                if (frameH > h - 20)
                {
                    frameH = h - 20;
                    frameW = (int)(frameH * 16.0 / 9.0);
                }
                int fx = (w - frameW) / 2;
                int fy = (h - frameH) / 2;
                g.DrawImage(_cctvBaseImage, fx, fy, frameW, frameH);
            }

            // Draw Simulated AI Object Detection Bounding Box
            int boxW = 80;
            int boxH = 50;
            int boxX = (w / 2) - 40;
            int boxY = (h / 2) - 25;
            using (var penBox = new Pen(Color.FromArgb(190, 18, 60), 2))
            {
                g.DrawRectangle(penBox, boxX, boxY, boxW, boxH);
            }
            using (var brushFill = new SolidBrush(Color.FromArgb(40, 190, 18, 60)))
            {
                g.FillRectangle(brushFill, boxX, boxY, boxW, boxH);
            }

            // Top-Left OSD Information
            string nowStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string osdLine1 = "● LYCHEE LIVE [30 FPS] | BITRATE: 8192 kbps | CODEC: H.265+ | 3840x2160 (4K UHD)";
            string osdLine2 = "TIMESTAMP: " + nowStr;

            using (var fontOsd = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var brushOsd = new SolidBrush(Color.FromArgb(190, 18, 60)))
            {
                g.DrawString(osdLine1, fontOsd, brushOsd, new PointF(12, 10));
            }
            using (var fontTs = new Font("Segoe UI", 8.5F, FontStyle.Regular))
            using (var brushTs = new SolidBrush(Color.FromArgb(241, 245, 249)))
            {
                g.DrawString(osdLine2, fontTs, brushTs, new PointF(12, 28));
            }
        }

        public void SelectCamera(string name)
        {
            var cam = _vmoService.GetCamera(name);
            if (cam == null) return;

            _currentCam = cam;

            if (_lblPreviewBottomName != null)
            {
                _lblPreviewBottomName.Text = "SELECTED CAMERA: " + cam.Name + " • [" + (string.IsNullOrEmpty(cam.MulticastIp) ? "192.168.1.202" : cam.MulticastIp) + "] • PROTOCOL: ONVIF Generic Profile S/G/T";
            }

            RenderActiveTabContent();
            if (_pbVideoFeed != null) _pbVideoFeed.Invalidate();
        }

        public void SaveCurrentCamera()
        {
            if (_currentCam == null) return;
            _vmoService.SaveCameraSettings(_currentCam);
        }

        public void PromptAddDeviceGroup()
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Add Lychee Camera";
                dlg.Size = new Size(400, 220);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(248, 250, 252);
                dlg.Font = new Font("Segoe UI", 9F);

                var lbl = new Label { Text = "Camera Name:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txt = new TextBox { Location = new Point(20, 45), Size = new Size(340, 24), Text = "LYCHEE NEW CAMERA " + (_vmoService.GetAllCameras().Count + 1) };

                var lblIp = new Label { Text = "IP Address:", Location = new Point(20, 75), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtIp = new TextBox { Location = new Point(20, 95), Size = new Size(340, 24), Text = "192.168.1." + (200 + _vmoService.GetAllCameras().Count + 1) };

                var btnOk = new Button { Text = "Add Camera", Location = new Point(160, 135), Size = new Size(100, 28), DialogResult = DialogResult.OK, BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(270, 135), Size = new Size(90, 28), DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat };

                dlg.Controls.AddRange(new Control[] { lbl, txt, lblIp, txtIp, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text))
                {
                    _vmoService.AddCameraToGroup(txt.Text.Trim(), "Lychee Recording Cluster", "3840x2160 (4K UHD)");
                    var cam = _vmoService.GetCamera(txt.Text.Trim());
                    if (cam != null) cam.MulticastIp = txtIp.Text.Trim();
                    PopulateDevicesTree();
                    SelectCamera(txt.Text.Trim());
                }
            }
        }

        public void PromptAddServer()
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Add Recording Server";
                dlg.Size = new Size(400, 240);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(248, 250, 252);
                dlg.Font = new Font("Segoe UI", 9F);

                var lblName = new Label { Text = "Server Display Name:", Location = new Point(20, 15), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtName = new TextBox { Location = new Point(20, 38), Size = new Size(340, 24), Text = "LYCHEE-REC-SRV-0" + (_vmoService.GetServers().Count + 1), Font = new Font("Segoe UI", 9.5F) };

                var lblHost = new Label { Text = "IP Address / FQDN:", Location = new Point(20, 72), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtHost = new TextBox { Location = new Point(20, 95), Size = new Size(210, 24), Text = "192.168.1." + (100 + _vmoService.GetServers().Count), Font = new Font("Segoe UI", 9.5F) };

                var lblPort = new Label { Text = "Port:", Location = new Point(245, 72), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var numPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 7563, Location = new Point(245, 95), Size = new Size(115, 24), Font = new Font("Segoe UI", 9.5F) };

                var btnOk = new Button { Text = "➕ Add Server", Location = new Point(140, 145), Size = new Size(110, 30), DialogResult = DialogResult.OK, BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(260, 145), Size = new Size(100, 30), DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat };

                dlg.Controls.AddRange(new Control[] { lblName, txtName, lblHost, txtHost, lblPort, numPort, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txtName.Text))
                {
                    _vmoService.AddRecordingServer(txtName.Text.Trim(), txtHost.Text.Trim(), (int)numPort.Value);
                    PopulateDevicesTree();
                }
            }
        }

        public void PromptRenameServer(string currentName)
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Rename Recording Server";
                dlg.Size = new Size(380, 180);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(248, 250, 252);
                dlg.Font = new Font("Segoe UI", 9F);

                var lbl = new Label { Text = "Recording Server Name:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txt = new TextBox { Location = new Point(20, 45), Size = new Size(320, 24), Text = currentName, Font = new Font("Segoe UI", 9.5F) };

                var btnOk = new Button { Text = "Rename", Location = new Point(140, 95), Size = new Size(95, 28), DialogResult = DialogResult.OK, BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(245, 95), Size = new Size(95, 28), DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat };

                dlg.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text))
                {
                    _vmoService.RenameRecordingServer(currentName, txt.Text.Trim());
                    PopulateDevicesTree();
                }
            }
        }
        public void PromptRenameDevice(CameraModel cam)
        {
            if (cam == null) return;
            using (var dlg = new Form())
            {
                dlg.Text = "Rename Camera";
                dlg.Size = new Size(380, 180);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(248, 250, 252);
                dlg.Font = new Font("Segoe UI", 9F);

                var lbl = new Label { Text = "Camera Display Name:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txt = new TextBox { Location = new Point(20, 45), Size = new Size(320, 24), Text = cam.Name, Font = new Font("Segoe UI", 9.5F) };

                var btnOk = new Button { Text = "Rename", Location = new Point(140, 95), Size = new Size(95, 28), DialogResult = DialogResult.OK, BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(245, 95), Size = new Size(95, 28), DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat };

                dlg.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text))
                {
                    _vmoService.RenameCamera(cam.Name, txt.Text.Trim());
                    PopulateDevicesTree();
                    SelectCamera(txt.Text.Trim());
                }
            }
        }
    }
}
