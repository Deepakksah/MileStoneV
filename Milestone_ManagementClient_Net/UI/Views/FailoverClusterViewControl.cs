using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Core.Failover;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class FailoverClusterViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private readonly FailoverClusterManager _failoverManager;

        private ListView _lvNodes;
        private Label _lblNodeName;
        private Label _lblNodeStatus;
        private Button _btnSimulateFailover;
        private FailoverServerModel _selectedNode;

        public FailoverClusterViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            _failoverManager = new FailoverClusterManager();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshNodes();
        }

        private void BuildUI()
        {
            this.Controls.Clear();

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };
            var lblTitle = new Label
            {
                Text = "🛡️ Failover Recording Servers & High Availability Cluster",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 10),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "Configure hot standby secondary recording nodes to seamlessly take over live recording on primary server hardware failure.",
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(16, 34),
                AutoSize = true
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub });
            this.Controls.Add(pnlHeader);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 480,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            var pnlList = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            pnlList.Controls.Add(new Label { Text = "Configured Failover Groups & Hot Standby Nodes:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, Height = 25 });

            _lvNodes = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9F)
            };
            _lvNodes.Columns.Add("Failover Node", 170);
            _lvNodes.Columns.Add("Primary Target", 180);
            _lvNodes.Columns.Add("State", 110);
            _lvNodes.SelectedIndexChanged += LvNodes_SelectedIndexChanged;
            pnlList.Controls.Add(_lvNodes);
            _lvNodes.BringToFront();
            split.Panel1.Controls.Add(pnlList);

            var pnlDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16) };
            _lblNodeName = new Label { Text = "Select a failover node to manage standby state", Font = new Font("Segoe UI", 11F, FontStyle.Bold), Location = new Point(16, 12), AutoSize = true };
            _lblNodeStatus = new Label { Text = "Status: -", ForeColor = Color.FromArgb(16, 185, 129), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(16, 38), AutoSize = true };

            _btnSimulateFailover = new Button
            {
                Text = "⚡ Test Simulated Primary Crash / Failover Takeover",
                Location = new Point(16, 80),
                Size = new Size(340, 34),
                BackColor = Color.FromArgb(234, 88, 12),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnSimulateFailover.FlatAppearance.BorderSize = 0;
            _btnSimulateFailover.Click += (s, e) =>
            {
                if (_selectedNode != null)
                {
                    _failoverManager.TriggerFailover(_selectedNode.Id);
                    RefreshNodes();
                    string state = _selectedNode.IsActiveTakingOver ? "ACTIVE (Taking over recording)" : "HOT STANDBY (Monitoring primary)";
                    MessageBox.Show("Failover state toggled: " + state, "Failover Switched", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            pnlDetail.Controls.AddRange(new Control[] { _lblNodeName, _lblNodeStatus, _btnSimulateFailover });
            split.Panel2.Controls.Add(pnlDetail);

            this.Controls.Add(split);
            split.BringToFront();
        }

        private void RefreshNodes()
        {
            _lvNodes.Items.Clear();
            foreach (var n in _failoverManager.GetNodes())
            {
                var itm = new ListViewItem(n.ServerName);
                itm.SubItems.Add(n.AssignedPrimaryServer);
                itm.SubItems.Add(n.IsActiveTakingOver ? "🔴 Active (Took Over)" : "🟢 Hot Standby");
                itm.Tag = n;
                if (n.IsActiveTakingOver) itm.ForeColor = Color.FromArgb(220, 38, 38);
                else itm.ForeColor = Color.FromArgb(16, 185, 129);
                _lvNodes.Items.Add(itm);
            }
            if (_lvNodes.Items.Count > 0 && _selectedNode == null) _lvNodes.Items[0].Selected = true;
        }

        private void LvNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lvNodes.SelectedItems.Count > 0)
            {
                _selectedNode = _lvNodes.SelectedItems[0].Tag as FailoverServerModel;
                if (_selectedNode != null)
                {
                    _lblNodeName.Text = _selectedNode.ServerName + " [" + _selectedNode.HostIp + "]";
                    _lblNodeStatus.Text = "State: " + (_selectedNode.IsActiveTakingOver ? "🔴 ACTIVE (Taking over for " + _selectedNode.AssignedPrimaryServer + ")" : "🟢 HOT STANDBY (Monitoring primary node)");
                    _lblNodeStatus.ForeColor = _selectedNode.IsActiveTakingOver ? Color.FromArgb(220, 38, 38) : Color.FromArgb(16, 185, 129);
                }
            }
        }
    }
}
