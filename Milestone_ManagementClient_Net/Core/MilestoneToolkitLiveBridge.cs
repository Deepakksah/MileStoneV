using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core
{
    public class MilestoneToolkitLiveBridge
    {
        private static MilestoneToolkitLiveBridge _instance;
        public static MilestoneToolkitLiveBridge Instance
        {
            get { return _instance ?? (_instance = new MilestoneToolkitLiveBridge()); }
        }

        private CameraModel _activeCamera;
        private Thread _streamWorker;
        private volatile bool _isStreaming = false;
        private Action<Bitmap, string> _onFrameReceived;

        public void ConnectLiveSource(CameraModel camera, Action<Bitmap, string> onFrameReceived)
        {
            _activeCamera = camera;
            _onFrameReceived = onFrameReceived;

            if (!_isStreaming)
            {
                _isStreaming = true;
                _streamWorker = new Thread(StreamWorkerLoop) { IsBackground = true };
                _streamWorker.Start();
            }
        }

        public void Disconnect()
        {
            _isStreaming = false;
        }

        public static Bitmap TryFetchSnapshot(string ip, int port, string user, string pass, int timeoutMs = 2000)
        {
            var tempCam = new CameraModel
            {
                IpAddress = ip,
                HttpPort = port,
                UserName = user,
                Password = pass
            };
            return Instance.FetchLiveFrameMultiProtocol(tempCam);
        }

        private void StreamWorkerLoop()
        {
            while (_isStreaming)
            {
                try
                {
                    var cam = _activeCamera;
                    if (cam != null && !string.IsNullOrEmpty(cam.IpAddress))
                    {
                        Bitmap frame = FetchLiveFrameMultiProtocol(cam);
                        if (frame != null && _onFrameReceived != null)
                        {
                            _onFrameReceived(frame, cam.Name);
                        }
                    }
                    Thread.Sleep(40); // 25 FPS
                }
                catch { }
            }
        }

        private static readonly Dictionary<string, string> _workingEndpoints = new Dictionary<string, string>();
        private static readonly Dictionary<string, int> _endpointErrors = new Dictionary<string, int>();

        public Bitmap FetchLiveFrameMultiProtocol(CameraModel cam)
        {
            if (cam == null || string.IsNullOrEmpty(cam.IpAddress)) return null;

            string ip = cam.IpAddress;
            int port = cam.HttpPort > 0 ? cam.HttpPort : 80;
            string user = !string.IsNullOrEmpty(cam.UserName) ? cam.UserName : "admin";
            string pass = !string.IsNullOrEmpty(cam.Password) ? cam.Password : "admin";

            string cacheKey = ip + ":" + port;

            // 1. FAST PATH: Check previously verified working endpoint (generous timeout for 1080p full HD frame compression)
            string workingUrl = null;
            lock (_workingEndpoints)
            {
                _workingEndpoints.TryGetValue(cacheKey, out workingUrl);
            }

            if (!string.IsNullOrEmpty(workingUrl))
            {
                Bitmap fastBmp = TryFetchUrl(workingUrl, user, pass, 3500);
                if (fastBmp != null)
                {
                    lock (_endpointErrors)
                    {
                        _endpointErrors[cacheKey] = 0;
                    }
                    return fastBmp;
                }

                // Tolerate transient network hiccups: only evict after 5 consecutive failures
                lock (_workingEndpoints)
                {
                    int errs = 0;
                    lock (_endpointErrors)
                    {
                        _endpointErrors.TryGetValue(cacheKey, out errs);
                        errs++;
                        _endpointErrors[cacheKey] = errs;
                    }

                    if (errs >= 5)
                    {
                        _workingEndpoints.Remove(cacheKey);
                        lock (_endpointErrors) { _endpointErrors.Remove(cacheKey); }
                    }
                }
            }

            // 2. Build prioritized probe list (brand-first)
            List<string> probeList = new List<string>();

            if (cam.Manufacturer != null && cam.Manufacturer.IndexOf("Hikvision", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                probeList.Add(string.Format("http://{0}:{1}/ISAPI/Streaming/channels/101/picture", ip, port));
                probeList.Add(string.Format("http://{0}:{1}/ISAPI/Streaming/channels/1/picture", ip, port));
                probeList.Add(string.Format("http://{0}:8000/ISAPI/Streaming/channels/101/picture", ip));
            }
            else if (cam.Manufacturer != null && cam.Manufacturer.IndexOf("Axis", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                probeList.Add(string.Format("http://{0}:{1}/axis-cgi/jpg/image.cgi", ip, port));
                probeList.Add(string.Format("http://{0}:{1}/axis-cgi/mjpg/video.cgi", ip, port));
            }
            else if (cam.Manufacturer != null && cam.Manufacturer.IndexOf("Dahua", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                probeList.Add(string.Format("http://{0}:{1}/cgi-bin/snapshot.cgi", ip, port));
            }

            // Universal fallback endpoints
            probeList.Add(string.Format("http://{0}:{1}/axis-cgi/jpg/image.cgi", ip, port));
            probeList.Add(string.Format("http://{0}:{1}/ISAPI/Streaming/channels/101/picture", ip, port));
            probeList.Add(string.Format("http://{0}:{1}/onvif/snapshot", ip, port));
            probeList.Add(string.Format("http://{0}:2020/onvif/snapshot", ip));
            probeList.Add(string.Format("http://{0}:{1}/cgi-bin/snapshot.cgi", ip, port));
            probeList.Add(string.Format("http://{0}:{1}/snapshot.jpg", ip, port));
            probeList.Add(string.Format("http://{0}:{1}/image.jpg", ip, port));
            probeList.Add(string.Format("http://{0}:8080/onvif/snapshot", ip));
            probeList.Add(string.Format("http://{0}:8899/onvif/snapshot", ip));
            probeList.Add(string.Format("http://{0}:8000/ISAPI/Streaming/channels/101/picture", ip));

            foreach (var url in probeList)
            {
                // Give primary brand URLs up to 3000ms, generic ones 1500ms
                int probeTimeout = (probeList.IndexOf(url) < 3) ? 3000 : 1500;
                Bitmap bmp = TryFetchUrl(url, user, pass, probeTimeout);
                if (bmp != null)
                {
                    lock (_workingEndpoints)
                    {
                        _workingEndpoints[cacheKey] = url;
                    }
                    lock (_endpointErrors)
                    {
                        _endpointErrors[cacheKey] = 0;
                    }
                    return bmp;
                }
            }

            return null;
        }

        private Bitmap TryFetchUrl(string url, string user, string pass, int timeoutMs)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = timeoutMs;
                req.ReadWriteTimeout = timeoutMs;
                req.KeepAlive = false;
                req.ServicePoint.Expect100Continue = false;
                req.ServicePoint.ConnectionLimit = 50;

                var credCache = new CredentialCache();
                credCache.Add(new Uri(url), "Digest", new NetworkCredential(user, pass));
                credCache.Add(new Uri(url), "Basic", new NetworkCredential(user, pass));
                req.Credentials = credCache;
                req.PreAuthenticate = true;

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var stream = resp.GetResponseStream())
                {
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    if (ms.Length < 100) return null;

                    using (var src = new Bitmap(ms))
                    {
                        Bitmap copy = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        using (var g = Graphics.FromImage(copy))
                        {
                            g.DrawImage(src, 0, 0, src.Width, src.Height);
                        }
                        return copy;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
