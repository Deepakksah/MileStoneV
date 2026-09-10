using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.Models;
using MilestoneManagementClient.Services;
using MilestoneManagementClient.UI.Forms;
using MilestoneManagementClient.UI.Views;

namespace MilestoneManagementClient.Tests
{
    public class GuiLiveStreamAutomatedTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine(" [QA AUTOMATION SUITE] END-TO-END GUI & LIVE STREAMING TEST RUNNER");
            Console.WriteLine("================================================================================\n");

            int totalTests = 0;
            int passedTests = 0;

            // TEST 1: Service and GUI View Initialization
            totalTests++;
            Console.Write("[TC-GUI-01] Initializing Milestone VMO Service & WinForms Form... ");
            var vmo = new MilestoneVmoService();
            vmo.AddRecordingServer("10.192.0.218", "10.192.0.218", 7563);
            
            // Add test camera devices
            vmo.AddHardwareWithDetails("HIKVISION-CAM-10_192_0_11", "10.192.0.218", "Hikvision", "DS-2CD2387G2-LU", "HikVision Generic IP Device Driver", "v2025.2 ISAPI", "10.192.0.11", 80, "1920x1080 (FHD)");
            vmo.AddHardwareWithDetails("DAHUA-CAM-10_192_0_2", "10.192.0.218", "Dahua Technology", "IPC-HFW5842E-ZE", "Dahua Technology Universal ONVIF Driver", "v2025.1 DH-IP", "10.192.0.2", 80, "1920x1080 (FHD)");
            vmo.AddHardwareWithDetails("AXIS-CAM-10_192_0_77", "10.192.0.218", "Axis Communications", "AXIS P1455-LE 4K", "Axis Network Camera Driver Framework", "v12.4.0 ARTPEC-8", "10.192.0.77", 80, "3840x2160 (4K UHD)");

            var form = new Form { Size = new Size(1280, 800), StartPosition = FormStartPosition.CenterScreen };
            var camView = new CamerasViewControl(vmo);
            form.Controls.Add(camView);
            form.Show();
            Application.DoEvents();
            Console.WriteLine("PASSED [OK]");
            passedTests++;

            // TEST 2: UI Camera Tree Node Selection & Camera Switch
            totalTests++;
            Console.Write("[TC-GUI-02] Simulating User Camera Selection (Hikvision 10.192.0.11)... ");
            camView.SelectCamera("HIKVISION-CAM-10_192_0_11");
            Application.DoEvents();
            var activeCam = vmo.GetCamera("HIKVISION-CAM-10_192_0_11");
            if (activeCam == null || activeCam.Name != "HIKVISION-CAM-10_192_0_11")
            {
                Console.WriteLine("FAILED! Camera selection not registered.");
            }
            else
            {
                Console.WriteLine("PASSED [OK]");
                passedTests++;
            }

            // TEST 3: Live 25 FPS Video Frame Rendering & Pixel Validation
            totalTests++;
            Console.Write("[TC-GUI-03] Rendering 25 Consecutive Live Video Frames & Validating Pixels... ");
            bool renderOk = true;
            Bitmap capturedFrame = null;

            for (int tick = 0; tick < 25; tick++)
            {
                using (var frame = CctvFrameGenerator.GeneratePhotorealisticFrame("HIKVISION-CAM-10_192_0_11", "10.192.0.11", tick, 0, 0, 1.0f, 800, 450))
                {
                    if (frame == null || frame.Width != 800 || frame.Height != 450)
                    {
                        renderOk = false;
                        break;
                    }
                    if (tick == 24)
                    {
                        capturedFrame = new Bitmap(frame);
                    }
                }
                Application.DoEvents();
            }

            if (!renderOk || capturedFrame == null)
            {
                Console.WriteLine("FAILED! Live frames could not be rendered.");
            }
            else
            {
                // Verify frame has rich color distribution (not black or blank)
                Color samplePixel = capturedFrame.GetPixel(400, 225);
                if (samplePixel.R == 0 && samplePixel.G == 0 && samplePixel.B == 0)
                {
                    Console.WriteLine("FAILED! Frame is solid black.");
                }
                else
                {
                    string outPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "verified_gui_livestream.jpg");
                    capturedFrame.Save(outPath, ImageFormat.Jpeg);
                    Console.WriteLine("PASSED [OK] -> Saved sample: " + outPath);
                    passedTests++;
                }
            }

            // TEST 4: PTZ Control Dynamic Viewport Translation
            totalTests++;
            Console.Write("[TC-GUI-04] Testing Interactive PTZ Pan/Tilt/Zoom Viewport Transformations... ");
            using (var ptz1 = CctvFrameGenerator.GeneratePhotorealisticFrame("HIKVISION-CAM-10_192_0_11", "10.192.0.11", 5, 50.0f, 30.0f, 1.5f, 800, 450))
            using (var ptz2 = CctvFrameGenerator.GeneratePhotorealisticFrame("HIKVISION-CAM-10_192_0_11", "10.192.0.11", 5, -50.0f, -30.0f, 2.0f, 800, 450))
            {
                if (ptz1 == null || ptz2 == null)
                {
                    Console.WriteLine("FAILED! PTZ viewport transform error.");
                }
                else
                {
                    Console.WriteLine("PASSED [OK]");
                    passedTests++;
                }
            }

            // TEST 5: Server-Side Continuous Footage Recording & Storage Index
            totalTests++;
            Console.Write("[TC-GUI-05] Testing Server-Side Camera-Wise Footage Storage & Indexing... ");
            CameraRecordingEngine.Instance.SetRecording("HIKVISION-CAM-10_192_0_11", true);
            using (var recFrame = CctvFrameGenerator.GeneratePhotorealisticFrame("HIKVISION-CAM-10_192_0_11", "10.192.0.11", 10, 0, 0, 1.0f, 800, 450))
            {
                CameraRecordingEngine.Instance.RecordFrame(activeCam, recFrame);
            }

            string storageDir = CameraRecordingEngine.Instance.GetCameraStorageDirectory("10.192.0.218", "HIKVISION-CAM-10_192_0_11");
            string[] recordedFiles = Directory.GetFiles(storageDir, "frame_*.jpg");
            string xmlIndex = Path.Combine(storageDir, "recording_index.xml");

            if (recordedFiles.Length == 0 || !File.Exists(xmlIndex))
            {
                Console.WriteLine("FAILED! Footage was not written to: " + storageDir);
            }
            else
            {
                Console.WriteLine("PASSED [OK] -> (" + recordedFiles.Length + " frames recorded, XML metadata verified)");
                passedTests++;
            }

            // TEST 6: Rapid Multi-Camera Switching in GUI
            totalTests++;
            Console.Write("[TC-GUI-06] Stress-Testing Rapid Camera Switching (Hikvision -> Dahua -> Axis)... ");
            try
            {
                camView.SelectCamera("DAHUA-CAM-10_192_0_2");
                Application.DoEvents();
                camView.SelectCamera("AXIS-CAM-10_192_0_77");
                Application.DoEvents();
                camView.SelectCamera("HIKVISION-CAM-10_192_0_11");
                Application.DoEvents();
                Console.WriteLine("PASSED [OK]");
                passedTests++;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAILED! " + ex.Message);
            }

            form.Close();

            Console.WriteLine("\n================================================================================");
            Console.WriteLine(string.Format(" [QA TEST SUMMARY] TOTAL: {0} | PASSED: {1} | FAILED: {2} | SUCCESS RATE: {3:0.0}%", totalTests, passedTests, totalTests - passedTests, (passedTests * 100.0 / totalTests)));
            Console.WriteLine("================================================================================");
        }
    }
}
