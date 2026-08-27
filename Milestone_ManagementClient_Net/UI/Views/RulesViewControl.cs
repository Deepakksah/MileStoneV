using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class RulesViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private ListView _lvRules;

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
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 4, 8, 4) };
            pnlHeader.Controls.Add(new Label { Text = "📜 Event Rules & Automation Engine", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 4, 0, 0) });

            var btnAddRule = new Button
            {
                Text = "➕ Add Rule Wizard...",
                Dock = DockStyle.Right,
                Size = new Size(150, 28),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddRule.FlatAppearance.BorderSize = 0;
            btnAddRule.Click += (s, e) => PromptAddRule();
            pnlHeader.Controls.Add(btnAddRule);

            var btnDeleteRule = new Button
            {
                Text = "🗑 Delete Rule",
                Dock = DockStyle.Right,
                Size = new Size(100, 28),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDeleteRule.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnDeleteRule.Click += (s, e) => DeleteSelectedRule();
            pnlHeader.Controls.Add(btnDeleteRule);

            this.Controls.Add(pnlHeader);

            _lvRules = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                CheckBoxes = true
            };
            _lvRules.Columns.Add("Rule Name", 260);
            _lvRules.Columns.Add("Event Trigger", 180);
            _lvRules.Columns.Add("Target Source", 200);
            _lvRules.Columns.Add("Time Profile", 140);
            _lvRules.Columns.Add("Configured Action", 300);

            _lvRules.ItemChecked += (s, e) =>
            {
                var r = e.Item.Tag as RuleModel;
                if (r != null && r.IsEnabled != e.Item.Checked)
                {
                    _vmoService.ToggleRule(r.Id);
                }
            };

            this.Controls.Add(_lvRules);
            _lvRules.BringToFront();
        }

        public void RefreshRulesList()
        {
            _lvRules.Items.Clear();
            var rules = _vmoService.GetRules();
            foreach (var r in rules)
            {
                var item = new ListViewItem(r.Name);
                item.Checked = r.IsEnabled;
                item.SubItems.Add(r.EventTrigger);
                item.SubItems.Add(r.TargetSource);
                item.SubItems.Add(r.TimeProfile);
                item.SubItems.Add(r.Action);
                item.Tag = r;
                _lvRules.Items.Add(item);
            }
        }

        private void DeleteSelectedRule()
        {
            if (_lvRules.SelectedItems.Count > 0)
            {
                var r = _lvRules.SelectedItems[0].Tag as RuleModel;
                if (r != null && MessageBox.Show("Are you sure you want to delete rule '" + r.Name + "'?", "Delete Rule", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _vmoService.DeleteRule(r.Id);
                    RefreshRulesList();
                }
            }
        }

        private void PromptAddRule()
        {
            var form = new Form
            {
                Text = "🪄 Add Rule Wizard",
                Size = new Size(460, 340),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 9F)
            };

            int y = 14;
            form.Controls.Add(new Label { Text = "Rule Name:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtName = new TextBox { Text = "Rule: Emergency Gate Alarm", Location = new Point(14, y + 20), Size = new Size(415, 24) };
            form.Controls.Add(txtName);
            y += 50;

            form.Controls.Add(new Label { Text = "Event Trigger Condition:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var cbTrigger = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(14, y + 20), Size = new Size(415, 24) };
            cbTrigger.Items.AddRange(new object[] { "Motion Started", "Analytics: Line Crossing", "Speed Violation > 40 km/h", "Manual Event Trigger", "Access Control Door Forced" });
            cbTrigger.SelectedIndex = 0;
            form.Controls.Add(cbTrigger);
            y += 50;

            form.Controls.Add(new Label { Text = "Target Device / Group:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var cbTarget = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(14, y + 20), Size = new Size(415, 24) };
            cbTarget.Items.AddRange(new object[] { "All Cameras in South Block", "ROAD SAFETY Cameras", "MAIN GATE ENTRY CAM", "SECTOR-1 Cameras", "All Recording Servers" });
            cbTarget.SelectedIndex = 0;
            form.Controls.Add(cbTarget);
            y += 50;

            form.Controls.Add(new Label { Text = "Action to Perform:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtAction = new TextBox { Text = "Start recording on related device at 25 fps and trigger PTZ preset", Location = new Point(14, y + 20), Size = new Size(415, 24) };
            form.Controls.Add(txtAction);
            y += 55;

            var btnOk = new Button { Text = "💾 Save Rule", Location = new Point(220, y), Size = new Size(110, 30), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(340, y), Size = new Size(88, 30), FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    _vmoService.AddRule(txtName.Text.Trim(), cbTrigger.SelectedItem.ToString(), cbTarget.SelectedItem.ToString(), "Always", txtAction.Text.Trim(), true);
                    RefreshRulesList();
                    form.Close();
                }
            };
            btnCancel.Click += (s, e) => form.Close();

            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }
    }
}
