using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Dialogs
{
    public class AddHardwareWizardDialog : Form
    {
        private readonly MilestoneVmoService _vmoService;
        private readonly Action _onSuccess;

        private Panel _pnlLeftBanner;
        private Panel _pnlContent;
        private Button _btnHelp;
        private Button _btnBack;
        private Button _btnNext;
        private Button _btnCancel;

        private int _currentStep = 1;

        // Step 1 Controls
        private RadioButton _rbExpress;
        private RadioButton _rbAddressRange;
        private RadioButton _rbManual;

        // Step 2 Manual/Server Controls
        private TextBox _txtServerName;
        private TextBox _txtHostIp;
        private NumericUpDown _numPort;
        private TextBox _txtUsername;
        private TextBox _txtPassword;
        private ComboBox _cbDriver;
        private ComboBox _cbTargetServer;

        // Step 2 Address Range Controls
        private TextBox _txtStartIp;
        private TextBox _txtEndIp;
        private NumericUpDown _numScanPort;

        // Step 3 Progress Controls
        private ProgressBar _pbScan;
        private Label _lblScanStatus;
        private ListView _lvDiscovered;
        private List<DiscoveredHardwareItem> _discoveredList = new List<DiscoveredHardwareItem>();

        public AddHardwareWizardDialog(MilestoneVmoService vmoService, Action onSuccess = null)
        {
            _vmoService = vmoService;
            _onSuccess = onSuccess;

            this.Text = "Add Hardware Wizard — Milestone XProtect VMS 2025";
            this.Size = new Size(700, 500);
            this.MinimumSize = new Size(680, 470);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            string icoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lychee.ico");
            if (!System.IO.File.Exists(icoPath)) icoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (System.IO.File.Exists(icoPath)) try { this.Icon = new Icon(icoPath); } catch { }

            BuildLayout();
            ShowStep(1);
        }

        private void BuildLayout()
        {
            _pnlLeftBanner = new Panel
            {
                Dock = DockStyle.Left,
                Width = 180,
                BackColor = Color.FromArgb(0, 114, 206)
            };
            _pnlLeftBanner.Paint += PnlLeftBanner_Paint;
            this.Controls.Add(_pnlLeftBanner);

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(12, 10, 12, 10)
            };

            _btnHelp = new Button
            {
                Text = "Help",
                Location = new Point(12, 12),
                Size = new Size(80, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            _btnHelp.Click += (s, e) => MessageBox.Show("Milestone Add Hardware Wizard detects network cameras, video encoders, thermal sensors, and recording servers.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(590, 12),
                Size = new Size(80, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            _btnCancel.Click += (s, e) => this.Close();

            _btnNext = new Button
            {
                Text = "Next >",
                Location = new Point(500, 12),
                Size = new Size(80, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 114, 206),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            _btnNext.FlatAppearance.BorderSize = 0;
            _btnNext.Click += BtnNext_Click;

            _btnBack = new Button
            {
                Text = "< Back",
                Location = new Point(410, 12),
                Size = new Size(80, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Enabled = false
            };
            _btnBack.Click += (s, e) => { if (_currentStep > 1) ShowStep(_currentStep - 1); };

            pnlBottom.Controls.Add(_btnHelp);
            pnlBottom.Controls.Add(_btnBack);
            pnlBottom.Controls.Add(_btnNext);
            pnlBottom.Controls.Add(_btnCancel);
            this.Controls.Add(pnlBottom);

            _pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(24, 20, 24, 20)
            };
            this.Controls.Add(_pnlContent);
            _pnlContent.BringToFront();
        }

        private void PnlLeftBanner_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new LinearGradientBrush(_pnlLeftBanner.ClientRectangle, Color.FromArgb(0, 120, 215), Color.FromArgb(0, 48, 120), LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, _pnlLeftBanner.ClientRectangle);
            }

            using (var pen = new Pen(Color.FromArgb(90, 255, 255, 255), 1))
            {
                Point[] pts = {
                    new Point(30, 40), new Point(90, 25), new Point(140, 60),
                    new Point(110, 120), new Point(50, 100), new Point(75, 160),
                    new Point(135, 175), new Point(45, 220), new Point(120, 250)
                };

                g.DrawLine(pen, pts[0], pts[1]);
                g.DrawLine(pen, pts[1], pts[2]);
                g.DrawLine(pen, pts[0], pts[4]);
                g.DrawLine(pen, pts[4], pts[3]);
                g.DrawLine(pen, pts[2], pts[3]);
                g.DrawLine(pen, pts[3], pts[5]);
                g.DrawLine(pen, pts[4], pts[5]);
                g.DrawLine(pen, pts[5], pts[6]);
                g.DrawLine(pen, pts[5], pts[7]);
                g.DrawLine(pen, pts[6], pts[8]);
                g.DrawLine(pen, pts[7], pts[8]);

                using (var dotBrush = new SolidBrush(Color.White))
                {
                    foreach (var p in pts)
                    {
                        g.FillEllipse(dotBrush, p.X - 3, p.Y - 3, 6, 6);
                    }
                }
            }

            int midX = _pnlLeftBanner.Width / 2;
            int botY = _pnlLeftBanner.Height - 75;

            Point[] diamond = {
                new Point(midX, botY - 18),
                new Point(midX + 18, botY),
                new Point(midX, botY + 18),
                new Point(midX - 18, botY)
            };
            using (var diamBrush = new SolidBrush(Color.White))
            {
                g.FillPolygon(diamBrush, diamond);
            }

            using (var font = new Font("Segoe UI", 10.5F, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString("milestone", font, textBrush, new PointF(midX, botY + 24), sf);
            }
        }

        private void ShowStep(int step)
        {
            _currentStep = step;
            _pnlContent.Controls.Clear();
            _btnBack.Enabled = (step > 1);

            if (step == 1)
            {
                BuildStep1SelectMethod();
            }
            else if (step == 2)
            {
                if (_rbManual.Checked)
                {
                    BuildStep2ManualDetails();
                }
                else if (_rbAddressRange.Checked)
                {
                    BuildStep2AddressRange();
                }
                else
                {
                    BuildStep2ExpressDetails();
                }
            }
            else if (step == 3)
            {
                BuildStep3Scanning();
            }
            else if (step == 4)
            {
                BuildStep4Success();
            }
        }

        private void BuildStep1SelectMethod()
        {
            _btnNext.Text = "Next >";

            var lblTitle = new Label { Text = "Add Hardware Wizard", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "Select the hardware detection method to locate and register cameras in the cluster.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };
            var lblHeader = new Label { Text = "Hardware detection options:", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(0, 65), AutoSize = true };

            _rbExpress = new RadioButton { Text = "Express (recommended)", Checked = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Location = new Point(6, 95), AutoSize = true };
            var lblExpDesc = new Label { Text = "Scans local recording network and automatically detects multi-brand cameras.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(28, 118), AutoSize = true };

            _rbAddressRange = new RadioButton { Text = "Address range scanning", Checked = false, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Location = new Point(6, 155), AutoSize = true };
            var lblRangeDesc = new Label { Text = "Scans defined IP address range and probes for ONVIF & manufacturer ports.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(28, 177), AutoSize = true };

            _rbManual = new RadioButton { Text = "Manual", Checked = false, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Location = new Point(6, 215), AutoSize = true };
            var lblManDesc = new Label { Text = "Specify exact IP address, port, manufacturer credentials, and driver framework.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(28, 237), AutoSize = true };

            _pnlContent.Controls.AddRange(new Control[] { lblTitle, lblSub, lblHeader, _rbExpress, lblExpDesc, _rbAddressRange, lblRangeDesc, _rbManual, lblManDesc });
        }

        private void BuildStep2ManualDetails()
        {
            _btnNext.Text = "Next >";

            var lblTitle = new Label { Text = "Specify Recording Server & Hardware Information", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "Enter network address, port, and security credentials for the recording node.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };

            int y = 55;
            _pnlContent.Controls.Add(new Label { Text = "Server Display Name:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtServerName = new TextBox { Text = "JSPANGICCCREC04.JSPLICCC.LOCAL", Location = new Point(0, y + 20), Size = new Size(450, 24) };
            _pnlContent.Controls.Add(_txtServerName);
            y += 50;

            _pnlContent.Controls.Add(new Label { Text = "IP Address / Host Name:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _pnlContent.Controls.Add(new Label { Text = "Port:", Location = new Point(330, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtHostIp = new TextBox { Text = "192.168.1.104", Location = new Point(0, y + 20), Size = new Size(315, 24) };
            _numPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 7563, Location = new Point(330, y + 20), Size = new Size(120, 24) };
            _pnlContent.Controls.Add(_txtHostIp);
            _pnlContent.Controls.Add(_numPort);
            y += 50;

            _pnlContent.Controls.Add(new Label { Text = "Hardware Driver Framework (24+ Multi-Brand Supported):", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbDriver = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, y + 20), Size = new Size(450, 24) };
            _cbDriver.Items.AddRange(HardwareDriverRegistry.GetAllDriverNames());
            _cbDriver.SelectedIndex = 0;
            _pnlContent.Controls.Add(_cbDriver);
            y += 50;

            _pnlContent.Controls.Add(new Label { Text = "User Name:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _pnlContent.Controls.Add(new Label { Text = "Password:", Location = new Point(230, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtUsername = new TextBox { Text = "root", Location = new Point(0, y + 20), Size = new Size(210, 24) };
            _txtPassword = new TextBox { Text = "Admin@123", UseSystemPasswordChar = true, Location = new Point(230, y + 20), Size = new Size(220, 24) };
            _pnlContent.Controls.Add(_txtUsername);
            _pnlContent.Controls.Add(_txtPassword);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
        }

        private void BuildStep2AddressRange()
        {
            _btnNext.Text = "Start Scan >";

            var lblTitle = new Label { Text = "Configure Network Address Range", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "Specify the start and end IP range to discover all recording servers & cameras.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };

            int y = 60;
            _pnlContent.Controls.Add(new Label { Text = "Start IP Address:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtStartIp = new TextBox { Text = "192.168.1.100", Location = new Point(0, y + 20), Size = new Size(210, 24) };
            _pnlContent.Controls.Add(_txtStartIp);

            _pnlContent.Controls.Add(new Label { Text = "End IP Address:", Location = new Point(240, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtEndIp = new TextBox { Text = "192.168.1.120", Location = new Point(240, y + 20), Size = new Size(210, 24) };
            _pnlContent.Controls.Add(_txtEndIp);
            y += 55;

            _pnlContent.Controls.Add(new Label { Text = "Port to Probe:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _numScanPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 7563, Location = new Point(0, y + 20), Size = new Size(130, 24) };
            _pnlContent.Controls.Add(_numScanPort);
            y += 55;

            _pnlContent.Controls.Add(new Label { Text = "Default Manufacturer Credentials:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtUsername = new TextBox { Text = "admin", Location = new Point(0, y + 20), Size = new Size(210, 24) };
            _txtPassword = new TextBox { Text = "admin123", UseSystemPasswordChar = true, Location = new Point(240, y + 20), Size = new Size(160, 24) };
            var btnTogglePassRange = new Button { Text = "👁", Location = new Point(405, y + 19), Size = new Size(35, 26), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTogglePassRange.Click += (s, e) => { _txtPassword.UseSystemPasswordChar = !_txtPassword.UseSystemPasswordChar; };
            _pnlContent.Controls.Add(_txtUsername);
            _pnlContent.Controls.Add(_txtPassword);
            _pnlContent.Controls.Add(btnTogglePassRange);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
        }

        private void BuildStep2ExpressDetails()
        {
            _btnNext.Text = "Start Detection >";

            var subnet = NetworkDiscoveryEngine.GetActiveSubnetInfo();

            var lblTitle = new Label { Text = "Express Automatic Hardware Detection", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "The wizard will actively probe for ONVIF and multi-vendor IP cameras on your local network.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };

            int y = 60;
            _pnlContent.Controls.Add(new Label { Text = "Active Network Subnet (Auto-Detected):", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtSubnet = new TextBox { Text = string.Format("{0} • {1} ({2})", subnet.NetworkCidr, subnet.AdapterName, subnet.LocalIp), ReadOnly = true, Location = new Point(0, y + 20), Size = new Size(460, 24) };
            _pnlContent.Controls.Add(txtSubnet);
            y += 55;

            _pnlContent.Controls.Add(new Label { Text = "Assign Discovered Cameras to Recording Server:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbTargetServer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, y + 20), Size = new Size(460, 24) };
            var servers = _vmoService.GetServers();
            if (servers.Count > 0)
            {
                foreach (var s in servers) _cbTargetServer.Items.Add(s.Name);
            }
            else
            {
                _cbTargetServer.Items.Add("JSPANGICCCREC01.JSPLICCC.LOCAL");
            }
            _cbTargetServer.SelectedIndex = 0;
            _pnlContent.Controls.Add(_cbTargetServer);
            y += 55;

            _pnlContent.Controls.Add(new Label { Text = "Hardware Credentials (Used for Live Video Stream Authentication):", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            y += 20;

            _pnlContent.Controls.Add(new Label { Text = "Default User Name:", Location = new Point(0, y), AutoSize = true });
            _pnlContent.Controls.Add(new Label { Text = "Default Password:", Location = new Point(230, y), AutoSize = true });
            y += 20;

            _txtUsername = new TextBox { Text = "admin", Location = new Point(0, y), Size = new Size(210, 24) };
            _txtPassword = new TextBox { Text = "admin123", UseSystemPasswordChar = true, Location = new Point(230, y), Size = new Size(160, 24) };
            var btnTogglePassExp = new Button { Text = "👁", Location = new Point(395, y - 1), Size = new Size(35, 26), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnTogglePassExp.Click += (s, e) => { _txtPassword.UseSystemPasswordChar = !_txtPassword.UseSystemPasswordChar; };

            _pnlContent.Controls.Add(_txtUsername);
            _pnlContent.Controls.Add(_txtPassword);
            _pnlContent.Controls.Add(btnTogglePassExp);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
        }

        private void BuildStep3Scanning()
        {
            _btnNext.Enabled = true;
            _btnNext.Text = "Next >";

            var subnet = NetworkDiscoveryEngine.GetActiveSubnetInfo();

            var lblTitle = new Label { Text = "Scanning Network for Cameras & Hardware...", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            _lblScanStatus = new Label { Text = "Broadcasting ONVIF WS-Discovery probes on " + subnet.NetworkCidr + "...", ForeColor = Color.FromArgb(0, 102, 204), Location = new Point(0, 24), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            _pbScan = new ProgressBar { Location = new Point(0, 52), Size = new Size(460, 20), Minimum = 0, Maximum = 100, Value = 5 };

            _lvDiscovered = new ListView
            {
                Location = new Point(0, 84),
                Size = new Size(460, 230),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 8.5F)
            };
            _lvDiscovered.Columns.Add("Status", 75);
            _lvDiscovered.Columns.Add("Device / Node Name", 130);
            _lvDiscovered.Columns.Add("IP Address", 95);
            _lvDiscovered.Columns.Add("Brand / Model", 110);
            _lvDiscovered.Columns.Add("Credentials", 90);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(_lblScanStatus);
            _pnlContent.Controls.Add(_pbScan);
            _pnlContent.Controls.Add(_lvDiscovered);

            _discoveredList.Clear();

            string basePrefix = subnet.BaseIpPrefix;
            int startHost = 1;
            int endHost = 254;

            if (_rbAddressRange != null && _rbAddressRange.Checked && _txtStartIp != null && _txtEndIp != null)
            {
                try
                {
                    string startIp = _txtStartIp.Text.Trim();
                    string endIp = _txtEndIp.Text.Trim();
                    int lastDot = startIp.LastIndexOf('.');
                    if (lastDot > 0)
                    {
                        basePrefix = startIp.Substring(0, lastDot + 1);
                        startHost = int.Parse(startIp.Substring(lastDot + 1));
                        int lastDotEnd = endIp.LastIndexOf('.');
                        endHost = int.Parse(endIp.Substring(lastDotEnd + 1));
                    }
                }
                catch { }
            }

            int[] probePorts = new int[] { 80, 554, 8000, 37777, 8080, 8899, 7563, 443 };
            string scanUser = (_txtUsername != null && !string.IsNullOrWhiteSpace(_txtUsername.Text)) ? _txtUsername.Text.Trim() : "admin";
            string scanPass = (_txtPassword != null && !string.IsNullOrWhiteSpace(_txtPassword.Text)) ? _txtPassword.Text.Trim() : "admin123";

            NetworkDiscoveryEngine.ScanNetworkAsync(
                basePrefix,
                startHost,
                endHost,
                probePorts,
                (current, total, currentIp) =>
                {
                    if (this.IsDisposed || !this.IsHandleCreated) return;
                    try
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            int pct = (int)(((float)current / total) * 100);
                            if (pct > 100) pct = 100;
                            _pbScan.Value = pct;
                            _lblScanStatus.Text = string.Format("Probing {0} ({1}/{2})...", currentIp, current, total);
                        }));
                    }
                    catch { }
                },
                (device) =>
                {
                    if (this.IsDisposed || !this.IsHandleCreated) return;
                    try
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            _discoveredList.Add(device);
                            var itm = new ListViewItem("🟢 " + device.Status);
                            itm.ForeColor = Color.FromArgb(5, 150, 105);
                            string credDisplay = (!string.IsNullOrEmpty(device.UserName) ? device.UserName : scanUser) + ":***";
                            itm.SubItems.AddRange(new[] { device.DeviceName, device.IpAddress, device.Manufacturer + " " + device.Model, credDisplay });
                            _lvDiscovered.Items.Add(itm);
                        }));
                    }
                    catch { }
                },
                (allFound) =>
                {
                    if (this.IsDisposed || !this.IsHandleCreated) return;
                    try
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            _pbScan.Value = 100;
                            if (allFound.Count > 0)
                            {
                                _lblScanStatus.Text = string.Format("✔ Scan Complete: {0} network cameras discovered on {1}.", allFound.Count, subnet.NetworkCidr);
                                _lblScanStatus.ForeColor = Color.FromArgb(16, 185, 129);
                            }
                            else
                            {
                                _lblScanStatus.Text = string.Format("ℹ Scan Complete: 0 cameras found on {0}.", subnet.NetworkCidr);
                                _lblScanStatus.ForeColor = Color.FromArgb(71, 85, 105);
                            }
                            _btnNext.Enabled = true;
                            _btnNext.Text = "Next >";
                        }));
                    }
                    catch { }
                },
                scanUser,
                scanPass);
        }

        private void BuildStep4Success()
        {
            _btnNext.Text = "Finish";
            _btnBack.Enabled = false;

            string addedServerName = (_txtServerName != null && !string.IsNullOrWhiteSpace(_txtServerName.Text))
                ? _txtServerName.Text.Trim()
                : "JSPANGICCCREC04.JSPLICCC.LOCAL";

            string addedIp = (_txtHostIp != null && !string.IsNullOrWhiteSpace(_txtHostIp.Text))
                ? _txtHostIp.Text.Trim()
                : "192.168.1.104";

            int addedPort = (_numPort != null) ? (int)_numPort.Value : 7563;

            string targetServer = (_cbTargetServer != null && _cbTargetServer.SelectedItem != null)
                ? _cbTargetServer.SelectedItem.ToString()
                : "JSPANGICCCREC01.JSPLICCC.LOCAL";

            string selectedDriver = (_cbDriver != null && _cbDriver.SelectedItem != null)
                ? _cbDriver.SelectedItem.ToString()
                : "Universal ONVIF Conformance Driver";

            string defaultUser = (_txtUsername != null && !string.IsNullOrWhiteSpace(_txtUsername.Text)) ? _txtUsername.Text.Trim() : "admin";
            string defaultPass = (_txtPassword != null && !string.IsNullOrWhiteSpace(_txtPassword.Text)) ? _txtPassword.Text.Trim() : "admin123";

            // Commit Discovered Multi-Brand Cameras
            if ((_rbExpress != null && _rbExpress.Checked) || (_rbAddressRange != null && _rbAddressRange.Checked))
            {
                foreach (var d in _discoveredList)
                {
                    _vmoService.AddHardwareWithDetails(d.DeviceName, targetServer, d.Manufacturer, d.Model, d.DriverName, d.DriverVersion, d.IpAddress, d.Port, d.Resolution);
                    var cam = _vmoService.GetCamera(d.DeviceName);
                    if (cam != null)
                    {
                        cam.ServerName = targetServer;
                        cam.IpAddress = d.IpAddress;
                        cam.MulticastIp = d.IpAddress;
                        cam.HttpPort = d.Port;
                        cam.MacAddress = d.MacAddress;
                        cam.Manufacturer = d.Manufacturer;
                        cam.Model = d.Model;
                        cam.DriverName = d.DriverName;
                        cam.DriverVersion = d.DriverVersion;
                        cam.IsOnline = true;

                        // AUTOMATIC CREDENTIALS FOR INSTANT LIVE VIDEO PLAY
                        cam.UserName = !string.IsNullOrEmpty(d.UserName) ? d.UserName : defaultUser;
                        cam.Password = !string.IsNullOrEmpty(d.Password) ? d.Password : defaultPass;
                    }
                }
                _vmoService.SaveToDatabase();
            }
            else
            {
                var driverMeta = HardwareDriverRegistry.GetDriverByName(selectedDriver);
                string manCamName = addedServerName + " CAM 01";
                _vmoService.AddHardwareWithDetails(manCamName, targetServer, driverMeta.Brand, driverMeta.DefaultModel, driverMeta.DriverName, driverMeta.DriverVersion, addedIp, addedPort, "1920x1080 (FHD)");
                var manCam = _vmoService.GetCamera(manCamName);
                if (manCam != null)
                {
                    manCam.UserName = defaultUser;
                    manCam.Password = defaultPass;
                }
                _vmoService.AddRecordingServer(addedServerName, addedIp, addedPort);
                _vmoService.SaveToDatabase();
            }

            var lblTitle = new Label
            {
                Text = "Hardware Setup Complete",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 185, 129),
                Location = new Point(0, 0),
                AutoSize = true
            };

            var lblDesc = new Label
            {
                Text = string.Format("Total {0} live camera devices discovered on your network have been successfully registered under recording server '{1}'.\n\nDatabase synchronization complete with MSSQL Surveillance database.", _discoveredList.Count, targetServer),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 32),
                Size = new Size(460, 90),
                Font = new Font("Segoe UI", 9.5F)
            };

            var gbSummary = new GroupBox
            {
                Text = "Registered Multi-Brand Hardware Summary",
                Location = new Point(0, 130),
                Size = new Size(460, 175),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };

            int y = 24;
            gbSummary.Controls.Add(new Label { Text = "Server Name:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = addedServerName, Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(0, 102, 204), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "Selected Driver:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = selectedDriver.Length > 36 ? selectedDriver.Substring(0, 34) + "..." : selectedDriver, Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(79, 70, 229) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "Status:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = "🟢 Running (Active & Synced)", Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(16, 185, 129) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "Storage Pool:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = @"D:\Milestone\MediaDatabase\Live04", Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "MSSQL Record:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = "dbo.Hardware & dbo.Devices (Synced)", Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(124, 58, 237) });

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblDesc);
            _pnlContent.Controls.Add(gbSummary);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_currentStep == 1)
            {
                ShowStep(2);
            }
            else if (_currentStep == 2)
            {
                ShowStep(3);
            }
            else if (_currentStep == 3)
            {
                ShowStep(4);
            }
            else if (_currentStep == 4)
            {
                if (_onSuccess != null) _onSuccess();
                this.Close();
            }
        }
    }
}
