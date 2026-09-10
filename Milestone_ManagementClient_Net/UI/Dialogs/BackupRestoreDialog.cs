using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Dialogs
{
    public class BackupRestoreDialog : Form
    {
        private readonly BackupRestoreService _backupService;
        private TextBox _txtBackupPath;
        private TextBox _txtRestorePath;
        private Label _lblStatus;

        public BackupRestoreDialog()
        {
            _backupService = new BackupRestoreService();

            this.Text = "Milestone VMS — Configuration Backup & Restore";
            this.Size = new Size(560, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = Color.FromArgb(248, 250, 252);

            BuildUI();
        }

        private void BuildUI()
        {
            var tab = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

            // Backup Tab
            var tabBackup = new TabPage("💾 Backup Configuration");
            tabBackup.BackColor = Color.White;
            tabBackup.Padding = new Padding(16);

            var lblB = new Label { Text = "Export complete VMS configuration, camera drivers, rules, and user roles.", Location = new Point(16, 16), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            var lblDest = new Label { Text = "Backup File Destination:", Location = new Point(16, 50), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            string defaultBackup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".milestonebackup");
            _txtBackupPath = new TextBox { Location = new Point(16, 75), Size = new Size(500, 24), Text = defaultBackup };

            var btnDoBackup = new Button { Text = "💾 Start Backup Process", Location = new Point(16, 120), Size = new Size(180, 32), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnDoBackup.FlatAppearance.BorderSize = 0;
            btnDoBackup.Click += (s, e) =>
            {
                string msg;
                if (_backupService.CreateConfigurationBackup(_txtBackupPath.Text.Trim(), out msg))
                {
                    _lblStatus.Text = "✔ " + msg;
                    MessageBox.Show(msg, "Backup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _lblStatus.Text = "❌ " + msg;
                    MessageBox.Show(msg, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            tabBackup.Controls.AddRange(new Control[] { lblB, lblDest, _txtBackupPath, btnDoBackup });
            tab.TabPages.Add(tabBackup);

            // Restore Tab
            var tabRestore = new TabPage("🔄 Restore Configuration");
            tabRestore.BackColor = Color.White;
            tabRestore.Padding = new Padding(16);

            var lblR = new Label { Text = "Restore surveillance cluster database from an existing .milestonebackup package.", Location = new Point(16, 16), AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            var lblSrc = new Label { Text = "Backup Source File:", Location = new Point(16, 50), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _txtRestorePath = new TextBox { Location = new Point(16, 75), Size = new Size(500, 24), Text = defaultBackup };

            var btnDoRestore = new Button { Text = "🔄 Restore & Synchronize", Location = new Point(16, 120), Size = new Size(190, 32), BackColor = Color.FromArgb(239, 68, 68), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnDoRestore.FlatAppearance.BorderSize = 0;
            btnDoRestore.Click += (s, e) =>
            {
                string msg;
                if (_backupService.RestoreConfigurationBackup(_txtRestorePath.Text.Trim(), out msg))
                {
                    _lblStatus.Text = "✔ " + msg;
                    MessageBox.Show(msg, "Restore Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _lblStatus.Text = "❌ " + msg;
                    MessageBox.Show(msg, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            tabRestore.Controls.AddRange(new Control[] { lblR, lblSrc, _txtRestorePath, btnDoRestore });
            tab.TabPages.Add(tabRestore);

            this.Controls.Add(tab);

            _lblStatus = new Label { Dock = DockStyle.Bottom, Height = 28, BackColor = Color.FromArgb(15, 23, 42), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(12, 0, 0, 0), Text = "Ready." };
            this.Controls.Add(_lblStatus);
        }
    }
}
