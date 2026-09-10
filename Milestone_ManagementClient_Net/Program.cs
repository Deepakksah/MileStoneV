using System;
using System.IO;
using System.Windows.Forms;
using MilestoneManagementClient.Core;
using MilestoneManagementClient.UI.Dialogs;
using MilestoneManagementClient.UI.Forms;

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

            try
            {
                Logger.Info("STARTUP", "=========================================================");
                Logger.Info("STARTUP", "Milestone XProtect Management Client 2025 R3 Initializing");
                Logger.Info("STARTUP", "Log Location: " + Logger.GetLogFilePath());
                Logger.Info("STARTUP", "=========================================================");

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                Application.ThreadException += (s, e) =>
                {
                    Logger.Error("THREAD_CRASH", "Unhandled Thread Exception caught", e.Exception);
                    MessageBox.Show(
                        "An unexpected application error occurred:\n\n" + e.Exception.Message + "\n\nDetails have been logged to:\n" + Logger.GetLogFilePath(),
                        "Milestone System Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                };

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    Exception ex = e.ExceptionObject as Exception;
                    Logger.Error("DOMAIN_CRASH", "Fatal AppDomain Unhandled Exception", ex);
                    string msg = ex != null ? ex.Message : e.ExceptionObject.ToString();
                    MessageBox.Show(
                        "A critical system error occurred:\n\n" + msg + "\n\nDiagnostic log saved to:\n" + Logger.GetLogFilePath(),
                        "Milestone Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Logger.Info("STARTUP", "Launching ManagementClientForm UI shell...");
                ManagementClientForm mainForm = new ManagementClientForm();
                Application.Run(mainForm);
                Logger.Info("SHUTDOWN", "Milestone Management Client closed gracefully.");
            }
            catch (Exception ex)
            {
                Logger.Error("FATAL_MAIN", "Fatal unhandled exception in Program.Main", ex);
                MessageBox.Show("FATAL Startup Error:\n\n" + ex.Message + "\n\nSee log: " + Logger.GetLogFilePath(), "Milestone Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
