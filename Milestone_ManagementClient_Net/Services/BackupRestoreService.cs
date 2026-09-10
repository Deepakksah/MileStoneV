using System;
using System.IO;

namespace MilestoneManagementClient.Services
{
    public class BackupRestoreService
    {
        public bool CreateConfigurationBackup(string backupPath, out string message)
        {
            try
            {
                string dbJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "surveillance_database.json");
                string destDir = Path.GetDirectoryName(backupPath);
                if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                if (File.Exists(dbJson))
                {
                    File.Copy(dbJson, backupPath, true);
                }
                else
                {
                    File.WriteAllText(backupPath, "{ \"backupCreated\": \"" + DateTime.Now.ToString("o") + "\", \"vmsVersion\": \"2025 R3\" }");
                }

                message = "Backup created successfully at: " + backupPath;
                return true;
            }
            catch (Exception ex)
            {
                message = "Backup failed: " + ex.Message;
                return false;
            }
        }

        public bool RestoreConfigurationBackup(string backupPath, out string message)
        {
            try
            {
                if (!File.Exists(backupPath))
                {
                    message = "Specified backup file does not exist.";
                    return false;
                }

                string dbJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "surveillance_database.json");
                File.Copy(backupPath, dbJson, true);
                message = "Configuration restored successfully. System database synchronized.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Restore failed: " + ex.Message;
                return false;
            }
        }
    }
}
