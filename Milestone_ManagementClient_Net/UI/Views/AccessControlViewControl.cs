using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MilestoneManagementClient.Core.AccessControl;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class AccessControlViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private readonly AccessControlManager _accessManager;

        private ListView _lvDoors;
        private ListView _lvEvents;
        private ListView _lvCardholders;
        private PictureBox _pbDoorCam;
        private Label _lblDoorName;
        private Label _lblDoorStatus;
        private Label _lblDoorLocation;
        private Button _btnUnlock;
        private Button _btnLock;
        private Button _btnSimulateSwipe;
        private AccessDoorModel _selectedDoor;

        public AccessControlViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            _accessManager = new AccessControlManager();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
            RefreshDoors();
            RefreshEvents();
            RefreshCardholders();
        }

        private void BuildUI()
        {
            this.Controls.Clear();

            // Header Banner
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };
            var lblTitle = new Label
            {
                Text = "🚪 Physical Access Control (PACS) & Smart Doors Management",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 10),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "Manage electronic access doors, badge cardholders, reader transactions, and camera associations.",
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(16, 34),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            this.Controls.Add(pnlHeader);

            // Tab Control for Doors vs Cardholders
            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Padding = new Point(12, 6)
            };

            // Tab 1: Doors & Live Access Events
            var tabDoors = new TabPage("🚪 Access Doors & Live Feeds");
            var splitDoors = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 420,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Left: Doors List
            var pnlDoorList = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            pnlDoorList.Controls.Add(new Label { Text = "Configured Access Doors:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, Height = 25 });
            _lvDoors = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9F)
            };
            _lvDoors.Columns.Add("Door Name", 190);
            _lvDoors.Columns.Add("State", 90);
            _lvDoors.Columns.Add("Associated Camera", 130);
            _lvDoors.SelectedIndexChanged += LvDoors_SelectedIndexChanged;
            pnlDoorList.Controls.Add(_lvDoors);
            splitDoors.Panel1.Controls.Add(pnlDoorList);

            // Right: Selected Door Controls + Live Associated Camera + Access Events
            var splitRight = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 240,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Top Right: Door Details & Camera Stream
            var pnlDoorDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };

            _lblDoorName = new Label { Text = "Select a door from the list", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59), Location = new Point(12, 10), AutoSize = true };
            _lblDoorLocation = new Label { Text = "Location: -", ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(12, 34), AutoSize = true };
            _lblDoorStatus = new Label { Text = "State: -", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(5, 150, 105), Location = new Point(12, 54), AutoSize = true };

            _btnUnlock = new Button
            {
                Text = "🔓 Pulse Unlock (5s)",
                Location = new Point(12, 85),
                Size = new Size(150, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnUnlock.FlatAppearance.BorderSize = 0;
            _btnUnlock.Click += (s, e) =>
            {
                if (_selectedDoor != null)
                {
                    _accessManager.UnlockDoor(_selectedDoor.Id, 5);
                    RefreshDoors();
                    RefreshEvents();
                }
            };

            _btnLock = new Button
            {
                Text = "🔒 Force Lock",
                Location = new Point(170, 85),
                Size = new Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnLock.FlatAppearance.BorderSize = 0;
            _btnLock.Click += (s, e) =>
            {
                if (_selectedDoor != null)
                {
                    _accessManager.LockDoor(_selectedDoor.Id);
                    RefreshDoors();
                    RefreshEvents();
                }
            };

            _btnSimulateSwipe = new Button
            {
                Text = "💳 Simulate Card Swipe",
                Location = new Point(12, 125),
                Size = new Size(278, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnSimulateSwipe.FlatAppearance.BorderSize = 0;
            _btnSimulateSwipe.Click += (s, e) =>
            {
                if (_selectedDoor != null)
                {
                    _accessManager.LogAccessEvent(_selectedDoor.Name, "Deepak Sah (Engineer)", "IC-88091", true, "Access Granted (Badge Verified)");
                    RefreshEvents();
                }
            };

            // Associated Camera Preview Box
            _pbDoorCam = new PictureBox
            {
                Location = new Point(310, 10),
                Size = new Size(320, 180),
                BackColor = Color.FromArgb(15, 23, 42),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            _pbDoorCam.Paint += PbDoorCam_Paint;

            pnlDoorDetail.Controls.AddRange(new Control[] { _lblDoorName, _lblDoorLocation, _lblDoorStatus, _btnUnlock, _btnLock, _btnSimulateSwipe, _pbDoorCam });
            splitRight.Panel1.Controls.Add(pnlDoorDetail);

            // Bottom Right: Live Access Events
            var pnlEvents = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            pnlEvents.Controls.Add(new Label { Text = "Real-time Access & Badge Events Stream:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, Height = 25 });
            _lvEvents = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 8.5F)
            };
            _lvEvents.Columns.Add("Time", 90);
            _lvEvents.Columns.Add("Door", 150);
            _lvEvents.Columns.Add("Cardholder", 140);
            _lvEvents.Columns.Add("Badge ID", 90);
            _lvEvents.Columns.Add("Result", 75);
            _lvEvents.Columns.Add("Event Reason / Protocol", 200);

            pnlEvents.Controls.Add(_lvEvents);
            splitRight.Panel2.Controls.Add(pnlEvents);

            splitDoors.Panel2.Controls.Add(splitRight);
            tabDoors.Controls.Add(splitDoors);
            tabControl.TabPages.Add(tabDoors);

            // Tab 2: Cardholder & Credential Database
            var tabCards = new TabPage("👥 Cardholders & Access Credentials");
            var pnlCards = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            _lvCardholders = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9F)
            };
            _lvCardholders.Columns.Add("Cardholder Name", 180);
            _lvCardholders.Columns.Add("Badge ID", 110);
            _lvCardholders.Columns.Add("Department", 180);
            _lvCardholders.Columns.Add("Access Privileges", 220);
            _lvCardholders.Columns.Add("Status", 90);
            pnlCards.Controls.Add(_lvCardholders);
            tabCards.Controls.Add(pnlCards);
            tabControl.TabPages.Add(tabCards);

            this.Controls.Add(tabControl);
            tabControl.BringToFront();
        }

        private void RefreshDoors()
        {
            _lvDoors.Items.Clear();
            foreach (var d in _accessManager.GetDoors())
            {
                string stateIcon = (d.State == DoorState.Unlocked) ? "🟢 Unlocked" : "🔒 Locked";
                var itm = new ListViewItem(d.Name);
                itm.SubItems.Add(stateIcon);
                itm.SubItems.Add(d.AssociatedCameraName);
                itm.Tag = d;
                _lvDoors.Items.Add(itm);
            }

            if (_lvDoors.Items.Count > 0 && _selectedDoor == null)
            {
                _lvDoors.Items[0].Selected = true;
            }
        }

        private void RefreshEvents()
        {
            _lvEvents.Items.Clear();
            foreach (var ev in _accessManager.GetRecentEvents())
            {
                var itm = new ListViewItem(ev.Timestamp.ToString("HH:mm:ss"));
                itm.SubItems.Add(ev.DoorName);
                itm.SubItems.Add(ev.CardholderName);
                itm.SubItems.Add(ev.BadgeNumber);
                itm.SubItems.Add(ev.Granted ? "✔ Granted" : "❌ Denied");
                itm.SubItems.Add(ev.Reason);
                itm.ForeColor = ev.Granted ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38);
                _lvEvents.Items.Add(itm);
            }
        }

        private void RefreshCardholders()
        {
            _lvCardholders.Items.Clear();
            foreach (var c in _accessManager.GetCardholders())
            {
                var itm = new ListViewItem(c.FullName);
                itm.SubItems.Add(c.BadgeNumber);
                itm.SubItems.Add(c.Department);
                itm.SubItems.Add(c.AccessLevel);
                itm.SubItems.Add(c.IsActive ? "Active" : "Revoked");
                _lvCardholders.Items.Add(itm);
            }
        }

        private void LvDoors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lvDoors.SelectedItems.Count > 0)
            {
                _selectedDoor = _lvDoors.SelectedItems[0].Tag as AccessDoorModel;
                if (_selectedDoor != null)
                {
                    _lblDoorName.Text = _selectedDoor.Name;
                    _lblDoorLocation.Text = "Location: " + _selectedDoor.Location + " • Reader: " + _selectedDoor.ReaderType;
                    _lblDoorStatus.Text = "State: " + (_selectedDoor.State == DoorState.Unlocked ? "🟢 UNLOCKED" : "🔒 LOCKED");
                    _lblDoorStatus.ForeColor = (_selectedDoor.State == DoorState.Unlocked) ? Color.FromArgb(16, 185, 129) : Color.FromArgb(239, 68, 68);
                    _pbDoorCam.Invalidate();
                }
            }
        }

        private void PbDoorCam_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            string camName = (_selectedDoor != null && !string.IsNullOrEmpty(_selectedDoor.AssociatedCameraName))
                ? _selectedDoor.AssociatedCameraName
                : "PACS ASSOCIATED CAM";

            using (var brushBg = new SolidBrush(Color.FromArgb(15, 23, 42)))
            {
                g.FillRectangle(brushBg, _pbDoorCam.ClientRectangle);
            }

            using (var pen = new Pen(Color.FromArgb(59, 130, 246), 2))
            {
                g.DrawRectangle(pen, 1, 1, _pbDoorCam.Width - 3, _pbDoorCam.Height - 3);
            }

            using (var font = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(248, 250, 252)))
            {
                g.DrawString("● LIVE PACS STREAM • " + camName, font, brush, new PointF(10, 10));
                g.DrawString("TIMESTAMP: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), new Font("Segoe UI", 8F), Brushes.LightGray, new PointF(10, 30));
            }

            // Draw Door Overlay Status
            string doorStatus = (_selectedDoor != null && _selectedDoor.State == DoorState.Unlocked) ? "DOOR: UNLOCKED" : "DOOR: SECURE LOCKED";
            Color statusColor = (_selectedDoor != null && _selectedDoor.State == DoorState.Unlocked) ? Color.FromArgb(16, 185, 129) : Color.FromArgb(239, 68, 68);
            using (var fontStatus = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var brushStatus = new SolidBrush(statusColor))
            {
                g.DrawString("STATUS: " + doorStatus, fontStatus, brushStatus, new PointF(10, _pbDoorCam.Height - 25));
            }
        }
    }
}
