using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class RulesViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private TreeView _treeRules;
        private TextBox _txtName;
        private TextBox _txtDescription;
        private CheckBox _chkActive;
        private LinkLabel _lnkEventTrigger;
        private LinkLabel _lnkTargetSource;
        private LinkLabel _lnkLogEntry;
        private Label _lblActionSummary;
        private Button _btnSave;
        private Button _btnTestTrigger;
        private RuleModel _selectedRule;

        public RulesViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshRulesList();
        }

        private void BuildUI()
        {
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Vertical;
            split.FixedPanel = FixedPanel.Panel1;
            split.Panel1MinSize = 20;
            split.Panel2MinSize = 20;
            split.SplitterWidth = 6;
            split.BackColor = Color.FromArgb(203, 213, 225);
            split.Panel1.BackColor = Color.White;
            split.Panel2.BackColor = Color.White;
            this.Controls.Add(split);
            try { if (split.Width > 500) split.SplitterDistance = 340; } catch { }
            this.SizeChanged += (s, e) => { try { if (split.Width > 500) split.SplitterDistance = 340; } catch { } };

            // ====================================================
            // LEFT PANE: Rules Tree & Toolbar (Exact Milestone)
            // ====================================================
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // Header Banner
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 4, 8, 4) };
            pnlHeader.Controls.Add(new Label { Text = "📌 Rules", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 3, 0, 0) });

            var btnAdd = new Button
            {
                Text = "➕ Add Rule...",
                Dock = DockStyle.Right,
                Size = new Size(95, 24),
                BackColor = Color.FromArgb(190, 18, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => PromptAddRule();
            pnlHeader.Controls.Add(btnAdd);

            var btnDup = new Button
            {
                Text = "📋 Copy",
                Dock = DockStyle.Right,
                Size = new Size(65, 24),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            btnDup.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnDup.Click += (s, e) =>
            {
                if (_selectedRule != null)
                {
                    _vmoService.DuplicateRule(_selectedRule.Id);
                    RefreshRulesList();
                }
            };
            pnlHeader.Controls.Add(btnDup);

            var btnDel = new Button
            {
                Text = "🗑 Delete",
                Dock = DockStyle.Right,
                Size = new Size(70, 24),
                BackColor = Color.White,
                ForeColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnDel.Click += (s, e) => DeleteSelectedRule();
            pnlHeader.Controls.Add(btnDel);

            pnlLeft.Controls.Add(pnlHeader);

            // Rules Tree
            _treeRules = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                ShowLines = true,
                ShowPlusMinus = true,
                ShowRootLines = true,
                ItemHeight = 22,
                HideSelection = false
            };

            _treeRules.AfterSelect += (s, e) =>
            {
                if (e.Node != null && e.Node.Tag is RuleModel)
                {
                    DisplayRuleDetails((RuleModel)e.Node.Tag);
                }
            };

            // Setup Context Menu
            SetupRulesContextMenu();

            pnlLeft.Controls.Add(_treeRules);
            _treeRules.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // ====================================================
            // RIGHT PANE: Rule Information (Exact Milestone Layout)
            // ====================================================
            var pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(14) };

            // Section Header
            var lblInfoTitle = new Label
            {
                Text = "Rule Information",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Dock = DockStyle.Top,
                Height = 28
            };
            pnlRight.Controls.Add(lblInfoTitle);

            // 1. Name Field
            var pnlName = new Panel { Dock = DockStyle.Top, Height = 56 };
            pnlName.Controls.Add(new Label { Text = "Name:", Location = new Point(0, 4), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            _txtName = new TextBox { Location = new Point(0, 24), Size = new Size(480, 24), Font = new Font("Segoe UI", 9F) };
            _txtName.TextChanged += (s, e) => { if (_selectedRule != null) _btnSave.Enabled = true; };
            pnlName.Controls.Add(_txtName);
            pnlRight.Controls.Add(pnlName);

            // 2. Description Field
            var pnlDesc = new Panel { Dock = DockStyle.Top, Height = 100 };
            pnlDesc.Controls.Add(new Label { Text = "Description:", Location = new Point(0, 4), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            _txtDescription = new TextBox { Location = new Point(0, 24), Size = new Size(480, 68), Multiline = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Segoe UI", 9F) };
            _txtDescription.TextChanged += (s, e) => { if (_selectedRule != null) _btnSave.Enabled = true; };
            pnlDesc.Controls.Add(_txtDescription);
            pnlRight.Controls.Add(pnlDesc);

            // 3. Active Checkbox
            var pnlActive = new Panel { Dock = DockStyle.Top, Height = 34 };
            _chkActive = new CheckBox { Text = "Active", Checked = true, Location = new Point(0, 6), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _chkActive.CheckedChanged += (s, e) => { if (_selectedRule != null) _btnSave.Enabled = true; };
            pnlActive.Controls.Add(_chkActive);
            pnlRight.Controls.Add(pnlActive);

            // 4. Definition Group (Milestone Rule Grammar & Links)
            var gbDef = new GroupBox
            {
                Text = "Definition",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(12)
            };

            int dy = 26;
            gbDef.Controls.Add(new Label { Text = "Perform an action on", Location = new Point(12, dy), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            _lnkEventTrigger = new LinkLabel
            {
                Text = "Motion Started (HW)",
                Location = new Point(135, dy),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline),
                LinkColor = Color.FromArgb(0, 102, 204),
                Cursor = Cursors.Hand
            };
            _lnkEventTrigger.Click += (s, e) => PromptSelectEventTrigger();
            gbDef.Controls.Add(_lnkEventTrigger);
            dy += 26;

            gbDef.Controls.Add(new Label { Text = "from", Location = new Point(24, dy), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            _lnkTargetSource = new LinkLabel
            {
                Text = "Camera Group 1",
                Location = new Point(60, dy),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline),
                LinkColor = Color.FromArgb(0, 102, 204),
                Cursor = Cursors.Hand
            };
            _lnkTargetSource.Click += (s, e) => PromptSelectTargetSource();
            gbDef.Controls.Add(_lnkTargetSource);
            dy += 28;

            _lblActionSummary = new Label
            {
                Text = "Action: Start recording 3 seconds before event at 25 fps",
                Location = new Point(12, dy),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            gbDef.Controls.Add(_lblActionSummary);
            dy += 26;

            gbDef.Controls.Add(new Label { Text = "Create log entry: '", Location = new Point(12, dy), AutoSize = true, Font = new Font("Segoe UI", 9F) });
            _lnkLogEntry = new LinkLabel
            {
                Text = "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$",
                Location = new Point(115, dy),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline),
                LinkColor = Color.FromArgb(0, 102, 204),
                Cursor = Cursors.Hand
            };
            _lnkLogEntry.Click += (s, e) => PromptEditLogEntry();
            gbDef.Controls.Add(_lnkLogEntry);
            dy += 24;
            gbDef.Controls.Add(new Label { Text = "'", Location = new Point(540, dy - 24), AutoSize = true, Font = new Font("Segoe UI", 9F) });

            pnlRight.Controls.Add(gbDef);

            // 5. Bottom Action Bar
            var pnlActions = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(0, 14, 0, 0) };

            _btnSave = new Button
            {
                Text = "💾 Save Rule Changes",
                Location = new Point(0, 14),
                Size = new Size(160, 32),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += (s, e) => SaveCurrentRule();
            pnlActions.Controls.Add(_btnSave);

            _btnTestTrigger = new Button
            {
                Text = "⚡ Test Rule Trigger Live",
                Location = new Point(170, 14),
                Size = new Size(170, 32),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnTestTrigger.FlatAppearance.BorderSize = 0;
            _btnTestTrigger.Click += (s, e) => TestFireRule();
            pnlActions.Controls.Add(_btnTestTrigger);

            pnlRight.Controls.Add(pnlActions);
            split.Panel2.Controls.Add(pnlRight);
        }

        private void SetupRulesContextMenu()
        {
            var ctx = new ContextMenuStrip();
            ctx.Items.Add(new ToolStripMenuItem("➕ Add Rule...", null, (s, e) => PromptAddRule()) { Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            ctx.Items.Add(new ToolStripMenuItem("📋 Duplicate Rule", null, (s, e) =>
            {
                if (_selectedRule != null)
                {
                    _vmoService.DuplicateRule(_selectedRule.Id);
                    RefreshRulesList();
                }
            }));
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(new ToolStripMenuItem("⚡ Test Rule Trigger Live", null, (s, e) => TestFireRule()));
            ctx.Items.Add(new ToolStripMenuItem("⏯ Toggle Active / Disabled", null, (s, e) =>
            {
                if (_selectedRule != null)
                {
                    _vmoService.ToggleRule(_selectedRule.Id);
                    RefreshRulesList();
                }
            }));
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add(new ToolStripMenuItem("🗑 Delete Rule", null, (s, e) => DeleteSelectedRule()) { ForeColor = Color.Red });
            ctx.Items.Add(new ToolStripMenuItem("🔄 Refresh Rules", null, (s, e) => RefreshRulesList()));

            _treeRules.ContextMenuStrip = ctx;
            _treeRules.NodeMouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    _treeRules.SelectedNode = e.Node;
                }
            };
        }

        public void RefreshRulesList()
        {
            _treeRules.Nodes.Clear();

            var root = new TreeNode("Rules");
            root.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            root.ForeColor = Color.FromArgb(190, 18, 60);

            var rules = _vmoService.GetRules();
            TreeNode targetSelectNode = null;

            foreach (var r in rules)
            {
                string iconPrefix = r.IsDefaultRule ? "⭐ " : "📋 ";
                string stateSuffix = r.IsEnabled ? "" : " (Disabled)";
                var node = new TreeNode(iconPrefix + r.Name + stateSuffix)
                {
                    Tag = r,
                    ForeColor = r.IsEnabled ? Color.FromArgb(15, 23, 42) : Color.FromArgb(148, 163, 184)
                };

                if (_selectedRule != null && _selectedRule.Id == r.Id)
                {
                    targetSelectNode = node;
                }

                root.Nodes.Add(node);
            }

            _treeRules.Nodes.Add(root);
            root.Expand();

            if (targetSelectNode != null)
            {
                _treeRules.SelectedNode = targetSelectNode;
            }
            else if (root.Nodes.Count > 0)
            {
                // Default to "Analitics Triger by Motion detection" matching screenshot
                var defaultNode = root.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text.Contains("Motion detection")) ?? root.Nodes[0];
                _treeRules.SelectedNode = defaultNode;
            }
        }

        private void DisplayRuleDetails(RuleModel r)
        {
            _selectedRule = r;
            _txtName.Text = r.Name;
            _txtDescription.Text = r.Description;
            _chkActive.Checked = r.IsEnabled;
            _lnkEventTrigger.Text = r.EventTrigger;
            _lnkTargetSource.Text = r.TargetSource;
            _lnkLogEntry.Text = r.LogEntry;
            _lblActionSummary.Text = "Action: " + r.Action;
            _btnSave.Enabled = false;
        }

        private void SaveCurrentRule()
        {
            if (_selectedRule != null && !string.IsNullOrWhiteSpace(_txtName.Text))
            {
                _vmoService.UpdateRule(
                    _selectedRule.Id,
                    _txtName.Text.Trim(),
                    _txtDescription.Text.Trim(),
                    _chkActive.Checked,
                    _lnkEventTrigger.Text,
                    _lnkTargetSource.Text,
                    _lnkLogEntry.Text
                );
                _btnSave.Enabled = false;
                RefreshRulesList();
                MessageBox.Show(string.Format("Rule '{0}' saved successfully.", _txtName.Text.Trim()), "Rule Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TestFireRule()
        {
            if (_selectedRule != null)
            {
                string logMsg = string.Format("Rule '{0}' triggered by '{1}' on target '{2}'. Action executed: {3}", _selectedRule.Name, _selectedRule.EventTrigger, _selectedRule.TargetSource, _selectedRule.Action);
                _vmoService.AddLog("EVENT", "RulesEngine", logMsg, "RuleAutomationService");
                MessageBox.Show("⚡ LIVE RULE TRIGGER SIMULATION SUCCESS!\n\n" + logMsg + "\n\nEvent entry written to Milestone Server Logs & Alarms.", "Rule Simulation Fired", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PromptSelectEventTrigger()
        {
            var form = new Form { Text = "Select Event Trigger", Size = new Size(380, 320), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false };
            var lb = new ListBox { Dock = DockStyle.Top, Height = 220, Font = new Font("Segoe UI", 9F) };
            lb.Items.AddRange(new object[] {
                "Motion Started (HW)",
                "Motion Stopped (HW)",
                "Analytics: Line Crossing",
                "Analytics: Intrusion Started",
                "Analytics: Crowd Density High",
                "Analytics: Tripwire Crossed",
                "Access Control: Door Forced Open",
                "PTZ Inactivity Timeout",
                "Audio Request Triggered",
                "Bookmark Created",
                "Manual Record Requested",
                "Timer: Daily Midnight Cycle"
            });
            if (lb.Items.Contains(_lnkEventTrigger.Text)) lb.SelectedItem = _lnkEventTrigger.Text;
            else lb.SelectedIndex = 0;
            form.Controls.Add(lb);

            var btnOk = new Button { Text = "Select", Location = new Point(180, 235), Size = new Size(80, 28), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnOk.Click += (s, e) =>
            {
                if (lb.SelectedItem != null)
                {
                    _lnkEventTrigger.Text = lb.SelectedItem.ToString();
                    _btnSave.Enabled = true;
                }
                form.Close();
            };
            form.Controls.Add(btnOk);
            form.ShowDialog(this);
        }

        private void PromptSelectTargetSource()
        {
            var form = new Form { Text = "Select Device Source / Group", Size = new Size(380, 320), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false };
            var lb = new ListBox { Dock = DockStyle.Top, Height = 220, Font = new Font("Segoe UI", 9F) };
            lb.Items.AddRange(new object[] {
                "Camera Group 1",
                "All Cameras",
                "South Block",
                "ROAD SAFETY",
                "SECTOR-1",
                "SECTOR-2",
                "SECTOR-3",
                "SECTOR-4",
                "SECTOR - UTKAL -C- MINES",
                "Sharmik Vihar",
                "Universal PTZ & Thermal"
            });
            if (lb.Items.Contains(_lnkTargetSource.Text)) lb.SelectedItem = _lnkTargetSource.Text;
            else lb.SelectedIndex = 0;
            form.Controls.Add(lb);

            var btnOk = new Button { Text = "Select", Location = new Point(180, 235), Size = new Size(80, 28), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnOk.Click += (s, e) =>
            {
                if (lb.SelectedItem != null)
                {
                    _lnkTargetSource.Text = lb.SelectedItem.ToString();
                    _btnSave.Enabled = true;
                }
                form.Close();
            };
            form.Controls.Add(btnOk);
            form.ShowDialog(this);
        }

        private void PromptEditLogEntry()
        {
            var form = new Form { Text = "Edit Log Entry Token Format", Size = new Size(540, 180), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false };
            form.Controls.Add(new Label { Text = "Log Token Expression:", Location = new Point(14, 14), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txt = new TextBox { Text = _lnkLogEntry.Text, Location = new Point(14, 38), Size = new Size(495, 24), Font = new Font("Segoe UI", 9F) };
            form.Controls.Add(txt);

            var btnOk = new Button { Text = "Apply Token", Location = new Point(390, 80), Size = new Size(120, 28), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnOk.Click += (s, e) =>
            {
                _lnkLogEntry.Text = txt.Text.Trim();
                _btnSave.Enabled = true;
                form.Close();
            };
            form.Controls.Add(btnOk);
            form.ShowDialog(this);
        }

        private void PromptAddRule()
        {
            var form = new Form
            {
                Text = "📜 Add Rule Wizard",
                Size = new Size(520, 420),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 9F)
            };

            int y = 14;
            form.Controls.Add(new Label { Text = "Rule Name:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtRName = new TextBox { Text = "New Smart Analytics Rule", Location = new Point(16, y + 22), Size = new Size(470, 24) };
            form.Controls.Add(txtRName);
            y += 54;

            form.Controls.Add(new Label { Text = "Description:", Location = new Point(16, y), AutoSize = true });
            var txtRDesc = new TextBox { Text = "Automated video analytics detection trigger", Location = new Point(16, y + 20), Size = new Size(470, 24) };
            form.Controls.Add(txtRDesc);
            y += 50;

            form.Controls.Add(new Label { Text = "Event Trigger:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var cbTrig = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(16, y + 20), Size = new Size(470, 24) };
            cbTrig.Items.AddRange(new object[] { "Motion Started (HW)", "Analytics: Line Crossing", "Analytics: Intrusion Started", "Analytics: Crowd Density High", "Tripwire Crossed", "Bookmark Created", "Manual Record Requested" });
            cbTrig.SelectedIndex = 0;
            form.Controls.Add(cbTrig);
            y += 52;

            form.Controls.Add(new Label { Text = "Target Device Group:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var cbTarget = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(16, y + 20), Size = new Size(470, 24) };
            cbTarget.Items.AddRange(new object[] { "Camera Group 1", "All Cameras", "South Block", "ROAD SAFETY", "SECTOR-1", "Universal" });
            cbTarget.SelectedIndex = 0;
            form.Controls.Add(cbTarget);
            y += 52;

            form.Controls.Add(new Label { Text = "Configured Action:", Location = new Point(16, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtAction = new TextBox { Text = "Start recording on related device at 25 fps & create log entry", Location = new Point(16, y + 20), Size = new Size(470, 24) };
            form.Controls.Add(txtAction);
            y += 56;

            var btnOk = new Button { Text = "💾 Create Rule", Location = new Point(270, y), Size = new Size(120, 30), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(400, y), Size = new Size(86, 30), FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtRName.Text))
                {
                    _vmoService.AddRule(txtRName.Text.Trim(), cbTrig.SelectedItem.ToString(), cbTarget.SelectedItem.ToString(), "Always", txtAction.Text.Trim(), true, txtRDesc.Text.Trim());
                    RefreshRulesList();
                    form.Close();
                }
            };
            btnCancel.Click += (s, e) => form.Close();

            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }

        private void DeleteSelectedRule()
        {
            if (_selectedRule != null)
            {
                if (_selectedRule.IsDefaultRule)
                {
                    MessageBox.Show("Default system rules cannot be deleted.", "Protected Rule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(string.Format("Are you sure you want to delete rule '{0}'?", _selectedRule.Name), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _vmoService.DeleteRule(_selectedRule.Id);
                    RefreshRulesList();
                }
            }
        }
    }
}
