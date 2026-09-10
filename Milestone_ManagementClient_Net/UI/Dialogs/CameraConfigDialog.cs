using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Dialogs
{
    public class CameraConfigDialog : Form
    {
        private readonly MilestoneVmoService _vmoService;
        private readonly CameraModel _targetCam;
        private readonly bool _isEditMode;
        private readonly Action _onSaved;

        // Header controls
        private Label _lblHeaderTitle;
        private Label _lblHeaderSub;

        // Tab 1: General & Hardware
        private TextBox _txtName;
        private ComboBox _cbServer;
        private ComboBox _cbManufacturer;
        private TextBox _txtModel;
        private ComboBox _cbDriver;
        private TextBox _txtDriverVer;
        private TextBox _txtFirmwareVer;
        private TextBox _txtMac;
        private CheckBox _chkOnline;

        // Tab 2: Network & Authentication
        private TextBox _txtIp;
        private NumericUpDown _numHttpPort;
        private NumericUpDown _numRtspPort;
        private ComboBox _cbStreamMethod;
        private TextBox _txtUserName;
        private TextBox _txtPassword;
        private CheckBox _chkShowPassword;
        private TextBox _txtRtspUrlPreview;
        private Button _btnTestConnection;
        private Label _lblTestResult;

        // Tab 3: Video & Encoding
        private ComboBox _cbResolution;
        private ComboBox _cbCodec;
        private NumericUpDown _numFps;
        private ComboBox _cbBitrateMode;
        private NumericUpDown _numQuality;
        private ComboBox _cbStream2Res;
        private NumericUpDown _numStream2Fps;

        // Tab 4: Recording & Storage
        private ComboBox _cbRetention;
        private TextBox _txtStoragePath;
        private Button _btnBrowseStorage;
        private NumericUpDown _numRecordFps;
        private CheckBox _chkKeyframesOnly;
        private NumericUpDown _numPrebufferSec;
        private ComboBox _cbPrebufferType;
        private CheckBox _chkMotion;
        private TrackBar _tbMotionSens;
        private Label _lblMotionSensVal;

        // Bottom Action buttons
        private Button _btnSave;
        private Button _btnCancel;

        /// <summary>
        /// Constructor for Editing an Existing Camera
        /// </summary>
        public CameraConfigDialog(MilestoneVmoService vmoService, CameraModel existingCam, Action onSaved = null)
        {
            _vmoService = vmoService;
            _targetCam = existingCam;
            _isEditMode = true;
            _onSaved = onSaved;

            InitializeComponent();
            LoadCameraData();
        }

        /// <summary>
        /// Constructor for Adding a New Camera
        /// </summary>
        public CameraConfigDialog(MilestoneVmoService vmoService, string defaultServerName, Action onSaved = null)
        {
            _vmoService = vmoService;
            _isEditMode = false;
            _onSaved = onSaved;

            _targetCam = new CameraModel
            {
                Name = "CAM-0" + (_vmoService.GetAllCameras().Count + 1),
                ServerName = defaultServerName,
                Group = defaultServerName,
                IpAddress = "10.192.0." + (10 + _vmoService.GetAllCameras().Count + 1),
                MulticastIp = "10.192.0." + (10 + _vmoService.GetAllCameras().Count + 1),
                HttpPort = 80,
                RtspPort = 554,
                UserName = "admin",
                Password = "Password123",
                Manufacturer = "Universal ONVIF",
                Model = "Generic 4K IP Camera",
                DriverName = "Universal ONVIF Conformance Driver",
                IsOnline = true
            };

            InitializeComponent();
            LoadCameraData();
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode
                ? "Edit Camera & Hardware Settings • Milestone XProtect"
                : "Add New Camera & Hardware Configuration • Milestone XProtect";
            this.Size = new Size(760, 620);
            this.MinimumSize = new Size(740, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lychee.ico");
            if (!File.Exists(icoPath)) icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (File.Exists(icoPath)) try { this.Icon = new Icon(icoPath); } catch { }

            // 1. Header Banner
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(18, 12, 18, 10)
            };
            pnlHeader.Paint += (s, pe) =>
            {
                using (var brush = new LinearGradientBrush(pnlHeader.ClientRectangle, Color.FromArgb(15, 23, 42), Color.FromArgb(30, 58, 138), LinearGradientMode.Horizontal))
                {
                    pe.Graphics.FillRectangle(brush, pnlHeader.ClientRectangle);
                }
                using (var p = new Pen(Color.FromArgb(190, 18, 60), 3))
                {
                    pe.Graphics.DrawLine(p, 0, pnlHeader.Height - 2, pnlHeader.Width, pnlHeader.Height - 2);
                }
            };

            _lblHeaderTitle = new Label
            {
                Text = _isEditMode ? "✏ Edit Camera & Hardware Properties" : "➕ Add Camera & Hardware Configuration",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(18, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            _lblHeaderSub = new Label
            {
                Text = "Configure camera device metadata, network address, ONVIF/ISAPI credentials, stream encoding, and storage retention.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(203, 213, 225),
                Location = new Point(19, 34),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            pnlHeader.Controls.Add(_lblHeaderTitle);
            pnlHeader.Controls.Add(_lblHeaderSub);
            this.Controls.Add(pnlHeader);

            // 2. Bottom Action Panel
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(15, 10, 15, 10)
            };

            _btnSave = new Button
            {
                Text = _isEditMode ? "💾 Save Changes" : "➕ Add Camera",
                Size = new Size(130, 32),
                Location = new Point(this.ClientSize.Width - 250, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(85, 32),
                Location = new Point(this.ClientSize.Width - 105, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            _btnCancel.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);

            pnlBottom.Controls.Add(_btnSave);
            pnlBottom.Controls.Add(_btnCancel);
            this.Controls.Add(pnlBottom);

            // 3. TabControl
            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                Padding = new Point(12, 6)
            };

            tabControl.TabPages.Add(BuildGeneralTab());
            tabControl.TabPages.Add(BuildNetworkTab());
            tabControl.TabPages.Add(BuildVideoTab());
            tabControl.TabPages.Add(BuildStorageTab());

            this.Controls.Add(tabControl);
            tabControl.BringToFront();

            this.AcceptButton = _btnSave;
            this.CancelButton = _btnCancel;
        }

        // ==========================================
        // TAB 1: General & Hardware Identification
        // ==========================================
        private TabPage BuildGeneralTab()
        {
            var tab = new TabPage("ℹ️ Device & Hardware");
            tab.BackColor = Color.White;
            tab.Padding = new Padding(20, 15, 20, 15);
            tab.AutoScroll = true;

            int y = 15;

            // Camera Name
            tab.Controls.Add(new Label { Text = "Camera Display Name:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            _txtName = new TextBox { Location = new Point(20, y + 22), Size = new Size(400, 24), Font = new Font("Segoe UI", 9.5F) };
            tab.Controls.Add(_txtName);

            // Online Toggle
            _chkOnline = new CheckBox { Text = "Device Enabled & Online in VMS Cluster", Location = new Point(440, y + 22), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(5, 150, 105) };
            tab.Controls.Add(_chkOnline);
            y += 56;

            // Recording Server Assignment
            tab.Controls.Add(new Label { Text = "Recording Server Assignment:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            _cbServer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, y + 22), Size = new Size(660, 24) };
            var servers = _vmoService.GetServers();
            foreach (var s in servers) _cbServer.Items.Add(s.Name);
            if (_cbServer.Items.Count == 0) _cbServer.Items.Add("LYCHEE-REC-SRV-01");
            tab.Controls.Add(_cbServer);
            y += 56;

            // Manufacturer / Brand
            tab.Controls.Add(new Label { Text = "Manufacturer / Brand:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Hardware Model:", Location = new Point(360, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });

            _cbManufacturer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Location = new Point(20, y + 22), Size = new Size(320, 24) };
            string[] brands = new string[] {
                "Universal ONVIF", "Hikvision", "Axis Communications", "Dahua Technology",
                "Uniview", "Bosch Security", "Hanwha Vision", "Pelco", "Sony",
                "Panasonic / i-PRO", "CP PLUS", "Honeywell", "FLIR Systems", "Mobotix",
                "Vivotek", "Avigilon", "TP-Link / Tapo", "Alcon"
            };
            _cbManufacturer.Items.AddRange(brands);
            _cbManufacturer.SelectedIndexChanged += CbManufacturer_SelectedIndexChanged;

            _txtModel = new TextBox { Location = new Point(360, y + 22), Size = new Size(320, 24), Font = new Font("Segoe UI", 9F) };
            tab.Controls.Add(_cbManufacturer);
            tab.Controls.Add(_txtModel);
            y += 56;

            // Driver Framework
            tab.Controls.Add(new Label { Text = "Hardware Driver Framework (24+ Multi-Brand Supported):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            _cbDriver = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, y + 22), Size = new Size(660, 24) };
            _cbDriver.Items.AddRange(HardwareDriverRegistry.GetAllDriverNames());
            tab.Controls.Add(_cbDriver);
            y += 56;

            // Driver Version & Firmware Version
            tab.Controls.Add(new Label { Text = "Driver Version:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Firmware Version:", Location = new Point(240, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "MAC Address:", Location = new Point(460, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });

            _txtDriverVer = new TextBox { Location = new Point(20, y + 20), Size = new Size(200, 24), Text = "v2025 R3" };
            _txtFirmwareVer = new TextBox { Location = new Point(240, y + 20), Size = new Size(200, 24), Text = "V5.7.12_240115" };
            _txtMac = new TextBox { Location = new Point(460, y + 20), Size = new Size(220, 24), Text = "00:40:8C:92:A4:1F" };

            tab.Controls.Add(_txtDriverVer);
            tab.Controls.Add(_txtFirmwareVer);
            tab.Controls.Add(_txtMac);

            return tab;
        }

        // ==========================================
        // TAB 2: Network & Authentication
        // ==========================================
        private TabPage BuildNetworkTab()
        {
            var tab = new TabPage("🌐 Network & Credentials");
            tab.BackColor = Color.White;
            tab.Padding = new Padding(20, 15, 20, 15);
            tab.AutoScroll = true;

            int y = 15;

            // IP Address / Host & Ports
            tab.Controls.Add(new Label { Text = "IP Address / Host Name:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "HTTP / Web Port:", Location = new Point(340, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "RTSP Port:", Location = new Point(510, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });

            _txtIp = new TextBox { Location = new Point(20, y + 22), Size = new Size(300, 24), Font = new Font("Segoe UI", 9.5F) };
            _txtIp.TextChanged += (s, e) => UpdateRtspPreview();

            _numHttpPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 80, Location = new Point(340, y + 22), Size = new Size(150, 24), Font = new Font("Segoe UI", 9.5F) };
            _numRtspPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 554, Location = new Point(510, y + 22), Size = new Size(170, 24), Font = new Font("Segoe UI", 9.5F) };
            _numRtspPort.ValueChanged += (s, e) => UpdateRtspPreview();

            tab.Controls.Add(_txtIp);
            tab.Controls.Add(_numHttpPort);
            tab.Controls.Add(_numRtspPort);
            y += 56;

            // Streaming Transport Protocol
            tab.Controls.Add(new Label { Text = "Streaming Transport Protocol:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            _cbStreamMethod = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, y + 22), Size = new Size(660, 24) };
            _cbStreamMethod.Items.AddRange(new object[] {
                "RTP/RTSP/TCP (Recommended / Reliable)",
                "RTP/RTSP/HTTP (Tunneling through firewall)",
                "UDP Multicast",
                "HTTP Snapshot / ISAPI Direct"
            });
            _cbStreamMethod.SelectedIndex = 0;
            tab.Controls.Add(_cbStreamMethod);
            y += 56;

            // Camera User Name & Password
            tab.Controls.Add(new Label { Text = "Camera User Name:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Camera Password:", Location = new Point(340, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });

            _txtUserName = new TextBox { Location = new Point(20, y + 22), Size = new Size(300, 24), Font = new Font("Segoe UI", 9.5F) };
            _txtUserName.TextChanged += (s, e) => UpdateRtspPreview();

            _txtPassword = new TextBox { Location = new Point(340, y + 22), Size = new Size(220, 24), UseSystemPasswordChar = true, Font = new Font("Segoe UI", 9.5F) };
            _txtPassword.TextChanged += (s, e) => UpdateRtspPreview();

            _chkShowPassword = new CheckBox { Text = "Show", Location = new Point(570, y + 24), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            _chkShowPassword.CheckedChanged += (s, e) => { _txtPassword.UseSystemPasswordChar = !_chkShowPassword.Checked; };

            tab.Controls.Add(_txtUserName);
            tab.Controls.Add(_txtPassword);
            tab.Controls.Add(_chkShowPassword);
            y += 56;

            // Stream URL Preview
            tab.Controls.Add(new Label { Text = "Primary Stream RTSP URI Preview:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) });
            _txtRtspUrlPreview = new TextBox { Location = new Point(20, y + 20), Size = new Size(660, 24), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249), Font = new Font("Consolas", 9F) };
            tab.Controls.Add(_txtRtspUrlPreview);
            y += 56;

            // Test Connection Box
            var grpTest = new GroupBox
            {
                Text = "⚡ Live Hardware Connection & Credential Verification",
                Location = new Point(20, y),
                Size = new Size(660, 75),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            _btnTestConnection = new Button
            {
                Text = "🔌 Test Connection",
                Location = new Point(15, 25),
                Size = new Size(160, 32),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnTestConnection.FlatAppearance.BorderSize = 0;
            _btnTestConnection.Click += BtnTestConnection_Click;

            _lblTestResult = new Label
            {
                Text = "Click 'Test Connection' to verify network reachability and login credentials with the camera.",
                Location = new Point(190, 32),
                Size = new Size(450, 24),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            grpTest.Controls.Add(_btnTestConnection);
            grpTest.Controls.Add(_lblTestResult);
            tab.Controls.Add(grpTest);

            return tab;
        }

        // ==========================================
        // TAB 3: Video Streams & Encoding
        // ==========================================
        private TabPage BuildVideoTab()
        {
            var tab = new TabPage("📹 Video Streams & Encoding");
            tab.BackColor = Color.White;
            tab.Padding = new Padding(20, 15, 20, 15);
            tab.AutoScroll = true;

            int y = 15;

            // Primary Resolution & Codec
            tab.Controls.Add(new Label { Text = "Primary Stream Resolution:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Video Codec:", Location = new Point(360, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });

            _cbResolution = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, y + 22), Size = new Size(320, 24) };
            _cbResolution.Items.AddRange(new object[] {
                "3840x2160 (4K UHD)",
                "2560x1440 (2K QHD)",
                "1920x1080 (1080p FHD)",
                "1280x720 (720p HD)",
                "704x576 (4CIF / D1)",
                "640x480 (VGA)"
            });
            _cbResolution.SelectedIndex = 2; // Default FHD

            _cbCodec = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(360, y + 22), Size = new Size(320, 24) };
            _cbCodec.Items.AddRange(new object[] { "H.265", "H.264", "MJPEG", "MPEG-4" });
            _cbCodec.SelectedIndex = 0;

            tab.Controls.Add(_cbResolution);
            tab.Controls.Add(_cbCodec);
            y += 56;

            // FPS, Bitrate Mode & Quality
            tab.Controls.Add(new Label { Text = "Frame Rate (FPS):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Bitrate Mode:", Location = new Point(240, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Quality Level (1-100%):", Location = new Point(480, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });

            _numFps = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 25, Location = new Point(20, y + 22), Size = new Size(200, 24) };

            _cbBitrateMode = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(240, y + 22), Size = new Size(220, 24) };
            _cbBitrateMode.Items.AddRange(new object[] { "Variable bit rate (VBR)", "Constant bit rate (CBR)" });
            _cbBitrateMode.SelectedIndex = 0;

            _numQuality = new NumericUpDown { Minimum = 10, Maximum = 100, Value = 60, Location = new Point(480, y + 22), Size = new Size(200, 24) };

            tab.Controls.Add(_numFps);
            tab.Controls.Add(_cbBitrateMode);
            tab.Controls.Add(_numQuality);
            y += 66;

            // Substream (Stream 2) Box
            var grpSub = new GroupBox
            {
                Text = "Substream (Stream 2) Configuration (Mobile & Live Multi-Grid)",
                Location = new Point(20, y),
                Size = new Size(660, 95),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            grpSub.Controls.Add(new Label { Text = "Substream Resolution:", Location = new Point(15, 25), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _cbStream2Res = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(15, 48), Size = new Size(300, 23), Font = new Font("Segoe UI", 8.5F) };
            _cbStream2Res.Items.AddRange(new object[] { "640x360", "320x240", "704x480" });
            _cbStream2Res.SelectedIndex = 0;

            grpSub.Controls.Add(new Label { Text = "Substream FPS:", Location = new Point(340, 25), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _numStream2Fps = new NumericUpDown { Minimum = 1, Maximum = 30, Value = 25, Location = new Point(340, 48), Size = new Size(150, 23), Font = new Font("Segoe UI", 8.5F) };

            grpSub.Controls.Add(_cbStream2Res);
            grpSub.Controls.Add(_numStream2Fps);
            tab.Controls.Add(grpSub);

            return tab;
        }

        // ==========================================
        // TAB 4: Recording & Storage
        // ==========================================
        private TabPage BuildStorageTab()
        {
            var tab = new TabPage("🔴 Recording & Storage");
            tab.BackColor = Color.White;
            tab.Padding = new Padding(20, 15, 20, 15);
            tab.AutoScroll = true;

            int y = 15;

            // Retention Period
            tab.Controls.Add(new Label { Text = "Archive Retention Period:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            _cbRetention = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(20, y + 22), Size = new Size(660, 24) };
            _cbRetention.Items.AddRange(new object[] {
                "30 Days (Live-30days standard)",
                "60 Days (Live-60days recommended)",
                "90 Days (Live-90days compliance)",
                "180 Days (6 Months Extended Archive)",
                "365 Days (1 Year Legal Hold Vault)"
            });
            _cbRetention.SelectedIndex = 1;
            tab.Controls.Add(_cbRetention);
            y += 56;

            // Storage Directory
            tab.Controls.Add(new Label { Text = "Storage Database Path:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            _txtStoragePath = new TextBox { Location = new Point(20, y + 22), Size = new Size(540, 24), Text = @"D:\Milestone\MediaDatabase" };
            _btnBrowseStorage = new Button { Text = "📂 Browse...", Location = new Point(570, y + 21), Size = new Size(110, 26), Font = new Font("Segoe UI", 8.5F), FlatStyle = FlatStyle.Flat };
            _btnBrowseStorage.Click += (s, e) =>
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select Milestone Camera Media Database Folder";
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        _txtStoragePath.Text = fbd.SelectedPath;
                    }
                }
            };
            tab.Controls.Add(_txtStoragePath);
            tab.Controls.Add(_btnBrowseStorage);
            y += 56;

            // Recording Frame Rate & Prebuffer
            tab.Controls.Add(new Label { Text = "Recording Frame Rate (FPS):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Pre-buffer (Seconds):", Location = new Point(240, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            tab.Controls.Add(new Label { Text = "Pre-buffer Location:", Location = new Point(460, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });

            _numRecordFps = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 25, Location = new Point(20, y + 20), Size = new Size(200, 24) };
            _numPrebufferSec = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 5, Location = new Point(240, y + 20), Size = new Size(200, 24) };

            _cbPrebufferType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(460, y + 20), Size = new Size(220, 24) };
            _cbPrebufferType.Items.AddRange(new object[] { "In memory", "Disk buffer" });
            _cbPrebufferType.SelectedIndex = 0;

            tab.Controls.Add(_numRecordFps);
            tab.Controls.Add(_numPrebufferSec);
            tab.Controls.Add(_cbPrebufferType);
            y += 54;

            _chkKeyframesOnly = new CheckBox { Text = "Record keyframes only (MPEG-4/H.264/H.265 storage saving)", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            tab.Controls.Add(_chkKeyframesOnly);
            y += 36;

            // Motion Detection Section
            var grpMotion = new GroupBox
            {
                Text = "🏃 Motion Detection & Analytics",
                Location = new Point(20, y),
                Size = new Size(660, 80),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            _chkMotion = new CheckBox { Text = "Enable hardware motion detection on camera feed", Checked = true, Location = new Point(15, 22), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            grpMotion.Controls.Add(_chkMotion);

            grpMotion.Controls.Add(new Label { Text = "Sensitivity:", Location = new Point(15, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _tbMotionSens = new TrackBar { Minimum = 1, Maximum = 100, Value = 80, Location = new Point(85, 45), Size = new Size(300, 25), TickStyle = TickStyle.None };
            _lblMotionSensVal = new Label { Text = "80%", Location = new Point(395, 48), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60) };
            _tbMotionSens.ValueChanged += (s, e) => { _lblMotionSensVal.Text = _tbMotionSens.Value + "%"; };

            grpMotion.Controls.Add(_tbMotionSens);
            grpMotion.Controls.Add(_lblMotionSensVal);

            tab.Controls.Add(grpMotion);

            return tab;
        }

        private void LoadCameraData()
        {
            if (_targetCam == null) return;

            _txtName.Text = _targetCam.Name ?? "";
            _chkOnline.Checked = _targetCam.IsOnline;

            // Server
            if (!string.IsNullOrEmpty(_targetCam.ServerName))
            {
                int sIdx = _cbServer.FindStringExact(_targetCam.ServerName);
                if (sIdx >= 0) _cbServer.SelectedIndex = sIdx;
                else
                {
                    _cbServer.Items.Add(_targetCam.ServerName);
                    _cbServer.SelectedItem = _targetCam.ServerName;
                }
            }
            else if (_cbServer.Items.Count > 0)
            {
                _cbServer.SelectedIndex = 0;
            }

            // Manufacturer
            if (!string.IsNullOrEmpty(_targetCam.Manufacturer))
            {
                int mIdx = _cbManufacturer.FindStringExact(_targetCam.Manufacturer);
                if (mIdx >= 0) _cbManufacturer.SelectedIndex = mIdx;
                else _cbManufacturer.Text = _targetCam.Manufacturer;
            }
            else
            {
                _cbManufacturer.SelectedIndex = 0;
            }

            _txtModel.Text = !string.IsNullOrEmpty(_targetCam.Model) ? _targetCam.Model : "Generic Profile S/G/T Network Camera";

            // Driver
            if (!string.IsNullOrEmpty(_targetCam.DriverName))
            {
                int dIdx = _cbDriver.FindStringExact(_targetCam.DriverName);
                if (dIdx >= 0) _cbDriver.SelectedIndex = dIdx;
                else
                {
                    for (int i = 0; i < _cbDriver.Items.Count; i++)
                    {
                        if (_cbDriver.Items[i].ToString().IndexOf(_targetCam.DriverName, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            _cbDriver.SelectedIndex = i;
                            break;
                        }
                    }
                    if (_cbDriver.SelectedIndex < 0 && _cbDriver.Items.Count > 0) _cbDriver.SelectedIndex = 0;
                }
            }
            else if (_cbDriver.Items.Count > 0)
            {
                _cbDriver.SelectedIndex = 0;
            }

            _txtDriverVer.Text = !string.IsNullOrEmpty(_targetCam.DriverVersion) ? _targetCam.DriverVersion : "v2025 R3";
            _txtFirmwareVer.Text = !string.IsNullOrEmpty(_targetCam.FirmwareVersion) ? _targetCam.FirmwareVersion : "V5.7.12_240115";
            _txtMac.Text = !string.IsNullOrEmpty(_targetCam.MacAddress) ? _targetCam.MacAddress : "00:40:8C:92:A4:1F";

            // Network
            string ip = !string.IsNullOrEmpty(_targetCam.IpAddress)
                ? _targetCam.IpAddress
                : (!string.IsNullOrEmpty(_targetCam.MulticastIp) && _targetCam.MulticastIp != "0.0.0.0" ? _targetCam.MulticastIp : "10.192.0.77");
            _txtIp.Text = ip;

            _numHttpPort.Value = _targetCam.HttpPort > 0 ? _targetCam.HttpPort : 80;
            _numRtspPort.Value = _targetCam.RtspPort > 0 ? _targetCam.RtspPort : 554;

            _txtUserName.Text = _targetCam.UserName ?? "admin";
            _txtPassword.Text = _targetCam.Password ?? "";

            if (!string.IsNullOrEmpty(_targetCam.StreamingMethod))
            {
                int smIdx = _cbStreamMethod.FindString(_targetCam.StreamingMethod);
                if (smIdx >= 0) _cbStreamMethod.SelectedIndex = smIdx;
            }

            // Video
            if (!string.IsNullOrEmpty(_targetCam.Resolution))
            {
                int rIdx = _cbResolution.FindString(_targetCam.Resolution.Split(' ')[0]);
                if (rIdx >= 0) _cbResolution.SelectedIndex = rIdx;
            }
            if (!string.IsNullOrEmpty(_targetCam.Codec))
            {
                int cIdx = _cbCodec.FindStringExact(_targetCam.Codec);
                if (cIdx >= 0) _cbCodec.SelectedIndex = cIdx;
            }
            _numFps.Value = _targetCam.Fps > 0 ? Math.Min(60, Math.Max(1, _targetCam.Fps)) : 25;
            _numQuality.Value = _targetCam.Quality > 0 ? Math.Min(100, Math.Max(10, _targetCam.Quality)) : 60;
            if (!string.IsNullOrEmpty(_targetCam.BitrateMode))
            {
                int bIdx = _cbBitrateMode.FindString(_targetCam.BitrateMode);
                if (bIdx >= 0) _cbBitrateMode.SelectedIndex = bIdx;
            }

            if (!string.IsNullOrEmpty(_targetCam.Stream2Res))
            {
                int s2Idx = _cbStream2Res.FindStringExact(_targetCam.Stream2Res);
                if (s2Idx >= 0) _cbStream2Res.SelectedIndex = s2Idx;
            }
            _numStream2Fps.Value = _targetCam.Stream2Fps > 0 ? Math.Min(30, Math.Max(1, _targetCam.Stream2Fps)) : 25;

            // Storage
            if (_targetCam.RetentionDays == 30) _cbRetention.SelectedIndex = 0;
            else if (_targetCam.RetentionDays == 60) _cbRetention.SelectedIndex = 1;
            else if (_targetCam.RetentionDays == 90) _cbRetention.SelectedIndex = 2;
            else if (_targetCam.RetentionDays == 180) _cbRetention.SelectedIndex = 3;
            else if (_targetCam.RetentionDays == 365) _cbRetention.SelectedIndex = 4;
            else _cbRetention.SelectedIndex = 1;

            _txtStoragePath.Text = !string.IsNullOrEmpty(_targetCam.StoragePath) ? _targetCam.StoragePath : @"D:\Milestone\MediaDatabase";
            _numRecordFps.Value = _targetCam.RecordFps > 0 ? Math.Min(60, Math.Max(1, _targetCam.RecordFps)) : 25;
            _chkKeyframesOnly.Checked = _targetCam.RecordKeyframesOnly;
            _numPrebufferSec.Value = _targetCam.PrebufferSeconds > 0 ? Math.Min(60, Math.Max(1, _targetCam.PrebufferSeconds)) : 5;
            if (!string.IsNullOrEmpty(_targetCam.PrebufferType))
            {
                int pbIdx = _cbPrebufferType.FindStringExact(_targetCam.PrebufferType);
                if (pbIdx >= 0) _cbPrebufferType.SelectedIndex = pbIdx;
            }
            _chkMotion.Checked = _targetCam.MotionDetectionEnabled;
            _tbMotionSens.Value = _targetCam.MotionSensitivity > 0 ? Math.Min(100, Math.Max(1, _targetCam.MotionSensitivity)) : 80;
            _lblMotionSensVal.Text = _tbMotionSens.Value + "%";

            UpdateRtspPreview();
        }

        private void CbManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cbManufacturer.SelectedItem == null) return;
            string brand = _cbManufacturer.SelectedItem.ToString();
            var driver = HardwareDriverRegistry.GetDriverByBrand(brand);
            if (driver != null)
            {
                for (int i = 0; i < _cbDriver.Items.Count; i++)
                {
                    if (_cbDriver.Items[i].ToString().IndexOf(driver.DriverName, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        _cbDriver.Items[i].ToString().IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        _cbDriver.SelectedIndex = i;
                        break;
                    }
                }
                if (driver.DefaultPort > 0 && (_numHttpPort.Value == 80 || _numHttpPort.Value == 8000 || _numHttpPort.Value == 37777))
                {
                    _numHttpPort.Value = driver.DefaultPort;
                }
                if (!string.IsNullOrEmpty(driver.DefaultModel) && (_txtModel.Text.Contains("Generic") || string.IsNullOrWhiteSpace(_txtModel.Text)))
                {
                    _txtModel.Text = driver.DefaultModel;
                }
            }
        }

        private void UpdateRtspPreview()
        {
            string ip = _txtIp != null ? _txtIp.Text.Trim() : "127.0.0.1";
            string port = _numRtspPort != null ? _numRtspPort.Value.ToString() : "554";
            string user = _txtUserName != null ? _txtUserName.Text.Trim() : "admin";
            string pass = _txtPassword != null ? _txtPassword.Text : "";

            string creds = !string.IsNullOrEmpty(user) ? user + (string.IsNullOrEmpty(pass) ? "" : ":" + pass) + "@" : "";
            if (_txtRtspUrlPreview != null)
            {
                _txtRtspUrlPreview.Text = string.Format("rtsp://{0}{1}:{2}/Streaming/Channels/101", creds, ip, port);
            }
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            string ip = _txtIp.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Please enter a valid IP address or hostname to test.", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int httpPort = (int)_numHttpPort.Value;
            int rtspPort = (int)_numRtspPort.Value;
            string user = _txtUserName.Text.Trim();
            string pass = _txtPassword.Text;

            _btnTestConnection.Enabled = false;
            _lblTestResult.Text = string.Format("⏳ Probing {0}:{1} with credentials...", ip, httpPort);
            _lblTestResult.ForeColor = Color.FromArgb(0, 102, 204);

            ThreadPool.QueueUserWorkItem(state =>
            {
                bool success = false;
                string detail = "";

                try
                {
                    // 1. Try real HTTP/ISAPI snapshot
                    Image img = MilestoneToolkitLiveBridge.TryFetchSnapshot(ip, httpPort, user, pass, 2500);
                    if (img != null)
                    {
                        success = true;
                        detail = string.Format("Live snapshot verified ({0}x{1} px)", img.Width, img.Height);
                        img.Dispose();
                    }
                    else
                    {
                        // 2. Check TCP port
                        using (var client = new System.Net.Sockets.TcpClient())
                        {
                            var ar = client.BeginConnect(ip, httpPort, null, null);
                            if (ar.AsyncWaitHandle.WaitOne(2000))
                            {
                                client.EndConnect(ar);
                                success = true;
                                detail = string.Format("TCP Port {0} responded (HTTP server active)", httpPort);
                            }
                            else
                            {
                                detail = string.Format("Timeout connecting to {0}:{1}", ip, httpPort);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    detail = ex.Message;
                }

                if (this.IsDisposed || !this.IsHandleCreated) return;
                this.BeginInvoke(new Action(() =>
                {
                    _btnTestConnection.Enabled = true;
                    if (success)
                    {
                        _lblTestResult.Text = "🟢 Connection Successful! " + detail;
                        _lblTestResult.ForeColor = Color.FromArgb(16, 185, 129);
                    }
                    else
                    {
                        _lblTestResult.Text = "🔴 Connection Failed: " + detail;
                        _lblTestResult.ForeColor = Color.FromArgb(220, 38, 38);
                    }
                }));
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string newName = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Please enter a valid Camera Display Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            string ip = _txtIp.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Please enter a valid IP Address or RTSP Host.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtIp.Focus();
                return;
            }

            string serverName = _cbServer.SelectedItem != null ? _cbServer.SelectedItem.ToString() : "LYCHEE-REC-SRV-01";

            if (!_isEditMode)
            {
                if (_vmoService.GetCamera(newName) != null)
                {
                    MessageBox.Show("A camera with the name '" + newName + "' already exists. Please choose a different name.", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtName.Focus();
                    return;
                }
            }

            string oldName = _isEditMode ? _targetCam.Name : newName;

            // Apply all fields to _targetCam
            _targetCam.Name = newName;
            _targetCam.ServerName = serverName;
            _targetCam.Group = serverName;
            _targetCam.IsOnline = _chkOnline.Checked;

            _targetCam.Manufacturer = _cbManufacturer.Text.Trim();
            _targetCam.Model = _txtModel.Text.Trim();
            _targetCam.DriverName = _cbDriver.SelectedItem != null ? _cbDriver.SelectedItem.ToString() : "Universal ONVIF Conformance Driver";
            _targetCam.DriverVersion = _txtDriverVer.Text.Trim();
            _targetCam.FirmwareVersion = _txtFirmwareVer.Text.Trim();
            _targetCam.MacAddress = _txtMac.Text.Trim();

            _targetCam.IpAddress = ip;
            _targetCam.MulticastIp = ip;
            _targetCam.HttpPort = (int)_numHttpPort.Value;
            _targetCam.Port1 = (int)_numHttpPort.Value;
            _targetCam.RtspPort = (int)_numRtspPort.Value;
            _targetCam.Port2 = (int)_numRtspPort.Value;
            _targetCam.StreamingMethod = _cbStreamMethod.SelectedItem != null ? _cbStreamMethod.SelectedItem.ToString() : "RTP/RTSP/TCP";

            _targetCam.UserName = _txtUserName.Text.Trim();
            _targetCam.Password = _txtPassword.Text;

            _targetCam.Resolution = _cbResolution.SelectedItem != null ? _cbResolution.SelectedItem.ToString() : "1920x1080 (1080p FHD)";
            _targetCam.Codec = _cbCodec.SelectedItem != null ? _cbCodec.SelectedItem.ToString() : "H.265";
            _targetCam.Fps = (int)_numFps.Value;
            _targetCam.BitrateMode = _cbBitrateMode.SelectedItem != null ? _cbBitrateMode.SelectedItem.ToString() : "Variable bit rate";
            _targetCam.Quality = (int)_numQuality.Value;

            _targetCam.Stream2Res = _cbStream2Res.SelectedItem != null ? _cbStream2Res.SelectedItem.ToString() : "640x360";
            _targetCam.Stream2Fps = (int)_numStream2Fps.Value;

            int retDays = 60;
            if (_cbRetention.SelectedIndex == 0) retDays = 30;
            else if (_cbRetention.SelectedIndex == 1) retDays = 60;
            else if (_cbRetention.SelectedIndex == 2) retDays = 90;
            else if (_cbRetention.SelectedIndex == 3) retDays = 180;
            else if (_cbRetention.SelectedIndex == 4) retDays = 365;
            _targetCam.RetentionDays = retDays;

            _targetCam.StoragePath = _txtStoragePath.Text.Trim();
            _targetCam.RecordFps = (int)_numRecordFps.Value;
            _targetCam.RecordKeyframesOnly = _chkKeyframesOnly.Checked;
            _targetCam.PrebufferSeconds = (int)_numPrebufferSec.Value;
            _targetCam.PrebufferType = _cbPrebufferType.SelectedItem != null ? _cbPrebufferType.SelectedItem.ToString() : "In memory";
            _targetCam.MotionDetectionEnabled = _chkMotion.Checked;
            _targetCam.MotionSensitivity = _tbMotionSens.Value;

            if (_isEditMode)
            {
                if (!oldName.Equals(newName, StringComparison.OrdinalIgnoreCase))
                {
                    _vmoService.RenameCamera(oldName, newName);
                }
                _vmoService.SaveCameraSettings(_targetCam);
            }
            else
            {
                _vmoService.AddCamera(_targetCam);
            }

            _vmoService.SaveToDatabase();

            if (_onSaved != null)
            {
                try { _onSaved(); } catch { }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
