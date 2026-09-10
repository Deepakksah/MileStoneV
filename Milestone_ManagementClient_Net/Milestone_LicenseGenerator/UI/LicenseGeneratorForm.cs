using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MilestoneLicenseGenerator.Core;

namespace MilestoneLicenseGenerator.UI
{
    public class LicenseGeneratorForm : Form
    {
        private TextBox _txtSlc;
        private ComboBox _cbProduct;
        private NumericUpDown _numCameras;
        private TextBox _txtCustomer;
        private TextBox _txtOrg;
        private CheckBox _chkPerpetual;
        private DateTimePicker _dtpExpiry;
        private CheckBox _chkSmartMap;
        private CheckBox _chkFailover;
        private CheckBox _chkAccessControl;
        private CheckBox _chkIncident;
        private CheckBox _chkAnalytics;
        private CheckBox _chkInterconnect;
        private RichTextBox _rtbPreview;
        private Button _btnGenerate;
        private Button _btnRandomSlc;
        private Button _btnOpenFolder;
        private Label _lblStatus;

        public LicenseGeneratorForm()
        {
            InitializeComponent();
            GenerateRandomSlc();
        }

        private void InitializeComponent()
        {
            this.Text = "Milestone XProtect VMS - Software License Generator 2025 R3";
            this.Size = new Size(950, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Top Header Banner
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.FromArgb(15, 23, 42), Padding = new Padding(15, 10, 15, 10) };
            var lblHeaderTitle = new Label { Text = "🔑 Milestone XProtect Software License (.lic) Generator", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Location = new Point(15, 10) };
            var lblHeaderSub = new Label { Text = "Generates cryptographically signed .lic / .xml licenses for XProtect Corporate, Expert & Professional+", Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(148, 163, 184), AutoSize = true, Location = new Point(17, 36) };
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(lblHeaderSub);
            this.Controls.Add(pnlHeader);

            // Main Split: Left Form Inputs (450px), Right Live XML Preview (Fill)
            var split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 460, IsSplitterFixed = true };
            this.Controls.Add(split);
            split.BringToFront();

            // Left Input Controls Panel
            var pnlLeft = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(15) };
            split.Panel1.Controls.Add(pnlLeft);

            int y = 10;

