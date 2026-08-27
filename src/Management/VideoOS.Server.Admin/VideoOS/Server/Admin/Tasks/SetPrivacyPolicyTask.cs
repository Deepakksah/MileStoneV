using System;
using System.Data;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Tasks;

public class SetPrivacyPolicyTask
{
	private const string CollectUsageDataEnabled = "CollectUsageDataEnabled";

	public ILogger LogComponent { get; public set; }

	public SetPrivacyPolicyTask(ILogger logComponent)
	{
		LogComponent = logComponent;
	}

	public void Create(IDatabaseConnectionInfo databaseConnectionInfoMs, string collectUsageDataEnabled)
	{
		SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfoMs, LogComponent);
		try
		{
			sqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection sqlConnection)
			{
				using DatabaseCommand databaseCommand = sqlConnection.CreateCommand();
				databaseCommand.CommandType = CommandType.Text;
				databaseCommand.CommandText = "UPDATE [SystemSettings] SET [LastModified] = GETUTCDATE(), [Setting] = @Value WHERE [Name] = @Name";
				databaseCommand.AddParameter("@Name", SqlDbType.NVarChar, 256, "CollectUsageDataEnabled");
				databaseCommand.AddParameter("@Value", SqlDbType.NVarChar, 0, collectUsageDataEnabled);
				databaseCommand.ExecuteNonQuery();
				LoggerExtensions.LogDebug(LogComponent, SetPrivacyPolicyTask_Resources.CollectUsageDataEnabledUpdated, Array.Empty<object>());
			});
		}
		catch (Exception)
		{
			LoggerExtensions.LogDebug(LogComponent, SetPrivacyPolicyTask_Resources.CollectUsageDataEnabledFailedToUpdate, Array.Empty<object>());
		}
	}
}
