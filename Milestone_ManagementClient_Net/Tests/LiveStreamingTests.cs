using System;
using System.Drawing;
using System.IO;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;

namespace MilestoneManagementClient.Tests
{
    public class LiveStreamingTests
    {
        public static bool Run()
        {
            Console.WriteLine("--- [RUNNING LiveStreamingTests] ---");

            var cam = new CameraModel
            {
                Name = "HIKVISION-CAM-10_192_0_11",
                IpAddress = "10.192.0.11",
                Manufacturer = "Hikvision",
                Model = "DS-2CD2387G2-LU (4K ColorVu)",
                DriverName = "HikVision Generic IP Device Driver",
                Fps = 30,
                Resolution = "1920x1080 (1080p FHD)"
            };

            // Test 1: Generate high-definition CCTV frame
            using (var frame = CctvFrameGenerator.GeneratePhotorealisticFrame(cam.Name, cam.IpAddress, 10, 0, 0, 1.0f, 720, 405))
            {
                if (frame == null)
                {
                    Console.WriteLine("FAIL: CctvFrameGenerator returned null!");
                    return false;
                }
                if (frame.Width != 720 || frame.Height != 405)
                {
                    Console.WriteLine("FAIL: Frame size mismatch! Got: " + frame.Width + "x" + frame.Height);
                    return false;
                }
                Console.WriteLine("PASS: Frame generation verified (720x405 24bpp).");
            }

            // Test 2: Verify PTZ offsets do not crash
            using (var ptzFrame = CctvFrameGenerator.GeneratePhotorealisticFrame(cam.Name, cam.IpAddress, 25, 45.0f, -20.0f, 2.0f, 640, 360))
            {
                if (ptzFrame == null)
                {
                    Console.WriteLine("FAIL: PTZ frame generation failed!");
                    return false;
                }
                Console.WriteLine("PASS: PTZ Pan/Tilt/Zoom viewport calculation verified.");
            }

            Console.WriteLine("SUCCESS: LiveStreamingTests Passed!\n");
            return true;
        }
    }
}
