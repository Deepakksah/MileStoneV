using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Server.Admin.Utilities;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Tasks;

public class DatabaseUserTask
{
	public ILogger LogComponent { get; public set; }

	public DatabaseUserTask(ILogger logComponent)
	{
		LogComponent = logComponent;
	}

	public void AddUser(IDatabaseConnectionInfo connectionInfo, AccountInfo serviceAccount)
	{
		if (string.IsNullOrEmpty(connectionInfo.ServerName))
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Server_name_is_null_or_empty, "ServerName");
		}
		if (string.IsNullOrEmpty(connectionInfo.DatabaseName))
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Database_name_is_null_or_empty, "DatabaseName");
		}
		if (serviceAccount == null)
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Service_account_is_null, "serviceAccount");
		}
		using (DatabaseUtils databaseUtils = new DatabaseUtils(new SqlConnectionManager(connectionInfo, LogComponent), LogComponent))
		{
			databaseUtils.AddDatabaseAccount(serviceAccount);
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, DatabaseUserTask_Resources.Added_user_to_database_User, serviceAccount.AccountName, connectionInfo.ServerName, connectionInfo.DatabaseName), Array.Empty<object>());
	}

	public void AddUserToAdmin(IDatabaseConnectionInfo connectionInfo, AccountInfo serviceAccount)
	{
		if (string.IsNullOrEmpty(connectionInfo.ServerName))
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Server_name_is_null_or_empty, "ServerName");
		}
		if (string.IsNullOrEmpty(connectionInfo.DatabaseName))
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Database_name_is_null_or_empty, "DatabaseName");
		}
		if (serviceAccount == null)
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Service_account_is_null, "serviceAccount");
		}
		using (DatabaseUtils databaseUtils = new DatabaseUtils(new SqlConnectionManager(connectionInfo, LogComponent), LogComponent))
		{
			databaseUtils.SetAdminRoleAccount(serviceAccount);
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, DatabaseUserTask_Resources.Added_user_to_database_User, serviceAccount.AccountName, connectionInfo.ServerName, connectionInfo.DatabaseName), Array.Empty<object>());
	}

	public void RemoveUser(IDatabaseConnectionInfo connectionInfo, AccountInfo serviceAccount)
	{
		if (string.IsNullOrEmpty(connectionInfo.ServerName))
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Server_name_is_null_or_empty, "ServerName");
		}
		if (string.IsNullOrEmpty(connectionInfo.DatabaseName))
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Database_name_is_null_or_empty, "DatabaseName");
		}
		if (serviceAccount == null)
		{
			throw new ArgumentException(DatabaseUserTask_Resources.Service_account_is_null, "serviceAccount");
		}
		using (DatabaseUtils databaseUtils = new DatabaseUtils(new SqlConnectionManager(connectionInfo, LogComponent), LogComponent))
		{
			databaseUtils.RemoveDatabaseAccount(serviceAccount);
		}
		LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, DatabaseUserTask_Resources.Removed_user_from_database, serviceAccount.AccountName, connectionInfo.ServerName, connectionInfo.DatabaseName), Array.Empty<object>());
	}
}
