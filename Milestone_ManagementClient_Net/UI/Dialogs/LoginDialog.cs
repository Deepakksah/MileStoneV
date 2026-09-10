using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Windows.Forms;

namespace MilestoneManagementClient.UI.Dialogs
{
    public enum AuthType
    {
        WindowsDefault,
        Windows,
        Simple
    }

    public class LoginDialog : Form
    {
        private Panel _topBanner;
        private PictureBox _pbLogo;
        private Label _lblProduct;
        private Label _lblTagline;

        private Label _lblServerAddr;
        private ComboBox _cboServerAddr;
        private Label _lblAuthType;
        private ComboBox _cboAuthType;
        private Label _lblUsername;
        private TextBox _txtUsername;
        private Label _lblPassword;
        private TextBox _txtPassword;
        private CheckBox _chkRemember;
        private CheckBox _chkAutoLogin;

        private Button _btnConnect;
        private Button _btnCancel;
        private Panel _statusBar;
        private Label _lblStatus;

        public string ServerAddress { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public AuthType AuthenticationMode { get; private set; }
        public bool RememberPassword { get; private set; }
        public bool AutoLogin { get; private set; }

        public LoginDialog()
        {
            this.Text = "Milestone XProtect Management Client – Login";
            this.Size = new Size(500, 540);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = true;
            this.TopMost = true;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.5f);

            string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lychee.ico");
            if (!File.Exists(icoPath)) icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (File.Exists(icoPath)) try { this.Icon = new Icon(icoPath); } catch { }

            BuildUI();
            PopulateDefaults();

            this.Shown += (s, e) =>
            {
                this.TopMost = false;
                this.BringToFront();
                this.Activate();
            };
        }

        private void BuildUI()
        {
            _topBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = Color.FromArgb(16, 24, 40)
            };
            _topBanner.Paint += TopBanner_Paint;

            _pbLogo = new PictureBox
            {
                Size = new Size(44, 44),
                Location = new Point(24, 33),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lychee.ico");
            if (!File.Exists(icoPath)) icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (File.Exists(icoPath)) try { _pbLogo.Image = new Icon(icoPath, 44, 44).ToBitmap(); } catch { }
            _topBanner.Controls.Add(_pbLogo);

            _lblProduct = new Label
            {
                Text = "Milestone XProtect®",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(78, 30),
                BackColor = Color.Transparent
            };
            _topBanner.Controls.Add(_lblProduct);

            _lblTagline = new Label
            {
                Text = "Management Client  2025 R3",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(180, 190, 210),
                AutoSize = true,
                Location = new Point(80, 58),
                BackColor = Color.Transparent
            };
            _topBanner.Controls.Add(_lblTagline);
            this.Controls.Add(_topBanner);

            int lx = 34, fx = 34, fw = 418, lH = 18, fH = 28;
            int y = 135;

            _lblServerAddr = MakeLabel("Management Server Address:", lx, y); y += lH + 2;
            _cboServerAddr = new ComboBox { Location = new Point(fx, y), Width = fw, Height = fH, DropDownStyle = ComboBoxStyle.DropDown };
            ApplyFieldStyle(_cboServerAddr);
            this.Controls.Add(_lblServerAddr);
            this.Controls.Add(_cboServerAddr);
            y += fH + 14;

            _lblAuthType = MakeLabel("Authentication:", lx, y); y += lH + 2;
            _cboAuthType = new ComboBox { Location = new Point(fx, y), Width = fw, Height = fH, DropDownStyle = ComboBoxStyle.DropDownList };
            _cboAuthType.Items.AddRange(new object[] { "Windows Authentication (Current User)", "Windows User (Specify)", "Basic Authentication (Username/Password)" });
            ApplyFieldStyle(_cboAuthType);
            _cboAuthType.SelectedIndexChanged += AuthType_Changed;
            this.Controls.Add(_lblAuthType);
            this.Controls.Add(_cboAuthType);
            y += fH + 14;

            _lblUsername = MakeLabel("Username:", lx, y); y += lH + 2;
            _txtUsername = new TextBox { Location = new Point(fx, y), Width = fw, Height = fH };
            ApplyFieldStyle(_txtUsername);
            this.Controls.Add(_lblUsername);
            this.Controls.Add(_txtUsername);
            y += fH + 14;

            _lblPassword = MakeLabel("Password:", lx, y); y += lH + 2;
            _txtPassword = new TextBox { Location = new Point(fx, y), Width = fw, Height = fH, PasswordChar = '●' };
            ApplyFieldStyle(_txtPassword);
            _txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoConnect(); };
            this.Controls.Add(_lblPassword);
            this.Controls.Add(_txtPassword);
            y += fH + 14;

            _chkRemember = new CheckBox
            {
                Text = "Remember Password",
                Location = new Point(fx, y), AutoSize = true,
                ForeColor = Color.FromArgb(50, 60, 80)
            };
            _chkAutoLogin = new CheckBox
            {
                Text = "Auto Login",
                Location = new Point(fx + 180, y), AutoSize = true,
                ForeColor = Color.FromArgb(50, 60, 80)
            };
            this.Controls.Add(_chkRemember);
            this.Controls.Add(_chkAutoLogin);
            y += 30;

            Panel div = new Panel { Location = new Point(0, y), Width = 500, Height = 1, BackColor = Color.FromArgb(220, 225, 235) };
            this.Controls.Add(div);
            y += 16;

