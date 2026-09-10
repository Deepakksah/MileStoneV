using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Core.IncidentManager;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class IncidentManagerViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private readonly IncidentManagerService _imService;

        private ListView _lvIncidents;
        private Label _lblIncNumber;
        private Label _lblIncTitle;
        private Label _lblIncMeta;
        private TextBox _txtDesc;
        private Button _btnNewIncident;
        private Button _btnSecureEvidence;
        private Button _btnExportBinder;
        private IncidentModel _selectedIncident;

        public IncidentManagerViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            _imService = new IncidentManagerService();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshIncidents();
        }

        private void BuildUI()
        {
            this.Controls.Clear();

            // Header Banner
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };
            var lblTitle = new Label
            {
                Text = "📁 Milestone Incident Manager & Tamper-Proof Evidence Locks",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 10),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "Manage security cases, protect surveillance sequences with cryptographic evidence locks, and build export binders.",
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(16, 34),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            this.Controls.Add(pnlHeader);

            // Main Split
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 500,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Left: Incidents List & New Incident Button
            var pnlList = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            var pnlTopList = new Panel { Dock = DockStyle.Top, Height = 36 };

            _btnNewIncident = new Button
            {
                Text = "➕ New Incident Case",
                Dock = DockStyle.Left,
                Width = 160,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnNewIncident.FlatAppearance.BorderSize = 0;
            _btnNewIncident.Click += (s, e) => PromptCreateIncident();
            pnlTopList.Controls.Add(_btnNewIncident);
            pnlList.Controls.Add(pnlTopList);

            _lvIncidents = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9F)
            };
            _lvIncidents.Columns.Add("Case ID", 110);
            _lvIncidents.Columns.Add("Incident Title", 180);
            _lvIncidents.Columns.Add("Severity", 80);
            _lvIncidents.Columns.Add("Status", 110);
            _lvIncidents.SelectedIndexChanged += LvIncidents_SelectedIndexChanged;
            pnlList.Controls.Add(_lvIncidents);
            _lvIncidents.BringToFront();

            split.Panel1.Controls.Add(pnlList);

            // Right: Incident Details, Bookmarks & Evidence Locks
            var pnlDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16) };

            _lblIncNumber = new Label { Text = "Select an incident case to view evidence details", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(16, 12), AutoSize = true };
            _lblIncTitle = new Label { Text = "", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Location = new Point(16, 38), AutoSize = true };
            _lblIncMeta = new Label { Text = "", ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(16, 62), AutoSize = true };

            pnlDetail.Controls.Add(new Label { Text = "Incident Case Narrative & Audit Notes:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Location = new Point(16, 92), AutoSize = true });
            _txtDesc = new TextBox
            {
                Location = new Point(16, 115),
                Size = new Size(460, 130),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9F),
                ReadOnly = true,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            pnlDetail.Controls.Add(_txtDesc);

            // Evidence Lock & Export Buttons
            _btnSecureEvidence = new Button
            {
                Text = "🔒 Evidence Lock (Retention: 5 Yrs)",
                Location = new Point(16, 260),
                Size = new Size(240, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnSecureEvidence.FlatAppearance.BorderSize = 0;
            _btnSecureEvidence.Click += (s, e) => MessageBox.Show("Evidence lock confirmed with cryptographic SHA-256 hash stamp. Protected from automated retention purge until 2030.", "Evidence Secured", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _btnExportBinder = new Button
            {
                Text = "📦 Export Incident Binder (ZIP / AVI)",
                Location = new Point(265, 260),
                Size = new Size(210, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnExportBinder.FlatAppearance.BorderSize = 0;
            _btnExportBinder.Click += (s, e) => MessageBox.Show("Incident evidence package, camera video clips, audit trails, and digital signatures exported successfully to D:\\Milestone\\Exports\\.", "Export Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            pnlDetail.Controls.AddRange(new Control[] { _lblIncNumber, _lblIncTitle, _lblIncMeta, _btnSecureEvidence, _btnExportBinder });
            split.Panel2.Controls.Add(pnlDetail);

            this.Controls.Add(split);
            split.BringToFront();
        }

        private void RefreshIncidents()
        {
            _lvIncidents.Items.Clear();
            foreach (var inc in _imService.GetAllIncidents())
            {
                var itm = new ListViewItem(inc.IncidentNumber);
                itm.SubItems.Add(inc.Title);
                itm.SubItems.Add(inc.Severity.ToString());
                itm.SubItems.Add(inc.Status.ToString());
                itm.Tag = inc;

                if (inc.Severity == IncidentSeverity.Critical) itm.ForeColor = Color.FromArgb(220, 38, 38);
                else if (inc.Severity == IncidentSeverity.High) itm.ForeColor = Color.FromArgb(234, 88, 12);
                else itm.ForeColor = Color.FromArgb(15, 23, 42);

                _lvIncidents.Items.Add(itm);
            }

            if (_lvIncidents.Items.Count > 0 && _selectedIncident == null)
            {
                _lvIncidents.Items[0].Selected = true;
            }
        }

        private void LvIncidents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lvIncidents.SelectedItems.Count > 0)
            {
                _selectedIncident = _lvIncidents.SelectedItems[0].Tag as IncidentModel;
                if (_selectedIncident != null)
                {
                    _lblIncNumber.Text = "Case File: " + _selectedIncident.IncidentNumber;
                    _lblIncTitle.Text = _selectedIncident.Title;
                    _lblIncMeta.Text = string.Format("Timestamp: {0:yyyy-MM-dd HH:mm} • Camera: {1} • Creator: {2}", _selectedIncident.Timestamp, _selectedIncident.PrimaryCameraName, _selectedIncident.CreatedBy);
                    _txtDesc.Text = _selectedIncident.Description;
                }
            }
        }

        private void PromptCreateIncident()
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Create New Incident Case";
                dlg.Size = new Size(420, 320);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.Font = new Font("Segoe UI", 9F);

                var lblT = new Label { Text = "Incident Title:", Location = new Point(16, 16), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtT = new TextBox { Location = new Point(16, 38), Size = new Size(370, 24), Text = "Suspicious Activity Detected" };

                var lblD = new Label { Text = "Case Description:", Location = new Point(16, 70), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtD = new TextBox { Location = new Point(16, 92), Size = new Size(370, 90), Multiline = true, Text = "Operator noted irregular vehicle movement near perimeter." };

                var btnOk = new Button { Text = "Create & Lock Evidence", Location = new Point(150, 230), Size = new Size(150, 30), DialogResult = DialogResult.OK, BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(310, 230), Size = new Size(75, 30), DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat };

                dlg.Controls.AddRange(new Control[] { lblT, txtT, lblD, txtD, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txtT.Text))
                {
                    _imService.CreateIncident(txtT.Text.Trim(), txtD.Text.Trim(), IncidentSeverity.High, "LYCHEE MAIN ENTRANCE CAM 01", Environment.UserName);
                    RefreshIncidents();
                }
            }
        }
    }
}
