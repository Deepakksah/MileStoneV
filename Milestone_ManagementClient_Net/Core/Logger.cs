using System;
using System.IO;
using System.Text;

namespace MilestoneManagementClient.Core
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static string _logDirectory;
        private static string _logFilePath;

        static Logger()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                _logDirectory = Path.Combine(baseDir, "logs");
                if (!Directory.Exists(_logDirectory))
                {
                    Directory.CreateDirectory(_logDirectory);
                }
                _logFilePath = Path.Combine(_logDirectory, "management_client_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            }
            catch
            {
                _logFilePath = "management_client.log";
            }
        }

        public static void Info(string module, string message)
        {
            WriteLog("INFO", module, message, null);
        }

        public static void Warn(string module, string message)
        {
            WriteLog("WARN", module, message, null);
        }

        public static void Error(string module, string message, Exception ex = null)
        {
            WriteLog("ERROR", module, message, ex);
        }

        public static void Debug(string module, string message)
        {
            WriteLog("DEBUG", module, message, null);
        }

        private static void WriteLog(string level, string module, string message, Exception ex)
        {
            try
            {
                lock (_lock)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var sb = new StringBuilder();
                    sb.AppendFormat("[{0}] [{1,-5}] [{2}] {3}", timestamp, level, module, message);
                    if (ex != null)
                    {
                        sb.AppendLine();
                        sb.AppendFormat("   EXCEPTION: {0} - {1}", ex.GetType().FullName, ex.Message);
                        sb.AppendLine();
                        sb.AppendFormat("   STACK TRACE: {0}", ex.StackTrace);
                    }
                    sb.AppendLine();

                    string logText = sb.ToString();
                    System.Diagnostics.Debug.Write(logText);
                    Console.Write(logText);

                    if (!string.IsNullOrEmpty(_logFilePath))
                    {
                        File.AppendAllText(_logFilePath, logText, Encoding.UTF8);
                    }
                }
            }
            catch
            {
                // Never allow logging failures to crash application
            }
        }

        public static string GetLogFilePath()
        {
            return _logFilePath;
        }
    }
}
