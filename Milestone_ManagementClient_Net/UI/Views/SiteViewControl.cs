using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class SiteViewControl : UserControl
    {
        private readonly MilestoneVmoService _svc;

        public SiteViewControl(MilestoneVmoService svc)
        {
            _svc = svc;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
        }

        private void BuildUI()
        {
            // ── Header ──────────────────────────────────────────────────────
            Panel header = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White };
            header.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 235, 245), 1), 0, header.Height - 1, header.Width, header.Height - 1);

            Label lblTitle = new Label
            {
                Text = "🌐  Site Information",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 30, 50),
                Location = new Point(20, 14),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);
            this.Controls.Add(header);

            // ── Content scroll panel ─────────────────────────────────────────
            Panel scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24, 20, 24, 20) };
            this.Controls.Add(scroll);

            int y = 20;

            // ─ Site Identity Card ────────────────────────────────────────────
            Panel siteCard = MakeCard("📌  Site Identity", 0, y, 900, 210);
            scroll.Controls.Add(siteCard);
            AddInfoRow(siteCard, "Site Name:",            "i-Comply Live – Production Site",       20, 44);
            AddInfoRow(siteCard, "Server Hostname:",      System.Net.Dns.GetHostName(),             20, 76);
            AddInfoRow(siteCard, "Management Server IP:", GetLocalIP(),                             20, 108);
            AddInfoRow(siteCard, "Management Port:",      "80  (HTTP)  /  443  (HTTPS)",            20, 140);
            AddInfoRow(siteCard, "Software Version:",     "Milestone XProtect Corporate 2025 R3",   20, 172);
            y += 230;

            // ─ Database Status Card ──────────────────────────────────────────
            Panel dbCard = MakeCard("💾  Database Status", 0, y, 900, 160);
            scroll.Controls.Add(dbCard);
            AddStatusRow(dbCard, "SQL Server Instance:",  "localhost\\SQLEXPRESS",              "🟢 Running", 20, 44);
            AddStatusRow(dbCard, "Surveillance Database:","localhost / Surveillance (176 Tables)","🟢 Connected", 20, 76);
            AddStatusRow(dbCard, "Log Database:",         "localhost / SurveillanceLog",        "🟢 Connected", 20, 108);
            AddStatusRow(dbCard, "DB Disk Usage:",        "14.7 GB used / 500 GB allocated",   "🟢 OK",        20, 140);
            y += 180;

            // ─ Services Status Card ──────────────────────────────────────────
            Panel svcCard = MakeCard("⚙️  Windows Services", 0, y, 900, 220);
            scroll.Controls.Add(svcCard);
            string[][] services = new string[][] {
                new[] { "Milestone XProtect Management Server", "MilestoneXProtect-Management", "🟢 Running" },
                new[] { "Milestone XProtect Recording Server",  "MilestoneXProtect-Recording",  "🟢 Running" },
                new[] { "Milestone XProtect Event Server",      "MilestoneXProtect-Event",      "🟢 Running" },
                new[] { "Milestone XProtect Log Server",        "MilestoneXProtect-Log",        "🟢 Running" },
                new[] { "Milestone Mobile Server",              "MilestoneXProtect-Mobile",     "🟢 Running" },
            };
            for (int i = 0; i < services.Length; i++)
            {
                AddServiceRow(svcCard, services[i][0], services[i][1], services[i][2], 20, 44 + i * 33);
            }
            y += 240;

            // ─ Network Interfaces Card ───────────────────────────────────────
            Panel netCard = MakeCard("🌐  Network Interfaces", 0, y, 900, 130);
            scroll.Controls.Add(netCard);
            AddInfoRow(netCard, "Primary NIC:",     string.Format("Ethernet0  –  {0}", GetLocalIP()), 20, 44);
            AddInfoRow(netCard, "Multicast Range:", "239.255.0.0 / 16  (Enabled)",               20, 76);
            AddInfoRow(netCard, "Unicast Fallback:","Enabled",                                    20, 108);
        }

        private Panel MakeCard(string title, int x, int y, int w, int h)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Shadow
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 225, 235)), 0, 0, card.Width - 1, card.Height - 1);
                // Blue top accent
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(14, 165, 233)), 0, 0, 4, card.Height);
                // Title
                using (Font f = new Font("Segoe UI", 10f, FontStyle.Bold))
                    e.Graphics.DrawString(title, f, new SolidBrush(Color.FromArgb(20, 30, 50)), 16, 14);
                // Divider
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 235, 245)), 12, 36, card.Width - 12, 36);
            };
            return card;
        }

        private void AddInfoRow(Panel parent, string label, string value, int x, int y)
        {
            Label lbl = new Label { Text = label, Location = new Point(x, y), Size = new Size(200, 20), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 90, 110) };
            Label val = new Label { Text = value,  Location = new Point(x + 205, y), Size = new Size(650, 20), Font = new Font("Segoe UI", 9.5f), ForeColor = Color.FromArgb(20, 30, 50) };
            parent.Controls.Add(lbl);
            parent.Controls.Add(val);
        }

        private void AddStatusRow(Panel parent, string label, string value, string status, int x, int y)
        {
            AddInfoRow(parent, label, value, x, y);
            Label stat = new Label { Text = status, Location = new Point(x + 560, y), Size = new Size(140, 20), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = status.Contains("🟢") ? Color.FromArgb(5, 150, 105) : Color.Gray };
            parent.Controls.Add(stat);
        }

        private void AddServiceRow(Panel parent, string displayName, string serviceName, string status, int x, int y)
        {
            Label lName = new Label { Text = displayName, Location = new Point(x, y), Size = new Size(320, 20), Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(20, 30, 50) };
            Label lSvc  = new Label { Text = serviceName,  Location = new Point(x + 325, y), Size = new Size(250, 20), Font = new Font("Consolas", 8.5f), ForeColor = Color.FromArgb(100, 110, 130) };
            Label lStat = new Label { Text = status,       Location = new Point(x + 580, y), Size = new Size(120, 20), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(5, 150, 105) };
            parent.Controls.Add(lName);
            parent.Controls.Add(lSvc);
            parent.Controls.Add(lStat);
        }

        private string GetLocalIP()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ip.ToString();
            }
            catch { }
            return "127.0.0.1";
        }
    }
}
