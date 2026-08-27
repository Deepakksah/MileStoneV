using System;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Network;
using VideoOS.Vms.BackupRestore;
using VideoOS.Vms.Utilities.Oem;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server.Admin.Tasks;

public class SetupMiscTask
{
	public ILogger LogComponent { get; public set; }

	public SetupMiscTask(ILogger logComponent)
	{
		LogComponent = logComponent;
	}

	public void Install(IDatabaseConnectionInfo connectionInfoMs, string managementServerInstallDirectory, int managementServerPort, bool isExternallyManaged)
	{
		if (string.IsNullOrEmpty(managementServerInstallDirectory))
		{
			throw new ArgumentException(SetupMiscTask_Resources.Management_server_install_directory_is_null_or_empty, "managementServerInstallDirectory");
		}
		if (managementServerPort <= 0)
		{
			throw new ArgumentException(SetupMiscTask_Resources.Management_server_is_0_or_less, "managementServerPort");
		}
		if (connectionInfoMs == null)
		{
			throw new ArgumentNullException("connectionInfoMs");
		}
		SetConfigFilePath(managementServerInstallDirectory);
		if (!isExternallyManaged)
		{
			CreateBackupShareFolder(connectionInfoMs);
		}
		EnsureServerLanAddress(connectionInfoMs, managementServerPort);
	}

	private void SetConfigFilePath(string managementServerInstallDirectory)
	{
		LoggerExtensions.LogDebug(LogComponent, SetupMiscTask_Resources.Setting_config_file_paths_for_both_32bit_and_64bit_registry, Array.Empty<object>());
		RegistryKeyEx registryKeyEx = RegistryEx.LocalMachine.CreateSubKey("SOFTWARE\\VideoOS\\Server\\Installation", create64BitKey: true);
		registryKeyEx.SetValue("ExternalPluginPath", Path.Combine(managementServerInstallDirectory, "ExternalPlugins"));
		registryKeyEx.SetValue("ClientVersionConfigPath", Path.Combine(managementServerInstallDirectory, "ClientVersion"));
		registryKeyEx.Close();
		RegistryKeyEx registryKeyEx2 = RegistryEx.LocalMachine.CreateSubKey("SOFTWARE\\VideoOS\\Server\\Installation", create64BitKey: false);
		registryKeyEx2.SetValue("ExternalPluginPath", Path.Combine(managementServerInstallDirectory, "ExternalPlugins"));
		registryKeyEx2.SetValue("ClientVersionConfigPath", Path.Combine(managementServerInstallDirectory, "ClientVersion"));
		registryKeyEx2.Close();
	}

	private void CreateBackupShareFolder(IDatabaseConnectionInfo databaseConnectionInfoMs)
	{
		LoggerExtensions.LogDebug(LogComponent, SetupMiscTask_Resources.Creating_backup_share_folder, Array.Empty<object>());
		string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Server.DefaultInstallDir);
		string text = Path.Combine(path, BackupResources.DefaultBackupFolderName);
		DirectoryUtilities.DeleteFiles(text);
		DirectoryUtilities.RemoveShare(BackupResources.DefaultBackupFolderName);
		DirectoryUtilities.DeleteFolder(text);
		string text2 = Path.Combine(path, BackupResources.DefaultBackupFolderName);
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, SetupMiscTask_Resources.Backup_share_folder, text2), Array.Empty<object>());
		CorporateSqlData corporateSqlData = new CorporateSqlData(new SqlConnectionManager(databaseConnectionInfoMs, LogComponent));
		DirectoryUtilities.CreateSharedFolderSqlServerAccount(text2, BackupResources.DefaultBackupFolderName, corporateSqlData);
		LoggerExtensions.LogDebug(LogComponent, SetupMiscTask_Resources.Backup_share_folder_created, Array.Empty<object>());
	}

	private void EnsureServerLanAddress(IDatabaseConnectionInfo databaseConnectionInfoMs, int serverPort)
	{
		LoggerExtensions.LogDebug(LogComponent, SetupMiscTask_Resources.Setting_server_address_in_database_if_it_is_not_present, Array.Empty<object>());
		new SqlConnectionManager(databaseConnectionInfoMs, LogComponent).ExecuteRetryable(delegate(DatabaseConnection connection)
		{
			string text = null;
			string name = Enum.GetName(typeof(SettingList), SettingList.ServerLanAddress);
			using (DatabaseCommand databaseCommand = connection.CreateCommand())
			{
				databaseCommand.CommandType = CommandType.Text;
				databaseCommand.CommandText = "SELECT [Setting] FROM [SystemSettings] WHERE [Name] = @Name";
				databaseCommand.AddParameter("@Name", SqlDbType.NVarChar, 256, name);
				using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
				if (!databaseDataReader.Read())
				{
					LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, SetupMiscTask_Resources.System_setting_was_not_found_in_the_database, name), Array.Empty<object>());
					return;
				}
				text = databaseDataReader.GetString(0);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}/", IPHelper.GetFullCumputerName(), serverPort);
				UriBuilder uriBuilder = new UriBuilder(text);
				LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, SetupMiscTask_Resources.Inserting_new_server_address, text), Array.Empty<object>());
				using DatabaseCommand databaseCommand2 = connection.CreateCommand();
				databaseCommand2.CommandType = CommandType.Text;
				databaseCommand2.CommandText = "UPDATE [SystemSettings] SET [Setting] = @Value WHERE [Name] = @Name";
				databaseCommand2.AddParameter("@Name", SqlDbType.NVarChar, 256, name);
				databaseCommand2.AddParameter("@Value", SqlDbType.NVarChar, 0, uriBuilder.Uri.AbsoluteUri);
				databaseCommand2.ExecuteNonQuery();
				return;
			}
			LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, SetupMiscTask_Resources.Server_address_was_already_present_in_database, text), Array.Empty<object>());
		});
	}
}
