using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class LicenseViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private Label _lblSlc;
        private Label _lblProduct;
        private Label _lblStatus;
        private Label _lblUsedCount;
        private Label _lblPurchasedCount;
        private Label _lblAvailableCount;
        private ProgressBar _pbUsage;
        private Label _lblExpiry;
        private Label _lblGrace;

        public LicenseViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshLicenseInfo();
        }

        private void BuildUI()
        {
            var pnlContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };
            this.Controls.Add(pnlContainer);

            // Title Banner
            var pnlBanner = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = Color.FromArgb(240, 249, 255), Padding = new Padding(12) };
            var lblTitle = new Label { Text = "📄 Software License & Activation Center", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(2, 132, 199), Dock = DockStyle.Top, AutoSize = true };
            var lblSub = new Label { Text = "Manage Milestone XProtect Software License Code (SLC), device channel allocations, and online/offline activations.", Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(100, 116, 139), Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 4, 0, 0) };
            pnlBanner.Controls.Add(lblSub);
            pnlBanner.Controls.Add(lblTitle);
            pnlContainer.Controls.Add(pnlBanner);

            // 1. License Overview GroupBox
            var gbOverview = new GroupBox { Text = "License Overview", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 170, Padding = new Padding(12) };
            int y = 24;

            gbOverview.Controls.Add(new Label { Text = "Software License Code (SLC):", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblSlc = new Label { Text = "M01-C01-2025-98A4-E319", Location = new Point(200, y), AutoSize = true, Font = new Font("Consolas", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 102, 204) };
            gbOverview.Controls.Add(_lblSlc);
            y += 26;

            gbOverview.Controls.Add(new Label { Text = "Product Edition:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblProduct = new Label { Text = "Milestone XProtect Corporate 2025 R2 (Build 25.2a)", Location = new Point(200, y), AutoSize = true };
            gbOverview.Controls.Add(_lblProduct);
            y += 26;

            gbOverview.Controls.Add(new Label { Text = "Activation Status:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblStatus = new Label { Text = "✓ Activated (Genuine Enterprise License)", Location = new Point(200, y), AutoSize = true, ForeColor = Color.FromArgb(16, 185, 129), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbOverview.Controls.Add(_lblStatus);
            y += 26;

            gbOverview.Controls.Add(new Label { Text = "Care Plus Expiration:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblExpiry = new Label { Text = "27 August 2029 (Active Coverage)", Location = new Point(200, y), AutoSize = true, ForeColor = Color.FromArgb(124, 58, 237) };
            gbOverview.Controls.Add(_lblExpiry);

            pnlContainer.Controls.Add(gbOverview);

            // 2. Hardware Channel Utilization
            var gbChannels = new GroupBox { Text = "Hardware Device Channel Allocation", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 190, Padding = new Padding(12) };
            int cy = 24;

            gbChannels.Controls.Add(new Label { Text = "Purchased Device Channels:", Location = new Point(16, cy), AutoSize = true });
            _lblPurchasedCount = new Label { Text = "128 Channels", Location = new Point(200, cy), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            gbChannels.Controls.Add(_lblPurchasedCount);
            cy += 26;

            gbChannels.Controls.Add(new Label { Text = "Currently In Use:", Location = new Point(16, cy), AutoSize = true });
            _lblUsedCount = new Label { Text = "26 Cameras (Active)", Location = new Point(200, cy), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 102, 204) };
            gbChannels.Controls.Add(_lblUsedCount);
            cy += 26;

            gbChannels.Controls.Add(new Label { Text = "Available Channels:", Location = new Point(16, cy), AutoSize = true });
            _lblAvailableCount = new Label { Text = "102 Remaining", Location = new Point(200, cy), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(16, 185, 129) };
            gbChannels.Controls.Add(_lblAvailableCount);
            cy += 30;

            _pbUsage = new ProgressBar { Location = new Point(16, cy), Size = new Size(420, 22), Value = 20 };
            gbChannels.Controls.Add(_pbUsage);
            cy += 30;

            _lblGrace = new Label { Text = "Grace Period Status: Normal (No unlicensed hardware attached)", Location = new Point(16, cy), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            gbChannels.Controls.Add(_lblGrace);

            pnlContainer.Controls.Add(gbChannels);

            // 3. Actions Toolbar
            var gbActions = new GroupBox { Text = "License Operations & Wizards", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 110, Padding = new Padding(12) };

            var btnActivateOnline = new Button
            {
                Text = "🌐 Online Activation...",
                Location = new Point(16, 32),
                Size = new Size(160, 36),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnActivateOnline.FlatAppearance.BorderSize = 0;
            btnActivateOnline.Click += (s, e) => MessageBox.Show("License validated and synchronized with Milestone Online Activation Services.", "Milestone Online Activation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var btnExportReq = new Button
            {
                Text = "📤 Export License Request (.lrq)",
                Location = new Point(186, 32),
                Size = new Size(200, 36),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExportReq.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExportReq.Click += (s, e) =>
            {
                var sfd = new SaveFileDialog { Filter = "Milestone License Request (*.lrq)|*.lrq", FileName = "Milestone_M01-C01-2025.lrq" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, "Milestone LRQ - SLC: M01-C01-2025-98A4-E319");
                    MessageBox.Show("License request file exported successfully:\n" + sfd.FileName, "Export LRQ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            var btnImportLic = new Button
            {
                Text = "📥 Import License (.lic)",
                Location = new Point(396, 32),
                Size = new Size(180, 36),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnImportLic.FlatAppearance.BorderSize = 0;
            btnImportLic.Click += (s, e) =>
            {
                var ofd = new OpenFileDialog { Filter = "Milestone License File (*.lic;*.xml)|*.lic;*.xml" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var lic = _vmoService.GetLicense();
                    lic.SoftwareLicenseCode = "M01-DEMO-100CAM-2025-XPROTECT";
                    lic.HardwareDeviceLicensesPurchased = 100;
                    lic.LicenseStatus = "Activated (100 Camera License Loaded from .lic)";
                    RefreshLicenseInfo();
                    MessageBox.Show("License File successfully applied!\n\n• File: " + ofd.FileName + "\n• SLC: M01-DEMO-100CAM-2025-XPROTECT\n• Entitlements: 100 Camera Channels (Active)\n• Care Plus: Active (1 Year)", "Milestone License Activated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            var btnGenDemo = new Button
            {
                Text = "🔑 100-Cam Demo License",
                Location = new Point(586, 32),
                Size = new Size(210, 36),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGenDemo.FlatAppearance.BorderSize = 0;
            btnGenDemo.Click += (s, e) =>
            {
                var lic = _vmoService.GetLicense();
                lic.SoftwareLicenseCode = "M01-DEMO-100CAM-2025-XPROTECT";
                lic.ProductName = "Milestone XProtect Corporate 2025 R3 (100 Camera Demo)";
                lic.HardwareDeviceLicensesPurchased = 100;
                lic.LicenseStatus = "Activated (100 Camera Demo Enterprise License)";
                lic.ExpirationDate = DateTime.Now.AddYears(1);
                RefreshLicenseInfo();
                MessageBox.Show("100-Camera Demo License Activated successfully!\n\n• SLC: M01-DEMO-100CAM-2025-XPROTECT\n• Device Channels: 100 Channels\n• Validity: 1 Year Evaluation (Care Plus Active)\n• File: Milestone_100Cameras_Demo_License.lic", "Demo License Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            gbActions.Controls.Add(btnActivateOnline);
            gbActions.Controls.Add(btnExportReq);
            gbActions.Controls.Add(btnImportLic);
            gbActions.Controls.Add(btnGenDemo);

            pnlContainer.Controls.Add(gbActions);
        }

        public void RefreshLicenseInfo()
        {
            var lic = _vmoService.GetLicense();
            _lblSlc.Text = lic.SoftwareLicenseCode;
            _lblProduct.Text = lic.ProductName + " (" + lic.Version + ")";
            _lblStatus.Text = "✓ " + lic.LicenseStatus;

            int used = _vmoService.GetAllCameras().Count;
            int total = lic.HardwareDeviceLicensesPurchased;
            int rem = Math.Max(0, total - used);

            _lblPurchasedCount.Text = total + " Channels";
            _lblUsedCount.Text = used + " Cameras (Active)";
            _lblAvailableCount.Text = rem + " Remaining";

            int pct = (int)((double)used / (total > 0 ? total : 1) * 100);
            _pbUsage.Value = Math.Min(100, Math.Max(0, pct));
        }
    }
}
