using System;
using System.Data;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Tasks;

public class RegisterServiceTask
{
	public ILogger LogComponent { get; public set; }

	public RegisterServiceTask(ILogger logComponent)
	{
		LogComponent = logComponent;
	}

	public void Register(IDatabaseConnectionInfo connectionInfoMs, Guid serviceTypeId, string serviceName, Uri serviceUrl)
	{
		if (serviceUrl == null)
		{
			throw new ArgumentNullException("serviceUrl");
		}
		if (string.IsNullOrEmpty(serviceName))
		{
			throw new ArgumentException("Service is null or empty", "serviceName");
		}
		if (connectionInfoMs == null)
		{
			throw new ArgumentException("Connection info is null", "connectionInfoMs");
		}
		new SqlConnectionManager(connectionInfoMs, LogComponent).ExecuteRetryable(delegate(DatabaseConnection databaseConnection)
		{
			bool flag = false;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand())
			{
				databaseCommand.CommandType = CommandType.Text;
				databaseCommand.CommandText = "SELECT * FROM [RegisteredServers] WHERE Type = @Type";
				databaseCommand.AddParameter("@Type", SqlDbType.Char, 36, serviceTypeId.ToString());
				using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleResult);
				if (databaseDataReader.HasRows)
				{
					flag = true;
					LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, RegisterServiceTask_Resources.The_service_is_already_registered, serviceName, serviceTypeId), Array.Empty<object>());
				}
			}
			if (!flag)
			{
				using (DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand())
				{
					string value = string.Format(CultureInfo.InvariantCulture, "<Addresses><Address url=\"{0}\" type=\"Internal\" /></Addresses>", serviceUrl.ToString());
					databaseCommand2.CommandType = CommandType.Text;
					databaseCommand2.CommandText = "INSERT [RegisteredServers] ([IDServer], [Name], [Description], [Type], [Addresses], [Trusted], [CustomData], [Enabled], [LastModified]) VALUES (@IDServer, @Name, @Description, @Type, @Addresses,  1, '',  1,  GETUTCDATE())";
					databaseCommand2.AddParameter("@IDServer", SqlDbType.Char, 36, Guid.NewGuid().ToString());
					databaseCommand2.AddParameter("@Name", SqlDbType.NVarChar, 256, serviceName);
					databaseCommand2.AddParameter("@Description", SqlDbType.NVarChar, 256, serviceName + " registered by ServerAdminCmd.");
					databaseCommand2.AddParameter("@Type", SqlDbType.Char, 36, serviceTypeId.ToString());
					databaseCommand2.AddParameter("@Addresses", SqlDbType.NVarChar, 0, value);
					databaseCommand2.ExecuteNonQuery();
					LoggerExtensions.LogDebug(LogComponent, string.Format(CultureInfo.InvariantCulture, RegisterServiceTask_Resources.Service_registered, serviceName, serviceTypeId), Array.Empty<object>());
				}
			}
		});
	}
}
