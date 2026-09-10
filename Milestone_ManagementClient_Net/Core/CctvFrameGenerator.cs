using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MilestoneManagementClient.Core
{
    public static class CctvFrameGenerator
    {
        public static Bitmap GeneratePhotorealisticFrame(
            string cameraName,
            string ipAddress,
            int tick,
            float pan,
            float tilt,
            float zoom,
            int targetWidth,
            int targetHeight)
        {
            int w = Math.Max(640, targetWidth);
            int h = Math.Max(360, targetHeight);
            var bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);

            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Base Background
                using (var baseBrush = new SolidBrush(Color.FromArgb(15, 23, 42)))
                {
                    g.FillRectangle(baseBrush, 0, 0, w, h);
                }

                // Apply PTZ Viewport Transformation to Surveillance Scene
                float effectiveZoom = Math.Max(0.5f, Math.Min(4.0f, zoom <= 0 ? 1.0f : zoom));
                var state = g.Save();
                g.TranslateTransform(w / 2f + pan, h / 2f + tilt);
                g.ScaleTransform(effectiveZoom, effectiveZoom);
                g.TranslateTransform(-w / 2f, -h / 2f);

                int seed = Math.Abs((cameraName ?? "CAM").GetHashCode());

                // 1. Photorealistic Sky & Atmosphere
                using (var skyBrush = new LinearGradientBrush(
                    new Point(0, -100),
                    new Point(0, (int)(h * 0.52)),
                    Color.FromArgb(45, 75, 110),
                    Color.FromArgb(165, 190, 215)))
                {
                    g.FillRectangle(skyBrush, -200, -200, w + 400, (int)(h * 0.52) + 200);
                }

                // 2. Realistic Distant Mountains & Horizon Line
                using (var mtnBrush = new SolidBrush(Color.FromArgb(55, 68, 85)))
                {
                    g.FillPolygon(mtnBrush, new Point[]
                    {
                        new Point(-100, (int)(h * 0.48)),
                        new Point((int)(w * 0.20), (int)(h * 0.38)),
                        new Point((int)(w * 0.45), (int)(h * 0.44)),
                        new Point((int)(w * 0.70), (int)(h * 0.35)),
                        new Point((int)(w * 0.90), (int)(h * 0.42)),
                        new Point(w + 100, (int)(h * 0.48))
                    });
                }

                // 3. Realistic Industrial Buildings & Warehouse Complex
                int bldBaseY = (int)(h * 0.48);
                using (var wallBrush = new LinearGradientBrush(new Point(0, (int)(h * 0.2)), new Point(0, bldBaseY), Color.FromArgb(148, 163, 184), Color.FromArgb(71, 85, 105)))
                using (var roofBrush = new SolidBrush(Color.FromArgb(30, 41, 59)))
                using (var winLitBrush = new SolidBrush(Color.FromArgb(254, 240, 138)))
                using (var winDarkBrush = new SolidBrush(Color.FromArgb(30, 58, 138)))
                {
                    // Main Building 1
                    int b1X = (seed % 80) + 70;
                    int b1W = (int)(w * 0.32);
                    int b1H = (int)(h * 0.28);
                    int b1Y = bldBaseY - b1H;

                    g.FillRectangle(wallBrush, b1X, b1Y, b1W, b1H);
                    g.FillRectangle(roofBrush, b1X - 6, b1Y - 8, b1W + 12, 10);

                    // Structured Multi-Pane Windows
                    for (int wx = b1X + 18; wx < b1X + b1W - 20; wx += 32)
                    {
                        for (int wy = b1Y + 16; wy < bldBaseY - 18; wy += 26)
                        {
                            bool lit = ((wx + wy + seed) % 3 != 0);
                            g.FillRectangle(lit ? winLitBrush : winDarkBrush, wx, wy, 20, 16);
                            using (var penFrame = new Pen(Color.FromArgb(15, 23, 42), 1))
                            {
                                g.DrawRectangle(penFrame, wx, wy, 20, 16);
                                g.DrawLine(penFrame, wx + 10, wy, wx + 10, wy + 16);
                                g.DrawLine(penFrame, wx, wy + 8, wx + 20, wy + 8);
                            }
                        }
                    }

                    // Secondary Building / Warehouse
                    int b2X = b1X + b1W + 30;
                    int b2W = (int)(w * 0.28);
                    int b2H = (int)(h * 0.22);
                    int b2Y = bldBaseY - b2H;

                    using (var wall2 = new SolidBrush(Color.FromArgb(100, 116, 139)))
                    {
                        g.FillRectangle(wall2, b2X, b2Y, b2W, b2H);
                        g.FillRectangle(roofBrush, b2X - 4, b2Y - 6, b2W + 8, 8);
                    }
                }

                // 4. Photorealistic Ground & Asphalt Road with Perspective
                using (var groundBrush = new LinearGradientBrush(new Point(0, bldBaseY), new Point(0, h), Color.FromArgb(51, 65, 85), Color.FromArgb(15, 23, 42)))
                {
                    g.FillRectangle(groundBrush, -200, bldBaseY, w + 400, h - bldBaseY + 200);
                }

                // Asphalt Roadway with Perspective
                using (var asphaltBrush = new LinearGradientBrush(new Point(0, (int)(h * 0.62)), new Point(0, h), Color.FromArgb(30, 41, 59), Color.FromArgb(15, 23, 42)))
                {
                    Point[] roadPolygon = new Point[]
                    {
                        new Point((int)(w * 0.35), (int)(h * 0.60)),
                        new Point((int)(w * 0.65), (int)(h * 0.60)),
                        new Point(w + 150, h + 100),
                        new Point(-150, h + 100)
                    };
                    g.FillPolygon(asphaltBrush, roadPolygon);
                }

                // Road Lane Markings
                using (var lanePen = new Pen(Color.FromArgb(226, 232, 240), 3) { DashStyle = DashStyle.Dash })
                {
                    g.DrawLine(lanePen, (int)(w * 0.5), (int)(h * 0.60), (int)(w * 0.5), h + 100);
                }

                // Security Perimeter Fence
                using (var fencePen = new Pen(Color.FromArgb(100, 148, 163, 184), 1.5f))
                {
                    int fenceY = (int)(h * 0.56);
                    g.DrawLine(fencePen, -200, fenceY, w + 200, fenceY);
                    for (int fx = -180; fx < w + 180; fx += 18)
                    {
                        g.DrawLine(fencePen, fx, fenceY - 20, fx, fenceY + 10);
                        g.DrawLine(fencePen, fx, fenceY - 20, fx + 18, fenceY);
                        g.DrawLine(fencePen, fx, fenceY, fx + 18, fenceY - 20);
                    }
                }

                // 5. Realistic Moving Vehicle on Road
                int speed = 6;
                int vX = ((tick * speed) % (w + 260)) - 130;
                int vY = (int)(h * 0.74);
                int vW = 125;
                int vH = 50;

                // Vehicle Shadow
                using (var shadowBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                {
                    g.FillEllipse(shadowBrush, vX - 10, vY + vH - 6, vW + 20, 14);
                }

                // Vehicle Body (Glossy Finish)
                using (var carBody = new LinearGradientBrush(new Point(vX, vY), new Point(vX, vY + vH), Color.FromArgb(185, 28, 28), Color.FromArgb(127, 29, 29)))
                using (var carRoof = new LinearGradientBrush(new Point(vX, vY), new Point(vX, vY + 20), Color.FromArgb(153, 27, 27), Color.FromArgb(69, 10, 10)))
                using (var glassBrush = new LinearGradientBrush(new Point(vX, vY), new Point(vX, vY + 20), Color.FromArgb(186, 230, 253), Color.FromArgb(30, 58, 138)))
                using (var tireBrush = new SolidBrush(Color.FromArgb(15, 23, 42)))
                using (var rimBrush = new SolidBrush(Color.FromArgb(203, 213, 225)))
                {
                    // Lower Body
                    g.FillRectangle(carBody, vX, vY + 16, vW, vH - 16);

                    // Upper Cabin / Windshield
                    Point[] cabin = new Point[]
                    {
                        new Point(vX + 22, vY + 16),
                        new Point(vX + 42, vY + 2),
                        new Point(vX + 88, vY + 2),
                        new Point(vX + 106, vY + 16)
                    };
                    g.FillPolygon(carRoof, cabin);

                    // Windows
                    Point[] glass = new Point[]
                    {
                        new Point(vX + 28, vY + 15),
                        new Point(vX + 44, vY + 4),
                        new Point(vX + 84, vY + 4),
                        new Point(vX + 100, vY + 15)
                    };
                    g.FillPolygon(glassBrush, glass);

                    // Headlights / Tail Lights
                    using (var lightFront = new SolidBrush(Color.FromArgb(254, 240, 138)))
                    using (var lightRear = new SolidBrush(Color.FromArgb(239, 68, 68)))
                    {
                        g.FillRectangle(lightFront, vX + vW - 4, vY + 22, 4, 8);
                        g.FillRectangle(lightRear, vX, vY + 22, 4, 8);
                    }

                    // Tires & Alloy Rims
                    g.FillEllipse(tireBrush, vX + 18, vY + vH - 12, 22, 22);
                    g.FillEllipse(rimBrush, vX + 23, vY + vH - 7, 12, 12);

                    g.FillEllipse(tireBrush, vX + vW - 38, vY + vH - 12, 22, 22);
                    g.FillEllipse(rimBrush, vX + vW - 33, vY + vH - 7, 12, 12);
                }

                // 6. Professional AI Surveillance Target Box
                using (var penAI = new Pen(Color.FromArgb(239, 68, 68), 2))
                using (var fillAI = new SolidBrush(Color.FromArgb(30, 239, 68, 68)))
                using (var fontAI = new Font("Segoe UI", 8.5F, FontStyle.Bold))
                using (var textBrush = new SolidBrush(Color.FromArgb(239, 68, 68)))
                {
                    g.DrawRectangle(penAI, vX - 6, vY - 6, vW + 12, vH + 16);
                    g.FillRectangle(fillAI, vX - 6, vY - 6, vW + 12, vH + 16);
                    g.DrawString("[MOTION: VEHICLE 98.7% • INTRUSION]", fontAI, textBrush, new PointF(vX - 6, vY - 22));
                }

                // 7. Motion Detection Grid Indicator
                using (var penMotion = new Pen(Color.FromArgb(25, 16, 185, 129), 1))
                {
                    for (int gx = 0; gx < w; gx += 40) g.DrawLine(penMotion, gx, (int)(h * 0.5), gx, h);
                    for (int gy = (int)(h * 0.5); gy < h; gy += 30) g.DrawLine(penMotion, 0, gy, w, gy);
                }

                // Restore Graphics State before drawing OSD and HUD
                g.Restore(state);

                // 8. Subtle Surveillance CCTV Scanline Raster Effect
                using (var scanlineBrush = new SolidBrush(Color.FromArgb(10, 0, 0, 0)))
                {
                    for (int y = 0; y < h; y += 3)
                    {
                        g.FillRectangle(scanlineBrush, 0, y, w, 1);
                    }
                }

                // 9. Authentic Milestone OSD Header (Crisp Digital Font & Live Timestamp)
                string nowClock = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                using (var osdFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (var osdBg = new SolidBrush(Color.FromArgb(220, 11, 15, 25)))
                using (var osdRed = new SolidBrush(Color.FromArgb(239, 68, 68)))
                using (var osdGreen = new SolidBrush(Color.FromArgb(16, 185, 129)))
                using (var osdWhite = new SolidBrush(Color.FromArgb(241, 245, 249)))
                {
                    string statusLine = string.Format("● LIVE [30 FPS] • H.265+ • 8192 kbps • {0} • {1}", cameraName ?? "IP CAMERA", nowClock);
                    var sz = g.MeasureString(statusLine, osdFont);
                    g.FillRectangle(osdBg, 10, 8, sz.Width + 14, sz.Height + 6);
                    g.DrawString(statusLine, osdFont, osdRed, new PointF(16, 11));

                    // PTZ & Viewport Coordinates Telemetry Badge (Top Right)
                    string ptzTelemetry = string.Format("🎯 PTZ: Pan: {0:+0;-0;0}° | Tilt: {1:+0;-0;0}° | Zoom: {2:0.00}x", pan, tilt, effectiveZoom);
                    var szPtz = g.MeasureString(ptzTelemetry, osdFont);
                    g.FillRectangle(osdBg, w - szPtz.Width - 18, 8, szPtz.Width + 12, szPtz.Height + 6);
                    g.DrawString(ptzTelemetry, osdFont, osdGreen, new PointF(w - szPtz.Width - 12, 11));
                }

                // 10. Camera ID Watermark (Bottom-Right)
                using (var wmFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (var wmBg = new SolidBrush(Color.FromArgb(220, 11, 15, 25)))
                using (var wmYellow = new SolidBrush(Color.FromArgb(254, 240, 138)))
                {
                    string wm = string.Format("📹 {0} • IP: {1}", cameraName ?? "Camera 01", ipAddress ?? "192.168.1.201");
                    var szWm = g.MeasureString(wm, wmFont);
                    g.FillRectangle(wmBg, w - szWm.Width - 18, h - szWm.Height - 10, szWm.Width + 12, szWm.Height + 6);
                    g.DrawString(wm, wmFont, wmYellow, new PointF(w - szWm.Width - 12, h - szWm.Height - 7));
                }
            }

            return bmp;
        }
    }
}
