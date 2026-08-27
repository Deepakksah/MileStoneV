using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class SecurityViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;

        // Top Navigation
        private Button _btnTabRoles;
        private Button _btnTabUsers;
        private Panel _pnlRolesContainer;
        private Panel _pnlUsersContainer;

        // --- Roles Controls ---
        private TreeView _treeRoles;
        private TextBox _txtRoleName;
        private TextBox _txtRoleDesc;
        private CheckedListBox _clbOverallRights;
        private ListView _lvRoleUsers;
        private ComboBox _cbTimeProfile;
        private Button _btnSaveRole;
        private RoleModel _selectedRole;

        // --- Users Controls ---
        private ListView _lvUsers;
        private TextBox _txtUserName;
        private TextBox _txtUserDisplayName;
        private ComboBox _cbUserRole;
        private ComboBox _cbAuthType;
        private CheckBox _chkUserEnabled;
        private Label _lblLastLogin;
        private Button _btnSaveUser;
        private UserModel _selectedUser;

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
            // 1. Top Section Header with Subtab Navigation
            var pnlTopNav = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(8, 6, 8, 4) };

            _btnTabRoles = new Button
            {
                Text = "🛡️ Security Roles & Permissions",
                Location = new Point(8, 6),
                Size = new Size(210, 28),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(190, 18, 60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnTabRoles.FlatAppearance.BorderColor = Color.FromArgb(190, 18, 60);
            _btnTabRoles.Click += (s, e) => SwitchSecurityTab(true);
            pnlTopNav.Controls.Add(_btnTabRoles);

            _btnTabUsers = new Button
            {
                Text = "👤 Basic & Windows Users",
                Location = new Point(226, 6),
                Size = new Size(180, 28),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            _btnTabUsers.FlatAppearance.BorderSize = 0;
            _btnTabUsers.Click += (s, e) => SwitchSecurityTab(false);
            pnlTopNav.Controls.Add(_btnTabUsers);

            this.Controls.Add(pnlTopNav);

            // 2. Main Content Area
            var pnlMainContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // Create Roles Container
            _pnlRolesContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            BuildRolesUI(_pnlRolesContainer);
            pnlMainContent.Controls.Add(_pnlRolesContainer);

            // Create Users Container
            _pnlUsersContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Visible = false };
            BuildUsersUI(_pnlUsersContainer);
            pnlMainContent.Controls.Add(_pnlUsersContainer);

            this.Controls.Add(pnlMainContent);
            pnlMainContent.BringToFront();
        }

        private void SwitchSecurityTab(bool showRoles)
        {
            _pnlRolesContainer.Visible = showRoles;
            _pnlUsersContainer.Visible = !showRoles;

            if (showRoles)
            {
                _btnTabRoles.BackColor = Color.White;
                _btnTabRoles.ForeColor = Color.FromArgb(190, 18, 60);
                _btnTabRoles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                _btnTabRoles.FlatAppearance.BorderSize = 1;
                _btnTabRoles.FlatAppearance.BorderColor = Color.FromArgb(190, 18, 60);

                _btnTabUsers.BackColor = Color.FromArgb(241, 245, 249);
                _btnTabUsers.ForeColor = Color.FromArgb(71, 85, 105);
                _btnTabUsers.Font = new Font("Segoe UI", 9F);
                _btnTabUsers.FlatAppearance.BorderSize = 0;
            }
            else
            {
                _btnTabUsers.BackColor = Color.White;
                _btnTabUsers.ForeColor = Color.FromArgb(190, 18, 60);
                _btnTabUsers.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                _btnTabUsers.FlatAppearance.BorderSize = 1;
                _btnTabUsers.FlatAppearance.BorderColor = Color.FromArgb(190, 18, 60);

                _btnTabRoles.BackColor = Color.FromArgb(241, 245, 249);
                _btnTabRoles.ForeColor = Color.FromArgb(71, 85, 105);
                _btnTabRoles.Font = new Font("Segoe UI", 9F);
                _btnTabRoles.FlatAppearance.BorderSize = 0;
            }
        }

        // ====================================================
        // ROLES MANAGEMENT VIEW
        // ====================================================
        private void BuildRolesUI(Panel container)
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
            container.Controls.Add(split);
            try { if (split.Width > 500) split.SplitterDistance = 320; } catch { }

            // Left Roles Tree
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = Color.FromArgb(255, 241, 242), Padding = new Padding(8, 4, 8, 4) };
            pnlHeader.Controls.Add(new Label { Text = "Security Roles", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 3, 0, 0) });

            var btnAddRole = new Button { Text = "➕ Add Role...", Dock = DockStyle.Right, Size = new Size(95, 24), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAddRole.FlatAppearance.BorderSize = 0;
            btnAddRole.Click += (s, e) => PromptAddRole();
            pnlHeader.Controls.Add(btnAddRole);

            var btnDelRole = new Button { Text = "🗑 Delete", Dock = DockStyle.Right, Size = new Size(65, 24), BackColor = Color.White, ForeColor = Color.Red, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8.5F), Cursor = Cursors.Hand };
            btnDelRole.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnDelRole.Click += (s, e) => DeleteSelectedRole();
            pnlHeader.Controls.Add(btnDelRole);
            pnlLeft.Controls.Add(pnlHeader);

            _treeRoles = new TreeView
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
            _treeRoles.AfterSelect += (s, e) =>
            {
                if (e.Node != null && e.Node.Tag is RoleModel) DisplayRoleDetails((RoleModel)e.Node.Tag);
            };
            pnlLeft.Controls.Add(_treeRoles);
            _treeRoles.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // Right Roles Properties Pane
            var pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(12) };

            // Subtabs Header
            var pnlSubTabs = new Panel { Dock = DockStyle.Top, Height = 28, BackColor = Color.FromArgb(241, 245, 249), Padding = new Padding(2) };
            var tabBtns = new[] { "Info", "Users", "Overall Security", "Device Permissions", "Time Profile" };
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
                    BackColor = (t == "Info" || t == "Overall Security") ? Color.White : Color.FromArgb(241, 245, 249),
                    ForeColor = (t == "Info" || t == "Overall Security") ? Color.FromArgb(190, 18, 60) : Color.FromArgb(71, 85, 105),
                    Font = new Font("Segoe UI", 8.5F, (t == "Info") ? FontStyle.Bold : FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                b.FlatAppearance.BorderSize = 0;
                pnlSubTabs.Controls.Add(b);
                tx += b.PreferredSize.Width + 4;
            }
            pnlRight.Controls.Add(pnlSubTabs);

            // 1. Role Info Group
            var gbInfo = new GroupBox { Text = "Role Information", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 140, Padding = new Padding(10) };
            gbInfo.Controls.Add(new Label { Text = "Role Name:", Location = new Point(12, 24), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _txtRoleName = new TextBox { Location = new Point(120, 22), Size = new Size(360, 24), Font = new Font("Segoe UI", 9F) };
            _txtRoleName.TextChanged += (s, e) => { if (_selectedRole != null) _btnSaveRole.Enabled = true; };
            gbInfo.Controls.Add(_txtRoleName);

            gbInfo.Controls.Add(new Label { Text = "Description:", Location = new Point(12, 56), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _txtRoleDesc = new TextBox { Location = new Point(120, 54), Size = new Size(360, 48), Multiline = true, Font = new Font("Segoe UI", 9F) };
            _txtRoleDesc.TextChanged += (s, e) => { if (_selectedRole != null) _btnSaveRole.Enabled = true; };
            gbInfo.Controls.Add(_txtRoleDesc);
            pnlRight.Controls.Add(gbInfo);
            gbInfo.BringToFront();

            // 2. Overall Security Permissions
            var gbRights = new GroupBox { Text = "Overall Security System Rights", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 180, Padding = new Padding(10) };
            _clbOverallRights = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                ThreeDCheckBoxes = false
            };
            _clbOverallRights.Items.AddRange(new object[] {
                "Full Control & Management Server Administration",
                "Live Video Stream Viewing",
                "Playback & Investigation Archive Search",
                "Video Export & AVI / MKV Generation",
                "PTZ Priority Control & Preset Navigation",
                "Audio Listen & Microphone Broadcast",
                "Manual Recording Trigger & Bookmark Creation",
                "Alarms & Analytics Events Acknowledgement",
                "Access Control Door Unlock & Lock Override"
            });
            _clbOverallRights.ItemCheck += (s, e) => { if (_selectedRole != null) _btnSaveRole.Enabled = true; };
            gbRights.Controls.Add(_clbOverallRights);
            pnlRight.Controls.Add(gbRights);
            gbRights.BringToFront();

            // 3. Assigned Users in Role
            var gbUsers = new GroupBox { Text = "Users Assigned to this Role", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 140, Padding = new Padding(10) };
            _lvRoleUsers = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, GridLines = true, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 8.5F) };
            _lvRoleUsers.Columns.Add("Username", 120);
            _lvRoleUsers.Columns.Add("Full Name", 180);
            _lvRoleUsers.Columns.Add("Auth Type", 110);
            gbUsers.Controls.Add(_lvRoleUsers);
            pnlRight.Controls.Add(gbUsers);
            gbUsers.BringToFront();

            // 4. Time Profile
            var gbTz = new GroupBox { Text = "Time Profile Schedule", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 65, Padding = new Padding(10) };
            gbTz.Controls.Add(new Label { Text = "Active Schedule:", Location = new Point(12, 26), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _cbTimeProfile = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(130, 24), Size = new Size(350, 24) };
            _cbTimeProfile.Items.AddRange(new object[] { "Always (24x7 Unrestricted Access)", "Day Shift (08:00 - 20:00)", "Night Shift (20:00 - 08:00)", "Weekend Operations Only" });
            _cbTimeProfile.SelectedIndex = 0;
            _cbTimeProfile.SelectedIndexChanged += (s, e) => { if (_selectedRole != null) _btnSaveRole.Enabled = true; };
            gbTz.Controls.Add(_cbTimeProfile);
            pnlRight.Controls.Add(gbTz);
            gbTz.BringToFront();

            // 5. Action Buttons
            var pnlActions = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(0, 10, 0, 0) };
            _btnSaveRole = new Button
            {
                Text = "💾 Save Role Permissions",
                Location = new Point(0, 10),
                Size = new Size(180, 32),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            _btnSaveRole.FlatAppearance.BorderSize = 0;
            _btnSaveRole.Click += (s, e) => SaveCurrentRole();
            pnlActions.Controls.Add(_btnSaveRole);
            pnlRight.Controls.Add(pnlActions);
            pnlActions.BringToFront();

            split.Panel2.Controls.Add(pnlRight);
        }

        // ====================================================
        // BASIC USERS MANAGEMENT VIEW
        // ====================================================
        private void BuildUsersUI(Panel container)
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
            container.Controls.Add(split);
            try { if (split.Width > 500) split.SplitterDistance = 380; } catch { }

            // Left Users List
            var pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = Color.FromArgb(255, 241, 242), Padding = new Padding(8, 4, 8, 4) };
            pnlHeader.Controls.Add(new Label { Text = "Basic & Windows Users", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(190, 18, 60), Dock = DockStyle.Left, AutoSize = true, Padding = new Padding(0, 3, 0, 0) });

            var btnAddUser = new Button { Text = "➕ Add User...", Dock = DockStyle.Right, Size = new Size(95, 24), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.Click += (s, e) => PromptAddUser();
            pnlHeader.Controls.Add(btnAddUser);

            var btnDelUser = new Button { Text = "🗑 Delete", Dock = DockStyle.Right, Size = new Size(65, 24), BackColor = Color.White, ForeColor = Color.Red, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8.5F), Cursor = Cursors.Hand };
            btnDelUser.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnDelUser.Click += (s, e) => DeleteSelectedUser();
            pnlHeader.Controls.Add(btnDelUser);
            pnlLeft.Controls.Add(pnlHeader);

            _lvUsers = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                HideSelection = false
            };
            _lvUsers.Columns.Add("Username", 110);
            _lvUsers.Columns.Add("Display Name", 140);
            _lvUsers.Columns.Add("Role", 110);
            _lvUsers.Columns.Add("Status", 75);

            _lvUsers.SelectedIndexChanged += (s, e) =>
            {
                if (_lvUsers.SelectedItems.Count > 0 && _lvUsers.SelectedItems[0].Tag is UserModel)
                {
                    DisplayUserDetails((UserModel)_lvUsers.SelectedItems[0].Tag);
                }
            };
            pnlLeft.Controls.Add(_lvUsers);
            _lvUsers.BringToFront();
            split.Panel1.Controls.Add(pnlLeft);

            // Right User Properties Pane
            var pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true, Padding = new Padding(14) };

            var lblUserTitle = new Label { Text = "User Account & Security Profile", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Dock = DockStyle.Top, Height = 28 };
            pnlRight.Controls.Add(lblUserTitle);

            var gbUserProps = new GroupBox { Text = "Account Credentials & Permissions", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Dock = DockStyle.Top, Height = 250, Padding = new Padding(12) };
            int uy = 24;

            gbUserProps.Controls.Add(new Label { Text = "Username:", Location = new Point(12, uy), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _txtUserName = new TextBox { Location = new Point(140, uy - 2), Size = new Size(320, 24), ReadOnly = true, BackColor = Color.FromArgb(248, 250, 252) };
            gbUserProps.Controls.Add(_txtUserName);
            uy += 32;

            gbUserProps.Controls.Add(new Label { Text = "Display Name:", Location = new Point(12, uy), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _txtUserDisplayName = new TextBox { Location = new Point(140, uy - 2), Size = new Size(320, 24) };
            _txtUserDisplayName.TextChanged += (s, e) => { if (_selectedUser != null) _btnSaveUser.Enabled = true; };
            gbUserProps.Controls.Add(_txtUserDisplayName);
            uy += 32;

            gbUserProps.Controls.Add(new Label { Text = "Assigned Role:", Location = new Point(12, uy), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            _cbUserRole = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(140, uy - 2), Size = new Size(320, 24) };
            _cbUserRole.SelectedIndexChanged += (s, e) => { if (_selectedUser != null) _btnSaveUser.Enabled = true; };
            gbUserProps.Controls.Add(_cbUserRole);
            uy += 32;

            gbUserProps.Controls.Add(new Label { Text = "Authentication Type:", Location = new Point(12, uy), AutoSize = true, Font = new Font("Segoe UI", 8.5F) });
            _cbAuthType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(140, uy - 2), Size = new Size(320, 24) };
            _cbAuthType.Items.AddRange(new object[] { "Basic User", "Windows User (Active Directory)" });
            _cbAuthType.SelectedIndex = 0;
            _cbAuthType.SelectedIndexChanged += (s, e) => { if (_selectedUser != null) _btnSaveUser.Enabled = true; };
            gbUserProps.Controls.Add(_cbAuthType);
            uy += 32;

            _chkUserEnabled = new CheckBox { Text = "Account is Enabled & Active", Checked = true, Location = new Point(140, uy), AutoSize = true, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) };
            _chkUserEnabled.CheckedChanged += (s, e) => { if (_selectedUser != null) _btnSaveUser.Enabled = true; };
            gbUserProps.Controls.Add(_chkUserEnabled);
            uy += 30;

            _lblLastLogin = new Label { Text = "Last Login: Active session", Location = new Point(140, uy), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105), Font = new Font("Segoe UI", 8.5F) };
            gbUserProps.Controls.Add(_lblLastLogin);

            pnlRight.Controls.Add(gbUserProps);

            // User Actions Bar
            var pnlUserActions = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(0, 10, 0, 0) };

            _btnSaveUser = new Button
            {
                Text = "💾 Save User Changes",
                Location = new Point(0, 10),
                Size = new Size(160, 32),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            _btnSaveUser.FlatAppearance.BorderSize = 0;
            _btnSaveUser.Click += (s, e) => SaveCurrentUser();
            pnlUserActions.Controls.Add(_btnSaveUser);

            var btnChangePass = new Button
            {
                Text = "🔑 Change Password...",
                Location = new Point(170, 10),
                Size = new Size(160, 32),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChangePass.FlatAppearance.BorderSize = 0;
            btnChangePass.Click += (s, e) => PromptChangePassword();
            pnlUserActions.Controls.Add(btnChangePass);

            pnlRight.Controls.Add(pnlUserActions);
            split.Panel2.Controls.Add(pnlRight);
        }

        // ====================================================
        // DATA REFRESH & BINDING
        // ====================================================
        public void RefreshSecurityData()
        {
            // 1. Refresh Roles
            _treeRoles.Nodes.Clear();
            var rootRole = new TreeNode("🛡️ Security Roles");
            rootRole.NodeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            rootRole.ForeColor = Color.FromArgb(190, 18, 60);

            var roles = _vmoService.GetRoles();
            _cbUserRole.Items.Clear();

            foreach (var r in roles)
            {
                var rNode = new TreeNode("👥 " + r.RoleName) { Tag = r };
                rootRole.Nodes.Add(rNode);
                _cbUserRole.Items.Add(r.RoleName);
            }

            _treeRoles.Nodes.Add(rootRole);
            rootRole.Expand();
            if (rootRole.Nodes.Count > 0)
            {
                _treeRoles.SelectedNode = rootRole.Nodes[0];
                DisplayRoleDetails((RoleModel)rootRole.Nodes[0].Tag);
            }

            // 2. Refresh Users
            _lvUsers.Items.Clear();
            var users = _vmoService.GetUsers();
            foreach (var u in users)
            {
                var item = new ListViewItem(u.Username);
                item.SubItems.Add(u.DisplayName);
                item.SubItems.Add(u.Role);
                item.SubItems.Add(u.IsEnabled ? "● Active" : "Disabled");
                item.ForeColor = u.IsEnabled ? Color.FromArgb(15, 23, 42) : Color.FromArgb(148, 163, 184);
                item.Tag = u;
                _lvUsers.Items.Add(item);
            }
            if (_lvUsers.Items.Count > 0)
            {
                _lvUsers.Items[0].Selected = true;
                DisplayUserDetails((UserModel)_lvUsers.Items[0].Tag);
            }
        }

        private void DisplayRoleDetails(RoleModel r)
        {
            _selectedRole = r;
            _txtRoleName.Text = r.RoleName;
            _txtRoleDesc.Text = r.Description;

            // Check rights
            for (int i = 0; i < _clbOverallRights.Items.Count; i++)
            {
                string itemText = _clbOverallRights.Items[i].ToString();
                bool hasRight = r.RoleName == "Administrators" ||
                                (r.RoleName == "Operators" && !itemText.Contains("Administration") && !itemText.Contains("Access Control")) ||
                                (r.RoleName.Contains("Auditor") && (itemText.Contains("Viewing") || itemText.Contains("Investigation")));
                _clbOverallRights.SetItemChecked(i, hasRight);
            }

            // Populate assigned users
            _lvRoleUsers.Items.Clear();
            var usersInRole = _vmoService.GetUsers().Where(u => u.Role == r.RoleName);
            foreach (var u in usersInRole)
            {
                var itm = new ListViewItem(u.Username);
                itm.SubItems.Add(u.DisplayName);
                itm.SubItems.Add(u.AuthType);
                _lvRoleUsers.Items.Add(itm);
            }

            _btnSaveRole.Enabled = false;
        }

        private void DisplayUserDetails(UserModel u)
        {
            _selectedUser = u;
            _txtUserName.Text = u.Username;
            _txtUserDisplayName.Text = u.DisplayName;
            _cbUserRole.SelectedItem = u.Role;
            _cbAuthType.SelectedItem = u.AuthType;
            _chkUserEnabled.Checked = u.IsEnabled;
            _lblLastLogin.Text = "Last Login: " + u.LastLogin.ToString("dd MMM yyyy, HH:mm:ss");
            _btnSaveUser.Enabled = false;
        }

        private void SaveCurrentRole()
        {
            if (_selectedRole != null && !string.IsNullOrWhiteSpace(_txtRoleName.Text))
            {
                var perms = new List<string>();
                foreach (var checkedItem in _clbOverallRights.CheckedItems) perms.Add(checkedItem.ToString());
                _vmoService.UpdateRole(_selectedRole.RoleName, _txtRoleName.Text.Trim(), _txtRoleDesc.Text.Trim(), perms.ToArray());
                _btnSaveRole.Enabled = false;
                RefreshSecurityData();
                MessageBox.Show(string.Format("Role '{0}' permissions saved successfully.", _txtRoleName.Text.Trim()), "Role Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveCurrentUser()
        {
            if (_selectedUser != null)
            {
                _vmoService.UpdateUser(
                    _selectedUser.Username,
                    _txtUserDisplayName.Text.Trim(),
                    _cbUserRole.SelectedItem != null ? _cbUserRole.SelectedItem.ToString() : _selectedUser.Role,
                    _chkUserEnabled.Checked
                );
                _btnSaveUser.Enabled = false;
                RefreshSecurityData();
                MessageBox.Show(string.Format("User '{0}' profile updated successfully.", _selectedUser.Username), "User Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PromptAddRole()
        {
            var form = new Form { Text = "🛡️ Add Security Role", Size = new Size(440, 240), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, BackColor = Color.FromArgb(248, 250, 252) };
            form.Controls.Add(new Label { Text = "Role Name:", Location = new Point(14, 14), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtRName = new TextBox { Text = "Control Room Supervisors", Location = new Point(14, 36), Size = new Size(395, 24) };
            form.Controls.Add(txtRName);

            form.Controls.Add(new Label { Text = "Description:", Location = new Point(14, 70), AutoSize = true });
            var txtRDesc = new TextBox { Text = "Supervisory live access, evidence export, and PTZ priority", Location = new Point(14, 92), Size = new Size(395, 24) };
            form.Controls.Add(txtRDesc);

            var btnOk = new Button { Text = "💾 Create Role", Location = new Point(200, 140), Size = new Size(110, 30), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnOk.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtRName.Text))
                {
                    _vmoService.AddRole(txtRName.Text.Trim(), txtRDesc.Text.Trim(), new[] { "Live & Playback", "PTZ Control", "Export Video" });
                    RefreshSecurityData();
                    form.Close();
                }
            };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(320, 140), Size = new Size(86, 30), FlatStyle = FlatStyle.Flat };
            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }

        private void PromptAddUser()
        {
            var form = new Form { Text = "👤 Add Basic User", Size = new Size(440, 320), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, BackColor = Color.FromArgb(248, 250, 252) };
            int y = 14;

            form.Controls.Add(new Label { Text = "Username:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtU = new TextBox { Text = "patrol_officer", Location = new Point(14, y + 20), Size = new Size(395, 24) };
            form.Controls.Add(txtU);
            y += 50;

            form.Controls.Add(new Label { Text = "Full / Display Name:", Location = new Point(14, y), AutoSize = true });
            var txtD = new TextBox { Text = "Patrol Officer Unit 4", Location = new Point(14, y + 20), Size = new Size(395, 24) };
            form.Controls.Add(txtD);
            y += 50;

            form.Controls.Add(new Label { Text = "Security Role:", Location = new Point(14, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var cbR = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(14, y + 20), Size = new Size(395, 24) };
            foreach (var r in _vmoService.GetRoles()) cbR.Items.Add(r.RoleName);
            cbR.SelectedIndex = 0;
            form.Controls.Add(cbR);
            y += 54;

            form.Controls.Add(new Label { Text = "Initial Password:", Location = new Point(14, y), AutoSize = true });
            var txtP = new TextBox { Text = "Milestone2025!", UseSystemPasswordChar = true, Location = new Point(14, y + 20), Size = new Size(395, 24) };
            form.Controls.Add(txtP);
            y += 54;

            var btnOk = new Button { Text = "💾 Create User", Location = new Point(200, y), Size = new Size(110, 30), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnOk.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtU.Text))
                {
                    _vmoService.AddUser(txtU.Text.Trim(), txtD.Text.Trim(), cbR.SelectedItem.ToString(), "Basic User");
                    RefreshSecurityData();
                    form.Close();
                }
            };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(320, y), Size = new Size(86, 30), FlatStyle = FlatStyle.Flat };
            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }

        private void PromptChangePassword()
        {
            if (_selectedUser == null) return;
            var form = new Form { Text = "🔑 Change Password – " + _selectedUser.Username, Size = new Size(400, 220), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, BackColor = Color.FromArgb(248, 250, 252) };
            form.Controls.Add(new Label { Text = "New Password:", Location = new Point(14, 14), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtP1 = new TextBox { Location = new Point(14, 36), Size = new Size(350, 24), UseSystemPasswordChar = true };
            form.Controls.Add(txtP1);

            form.Controls.Add(new Label { Text = "Confirm New Password:", Location = new Point(14, 70), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            var txtP2 = new TextBox { Location = new Point(14, 92), Size = new Size(350, 24), UseSystemPasswordChar = true };
            form.Controls.Add(txtP2);

            var btnOk = new Button { Text = "Update Password", Location = new Point(140, 134), Size = new Size(130, 30), BackColor = Color.FromArgb(190, 18, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnOk.Click += (s, e) =>
            {
                if (txtP1.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtP1.Text != txtP2.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _vmoService.ChangeUserPassword(_selectedUser.Username, txtP1.Text);
                MessageBox.Show(string.Format("Password for user '{0}' updated successfully.", _selectedUser.Username), "Password Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                form.Close();
            };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(280, 134), Size = new Size(84, 30), FlatStyle = FlatStyle.Flat };
            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }

        private void DeleteSelectedRole()
        {
            if (_selectedRole != null)
            {
                if (_selectedRole.RoleName == "Administrators")
                {
                    MessageBox.Show("The built-in Administrators role cannot be deleted.", "Protected Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show(string.Format("Are you sure you want to delete role '{0}'?", _selectedRole.RoleName), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _vmoService.DeleteRole(_selectedRole.RoleName);
                    RefreshSecurityData();
                }
            }
        }

        private void DeleteSelectedUser()
        {
            if (_selectedUser != null)
            {
                if (_selectedUser.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("The primary Administrator account cannot be deleted.", "Protected User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show(string.Format("Are you sure you want to delete user '{0}'?", _selectedUser.Username), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _vmoService.DeleteUser(_selectedUser.Username);
                    RefreshSecurityData();
                }
            }
        }
    }
}
