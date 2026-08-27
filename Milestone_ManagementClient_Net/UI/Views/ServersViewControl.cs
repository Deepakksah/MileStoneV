using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class ServersViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private ListView _lvServers;
        private Panel _pnlDetails;
        private Label _lblName;
        private Label _lblHost;
        private Label _lblStatus;
        private ProgressBar _pbCpu;
        private Label _lblCpu;
        private ProgressBar _pbRam;
        private Label _lblRam;
        private ProgressBar _pbStorage;
        private Label _lblStorage;
        private Label _lblLivePath;
        private Label _lblArchivePath;
        private Label _lblFailover;

        public ServersViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshList();
        }

        private void BuildUI()
        {
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Vertical;
            split.FixedPanel = FixedPanel.Panel1;
            split.Panel1MinSize = 25;
            split.Panel2MinSize = 25;
            split.SplitterWidth = 6;
            split.BackColor = Color.FromArgb(203, 213, 225);
            split.Panel1.BackColor = Color.White;
            split.Panel2.BackColor = Color.White;
            this.Controls.Add(split);
            try { if (split.Width > 500) split.SplitterDistance = 450; } catch { }
            this.SizeChanged += (s, e) => { try { if (split.Width > 500) split.SplitterDistance = 450; } catch { } };

            // Left: Servers List
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = Color.FromArgb(255, 241, 242), Padding = new Padding(6, 4, 6, 4) };
            pnlHeader.Controls.Add(new Label { Text = "Recording Servers", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 3, 0, 0) });

            var btnDeleteServer = new Button
            {
                Text = "🗑 Delete",
                Dock = DockStyle.Right,
                Size = new Size(80, 24),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(190, 18, 60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDeleteServer.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnDeleteServer.Click += (s, e) =>
            {
                if (_lvServers.SelectedItems.Count > 0)
                {
                    var srv = _lvServers.SelectedItems[0].Tag as RecordingServerModel;
                    if (srv != null) PromptDeleteServer(srv.Name);
                }
                else
                {
                    MessageBox.Show("Please select a recording server to delete.", "Delete Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            pnlHeader.Controls.Add(btnDeleteServer);

            var btnRenameServer = new Button
            {
                Text = "✏ Rename",
                Dock = DockStyle.Right,
                Size = new Size(85, 24),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRenameServer.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnRenameServer.Click += (s, e) =>
            {
                if (_lvServers.SelectedItems.Count > 0)
                {
                    var srv = _lvServers.SelectedItems[0].Tag as RecordingServerModel;
                    if (srv != null) PromptRenameServer(srv.Name);
                }
                else
                {
                    MessageBox.Show("Please select a recording server to rename.", "Rename Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            pnlHeader.Controls.Add(btnRenameServer);

            var btnAddServer = new Button
            {
                Text = "➕ Add Server...",
                Dock = DockStyle.Right,
                Size = new Size(110, 24),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddServer.FlatAppearance.BorderSize = 0;
            btnAddServer.Click += (s, e) => PromptAddServer();
            pnlHeader.Controls.Add(btnAddServer);
            pnlLeft.Controls.Add(pnlHeader);

            _lvServers = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F)
            };
            _lvServers.Columns.Add("Server Name", 190);
            _lvServers.Columns.Add("IP / Host", 110);
            _lvServers.Columns.Add("Status", 110);
            _lvServers.Columns.Add("Cameras", 70);

            // Context Menu for Right Click Actions
            var ctx = new ContextMenuStrip();
            var mnuAdd = new ToolStripMenuItem("➕ Add Recording Server...", null, (s, e) => PromptAddServer());
            var mnuRename = new ToolStripMenuItem("✏ Rename Server...", null, (s, e) =>
            {
                if (_lvServers.SelectedItems.Count > 0)
                {
                    var srv = _lvServers.SelectedItems[0].Tag as RecordingServerModel;
                    if (srv != null) PromptRenameServer(srv.Name);
                }
            });
            var mnuDelete = new ToolStripMenuItem("🗑 Delete Server", null, (s, e) =>
            {
                if (_lvServers.SelectedItems.Count > 0)
                {
                    var srv = _lvServers.SelectedItems[0].Tag as RecordingServerModel;
                    if (srv != null) PromptDeleteServer(srv.Name);
                }
            });
            var mnuRefresh = new ToolStripMenuItem("🔄 Refresh", null, (s, e) => RefreshList());

            ctx.Items.Add(mnuAdd);
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(mnuRename);
            ctx.Items.Add(mnuDelete);
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(mnuRefresh);
            _lvServers.ContextMenuStrip = ctx;

            _lvServers.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete && _lvServers.SelectedItems.Count > 0)
                {
                    var srv = _lvServers.SelectedItems[0].Tag as RecordingServerModel;
                    if (srv != null) PromptDeleteServer(srv.Name);
                }
            };

            _lvServers.SelectedIndexChanged += (s, e) =>
            {
                if (_lvServers.SelectedItems.Count > 0)
                {
                    var srv = _lvServers.SelectedItems[0].Tag as RecordingServerModel;
                    if (srv != null) DisplayServerDetails(srv);
                }
            };

            pnlLeft.Controls.Add(_lvServers);
            _lvServers.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // Right: Detailed Info & Gauges
            _pnlDetails = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(12) };
            BuildDetailsPanel();
            split.Panel2.Controls.Add(_pnlDetails);
        }

        private void BuildDetailsPanel()
        {
            _pnlDetails.Controls.Clear();

            // Milestone Tabs Header
            var pnlSubTabs = new Panel { Dock = DockStyle.Top, Height = 28, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(2) };
            var tabBtns = new[] { "Recording server information", "Failover", "Multicast", "Network", "Info" };
            int tx = 2;
            foreach (var t in tabBtns)
            {
                var b = new Button
                {
                    Text = t,
                    Location = new Point(tx, 2),
                    Height = 24,
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = (t == "Recording server information") ? Color.White : Color.FromArgb(241, 245, 249),
                    ForeColor = (t == "Recording server information") ? Color.FromArgb(190, 18, 60) : Color.FromArgb(71, 85, 105),
                    Font = new Font("Segoe UI", 8.5F, (t == "Recording server information") ? FontStyle.Bold : FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                b.FlatAppearance.BorderSize = 0;
                pnlSubTabs.Controls.Add(b);
                tx += b.PreferredSize.Width + 4;
            }
            _pnlDetails.Controls.Add(pnlSubTabs);

            // Group: Recording server information (Exact Milestone Screenshot)
            var gbInfo = new GroupBox
            {
                Text = "Recording server information",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(12)
            };
            int y = 24;

            gbInfo.Controls.Add(new Label { Text = "Name:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblName = new Label { Text = "JSPANGICCCREC01.JSPLICCC.LOCAL", Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(0, 102, 204), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            gbInfo.Controls.Add(_lblName);
            y += 26;

            gbInfo.Controls.Add(new Label { Text = "Description:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            var txtDesc = new TextBox { Text = "JSPL ICC Central Command Recording Server", Location = new Point(140, y - 2), Size = new Size(340, 23) };
            gbInfo.Controls.Add(txtDesc);
            y += 28;

            gbInfo.Controls.Add(new Label { Text = "Host name:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblHost = new Label { Text = "jspangicccrec01.jspliccc.local", Location = new Point(140, y), AutoSize = true };
            gbInfo.Controls.Add(_lblHost);
            y += 26;

            gbInfo.Controls.Add(new Label { Text = "Web server address:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var txtWeb = new TextBox { Text = "http://jspangicccrec01.jspliccc.local:7563/", ReadOnly = true, Location = new Point(140, y - 2), Size = new Size(340, 23), BackColor = Color.FromArgb(248, 250, 252) };
            gbInfo.Controls.Add(txtWeb);
            y += 30;

            gbInfo.Controls.Add(new Label { Text = "Time zone:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            var cbTz = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(140, y - 2), Size = new Size(340, 23) };
            cbTz.Items.AddRange(new object[] { "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", "(UTC+00:00) UTC Universal Coordinated Time", "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm", "(UTC-05:00) Eastern Time (US & Canada)" });
            cbTz.SelectedIndex = 0;
            gbInfo.Controls.Add(cbTz);
            y += 30;

            gbInfo.Controls.Add(new Label { Text = "Service Status:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblStatus = new Label { Text = "● Running (Active)", Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(16, 185, 129), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbInfo.Controls.Add(_lblStatus);

            _pnlDetails.Controls.Add(gbInfo);
            gbInfo.BringToFront();

            // Performance Gauges
            var gbPerf = new GroupBox { Text = "Hardware Performance Metrics", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 140, Padding = new Padding(10) };
            int py = 26;

            gbPerf.Controls.Add(new Label { Text = "CPU Load:", Location = new Point(12, py), AutoSize = true });
            _pbCpu = new ProgressBar { Location = new Point(140, py), Size = new Size(180, 18), Value = 32 };
            _lblCpu = new Label { Text = "32%", Location = new Point(330, py), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbPerf.Controls.Add(_pbCpu);
            gbPerf.Controls.Add(_lblCpu);
            py += 32;

            gbPerf.Controls.Add(new Label { Text = "RAM Memory:", Location = new Point(12, py), AutoSize = true });
            _pbRam = new ProgressBar { Location = new Point(140, py), Size = new Size(180, 18), Value = 48 };
            _lblRam = new Label { Text = "48% (7.68 GB / 16 GB)", Location = new Point(330, py), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbPerf.Controls.Add(_pbRam);
            gbPerf.Controls.Add(_lblRam);
            py += 32;

            gbPerf.Controls.Add(new Label { Text = "Failover Server:", Location = new Point(12, py), AutoSize = true });
            _lblFailover = new Label { Text = "FAILOVER-SRV-01.JSPLICCC.LOCAL (Hot Standby)", Location = new Point(140, py), AutoSize = true, ForeColor = Color.FromArgb(79, 70, 229), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbPerf.Controls.Add(_lblFailover);

            _pnlDetails.Controls.Add(gbPerf);
            gbPerf.BringToFront();

            // Storage Pools
            var gbStorage = new GroupBox { Text = "Media Database & Storage Pools", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 160, Padding = new Padding(10) };
            int sy = 24;

            gbStorage.Controls.Add(new Label { Text = "Live Recording Pool:", Location = new Point(12, sy), AutoSize = true });
            _lblLivePath = new Label { Text = @"D:\Milestone\MediaDatabase\Live01", Location = new Point(150, sy), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            gbStorage.Controls.Add(_lblLivePath);
            sy += 24;

            _pbStorage = new ProgressBar { Location = new Point(12, sy), Size = new Size(320, 20), Value = 42 };
            _lblStorage = new Label { Text = "4.2 TB used of 10.0 TB (42%)", Location = new Point(12, sy + 24), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbStorage.Controls.Add(_pbStorage);
            gbStorage.Controls.Add(_lblStorage);
            sy += 50;

            gbStorage.Controls.Add(new Label { Text = "Archive Storage Pool:", Location = new Point(12, sy), AutoSize = true });
            _lblArchivePath = new Label { Text = @"E:\Milestone\Archive01 (18 TB / 40 TB)", Location = new Point(150, sy), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            gbStorage.Controls.Add(_lblArchivePath);
            _pnlDetails.Controls.Add(gbStorage);
            gbStorage.BringToFront();
        }

        public void RefreshList()
        {
            _lvServers.Items.Clear();
            var servers = _vmoService.GetServers();
            foreach (var srv in servers)
            {
                var item = new ListViewItem(srv.Name);
                item.SubItems.Add(srv.Hostname);
                item.SubItems.Add(srv.Status);
                item.SubItems.Add(srv.DeviceCount.ToString());
                item.Tag = srv;
                _lvServers.Items.Add(item);
            }
            if (_lvServers.Items.Count > 0)
            {
                _lvServers.Items[0].Selected = true;
            }
        }

        private void DisplayServerDetails(RecordingServerModel srv)
        {
            _lblName.Text = srv.Name;
            _lblHost.Text = srv.Hostname + ":" + srv.Port;
            _lblStatus.Text = "● " + srv.Status;
            _lblFailover.Text = srv.IsFailoverConfigured ? srv.FailoverServerName : "None configured";

            _pbCpu.Value = Math.Min(100, Math.Max(0, srv.CpuUsage));
            _lblCpu.Text = srv.CpuUsage + "%";

            _pbRam.Value = Math.Min(100, Math.Max(0, srv.RamUsage));
            _lblRam.Text = srv.RamUsage + "%";

            int storagePct = (int)((srv.StorageUsedGb / (srv.StorageTotalGb > 0 ? srv.StorageTotalGb : 1)) * 100);
            _pbStorage.Value = Math.Min(100, Math.Max(0, storagePct));
            _lblStorage.Text = string.Format("{0:N1} GB used of {1:N1} GB ({2}%)", srv.StorageUsedGb, srv.StorageTotalGb, storagePct);
            _lblLivePath.Text = srv.PrimaryStoragePath;
            _lblArchivePath.Text = srv.ArchiveStoragePath + " (" + srv.ArchiveUsedGb + " GB / " + srv.ArchiveTotalGb + " GB)";
        }

        private void PromptAddServer()
        {
            var dlg = new AddHardwareWizardDialog(_vmoService, RefreshList);
            dlg.ShowDialog(this);
        }

        private void PromptRenameServer(string currentName)
        {
            var form = new Form
            {
                Text = "✏ Rename Recording Server",
                Size = new Size(400, 180),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 9F)
            };

            form.Controls.Add(new Label { Text = "Recording Server Name:", Location = new Point(16, 16), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtName = new TextBox { Text = currentName, Location = new Point(16, 42), Size = new Size(350, 24), Font = new Font("Segoe UI", 9.5F) };
            form.Controls.Add(txtName);

            var btnOk = new Button { Text = "Rename", Location = new Point(170, 85), Size = new Size(100, 28), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(280, 85), Size = new Size(86, 28), FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtName.Text))
                {
                    _vmoService.RenameRecordingServer(currentName, txtName.Text.Trim());
                    RefreshList();
                    form.Close();
                }
            };
            btnCancel.Click += (s, e) => form.Close();

            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }

        private void PromptDeleteServer(string currentName)
        {
            var res = MessageBox.Show(
                string.Format("Are you sure you want to delete the recording server '{0}'?\n\nThis will remove the server from the cluster and update the MSSQL surveillance database.", currentName),
                "Confirm Delete Recording Server",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (res == DialogResult.Yes)
            {
                _vmoService.DeleteRecordingServer(currentName);
                RefreshList();
                MessageBox.Show(
                    string.Format("Recording server '{0}' has been successfully deleted.", currentName),
                    "Server Deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
