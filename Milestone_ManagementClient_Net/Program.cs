using System;
using System.IO;
using System.Windows.Forms;
using MilestoneManagementClient.UI;

namespace MilestoneManagementClient
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("shell32.dll", SetLastError = true)]
        private static extern void SetCurrentProcessExplicitAppUserModelID([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string AppID);

        [STAThread]
        private static void Main()
        {
            try
            {
                SetCurrentProcessExplicitAppUserModelID("Milestone.LycheeVMS.ManagementClient.2025");
            }
            catch { }

            string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup.log");
            try
            {
                File.WriteAllText(logFile, "Starting Milestone Management Client...\n");

                Application.ThreadException += (s, e) =>
                {
                    File.AppendAllText(logFile, "ThreadException: " + e.Exception + "\n");
                    MessageBox.Show(
                        "CRASH (ThreadException):\n\n" + e.Exception.GetType().Name + "\n" + e.Exception.Message + "\n\nStack:\n" + e.Exception.StackTrace,
                        "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    File.AppendAllText(logFile, "UnhandledException: " + e.ExceptionObject + "\n");
                    Exception ex = e.ExceptionObject as Exception;
                    string msg = ex != null ? ex.GetType().Name + "\n" + ex.Message + "\n\n" + ex.StackTrace : e.ExceptionObject.ToString();
                    MessageBox.Show("CRASH (UnhandledException):\n\n" + msg, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                File.AppendAllText(logFile, "Creating ManagementClientForm...\n");
                ManagementClientForm mainForm = new ManagementClientForm();
                File.AppendAllText(logFile, "Running Application.Run(mainForm)...\n");
                Application.Run(mainForm);
                File.AppendAllText(logFile, "Application.Run exited normally.\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, "FATAL in Main: " + ex + "\n");
                MessageBox.Show("FATAL Exception in Main:\n\n" + ex, "Milestone Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
