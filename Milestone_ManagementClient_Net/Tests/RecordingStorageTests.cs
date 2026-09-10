using System;
using System.Drawing;
using System.IO;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Tests
{
    public class RecordingStorageTests
    {
        public static bool Run()
        {
            Console.WriteLine("--- [RUNNING RecordingStorageTests] ---");

            var cam = new CameraModel
            {
                Name = "DAHUA-CAM-10_192_0_2",
                ServerName = "10.192.0.218",
                IpAddress = "10.192.0.2",
                Manufacturer = "Dahua Technology",
                Model = "IPC-HFW5842E-ZE",
                Resolution = "1920x1080",
                Codec = "H.265+"
            };

            // Enable recording
            CameraRecordingEngine.Instance.SetRecording(cam.Name, true);

            // Record test frame
            using (var bmp = new Bitmap(320, 240))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.DarkBlue);
                }
                CameraRecordingEngine.Instance.RecordFrame(cam, bmp);
            }

            string recDir = CameraRecordingEngine.Instance.GetCameraStorageDirectory(cam.ServerName, cam.Name);
            if (!Directory.Exists(recDir))
            {
                Console.WriteLine("FAIL: Recording storage directory does not exist: " + recDir);
                return false;
            }
            Console.WriteLine("PASS: Storage directory verified -> " + recDir);

            string[] files = Directory.GetFiles(recDir, "frame_*.jpg");
            if (files.Length == 0)
            {
                Console.WriteLine("FAIL: No footage frames recorded on disk!");
                return false;
            }
            Console.WriteLine("PASS: Recorded footage files verified (" + files.Length + " frames).");

            string metaFile = Path.Combine(recDir, "recording_index.xml");
            if (!File.Exists(metaFile))
            {
                Console.WriteLine("FAIL: recording_index.xml missing!");
                return false;
            }
            Console.WriteLine("PASS: Metadata recording_index.xml verified.");

            Console.WriteLine("SUCCESS: RecordingStorageTests Passed!\n");
            return true;
        }
    }
}
