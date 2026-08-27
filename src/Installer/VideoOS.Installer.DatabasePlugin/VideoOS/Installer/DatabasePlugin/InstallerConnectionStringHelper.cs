using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public static class InstallerConnectionStringHelper
{
	public static string GetConnectionString(SqlConnectionServerType serverType, ISqlConnectionReadonlyStorage legacyConnectionStorage)
	{
		return GetConnectionString(new DatabaseConnectionInfoFromStorage(serverType), legacyConnectionStorage);
	}

	public static string GetConnectionString(IDatabaseConnectionInfo databaseConnectionInfo, ISqlConnectionReadonlyStorage legacyConnectionStorage)
	{
		string text = string.Empty;
		try
		{
			text = databaseConnectionInfo.ConnectionString;
			if (string.IsNullOrEmpty(text) && legacyConnectionStorage != null)
			{
				text = legacyConnectionStorage.RetrieveConnectionString();
				if (!string.IsNullOrEmpty(text))
				{
					DatabaseConnectionInfo databaseConnectionInfo2 = new DatabaseConnectionInfo(text);
					text = new DatabaseConnectionInfo(databaseConnectionInfo2.ServerName, databaseConnectionInfo2.DatabaseName, AuthenticationType.IntegratedSecurityTrusted).ConnectionString;
				}
			}
		}
		catch
		{
		}
		return text;
	}
}
