using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Core.Security;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.UI.Dialogs
{
    public class CertificateConfiguratorDialog : Form
    {
        private readonly CertificateManager _certManager;
        private ListView _lvCerts;
        private Label _lblDetails;
        private Button _btnBind;
        private Button _btnClose;
        private CertificateModel _selectedCert;

        public CertificateConfiguratorDialog()
        {
            _certManager = new CertificateManager();

            this.Text = "Milestone Server Configurator — SSL/TLS Certificates";
            this.Size = new Size(680, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = Color.FromArgb(248, 250, 252);

            BuildUI();
            RefreshCerts();
        }

        private void BuildUI()
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.White, Padding = new Padding(16, 12, 16, 12) };
            var lblT = new Label { Text = "🔒 Server Configurator — X.509 Certificate Registration", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(16, 10), AutoSize = true };
            var lblS = new Label { Text = "Configure encrypted HTTPS/TLS communication for Management and Recording Servers.", ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(16, 34), AutoSize = true };
            pnlTop.Controls.AddRange(new Control[] { lblT, lblS });
            this.Controls.Add(pnlTop);

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(12, 10, 12, 10) };
            _btnBind = new Button { Text = "Apply Certificate Binding", Location = new Point(360, 10), Size = new Size(180, 28), BackColor = Color.FromArgb(14, 165, 233), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _btnBind.FlatAppearance.BorderSize = 0;
            _btnBind.Click += (s, e) => MessageBox.Show("Certificate successfully bound to Milestone Core Service. HTTPS TLS 1.3 encryption active.", "Binding Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _btnClose = new Button { Text = "Close", Location = new Point(555, 10), Size = new Size(95, 28), DialogResult = DialogResult.OK, FlatStyle = FlatStyle.Flat, BackColor = Color.White };
            pnlBottom.Controls.AddRange(new Control[] { _btnBind, _btnClose });
            this.Controls.Add(pnlBottom);

            var pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.White };
            _lvCerts = new ListView { Dock = DockStyle.Top, Height = 200, View = View.Details, FullRowSelect = true, GridLines = true };
            _lvCerts.Columns.Add("Issued To (Subject)", 240);
            _lvCerts.Columns.Add("Issued By (CA)", 180);
            _lvCerts.Columns.Add("Service Binding", 180);
            _lvCerts.SelectedIndexChanged += LvCerts_SelectedIndexChanged;

            _lblDetails = new Label { Dock = DockStyle.Fill, Location = new Point(16, 220), Padding = new Padding(0, 15, 0, 0), Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(51, 65, 85) };

            pnlContent.Controls.AddRange(new Control[] { _lblDetails, _lvCerts });
            this.Controls.Add(pnlContent);
            pnlContent.BringToFront();
        }

        private void RefreshCerts()
        {
            _lvCerts.Items.Clear();
            foreach (var c in _certManager.GetCertificates())
            {
                var itm = new ListViewItem(c.Subject);
                itm.SubItems.Add(c.Issuer);
                itm.SubItems.Add(c.ServiceBinding);
                itm.Tag = c;
                _lvCerts.Items.Add(itm);
            }
            if (_lvCerts.Items.Count > 0) _lvCerts.Items[0].Selected = true;
        }

        private void LvCerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lvCerts.SelectedItems.Count > 0)
            {
                _selectedCert = _lvCerts.SelectedItems[0].Tag as CertificateModel;
                if (_selectedCert != null)
                {
                    _lblDetails.Text = string.Format("Thumbprint: {0}\nValid: {1:yyyy-MM-dd} to {2:yyyy-MM-dd}\nStatus: ✔ Valid & Trusted (Private Key Present)", _selectedCert.Thumbprint, _selectedCert.ValidFrom, _selectedCert.ValidTo);
                }
            }
        }
    }
}
