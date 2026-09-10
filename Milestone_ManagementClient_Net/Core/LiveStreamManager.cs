using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core
{
    public static class LiveStreamManager
    {
        private static CameraModel _activeCamera;
        private static Thread _streamThread;
        private static volatile bool _isRunning;
        private static Action<Bitmap, string> _frameCallback;
        private static Image _baseTemplateImage;

        // PTZ state
        public static float PtzPanOffset = 0;
        public static float PtzTiltOffset = 0;
        public static float PtzZoomLevel = 1.0f;

        static LiveStreamManager()
        {
            LoadBaseImage();
        }

        private static void LoadBaseImage()
        {
            string[] paths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cctv_frame.png"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "cctv_frame.png"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "publish", "cctv_frame.png"),
                Path.Combine(Directory.GetCurrentDirectory(), "cctv_frame.png"),
                @"c:\Program Files (x86)\i-Comply\Milestone\Milestone XProtect VMS Products 2025 R3 System Installer\Milestone_ManagementClient_Net\cctv_frame.png"
            };

            foreach (var p in paths)
            {
                if (File.Exists(p))
                {
                    try
                    {
                        using (var stream = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            _baseTemplateImage = Image.FromStream(stream);
                        }
                        if (_baseTemplateImage != null) break;
                    }
                    catch { }
                }
            }
        }

        public static void SwitchCamera(CameraModel camera, Action<Bitmap, string> onFrameReady)
        {
            _activeCamera = camera;
            _frameCallback = onFrameReady;
            PtzPanOffset = 0;
            PtzTiltOffset = 0;
            PtzZoomLevel = 1.0f;

            if (_baseTemplateImage == null)
            {
                LoadBaseImage();
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _streamThread = new Thread(StreamWorkerLoop) { IsBackground = true };
                _streamThread.Start();
            }
        }

        public static void StopStream()
        {
            _isRunning = false;
        }

        private static void StreamWorkerLoop()
        {
            int frameIndex = 0;

            while (_isRunning)
            {
                try
                {
                    var cam = _activeCamera;
                    if (cam != null && _frameCallback != null)
                    {
                        Bitmap frame = null;

                        // 1. Try real physical snapshot if camera IP is reachable
                        if (!string.IsNullOrEmpty(cam.IpAddress) && cam.IpAddress != "0.0.0.0" && cam.IpAddress != "127.0.0.1")
                        {
                            frame = TryFetchRealCameraSnapshot(cam);
                        }

                        if (frame != null)
                        {
                            _frameCallback(frame, cam.Name);
                        }
                    }

                    frameIndex++;
                    Thread.Sleep(50); // ~20 FPS smooth live stream
                }
                catch { }
            }
        }

        private static Bitmap TryFetchRealCameraSnapshot(CameraModel cam)
        {
            string ip = cam.IpAddress;
            int port = cam.HttpPort > 0 ? cam.HttpPort : 80;
            string user = !string.IsNullOrEmpty(cam.UserName) ? cam.UserName : "admin";
            string pass = !string.IsNullOrEmpty(cam.Password) ? cam.Password : "admin";

            string[] snapshotUrls = new[]
            {
                string.Format("http://{0}:{1}/ISAPI/Streaming/channels/101/picture", ip, port),
                string.Format("http://{0}:{1}/axis-cgi/jpg/image.cgi", ip, port),
                string.Format("http://{0}:{1}/cgi-bin/snapshot.cgi", ip, port),
                string.Format("http://{0}:{1}/onvif/snapshot", ip, port),
                string.Format("http://{0}:{1}/snapshot.jpg", ip, port)
            };

            foreach (var url in snapshotUrls)
            {
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(url);
                    req.Timeout = 300;
                    req.ReadWriteTimeout = 300;
                    req.Credentials = new NetworkCredential(user, pass);
                    using (var resp = (HttpWebResponse)req.GetResponse())
                    using (var stream = resp.GetResponseStream())
                    {
                        var ms = new MemoryStream();
                        stream.CopyTo(ms);
                        ms.Position = 0;
                        return new Bitmap(ms);
                    }
                }
                catch { }
            }
            return null;
        }
    }
}
