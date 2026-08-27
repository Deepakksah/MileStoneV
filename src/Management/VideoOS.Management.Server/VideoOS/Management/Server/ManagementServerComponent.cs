using System;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public class ManagementServerComponent : GeminiResourceComponent, IManagementServerComponent, IDisposable
{
	private static class ServerInformationColumns
	{
		private static DatabaseColumnBinder SiteId = new DatabaseColumnBinder("IDSite");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Addresses = new DatabaseColumnBinder("Addresses");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder MasterSite = new DatabaseColumnBinder("IDSiteMaster");

		private static DatabaseColumnBinder LastStatusHandshake = new DatabaseColumnBinder("LastStatusHandshake");

		private static DatabaseColumnBinder SynchronizationStatus = new DatabaseColumnBinder("SynchronizationStatus");

		private static DatabaseColumnBinder PrivateViewGroupId = new DatabaseColumnBinder("PrivateViewGroupId");

		public static ServerInformation Bind(DatabaseDataReader reader)
		{
			string serverAddressXml = Addresses.GetString(reader);
			return new ServerInformation
			{
				Id = SiteId.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				MasterSite = MasterSite.GetString(reader, allowNulls: true, string.Empty),
				LastStatusHandshake = LastStatusHandshake.GetDateTime(reader),
				SynchronizationStatus = SynchronizationStatus.GetInt32(reader),
				Version = GeminiApplicationCore.ServerVersion(),
				Addresses = ServerAddresses.GetFromXml(serverAddressXml).ToArray(),
				PrivateViewGroupId = PrivateViewGroupId.GetString(reader, allowNulls: true)
			};
		}
	}

	private static class CreatedColumns
	{
		private static DatabaseColumnBinder Created = new DatabaseColumnBinder("Created");

		public static bool Bind(DatabaseDataReader reader)
		{
			return Created.GetBoolean(reader);
		}
	}

	public ServerInformation GetServerInformation(string userSid, out bool privateViewGroupCreated)
	{
		ServerInformation result;
		(result, privateViewGroupCreated) = base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_information");
			BindSIDType("@UserSid", userSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			ServerInformation item = ServerInformationColumns.Bind(databaseDataReader);
			if (!databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			databaseDataReader.Read();
			bool item2 = CreatedColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return (serverInformationResult: item, privateViewGroupCreatedResult: item2);
		});
		return result;
	}

	public string GetSystemSetting(SettingList setting)
	{
		return GetSystemSetting(setting.ToString());
	}

	public string GetSystemSetting(string setting)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_system_setting");
			BindNameType("@settingName", setting);
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Setting");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (databaseDataReader.Read())
			{
				string result = databaseColumnBinder.GetString(databaseDataReader);
				if (databaseDataReader.NextResult())
				{
					throw new DatabaseConfigExtraRowsException();
				}
				return result;
			}
			return string.Empty;
		});
	}

	public bool SetSystemSetting(string setting, string value)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_system_setting");
			BindNameType("@settingName", setting);
			BindText("@settingValue", value, allowNull: true);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return false;
			}
			int @int = new DatabaseColumnBinder("RowCount").GetInt32(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return @int > 0;
		});
	}
}