            // 1. SLC Code
            pnlLeft.Controls.Add(new Label { Text = "Software License Code (SLC):", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            y += 22;
            _txtSlc = new TextBox { Text = "M01-C02-252-01-6C49F7", Location = new Point(10, y), Size = new Size(280, 24), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            _btnRandomSlc = new Button { Text = "🎲 New SLC", Location = new Point(295, y - 1), Size = new Size(110, 26), Cursor = Cursors.Hand };
            _btnRandomSlc.Click += (s, e) => GenerateRandomSlc();
            pnlLeft.Controls.Add(_txtSlc);
            pnlLeft.Controls.Add(_btnRandomSlc);
            y += 35;

            // 2. Product Edition
            pnlLeft.Controls.Add(new Label { Text = "Milestone Product Edition (SKU):", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            y += 22;
            _cbProduct = new ComboBox { Location = new Point(10, y), Size = new Size(395, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            _cbProduct.Items.Add("XProtect Corporate 2025 R3 (XPCO) - Enterprise Full Features");
            _cbProduct.Items.Add("XProtect Expert 2025 R3 (XPEX) - Advanced Central Management");
            _cbProduct.Items.Add("XProtect Professional+ 2025 R3 (XPPR) - Multi-Server IP");
            _cbProduct.Items.Add("XProtect Express+ 2025 R3 (XPEX) - Mid-sized Surveillance");
            _cbProduct.Items.Add("XProtect Essential+ 2025 R3 (XPES) - Free Base License");
            _cbProduct.SelectedIndex = 0;
            pnlLeft.Controls.Add(_cbProduct);
            y += 35;

            // 3. Camera Channels Count
            pnlLeft.Controls.Add(new Label { Text = "Licensed Camera Channels (Device Count):", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            y += 22;
            _numCameras = new NumericUpDown { Location = new Point(10, y), Size = new Size(160, 24), Minimum = 1, Maximum = 10000, Value = 100, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnlLeft.Controls.Add(_numCameras);
            pnlLeft.Controls.Add(new Label { Text = "Cameras (Default: 100 Channels)", Location = new Point(180, y + 3), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) });
            y += 35;

            // 4. Customer & Org
            pnlLeft.Controls.Add(new Label { Text = "Licensed To (Customer / Org):", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            y += 22;
            _txtCustomer = new TextBox { Text = "i-Comply Security Solutions", Location = new Point(10, y), Size = new Size(195, 24) };
            _txtOrg = new TextBox { Text = "JSPLICCC Surveillance Network", Location = new Point(210, y), Size = new Size(195, 24) };
            pnlLeft.Controls.Add(_txtCustomer);
            pnlLeft.Controls.Add(_txtOrg);
            y += 35;

            // 5. Expiry Date
            _chkPerpetual = new CheckBox { Text = "Perpetual License (Never Expires / Lifetime)", Location = new Point(10, y), AutoSize = true, Checked = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            y += 24;
            _dtpExpiry = new DateTimePicker { Location = new Point(10, y), Size = new Size(200, 24), Enabled = false, Value = DateTime.Today.AddYears(10) };
            _chkPerpetual.CheckedChanged += (s, e) => _dtpExpiry.Enabled = !_chkPerpetual.Checked;
            pnlLeft.Controls.Add(_chkPerpetual);
            pnlLeft.Controls.Add(_dtpExpiry);
            y += 35;

            // 6. Advanced Milestone Features
            var grpFeatures = new GroupBox { Text = "Included Enterprise Features", Location = new Point(10, y), Size = new Size(400, 115) };
            _chkSmartMap = new CheckBox { Text = "Smart Map (GIS/CAD)", Location = new Point(15, 22), AutoSize = true, Checked = true };
            _chkFailover = new CheckBox { Text = "Failover Clustering", Location = new Point(15, 48), AutoSize = true, Checked = true };
            _chkAccessControl = new CheckBox { Text = "Access Control & Doors", Location = new Point(15, 74), AutoSize = true, Checked = true };
            _chkIncident = new CheckBox { Text = "Incident Manager", Location = new Point(200, 22), AutoSize = true, Checked = true };
            _chkAnalytics = new CheckBox { Text = "AI Video Analytics / LPR", Location = new Point(200, 48), AutoSize = true, Checked = true };
            _chkInterconnect = new CheckBox { Text = "Milestone Interconnect", Location = new Point(200, 74), AutoSize = true, Checked = true };

            grpFeatures.Controls.Add(_chkSmartMap);
            grpFeatures.Controls.Add(_chkFailover);
            grpFeatures.Controls.Add(_chkAccessControl);
            grpFeatures.Controls.Add(_chkIncident);
            grpFeatures.Controls.Add(_chkAnalytics);
            grpFeatures.Controls.Add(_chkInterconnect);
            pnlLeft.Controls.Add(grpFeatures);
            y += 125;

            // Action Buttons
            _btnGenerate = new Button
            {
                Text = "⚡ Generate & Save Signed .lic File",
                Location = new Point(10, y),
                Size = new Size(260, 38),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnGenerate.FlatAppearance.BorderSize = 0;
            _btnGenerate.Click += BtnGenerate_Click;
            pnlLeft.Controls.Add(_btnGenerate);

            _btnOpenFolder = new Button
            {
                Text = "📂 Open Folder",
                Location = new Point(275, y),
                Size = new Size(130, 38),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(15, 23, 42),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnOpenFolder.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            _btnOpenFolder.Click += (s, e) =>
            {
                string outDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeneratedLicenses");
                if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
                System.Diagnostics.Process.Start("explorer.exe", outDir);
            };
            pnlLeft.Controls.Add(_btnOpenFolder);
            y += 45;

            _lblStatus = new Label { Text = "Ready to generate 100 Camera License file.", Location = new Point(10, y), AutoSize = true, ForeColor = Color.FromArgb(22, 101, 52), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            pnlLeft.Controls.Add(_lblStatus);

            // Right Panel: Live XML Preview & Signed Hash
            var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            split.Panel2.Controls.Add(pnlRight);

            pnlRight.Controls.Add(new Label { Text = "Decoded Cryptographic License XML Preview:", Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
            _rtbPreview = new RichTextBox { Dock = DockStyle.Fill, Font = new Font("Consolas", 9F), BackColor = Color.FromArgb(15, 23, 42), ForeColor = Color.FromArgb(226, 232, 240), ReadOnly = true };
            pnlRight.Controls.Add(_rtbPreview);
            _rtbPreview.BringToFront();

            // Refresh initial preview
            UpdatePreview();
        }

        private void GenerateRandomSlc()
        {
            var rnd = new Random();
            string code = string.Format("M01-C02-252-01-{0:X6}", rnd.Next(0x100000, 0xFFFFFF));
            _txtSlc.Text = code;
            UpdatePreview();
        }

        private LicenseModel BuildCurrentModel()
        {
            string sku = "XPCO";
            string prodName = "XProtect Corporate 2025 R3";
            if (_cbProduct.SelectedIndex == 1) { sku = "XPEX"; prodName = "XProtect Expert 2025 R3"; }
            else if (_cbProduct.SelectedIndex == 2) { sku = "XPPR"; prodName = "XProtect Professional+ 2025 R3"; }
            else if (_cbProduct.SelectedIndex == 3) { sku = "XPEX"; prodName = "XProtect Express+ 2025 R3"; }
            else if (_cbProduct.SelectedIndex == 4) { sku = "XPES"; prodName = "XProtect Essential+ 2025 R3"; }

            return new LicenseModel
            {
                Slc = _txtSlc.Text.Trim(),
                ProductName = prodName,
                ProductSku = sku,
                CameraCount = (int)_numCameras.Value,
                CustomerName = _txtCustomer.Text.Trim(),
                Organization = _txtOrg.Text.Trim(),
                IsPerpetual = _chkPerpetual.Checked,
                ExpiryDate = _dtpExpiry.Value,
                EnableSmartMap = _chkSmartMap.Checked,
                EnableFailover = _chkFailover.Checked,
                EnableAccessControl = _chkAccessControl.Checked,
                EnableIncidentManager = _chkIncident.Checked,
                EnableAnalytics = _chkAnalytics.Checked,
                EnableInterconnect = _chkInterconnect.Checked
            };
        }

        private void UpdatePreview()
        {
            try
            {
                var model = BuildCurrentModel();
                string xml = MilestoneLicenseEngine.GenerateLicenseXml(model);
                _rtbPreview.Text = xml;
            }
            catch { }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                var model = BuildCurrentModel();
                string outDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeneratedLicenses");
                if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);

                string licFileName = string.Format("{0}_({1}Cameras)_(Activated).lic", model.Slc, model.CameraCount);
                string licPath = Path.Combine(outDir, licFileName);
                string xmlPath = Path.Combine(outDir, string.Format("{0}_LicenseData.xml", model.Slc));

                string xml = MilestoneLicenseEngine.SaveLicenseFile(licPath, model);
                File.WriteAllText(xmlPath, xml);

                // Also save a copy directly in bin folder for instant installer access
                string binLic = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, licFileName);
                File.Copy(licPath, binLic, true);

                _rtbPreview.Text = xml;
                _lblStatus.Text = string.Format("✅ SUCCESS: Generated {0} ({1} Cameras)", licFileName, model.CameraCount);
                _lblStatus.ForeColor = Color.FromArgb(22, 101, 52);

                MessageBox.Show(string.Format("Milestone License File Successfully Generated!\n\nSLC: {0}\nProduct: {1}\nCapacity: {2} Cameras\nStatus: Activated Perpetual\n\nSaved to:\n{3}", model.Slc, model.ProductName, model.CameraCount, licPath), "License Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _lblStatus.Text = "❌ Error: " + ex.Message;
                _lblStatus.ForeColor = Color.FromArgb(220, 38, 38);
                MessageBox.Show("Error generating license: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
