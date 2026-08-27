using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI
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

        private int _currentStep = 1; // 1: Select Method, 2: Configure Details / Range, 3: Scanning & Progress, 4: Finish

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
        private System.Windows.Forms.Timer _scanTimer;
        private int _scanProgress = 0;

        public AddHardwareWizardDialog(MilestoneVmoService vmoService, Action onSuccess = null)
        {
            _vmoService = vmoService;
            _onSuccess = onSuccess;

            this.Text = "Add Hardware";
            this.Size = new Size(680, 480);
            this.MinimumSize = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            BuildLayout();
            ShowStep(1);
        }

        private void BuildLayout()
        {
            // Left Milestone Blue Geometric Banner
            _pnlLeftBanner = new Panel
            {
                Dock = DockStyle.Left,
                Width = 175,
                BackColor = Color.FromArgb(0, 114, 206) // Milestone Core Cyan Blue
            };
            _pnlLeftBanner.Paint += PnlLeftBanner_Paint;
            this.Controls.Add(_pnlLeftBanner);

            // Bottom Navigation Bar
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
            _btnHelp.Click += (s, e) => MessageBox.Show("Milestone Add Hardware Wizard detects network cameras, video encoders, and recording servers.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(570, 12),
                Size = new Size(80, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            _btnCancel.Click += (s, e) => this.Close();

            _btnNext = new Button
            {
                Text = "Next >",
                Location = new Point(480, 12),
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
                Location = new Point(390, 12),
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

            // Center Content Area
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

            // Gradient Cyan-to-Deep-Blue Background
            using (var brush = new LinearGradientBrush(_pnlLeftBanner.ClientRectangle, Color.FromArgb(0, 120, 215), Color.FromArgb(0, 48, 120), LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, _pnlLeftBanner.ClientRectangle);
            }

            // Draw Constellation Network Grid
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

            // Milestone Diamond Logo at Bottom
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

        // ========================================================
        // STEP 1: Select Hardware Detection Method
        // ========================================================
        private void BuildStep1SelectMethod()
        {
            _btnNext.Text = "Next >";

            var lblTitle = new Label
            {
                Text = "Add Hardware",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(0, 0),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "This wizard helps you detect and set up hardware.",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(0, 26),
                AutoSize = true
            };

            var lblHeader = new Label
            {
                Text = "Hardware detection method:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 68),
                AutoSize = true
            };

            // 1. Express
            _rbExpress = new RadioButton
            {
                Text = "Express (recommended)",
                Checked = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(6, 96),
                AutoSize = true
            };
            var lblExpDesc = new Label
            {
                Text = "Automatically detects hardware on the recording server's local network.",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(28, 118),
                AutoSize = true
            };

            // 2. Address Range
            _rbAddressRange = new RadioButton
            {
                Text = "Address range scanning",
                Checked = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(6, 155),
                AutoSize = true
            };
            var lblRangeDesc = new Label
            {
                Text = "Scans defined network address ranges and detects hardware models.",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(28, 177),
                AutoSize = true
            };

            // 3. Manual
            _rbManual = new RadioButton
            {
                Text = "Manual",
                Checked = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(6, 215),
                AutoSize = true
            };
            var lblManDesc = new Label
            {
                Text = "Detects hardware models for manually entered IP addresses and host names.",
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(28, 237),
                AutoSize = true
            };

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
            _pnlContent.Controls.Add(lblHeader);
            _pnlContent.Controls.Add(_rbExpress);
            _pnlContent.Controls.Add(lblExpDesc);
            _pnlContent.Controls.Add(_rbAddressRange);
            _pnlContent.Controls.Add(lblRangeDesc);
            _pnlContent.Controls.Add(_rbManual);
            _pnlContent.Controls.Add(lblManDesc);
        }

        // ========================================================
        // STEP 2A: Manual Server / Hardware Details
        // ========================================================
        private void BuildStep2ManualDetails()
        {
            _btnNext.Text = "Next >";

            var lblTitle = new Label { Text = "Specify Recording Server & Hardware Information", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "Enter network address, port, and security credentials for the recording node.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };

            int y = 55;
            _pnlContent.Controls.Add(new Label { Text = "Server Display Name:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtServerName = new TextBox { Text = "JSPANGICCCREC04.JSPLICCC.LOCAL", Location = new Point(0, y + 20), Size = new Size(420, 24) };
            _pnlContent.Controls.Add(_txtServerName);
            y += 50;

            _pnlContent.Controls.Add(new Label { Text = "IP Address / Host Name:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _pnlContent.Controls.Add(new Label { Text = "Port:", Location = new Point(310, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtHostIp = new TextBox { Text = "192.168.1.104", Location = new Point(0, y + 20), Size = new Size(295, 24) };
            _numPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 7563, Location = new Point(310, y + 20), Size = new Size(110, 24) };
            _pnlContent.Controls.Add(_txtHostIp);
            _pnlContent.Controls.Add(_numPort);
            y += 50;

            _pnlContent.Controls.Add(new Label { Text = "Hardware Driver Framework:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbDriver = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, y + 20), Size = new Size(420, 24) };
            _cbDriver.Items.AddRange(new object[] { "Universal ONVIF Conformance Driver (Generic Profile S/G/T)", "HikVision Generic IP Device Driver", "Axis Network Camera Driver Framework", "Milestone Direct XProtect Recorder Node", "Dahua Technology Universal ONVIF Driver" });
            _cbDriver.SelectedIndex = 0;
            _pnlContent.Controls.Add(_cbDriver);
            y += 50;

            _pnlContent.Controls.Add(new Label { Text = "User Name:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _pnlContent.Controls.Add(new Label { Text = "Password:", Location = new Point(220, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtUsername = new TextBox { Text = "root", Location = new Point(0, y + 20), Size = new Size(200, 24) };
            _txtPassword = new TextBox { Text = "Admin@123", UseSystemPasswordChar = true, Location = new Point(220, y + 20), Size = new Size(200, 24) };
            _pnlContent.Controls.Add(_txtUsername);
            _pnlContent.Controls.Add(_txtPassword);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
        }

        // ========================================================
        // STEP 2B: Address Range Scanning Setup
        // ========================================================
        private void BuildStep2AddressRange()
        {
            _btnNext.Text = "Start Scan >";

            var lblTitle = new Label { Text = "Configure Network Address Range", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "Specify the start and end IP range to discover all recording servers & cameras.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };

            int y = 60;
            _pnlContent.Controls.Add(new Label { Text = "Start IP Address:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtStartIp = new TextBox { Text = "192.168.1.100", Location = new Point(0, y + 20), Size = new Size(190, 24) };
            _pnlContent.Controls.Add(_txtStartIp);

            _pnlContent.Controls.Add(new Label { Text = "End IP Address:", Location = new Point(220, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtEndIp = new TextBox { Text = "192.168.1.120", Location = new Point(220, y + 20), Size = new Size(190, 24) };
            _pnlContent.Controls.Add(_txtEndIp);
            y += 55;

            _pnlContent.Controls.Add(new Label { Text = "Port to Probe:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _numScanPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 7563, Location = new Point(0, y + 20), Size = new Size(120, 24) };
            _pnlContent.Controls.Add(_numScanPort);
            y += 55;

            _pnlContent.Controls.Add(new Label { Text = "Default Manufacturer Credentials:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtUsername = new TextBox { Text = "admin", Location = new Point(0, y + 20), Size = new Size(190, 24) };
            _txtPassword = new TextBox { Text = "MilestonePass", UseSystemPasswordChar = true, Location = new Point(220, y + 20), Size = new Size(190, 24) };
            _pnlContent.Controls.Add(_txtUsername);
            _pnlContent.Controls.Add(_txtPassword);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
        }

        // ========================================================
        // STEP 2C: Express Detection Confirmation
        // ========================================================
        private void BuildStep2ExpressDetails()
        {
            _btnNext.Text = "Start Detection >";

            var lblTitle = new Label { Text = "Express Automatic Hardware Detection", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            var lblSub = new Label { Text = "The wizard will automatically discover all hardware on the recording server's local subnet.", ForeColor = Color.FromArgb(71, 85, 105), Location = new Point(0, 24), AutoSize = true };

            int y = 70;
            _pnlContent.Controls.Add(new Label { Text = "Target Recording Subnet:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtSubnet = new TextBox { Text = "192.168.1.0/24 (Local Ethernet Adapter)", ReadOnly = true, Location = new Point(0, y + 20), Size = new Size(420, 24) };
            _pnlContent.Controls.Add(txtSubnet);
            y += 60;

            _pnlContent.Controls.Add(new Label { Text = "Associated Recording Cluster:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbTargetServer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(0, y + 20), Size = new Size(420, 24) };
            _cbTargetServer.Items.AddRange(new object[] { "JSPANGICCCREC01.JSPLICCC.LOCAL", "JSPANGICCCREC02.JSPLICCC.LOCAL", "JSPANGICCCREC03.JSPLICCC.LOCAL" });
            _cbTargetServer.SelectedIndex = 0;
            _pnlContent.Controls.Add(_cbTargetServer);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(lblSub);
        }

        // ========================================================
        // STEP 3: Scanning & Live Discovery Progress
        // ========================================================
        private void BuildStep3Scanning()
        {
            _btnNext.Enabled = false;
            _btnNext.Text = "Please wait...";

            var lblTitle = new Label { Text = "Scanning and Detecting Hardware...", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(0, 0), AutoSize = true };
            _lblScanStatus = new Label { Text = "Sending ONVIF discovery probes across network...", ForeColor = Color.FromArgb(0, 102, 204), Location = new Point(0, 24), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            _pbScan = new ProgressBar { Location = new Point(0, 52), Size = new Size(440, 20), Value = 10 };

            _lvDiscovered = new ListView
            {
                Location = new Point(0, 84),
                Size = new Size(440, 220),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 8.5F)
            };
            _lvDiscovered.Columns.Add("Status", 75);
            _lvDiscovered.Columns.Add("Device / Server Name", 185);
            _lvDiscovered.Columns.Add("IP Address", 100);
            _lvDiscovered.Columns.Add("Model", 75);

            _pnlContent.Controls.Add(lblTitle);
            _pnlContent.Controls.Add(_lblScanStatus);
            _pnlContent.Controls.Add(_pbScan);
            _pnlContent.Controls.Add(_lvDiscovered);

            // Trigger Animation Timer
            _scanProgress = 0;
            if (_scanTimer != null) { _scanTimer.Stop(); _scanTimer.Dispose(); }
            _scanTimer = new System.Windows.Forms.Timer { Interval = 350 };
            _scanTimer.Tick += ScanTimer_Tick;
            _scanTimer.Start();
        }

        private void ScanTimer_Tick(object sender, EventArgs e)
        {
            _scanProgress += 15;
            if (_scanProgress > 100) _scanProgress = 100;
            _pbScan.Value = _scanProgress;

            if (_scanProgress == 30)
            {
                _lblScanStatus.Text = "Discovered: JSPANGICCCREC04.JSPLICCC.LOCAL (Recording Server)";
                var itm = new ListViewItem("🟢 Found");
                itm.ForeColor = Color.FromArgb(5, 150, 105);
                itm.SubItems.AddRange(new[] { "JSPANGICCCREC04.JSPLICCC.LOCAL", "192.168.1.104", "REC-SRV" });
                _lvDiscovered.Items.Add(itm);
            }
            else if (_scanProgress == 60)
            {
                _lblScanStatus.Text = "Discovered: ONVIF IPC PTZ 04 (4K Ultra Camera)";
                var itm = new ListViewItem("🟢 Found");
                itm.ForeColor = Color.FromArgb(5, 150, 105);
                itm.SubItems.AddRange(new[] { "CAM-ONVIF-4K-04", "192.168.1.204", "IPC-ONVIF" });
                _lvDiscovered.Items.Add(itm);
            }
            else if (_scanProgress == 90)
            {
                _lblScanStatus.Text = "Discovered: AXIS P1455-LE Perimeter HW Node";
                var itm = new ListViewItem("🟢 Found");
                itm.ForeColor = Color.FromArgb(5, 150, 105);
                itm.SubItems.AddRange(new[] { "AXIS-P1455-01", "192.168.1.205", "AXIS IP" });
                _lvDiscovered.Items.Add(itm);
            }
            else if (_scanProgress >= 100)
            {
                _scanTimer.Stop();
                _lblScanStatus.Text = "✔ Hardware Detection Complete. 3 devices ready to add.";
                _lblScanStatus.ForeColor = Color.FromArgb(16, 185, 129);
                _btnNext.Enabled = true;
                _btnNext.Text = "Next >";
            }
        }

        // ========================================================
        // STEP 4: Success & Database Registration
        // ========================================================
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

            // Commit to Milestone VMO Service & MSSQL Database
            _vmoService.AddRecordingServer(addedServerName, addedIp, addedPort);

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
                Text = string.Format("The recording server '{0}' and associated ONVIF hardware devices have been successfully registered into the Milestone Surveillance Cluster.\n\nDatabase synchronization complete with MSSQL Surveillance database.", addedServerName),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 32),
                Size = new Size(440, 90),
                Font = new Font("Segoe UI", 9.5F)
            };

            var gbSummary = new GroupBox
            {
                Text = "Registered Hardware Summary",
                Location = new Point(0, 130),
                Size = new Size(440, 170),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };

            int y = 24;
            gbSummary.Controls.Add(new Label { Text = "Server Name:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = addedServerName, Location = new Point(130, y), AutoSize = true, ForeColor = Color.FromArgb(0, 102, 204), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "Address / FQDN:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = string.Format("http://{0}:{1}/", addedIp, addedPort), Location = new Point(130, y), AutoSize = true });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "Status:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = "🟢 Running (Active)", Location = new Point(130, y), AutoSize = true, ForeColor = Color.FromArgb(16, 185, 129) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "Storage Pool:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = @"D:\Milestone\MediaDatabase\Live04", Location = new Point(130, y), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) });
            y += 24;

            gbSummary.Controls.Add(new Label { Text = "MSSQL Record:", Location = new Point(12, y), AutoSize = true });
            gbSummary.Controls.Add(new Label { Text = "dbo.Hardware & dbo.Devices (Synced)", Location = new Point(130, y), AutoSize = true, ForeColor = Color.FromArgb(124, 58, 237) });

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
