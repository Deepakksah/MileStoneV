using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class ServersViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private TreeView _treeServers;
        private TextBox _txtFilter;
        private CheckBox _chkHideDisabled;
        private Panel _pnlDetails;

        // Details Panel Controls
        private Label _lblName;
        private TextBox _txtDesc;
        private Label _lblHost;
        private TextBox _txtWebAddress;
        private ComboBox _cbTimeZone;
        private Label _lblStatus;
        private Label _lblFailover;
        private ProgressBar _pbCpu;
        private Label _lblCpu;
        private ProgressBar _pbRam;
        private Label _lblRam;
        private ProgressBar _pbStorage;
        private Label _lblStorage;
        private Label _lblLivePath;
        private Label _lblArchivePath;

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
            try { if (split.Width > 500) split.SplitterDistance = 380; } catch { }
            this.SizeChanged += (s, e) => { try { if (split.Width > 500) split.SplitterDistance = 380; } catch { } };

            // ==========================================
            // LEFT PANE: Filter Bar + Recording Servers Tree
            // ==========================================
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // 1. Top Section Header
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 30, BackColor = Color.FromArgb(255, 241, 242), Padding = new Padding(8, 4, 8, 4) };
            pnlHeader.Controls.Add(new Label { Text = "Recording Servers", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 2, 0, 0) });
            pnlLeft.Controls.Add(pnlHeader);

            // 2. Filter Bar (Exact Milestone UI)
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(248, 250, 252), Padding = new Padding(8, 4, 8, 6) };
            _chkHideDisabled = new CheckBox
            {
                Text = "Hide disabled devices",
                Checked = true,
                Location = new Point(8, 6),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F)
            };
            pnlFilter.Controls.Add(_chkHideDisabled);

            _txtFilter = new TextBox
            {
                Location = new Point(8, 30),
                Size = new Size(260, 23),
                Font = new Font("Segoe UI", 8.5F),
                Text = "Type here to filter devices..."
            };
            _txtFilter.GotFocus += (s, e) => { if (_txtFilter.Text == "Type here to filter devices...") { _txtFilter.Text = ""; _txtFilter.ForeColor = Color.Black; } };
            _txtFilter.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(_txtFilter.Text)) { _txtFilter.Text = "Type here to filter devices..."; _txtFilter.ForeColor = Color.FromArgb(148, 163, 184); } };
            _txtFilter.TextChanged += (s, e) => FilterServerTree(_txtFilter.Text);
            pnlFilter.Controls.Add(_txtFilter);

            var btnApplyFilter = new Button
            {
                Text = "Apply filter",
                Location = new Point(274, 29),
                Size = new Size(85, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F)
            };
            btnApplyFilter.Click += (s, e) => FilterServerTree(_txtFilter.Text);
            pnlFilter.Controls.Add(btnApplyFilter);
            pnlLeft.Controls.Add(pnlFilter);

            // 3. Recording Servers TreeView
            _treeServers = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                FullRowSelect = false,
                HideSelection = false,
                ShowLines = true,
                ShowPlusMinus = true,
                ShowRootLines = true,
                ItemHeight = 22
            };

            _treeServers.AfterSelect += (s, e) =>
            {
                if (e.Node != null)
                {
                    var srv = e.Node.Tag as RecordingServerModel;
                    if (srv != null) DisplayServerDetails(srv);
                }
            };

            // Setup Exact Milestone Right-Click Context Menu
            SetupTreeContextMenu();

            // Keyboard Shortcuts
            _treeServers.KeyDown += (s, e) =>
            {
                var srv = GetSelectedServer();
                if (e.Control && e.KeyCode == Keys.N)
                {
                    PromptAddHardware();
                }
                else if (e.KeyCode == Keys.F2 && srv != null)
                {
                    PromptRenameServer(srv.Name);
                }
                else if (e.KeyCode == Keys.Delete && srv != null)
                {
                    PromptDeleteServer(srv.Name);
                }
                else if (e.KeyCode == Keys.F5)
                {
                    RefreshList();
                }
            };

            pnlLeft.Controls.Add(_treeServers);
            _treeServers.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // ==========================================
            // RIGHT PANE: Milestone Server Properties & Subtabs
            // ==========================================
            _pnlDetails = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(12) };
            BuildDetailsPanel();
            split.Panel2.Controls.Add(_pnlDetails);
        }

        // ==========================================
        // EXACT MILESTONE RIGHT-CLICK CONTEXT MENU
        // ==========================================
        private void SetupTreeContextMenu()
        {
            var ctx = new ContextMenuStrip();

            var mnuExpand = new ToolStripMenuItem("Expand", null, (s, e) => { if (_treeServers.SelectedNode != null) _treeServers.SelectedNode.Expand(); });
            var mnuAddHw = new ToolStripMenuItem("⭐ Add Hardware...\tCtrl+N", null, (s, e) => PromptAddHardware());
            mnuAddHw.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            var mnuMoveHw = new ToolStripMenuItem("Move Hardware...", null, (s, e) => MessageBox.Show("Select target recording server to move hardware channels.", "Move Hardware", MessageBoxButtons.OK, MessageBoxIcon.Information));
            var mnuDeleteAllHw = new ToolStripMenuItem("Delete All Hardware", null, (s, e) =>
            {
                var srv = GetSelectedServer();
                if (srv != null && MessageBox.Show(string.Format("Are you sure you want to delete all hardware under '{0}'?", srv.Name), "Confirm Delete All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    srv.DeviceCount = 0;
                    RefreshList();
                }
            });

            var mnuChangePass = new ToolStripMenuItem("🔒 Change Hardware Password...", null, (s, e) => MessageBox.Show("Enter new universal device hardware password.", "Change Hardware Password", MessageBoxButtons.OK, MessageBoxIcon.Information));
            var mnuRename = new ToolStripMenuItem("Rename Recording Server\tF2", null, (s, e) =>
            {
                var srv = GetSelectedServer();
                if (srv != null) PromptRenameServer(srv.Name);
            });

            var mnuRemove = new ToolStripMenuItem("❌ Remove Recording Server", null, (s, e) =>
            {
                var srv = GetSelectedServer();
                if (srv != null) PromptDeleteServer(srv.Name);
            });
            mnuRemove.ForeColor = Color.FromArgb(190, 18, 60);

            var mnuRefresh = new ToolStripMenuItem("🔄 Refresh\tF5", null, (s, e) => RefreshList());

            ctx.Items.Add(mnuExpand);
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(mnuAddHw);
            ctx.Items.Add(mnuMoveHw);
            ctx.Items.Add(mnuDeleteAllHw);
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(mnuChangePass);
            ctx.Items.Add(mnuRename);
            ctx.Items.Add(mnuRemove);
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(mnuRefresh);

            _treeServers.NodeMouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    _treeServers.SelectedNode = e.Node;
                }
            };
            _treeServers.ContextMenuStrip = ctx;
        }

        private RecordingServerModel GetSelectedServer()
        {
            if (_treeServers.SelectedNode != null)
            {
                return _treeServers.SelectedNode.Tag as RecordingServerModel;
            }
            return null;
        }

        // ==========================================
        // PROPERTIES PANEL: Exact Milestone Subtabs
        // ==========================================
        private void BuildDetailsPanel()
        {
            _pnlDetails.Controls.Clear();

            // 1. Milestone Properties Tabs Header
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

            // 2. Group: Recording server information (Screenshot Layout)
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
            _txtDesc = new TextBox { Text = "JSPL ICC Central Command Recording Server", Location = new Point(140, y - 2), Size = new Size(340, 23) };
            gbInfo.Controls.Add(_txtDesc);
            y += 28;

            gbInfo.Controls.Add(new Label { Text = "Host name:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblHost = new Label { Text = "jspangicccrec01.jspliccc.local", Location = new Point(140, y), AutoSize = true };
            gbInfo.Controls.Add(_lblHost);
            y += 26;

            gbInfo.Controls.Add(new Label { Text = "Web server address:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtWebAddress = new TextBox { Text = "http://jspangicccrec01.jspliccc.local:7563/", ReadOnly = true, Location = new Point(140, y - 2), Size = new Size(340, 23), BackColor = Color.FromArgb(248, 250, 252) };
            gbInfo.Controls.Add(_txtWebAddress);
            y += 30;

            gbInfo.Controls.Add(new Label { Text = "Time zone:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbTimeZone = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(140, y - 2), Size = new Size(340, 23) };
            _cbTimeZone.Items.AddRange(new object[] { "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", "(UTC+00:00) UTC Universal Coordinated Time", "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm", "(UTC-05:00) Eastern Time (US & Canada)" });
            _cbTimeZone.SelectedIndex = 0;
            gbInfo.Controls.Add(_cbTimeZone);
            y += 30;

            gbInfo.Controls.Add(new Label { Text = "Service Status:", Location = new Point(12, y), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _lblStatus = new Label { Text = "● Running (Active)", Location = new Point(140, y), AutoSize = true, ForeColor = Color.FromArgb(16, 185, 129), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            gbInfo.Controls.Add(_lblStatus);

            _pnlDetails.Controls.Add(gbInfo);
            gbInfo.BringToFront();

            // 3. Performance Metrics
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

            // 4. Storage Pools
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
            _treeServers.Nodes.Clear();

            var rootNode = new TreeNode("🖧 Recording Servers");
            rootNode.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            rootNode.ForeColor = Color.FromArgb(190, 18, 60);

            var servers = _vmoService.GetServers();
            var allCameras = _vmoService.GetAllCameras().Values.ToList();

            foreach (var srv in servers)
            {
                var srvNode = new TreeNode("🖥 " + srv.Name);
                srvNode.Tag = srv;
                srvNode.ForeColor = Color.FromArgb(15, 23, 42);

                // Add Child Hardware / Cameras to Server Node
                var srvCams = allCameras.Where(c => c.IpAddress != null && c.IpAddress.StartsWith("192.168.1.")).Take(4).ToList();
                foreach (var cam in srvCams)
                {
                    var camNode = new TreeNode("📹 " + cam.Name + " (" + cam.IpAddress + ")");
                    camNode.Tag = cam;
                    camNode.ForeColor = Color.FromArgb(71, 85, 105);
                    srvNode.Nodes.Add(camNode);
                }

                rootNode.Nodes.Add(srvNode);
            }

            _treeServers.Nodes.Add(rootNode);
            rootNode.Expand();

            if (rootNode.Nodes.Count > 0)
            {
                _treeServers.SelectedNode = rootNode.Nodes[0];
                var srv = rootNode.Nodes[0].Tag as RecordingServerModel;
                if (srv != null) DisplayServerDetails(srv);
            }
        }

        private void FilterServerTree(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query == "Type here to filter devices...")
            {
                RefreshList();
                return;
            }

            _treeServers.Nodes.Clear();
            var rootNode = new TreeNode("🖧 Recording Servers (Filtered)");
            rootNode.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            rootNode.ForeColor = Color.FromArgb(190, 18, 60);

            var servers = _vmoService.GetServers();
            foreach (var srv in servers)
            {
                if (srv.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    srv.Hostname.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var srvNode = new TreeNode("🖥 " + srv.Name) { Tag = srv };
                    rootNode.Nodes.Add(srvNode);
                }
            }

            _treeServers.Nodes.Add(rootNode);
            rootNode.ExpandAll();
        }

        private void DisplayServerDetails(RecordingServerModel srv)
        {
            _lblName.Text = srv.Name;
            _txtDesc.Text = "JSPL ICC Central Command Recording Server - " + srv.Name;
            _lblHost.Text = srv.Hostname;
            _txtWebAddress.Text = string.Format("http://{0}:{1}/", srv.Hostname.ToLower(), srv.Port);
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

        // ==========================================
        // ACTION HANDLERS
        // ==========================================
        private void PromptAddHardware()
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
                string.Format("Are you sure you want to remove the recording server '{0}'?\n\nThis will remove the server from the cluster and synchronize with the MSSQL surveillance database.", currentName),
                "Remove Recording Server",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (res == DialogResult.Yes)
            {
                _vmoService.DeleteRecordingServer(currentName);
                RefreshList();
                MessageBox.Show(
                    string.Format("Recording server '{0}' has been successfully removed.", currentName),
                    "Recording Server Removed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
