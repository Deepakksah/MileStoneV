using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common.Utilities.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.Common.Utilities;

public sealed class ConfigurationDataComponent : IConfigurationDataComponent, IDisposable
{
	private readonly string connectionString;

	private readonly ILogger logger;

	public ConfigurationDataComponent(string connectionString, ILogger logger)
	{
		if (connectionString == null)
		{
			throw new ArgumentNullException("connectionString");
		}
		this.connectionString = connectionString;
		this.logger = logger;
	}

	public int QueryRecordingServerCountPre2020R1()
	{
		try
		{
			DatabaseConnection connection = new DatabaseConnection(connectionString, logger);
			try
			{
				return connection.ExecuteRetryable(delegate
				{
					using DatabaseCommand databaseCommand = connection.CreateCommand(SqlResources.CheckForLegacyRecordingServers);
					databaseCommand.CommandType = CommandType.Text;
					return (int)databaseCommand.ExecuteScalar();
				});
			}
			finally
			{
				if (connection != null)
				{
					((IDisposable)connection).Dispose();
				}
			}
		}
		catch (Exception inner)
		{
			throw new ConfigurationDataException("Query to database failed. See inner exception for details.", inner);
		}
	}

	public (bool HasAnyDuplicateDevices, bool HasImportantDuplicateDevices, bool HasDuplicateRecordingDevices) QueryDuplicateDevicesInfo()
	{
		try
		{
			DatabaseConnection connection = new DatabaseConnection(connectionString, logger);
			try
			{
				return connection.ExecuteRetryable(delegate
				{
					using DatabaseCommand databaseCommand = connection.CreateCommand(SqlResources.QueryDuplicateDevicesInfo);
					databaseCommand.CommandType = CommandType.Text;
					List<(bool, int)> list = new List<(bool, int)>();
					using (DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader())
					{
						while (databaseDataReader.Read())
						{
							list.Add((databaseDataReader.GetBoolean(0), databaseDataReader.GetInt32(1)));
						}
					}
					return (HasAnyDuplicateDevices: list.Any(), HasImportantDuplicateDevices: list.Any<(bool, int)>(((bool IsRecordingDevice, int Importance) r) => r.Importance > 0), HasDuplicateRecordingDevices: list.Any<(bool, int)>(((bool IsRecordingDevice, int Importance) r) => r.IsRecordingDevice));
				});
			}
			finally
			{
				if (connection != null)
				{
					((IDisposable)connection).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			if (ex is DatabaseException && ((DatabaseException)ex).IsObjectNotFoundException)
			{
				return (HasAnyDuplicateDevices: false, HasImportantDuplicateDevices: false, HasDuplicateRecordingDevices: false);
			}
			throw new ConfigurationDataException("Query to database failed. See inner exception for details.", ex);
		}
	}

	public void Dispose()
	{
	}
}
