using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core
{
    public class CameraRecordingEngine
    {
        private static CameraRecordingEngine _instance;
        public static CameraRecordingEngine Instance
        {
            get { return _instance ?? (_instance = new CameraRecordingEngine()); }
        }

        private readonly Dictionary<string, bool> _activeRecordings = new Dictionary<string, bool>();
        private readonly Dictionary<string, long> _recordedBytesMap = new Dictionary<string, long>();
        private readonly object _lock = new object();
        private DateTime _lastRecordTime = DateTime.MinValue;

        public bool IsRecording(string cameraName)
        {
            lock (_lock)
            {
                bool rec;
                if (_activeRecordings.TryGetValue(cameraName, out rec)) return rec;
                return true; // Default 24x7 continuous recording active as per Milestone Rule!
            }
        }

        public void SetRecording(string cameraName, bool enable)
        {
            lock (_lock)
            {
                _activeRecordings[cameraName] = enable;
            }
        }

        public string GetCameraStorageDirectory(string serverName, string cameraName)
        {
            string srv = string.IsNullOrWhiteSpace(serverName) ? "10.192.0.218" : serverName;
            string cam = string.IsNullOrWhiteSpace(cameraName) ? "UNIVERSAL_ONVIF_CAM_01" : cameraName;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                srv = srv.Replace(c, '_');
                cam = cam.Replace(c, '_');
            }

            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MediaDatabase", "Recordings", srv, cam, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
            return baseDir;
        }

        public long GetRecordedBytes(string cameraName)
        {
            lock (_lock)
            {
                long b;
                if (_recordedBytesMap.TryGetValue(cameraName, out b)) return b;
                return 0;
            }
        }

        public void RecordFrame(CameraModel cam, Bitmap frame)
        {
            if (cam == null || frame == null) return;
            if (!IsRecording(cam.Name)) return;

            // Throttle recording to 5 frames per second on disk to optimize disk I/O
            if ((DateTime.Now - _lastRecordTime).TotalMilliseconds < 200) return;
            _lastRecordTime = DateTime.Now;

            try
            {
                string dir = GetCameraStorageDirectory(cam.ServerName, cam.Name);
                string fileName = string.Format("frame_{0}.jpg", DateTime.Now.ToString("HHmmss_fff"));
                string filePath = Path.Combine(dir, fileName);

                using (var clone = new Bitmap(frame))
                {
                    clone.Save(filePath, ImageFormat.Jpeg);
                }

                var fi = new FileInfo(filePath);
                lock (_lock)
                {
                    long current;
                    _recordedBytesMap.TryGetValue(cam.Name, out current);
                    _recordedBytesMap[cam.Name] = current + (fi.Exists ? fi.Length : 25000);
                }

                // Update Footage Metadata index file
                string metaFile = Path.Combine(dir, "recording_index.xml");
                if (!File.Exists(metaFile))
                {
                    string xmlHeader = string.Format(
                        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                        "<MediaRecording camera=\"{0}\" server=\"{1}\" ip=\"{2}\" resolution=\"{3}\" codec=\"{4}\" date=\"{5}\">\r\n" +
                        "  <FootageSession startTime=\"{6}\" format=\"JPEG/H.265 Continuous\" status=\"RECORDING\" />\r\n" +
                        "</MediaRecording>",
                        cam.Name, cam.ServerName ?? "10.192.0.218", cam.IpAddress ?? "10.192.0.11", cam.Resolution ?? "1920x1080", cam.Codec ?? "H.265+", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm:ss"));

                    File.WriteAllText(metaFile, xmlHeader, Encoding.UTF8);
                }
            }
            catch { }
        }
    }
}
