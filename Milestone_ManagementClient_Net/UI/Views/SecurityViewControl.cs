using System;
using System.Drawing;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class SecurityViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private ListView _lvUsers;
        private ListView _lvRoles;

        public SecurityViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshSecurityData();
        }

        private void BuildUI()
        {
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.Orientation = Orientation.Vertical;
            split.FixedPanel = FixedPanel.Panel1;
            split.Panel1MinSize = 25;
            split.Panel2MinSize = 25;
            split.SplitterWidth = 5;
            this.Controls.Add(split);
            try { if (split.Width > 500) split.SplitterDistance = 450; } catch { }
            this.SizeChanged += (s, e) => { try { if (split.Width > 500) split.SplitterDistance = 450; } catch { } };

            // Left: Users
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlUserHeader = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 4, 8, 4) };
            pnlUserHeader.Controls.Add(new Label { Text = "👥 Basic & Windows Users", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 4, 0, 0) });

            var btnAddUser = new Button
            {
                Text = "➕ Add User...",
                Dock = DockStyle.Right,
                Size = new Size(110, 26),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.Click += (s, e) => PromptAddUser();
            pnlUserHeader.Controls.Add(btnAddUser);
            pnlLeft.Controls.Add(pnlUserHeader);

            _lvUsers = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F)
            };
            _lvUsers.Columns.Add("Username", 120);
            _lvUsers.Columns.Add("Display Name", 160);
            _lvUsers.Columns.Add("Assigned Role", 130);
            _lvUsers.Columns.Add("Auth Type", 100);

            pnlLeft.Controls.Add(_lvUsers);
            _lvUsers.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // Right: Roles
            var pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlRoleHeader = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 4, 8, 4) };
            pnlRoleHeader.Controls.Add(new Label { Text = "🛡️ Security Roles & Permissions", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 4, 0, 0) });
            pnlRight.Controls.Add(pnlRoleHeader);

            _lvRoles = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F)
            };
            _lvRoles.Columns.Add("Role Name", 150);
            _lvRoles.Columns.Add("Description & Permissions", 350);

            pnlRight.Controls.Add(_lvRoles);
            _lvRoles.BringToFront();
            split.Panel2.Controls.Add(pnlRight);
        }

        public void RefreshSecurityData()
        {
            _lvUsers.Items.Clear();
            foreach (var u in _vmoService.GetUsers())
            {
                var item = new ListViewItem(u.Username);
                item.SubItems.Add(u.DisplayName);
                item.SubItems.Add(u.Role);
                item.SubItems.Add(u.AuthType);
                item.Tag = u;
                _lvUsers.Items.Add(item);
            }

            _lvRoles.Items.Clear();
            foreach (var r in _vmoService.GetRoles())
            {
                var item = new ListViewItem(r.RoleName);
                item.SubItems.Add(r.Description + " [" + string.Join(", ", r.Permissions.ToArray()) + "]");
                item.Tag = r;
                _lvRoles.Items.Add(item);
            }
        }

        private void PromptAddUser()
        {
            var form = new Form
            {
                Text = "👤 Add Basic User",
                Size = new Size(380, 260),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Font = new Font("Segoe UI", 9F)
            };

            int y = 14;
            form.Controls.Add(new Label { Text = "Username:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtUser = new TextBox { Location = new Point(14, y + 20), Size = new Size(335, 24) };
            form.Controls.Add(txtUser);
            y += 50;

            form.Controls.Add(new Label { Text = "Full Display Name:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtDisplay = new TextBox { Location = new Point(14, y + 20), Size = new Size(335, 24) };
            form.Controls.Add(txtDisplay);
            y += 50;

            form.Controls.Add(new Label { Text = "Assign Security Role:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var cbRole = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(14, y + 20), Size = new Size(335, 24) };
            cbRole.Items.AddRange(new object[] { "Administrators", "Operators", "Auditors / View Only" });
            cbRole.SelectedIndex = 1;
            form.Controls.Add(cbRole);
            y += 55;

            var btnOk = new Button { Text = "💾 Create User", Location = new Point(140, y), Size = new Size(110, 30), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(260, y), Size = new Size(88, 30), FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
                {
                    _vmoService.AddUser(txtUser.Text.Trim(), txtDisplay.Text.Trim(), cbRole.SelectedItem.ToString(), "Basic User");
                    RefreshSecurityData();
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
