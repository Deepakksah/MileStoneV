using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MilestoneManagementClient.Core.SmartMap;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.UI.Views
{
    public class SmartMapViewControl : UserControl
    {
        private readonly MilestoneVmoService _vmoService;
        private readonly SmartMapManager _mapManager;

        private Panel _pnlMapCanvas;
        private Label _lblMapStatus;
        private ComboBox _cbLayer;
        private MapPinModel _selectedPin;

        public SmartMapViewControl(MilestoneVmoService vmoService)
        {
            _vmoService = vmoService;
            _mapManager = new SmartMapManager();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            BuildUI();
        }

        private void BuildUI()
        {
            this.Controls.Clear();

            // Header Banner & Layer Selector
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };
            var lblTitle = new Label
            {
                Text = "🗺️ Interactive GIS Smart Map & Geographical Camera Hierarchy",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(16, 10),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = "Visualize camera locations, live FOV (Field of View) cones, alarm locations, and access doors geographically.",
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(16, 34),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // Layer Selector on Right
            var lblLayer = new Label { Text = "Map Layer:", Location = new Point(780, 18), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _cbLayer = new ComboBox { Location = new Point(860, 15), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            _cbLayer.Items.AddRange(new object[] { "OpenStreetMap (Standard GIS)", "High-Resolution Satellite Ortho", "Building 1 - Architectural CAD" });
            _cbLayer.SelectedIndex = 0;
            pnlHeader.Controls.Add(lblLayer);
            pnlHeader.Controls.Add(_cbLayer);

            this.Controls.Add(pnlHeader);

            // Map Canvas
            _pnlMapCanvas = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(226, 232, 240)
            };
            _pnlMapCanvas.Paint += PnlMapCanvas_Paint;
            _pnlMapCanvas.MouseDown += PnlMapCanvas_MouseDown;

            // Status Bar at Bottom
            _lblMapStatus = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(248, 250, 252),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Text = "🗺️ GIS MAP READY • Click on any Camera Pin to preview stream or inspect Field of View (FOV)."
            };

            this.Controls.Add(_pnlMapCanvas);
            this.Controls.Add(_lblMapStatus);
            _pnlMapCanvas.BringToFront();
        }

        private void PnlMapCanvas_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw Geographical Grid / Map Background
            int w = _pnlMapCanvas.Width;
            int h = _pnlMapCanvas.Height;

            using (var brushBg = new SolidBrush(Color.FromArgb(238, 242, 246)))
            {
                g.FillRectangle(brushBg, 0, 0, w, h);
            }

            // Draw Grid Lines
            using (var penGrid = new Pen(Color.FromArgb(203, 213, 225), 1))
            {
                for (int x = 0; x < w; x += 50) g.DrawLine(penGrid, x, 0, x, h);
                for (int y = 0; y < h; y += 50) g.DrawLine(penGrid, 0, y, w, y);
            }

            // Draw Building Footprint Simulation
            using (var brushBldg = new SolidBrush(Color.FromArgb(255, 255, 255)))
            using (var penBldg = new Pen(Color.FromArgb(148, 163, 184), 2))
            {
                Rectangle rBldg1 = new Rectangle(120, 100, 300, 200);
                g.FillRectangle(brushBldg, rBldg1);
                g.DrawRectangle(penBldg, rBldg1);

                Rectangle rBldg2 = new Rectangle(460, 180, 240, 220);
                g.FillRectangle(brushBldg, rBldg2);
                g.DrawRectangle(penBldg, rBldg2);
            }

            using (var fontLbl = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var brushLbl = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                g.DrawString("BUILDING 1 (MAIN HQ)", fontLbl, brushLbl, new PointF(130, 110));
                g.DrawString("LOGISTICS / WAREHOUSE", fontLbl, brushLbl, new PointF(470, 190));
            }

            // Draw Pins & FOV Cones
            foreach (var pin in _mapManager.GetPins())
            {
                // Draw FOV Cone if camera
                if (pin.DeviceType != "Door")
                {
                    using (var brushFov = new SolidBrush(Color.FromArgb(45, 59, 130, 246)))
                    using (var penFov = new Pen(Color.FromArgb(59, 130, 246), 1))
                    {
                        float startAngle = pin.DirectionFovAngle - (pin.FovSpan / 2);
                        g.FillPie(brushFov, pin.PixelX - 70, pin.PixelY - 70, 140, 140, startAngle, pin.FovSpan);
                        g.DrawPie(penFov, pin.PixelX - 70, pin.PixelY - 70, 140, 140, startAngle, pin.FovSpan);
                    }
                }

                // Pin Circle
                Color pinColor = pin.HasActiveAlarm ? Color.FromArgb(239, 68, 68) : Color.FromArgb(16, 185, 129);
                if (pin == _selectedPin) pinColor = Color.FromArgb(79, 70, 229);

                using (var brushPin = new SolidBrush(pinColor))
                using (var penPin = new Pen(Color.White, 2))
                {
                    g.FillEllipse(brushPin, pin.PixelX - 9, pin.PixelY - 9, 18, 18);
                    g.DrawEllipse(penPin, pin.PixelX - 9, pin.PixelY - 9, 18, 18);
                }

                // Pin Label
                using (var fontPin = new Font("Segoe UI", 8F, FontStyle.Bold))
                using (var brushText = new SolidBrush(Color.FromArgb(15, 23, 42)))
                {
                    g.DrawString(pin.Name, fontPin, brushText, new PointF(pin.PixelX + 12, pin.PixelY - 7));
                }
            }
        }

        private void PnlMapCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var pin in _mapManager.GetPins())
            {
                int dx = e.X - pin.PixelX;
                int dy = e.Y - pin.PixelY;
                if ((dx * dx) + (dy * dy) <= 150)
                {
                    _selectedPin = pin;
                    _lblMapStatus.Text = string.Format("📍 SELECTED PIN: {0} • Type: {1} • FOV Span: {2}° • Alarm: {3}", pin.Name, pin.DeviceType, pin.FovSpan, pin.HasActiveAlarm ? "ACTIVE ALARM" : "Normal");
                    _pnlMapCanvas.Invalidate();
                    return;
                }
            }
        }
    }
}
