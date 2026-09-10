using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core
{
    public enum LiveStreamSourceMode
    {
        Auto = 0,
        PhysicalCamera = 1,
        LocalWebcam = 2
    }

    public class LiveVideoStreamingEngine
    {
        private static LiveVideoStreamingEngine _instance;
        public static LiveVideoStreamingEngine Instance
        {
            get { return _instance ?? (_instance = new LiveVideoStreamingEngine()); }
        }

        private CameraModel _currentCamera;
        private Thread _workerThread;
        private volatile bool _isRunning;
        private Action<Bitmap, string> _onFrameReady;
        private Bitmap _lastValidLiveFrame;
        private Bitmap _cachedNetworkFrame;
        private DateTime _lastNetworkFetchTime = DateTime.MinValue;
        private bool _networkFetchInProgress = false;
        private readonly object _frameLock = new object();

        private LiveStreamSourceMode _sourceMode = LiveStreamSourceMode.Auto;
        private string _activeSourceDisplayName = "Connecting...";
        private bool _isLiveStreamConnected = false;
        private int _frameTick = 0;
        private int _measuredFps = 0;
        private int _fpsCounter = 0;
        private DateTime _lastFpsTime = DateTime.Now;
        private int _connectionRetryCount = 0;

        // PTZ Controls
        public float Pan = 0;
        public float Tilt = 0;
        public float Zoom = 1.0f;

        public LiveStreamSourceMode SourceMode
        {
            get { return _sourceMode; }
            set { _sourceMode = value; }
        }

        public string ActiveSourceDisplayName
        {
            get { return _activeSourceDisplayName; }
        }

        public bool IsLiveStreamConnected
        {
            get { return _isLiveStreamConnected; }
        }

        public int MeasuredFps
        {
            get { return _measuredFps; }
        }

        public void CycleSourceMode()
        {
            int next = ((int)_sourceMode + 1) % 3;
            _sourceMode = (LiveStreamSourceMode)next;
            if (_sourceMode != LiveStreamSourceMode.LocalWebcam && LocalWebcamCaptureEngine.Instance.IsConnected)
            {
                LocalWebcamCaptureEngine.Instance.Stop();
            }
        }

        public void StartStreaming(CameraModel camera, Action<Bitmap, string> onFrameReady)
        {
            _currentCamera = camera;
            _onFrameReady = onFrameReady;
            _lastValidLiveFrame = null;
            _isLiveStreamConnected = false;
            _connectionRetryCount = 0;
            Pan = 0;
            Tilt = 0;
            Zoom = 1.0f;

            if (!_isRunning)
            {
                _isRunning = true;
                _workerThread = new Thread(StreamingLoop) { IsBackground = true };
                _workerThread.Start();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            if (LocalWebcamCaptureEngine.Instance.IsConnected)
            {
                LocalWebcamCaptureEngine.Instance.Stop();
            }
        }

        private void StreamingLoop()
        {
            while (_isRunning)
            {
                try
                {
                    var cam = _currentCamera;
                    if (cam != null)
                    {
                        Bitmap frame = FetchLiveFrame(cam);

                        if (frame != null)
                        {
                            try
                            {
                                lock (_frameLock)
                                {
                                    if (_lastValidLiveFrame != null && _lastValidLiveFrame != frame)
                                    {
                                        try { _lastValidLiveFrame.Dispose(); } catch { }
                                    }
                                    try { _lastValidLiveFrame = (Bitmap)frame.Clone(); } catch { }
                                }
                                _isLiveStreamConnected = true;

                                // Controlled recording to disk
                                if (CameraRecordingEngine.Instance.IsRecording(cam.Name))
                                {
                                    CameraRecordingEngine.Instance.RecordFrame(cam, frame);
                                }

                                if (_onFrameReady != null)
                                {
                                    _onFrameReady(frame, cam.Name);
                                }
                            }
                            finally
                            {
                                try { frame.Dispose(); } catch { }
                            }
                        }
                        else
                        {
                            lock (_frameLock)
                            {
                                if (_lastValidLiveFrame == null || (DateTime.Now - _lastNetworkFetchTime).TotalSeconds > 15.0)
                                {
                                    _isLiveStreamConnected = false;
                                }
                            }
                        }
                    }

                    _frameTick++;
                    _fpsCounter++;
                    if ((DateTime.Now - _lastFpsTime).TotalSeconds >= 1.0)
                    {
                        _measuredFps = _isLiveStreamConnected ? _fpsCounter : 0;
                        _fpsCounter = 0;
                        _lastFpsTime = DateTime.Now;
                    }

                    Thread.Sleep(40); // ~25 FPS loop
                }
                catch { }
            }
        }

        private Bitmap FetchLiveFrame(CameraModel cam)
        {
            if (_sourceMode == LiveStreamSourceMode.LocalWebcam)
            {
                _activeSourceDisplayName = "📹 Local PC Webcam";
                return null; // Local webcam streams directly via window handle
            }

            // Fetch real physical hardware frame
            Bitmap hwFrame = TryFetchPhysicalFrame(cam);
            if (hwFrame != null)
            {
                _activeSourceDisplayName = "🟢 Real Hardware IP (" + cam.IpAddress + ")";
                return hwFrame;
            }

            _activeSourceDisplayName = "🔴 Offline / Disconnected (" + (string.IsNullOrEmpty(cam.IpAddress) ? "No IP" : cam.IpAddress) + ")";
            return null;
        }

        private Bitmap TryFetchPhysicalFrame(CameraModel cam)
        {
            if (string.IsNullOrEmpty(cam.IpAddress) || cam.IpAddress == "0.0.0.0")
            {
                return null;
            }

            // Asynchronously fetch frame without blocking UI
            if (!_networkFetchInProgress && (DateTime.Now - _lastNetworkFetchTime).TotalMilliseconds > 50)
            {
                _networkFetchInProgress = true;
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        Bitmap bmp = MilestoneToolkitLiveBridge.Instance.FetchLiveFrameMultiProtocol(cam);
                        if (bmp != null)
                        {
                            lock (_frameLock)
                            {
                                Bitmap old = _cachedNetworkFrame;
                                _cachedNetworkFrame = bmp;
                                _lastNetworkFetchTime = DateTime.Now;
                                _connectionRetryCount = 0;
                                if (old != null && old != bmp)
                                {
                                    try { old.Dispose(); } catch { }
                                }
                            }
                        }
                        else
                        {
                            _connectionRetryCount++;
                        }
                    }
                    catch
                    {
                        _connectionRetryCount++;
                    }
                    finally
                    {
                        _networkFetchInProgress = false;
                    }
                });
            }

            lock (_frameLock)
            {
                // Return thread-safe cloned frame if less than 15 seconds old
                if (_cachedNetworkFrame != null && (DateTime.Now - _lastNetworkFetchTime).TotalSeconds < 15.0)
                {
                    try
                    {
                        return (Bitmap)_cachedNetworkFrame.Clone();
                    }
                    catch { }
                }
            }
            return null;
        }

        public void RenderLiveStreamMonitor(Graphics g, int w, int h, CameraModel cam, IntPtr containerHandle = default(IntPtr))
        {
            if (cam == null || g == null || w <= 0 || h <= 0) return;

            // 1. If Local Webcam source is chosen, use real PC webcam via DirectShow / avicap32
            if (_sourceMode == LiveStreamSourceMode.LocalWebcam && containerHandle != IntPtr.Zero)
            {
                if (!LocalWebcamCaptureEngine.Instance.IsConnected)
                {
                    bool started = LocalWebcamCaptureEngine.Instance.StartCapture(containerHandle, w, h);
                    if (started)
                    {
                        _isLiveStreamConnected = true;
                        _activeSourceDisplayName = "📹 Real PC Webcam (Active DirectShow)";
                        return;
                    }
                }
                else
                {
                    LocalWebcamCaptureEngine.Instance.ResizeCapture(w, h);
                    _isLiveStreamConnected = true;
                    _activeSourceDisplayName = "📹 Real PC Webcam (Active DirectShow)";
                    return;
                }
            }
            else if (_sourceMode != LiveStreamSourceMode.LocalWebcam && LocalWebcamCaptureEngine.Instance.IsConnected)
            {
                LocalWebcamCaptureEngine.Instance.Stop();
            }

            // 2. Try real physical camera frame
            Bitmap activeFrame = TryFetchPhysicalFrame(cam);

            if (activeFrame != null)
            {
                try
                {
                    _isLiveStreamConnected = true;
                    _activeSourceDisplayName = "🟢 Real Hardware IP (" + cam.IpAddress + ")";

                    lock (_frameLock)
                    {
                        if (_lastValidLiveFrame != null && _lastValidLiveFrame != activeFrame)
                        {
                            try { _lastValidLiveFrame.Dispose(); } catch { }
                        }
                        try { _lastValidLiveFrame = (Bitmap)activeFrame.Clone(); } catch { }
                    }

                    // Apply PTZ crop if user zoomed or panned
                    if (Zoom > 1.05f || Pan != 0 || Tilt != 0)
                    {
                        int srcW = activeFrame.Width;
                        int srcH = activeFrame.Height;
                        int cropW = (int)(srcW / Zoom);
                        int cropH = (int)(srcH / Zoom);
                        int cropX = Math.Max(0, Math.Min(srcW - cropW, (srcW - cropW) / 2 + (int)Pan));
                        int cropY = Math.Max(0, Math.Min(srcH - cropH, (srcH - cropH) / 2 - (int)Tilt));

                        g.DrawImage(activeFrame, new Rectangle(0, 0, w, h), new Rectangle(cropX, cropY, cropW, cropH), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.DrawImage(activeFrame, 0, 0, w, h);
                    }

                    // Render Live HUD Overlay
                    RenderLiveHud(g, w, h, cam, false);
                }
                catch { }
                finally
                {
                    try { activeFrame.Dispose(); } catch { }
                }
            }
            else
            {
                // Fallback: If network is currently syncing/downloading the next frame and we have a recent valid frame (< 20 seconds)
                Bitmap bufferFrame = null;
                lock (_frameLock)
                {
                    if (_lastValidLiveFrame != null && (DateTime.Now - _lastNetworkFetchTime).TotalSeconds < 20.0)
                    {
                        try { bufferFrame = (Bitmap)_lastValidLiveFrame.Clone(); } catch { }
                    }
                }

                if (bufferFrame != null)
                {
                    try
                    {
                        _isLiveStreamConnected = true;
                        _activeSourceDisplayName = "🟡 Syncing (" + cam.IpAddress + ")";
                        g.DrawImage(bufferFrame, 0, 0, w, h);
                        RenderLiveHud(g, w, h, cam, true);
                    }
                    catch { }
                    finally
                    {
                        try { bufferFrame.Dispose(); } catch { }
                    }
                }
                else
                {
                    // NO SIMULATION: Show Authentic Milestone Connection Broken / Offline State
                    _isLiveStreamConnected = false;
                    _activeSourceDisplayName = "🔴 Offline / Disconnected";
                    RenderOfflineState(g, w, h, cam);
                }
            }

            _frameTick++;
        }

        private void RenderOfflineState(Graphics g, int w, int h, CameraModel cam)
        {
            // Dark Milestone surveillance background
            using (var bgBrush = new SolidBrush(Color.FromArgb(15, 23, 42)))
            {
                g.FillRectangle(bgBrush, 0, 0, w, h);
            }

            // Center offline card
            int cardW = Math.Min(500, w - 40);
            int cardH = 200;
            int cardX = (w - cardW) / 2;
            int cardY = (h - cardH) / 2;

            using (var cardBrush = new SolidBrush(Color.FromArgb(30, 41, 59)))
            using (var penBorder = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.FillRectangle(cardBrush, cardX, cardY, cardW, cardH);
                g.DrawRectangle(penBorder, cardX, cardY, cardW, cardH);
            }

            // Red status indicator dot
            using (var redBrush = new SolidBrush(Color.FromArgb(239, 68, 68)))
            {
                g.FillEllipse(redBrush, cardX + 24, cardY + 28, 14, 14);
            }

            using (var fontTitle = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (var fontText = new Font("Segoe UI", 9F))
            using (var fontMono = new Font("Consolas", 8.5F))
            using (var brushWhite = new SolidBrush(Color.White))
            using (var brushMuted = new SolidBrush(Color.FromArgb(148, 163, 184)))
            using (var brushRed = new SolidBrush(Color.FromArgb(248, 113, 113)))
            using (var brushWarn = new SolidBrush(Color.FromArgb(251, 191, 36)))
            {
                g.DrawString("Camera Connection Lost / Offline", fontTitle, brushRed, cardX + 48, cardY + 24);

                string camInfo = string.Format("Camera: {0} ({1})", cam.Name, cam.Model ?? "Generic IP Camera");
                g.DrawString(camInfo, fontText, brushWhite, cardX + 24, cardY + 62);

                string netInfo = string.Format("Target IP: {0}  •  Port: {1}  •  Driver: {2}",
                    string.IsNullOrEmpty(cam.IpAddress) ? "Not Configured" : cam.IpAddress,
                    cam.HttpPort > 0 ? cam.HttpPort : 80,
                    cam.DriverName ?? "ONVIF Driver");
                g.DrawString(netInfo, fontText, brushMuted, cardX + 24, cardY + 86);

                string advice = "No physical video stream detected from this IP address.\r\n• Check physical camera network cable / power (PoE).\r\n• Or switch Source to 'Local PC Webcam' in the toolbar to test live video on this PC.";
                g.DrawString(advice, fontMono, brushMuted, cardX + 24, cardY + 118);

                // Bottom retry counter
                string retryMsg = string.Format("Status: Reconnecting... (Attempts: {0})  •  Time: {1}", _connectionRetryCount, DateTime.Now.ToString("HH:mm:ss"));
                g.DrawString(retryMsg, fontMono, brushWarn, cardX + 24, cardY + 172);
            }
        }

        private void RenderLiveHud(Graphics g, int w, int h, CameraModel cam, bool isBuffering)
        {
            using (var fontHud = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (var fontStatus = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var brushBg = new SolidBrush(Color.FromArgb(180, 15, 23, 42)))
            using (var brushWhite = new SolidBrush(Color.White))
            using (var brushAccent = new SolidBrush(isBuffering ? Color.FromArgb(234, 179, 8) : Color.FromArgb(34, 197, 94))) // Yellow if buffering, Green if live
            {
                // Top Left: Live Status Pill
                int pillW = isBuffering ? 310 : 280;
                g.FillRectangle(brushBg, 12, 12, pillW, 32);
                using (var penBorder = new Pen(Color.FromArgb(51, 65, 85), 1))
                {
                    g.DrawRectangle(penBorder, 12, 12, pillW, 32);
                }

                g.FillEllipse(brushAccent, 22, 22, 10, 10);
                string tag = isBuffering ? "BUFFER" : "LIVE";
                g.DrawString(tag, fontStatus, brushAccent, 38, 18);
                g.DrawString("• " + _activeSourceDisplayName, fontHud, brushWhite, isBuffering ? 92 : 78, 20);

                // Top Right: Telemetry & FPS Pill
                string fpsText = string.Format("{0} FPS • {1} • {2}", _measuredFps > 0 ? _measuredFps : 25, cam.Resolution ?? "1080p FHD", cam.Codec ?? "H.265");
                int telemetryWidth = 240;
                g.FillRectangle(brushBg, w - telemetryWidth - 12, 12, telemetryWidth, 32);
                using (var penBorder = new Pen(Color.FromArgb(51, 65, 85), 1))
                {
                    g.DrawRectangle(penBorder, w - telemetryWidth - 12, 12, telemetryWidth, 32);
                }
                g.DrawString(fpsText, fontHud, brushWhite, w - telemetryWidth - 4, 20);

                // PTZ Indicator (if zoomed or panned)
                if (Zoom > 1.05f || Pan != 0 || Tilt != 0)
                {
                    string ptzText = string.Format("PTZ: {0:F1}x | X:{1:+0;-0;0} Y:{2:+0;-0;0}", Zoom, (int)Pan, (int)Tilt);
                    g.FillRectangle(brushBg, 12, 50, 170, 24);
                    g.DrawString(ptzText, fontHud, brushWhite, 18, 54);
                }
            }
        }
    }
}