            _btnConnect = new Button
            {
                Text = "Connect",
                Size = new Size(130, 36),
                Location = new Point(fw - 96, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(14, 165, 233),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnConnect.FlatAppearance.BorderSize = 0;
            _btnConnect.Click += (s, e) => DoConnect();
            _btnConnect.MouseEnter += (s, e) => _btnConnect.BackColor = Color.FromArgb(2, 132, 199);
            _btnConnect.MouseLeave += (s, e) => _btnConnect.BackColor = Color.FromArgb(14, 165, 233);

            _btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(100, 36),
                Location = new Point(fw - 96 - 116, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(230, 234, 240),
                ForeColor = Color.FromArgb(50, 60, 80),
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            _btnCancel.FlatAppearance.BorderSize = 0;
            _btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            _btnCancel.MouseEnter += (s, e) => _btnCancel.BackColor = Color.FromArgb(210, 215, 225);
            _btnCancel.MouseLeave += (s, e) => _btnCancel.BackColor = Color.FromArgb(230, 234, 240);

            this.Controls.Add(_btnConnect);
            this.Controls.Add(_btnCancel);
            y += 50;

            _statusBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = Color.FromArgb(240, 243, 248)
            };
            _lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(100, 110, 130),
                Font = new Font("Segoe UI", 8.5f),
                Padding = new Padding(10, 0, 0, 0)
            };
            _statusBar.Controls.Add(_lblStatus);
            this.Controls.Add(_statusBar);

            this.AcceptButton = _btnConnect;
            this.CancelButton = _btnCancel;
        }

        private void TopBanner_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush b = new LinearGradientBrush(
                _topBanner.ClientRectangle,
                Color.FromArgb(16, 24, 40),
                Color.FromArgb(25, 40, 65),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(b, _topBanner.ClientRectangle);
            }
            using (SolidBrush accent = new SolidBrush(Color.FromArgb(14, 165, 233)))
            {
                e.Graphics.FillRectangle(accent, 0, _topBanner.Height - 3, _topBanner.Width, 3);
            }
        }

        private Label MakeLabel(string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 90),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            this.Controls.Add(lbl);
            return lbl;
        }

        private void ApplyFieldStyle(Control c)
        {
            c.BackColor = Color.White;
            c.ForeColor = Color.FromArgb(20, 30, 50);
            c.Font = new Font("Segoe UI", 9.5f);
        }

        private void PopulateDefaults()
        {
            string hostname;
            try { hostname = Dns.GetHostEntry("localhost").HostName; }
            catch { hostname = Environment.MachineName; }

            _cboServerAddr.Items.Add("http://" + hostname);
            _cboServerAddr.Items.Add("http://localhost");
            _cboServerAddr.SelectedIndex = 0;

            _cboAuthType.SelectedIndex = 0;

            try { _txtUsername.Text = WindowsIdentity.GetCurrent().Name; }
            catch { }

            _chkRemember.Checked = true;
            _chkAutoLogin.Checked = false;

            UpdateFieldVisibility(AuthType.WindowsDefault);
            SetStatus("Enter Management Server address and credentials to connect.");
        }

        private void AuthType_Changed(object sender, EventArgs e)
        {
            switch (_cboAuthType.SelectedIndex)
            {
                case 0: UpdateFieldVisibility(AuthType.WindowsDefault); break;
                case 1: UpdateFieldVisibility(AuthType.Windows); break;
                case 2: UpdateFieldVisibility(AuthType.Simple); break;
            }
        }

        private void UpdateFieldVisibility(AuthType mode)
        {
            bool isWindowsDefault = (mode == AuthType.WindowsDefault);
            _lblUsername.Enabled = !isWindowsDefault;
            _txtUsername.Enabled = !isWindowsDefault;
            _lblPassword.Enabled = !isWindowsDefault;
            _txtPassword.Enabled = !isWindowsDefault;
            _chkRemember.Enabled = !isWindowsDefault;

            if (isWindowsDefault)
            {
                try { _txtUsername.Text = WindowsIdentity.GetCurrent().Name; }
                catch { _txtUsername.Text = string.Empty; }
                _txtPassword.Text = string.Empty;
                _txtUsername.ForeColor = Color.Gray;
                _txtPassword.ForeColor = Color.Gray;
            }
            else
            {
                _txtUsername.ForeColor = Color.FromArgb(20, 30, 50);
                _txtPassword.ForeColor = Color.FromArgb(20, 30, 50);
            }
        }

        private void DoConnect()
        {
            if (string.IsNullOrWhiteSpace(_cboServerAddr.Text))
            {
                SetStatus("⚠  Please specify the Management Server address.", true);
                _cboServerAddr.Focus();
                return;
            }

            AuthType mode = (AuthType)_cboAuthType.SelectedIndex;
            if (mode != AuthType.WindowsDefault)
            {
                if (string.IsNullOrWhiteSpace(_txtUsername.Text))
                {
                    SetStatus("⚠  Username is required for this authentication type.", true);
                    _txtUsername.Focus();
                    return;
                }
            }

            ServerAddress = _cboServerAddr.Text.Trim();
            AuthenticationMode = mode;
            Username = _txtUsername.Text.Trim();
            Password = _txtPassword.Text;
            RememberPassword = _chkRemember.Checked;
            AutoLogin = _chkAutoLogin.Checked;

            SetStatus("Connecting to " + ServerAddress + " …");
            _btnConnect.Enabled = false;

            System.Threading.Timer t = null;
            t = new System.Threading.Timer(state =>
            {
                t.Dispose();
                this.Invoke(new Action(() =>
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }));
            }, null, 800, System.Threading.Timeout.Infinite);
        }

        private void SetStatus(string msg, bool isError = false)
        {
            _lblStatus.Text = "  " + msg;
            _lblStatus.ForeColor = isError ? Color.FromArgb(200, 50, 50) : Color.FromArgb(100, 110, 130);
        }
    }
}
