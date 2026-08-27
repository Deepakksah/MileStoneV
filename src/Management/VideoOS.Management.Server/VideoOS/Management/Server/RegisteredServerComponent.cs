using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RegisteredServerComponent : GeminiResourceComponent, IRegisteredServerComponent, IDisposable
{
	private static class RegisteredServerColumns
	{
		private static DatabaseColumnBinder IDServer = new DatabaseColumnBinder("IDServer");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("Type");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Addresses = new DatabaseColumnBinder("Addresses");

		private static DatabaseColumnBinder Trusted = new DatabaseColumnBinder("Trusted");

		private static DatabaseColumnBinder CustomData = new DatabaseColumnBinder("CustomData");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static DatabaseColumnBinder EndpointDefinitions = new DatabaseColumnBinder("EndpointDefinitions");

		public static RegisteredServer Bind(DatabaseDataReader reader)
		{
			return new RegisteredServer
			{
				IDServer = new Guid(IDServer.GetGuid(reader)),
				Type = new Guid(Type.GetGuid(reader)),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				Addresses = ServerAddresses.GetFromXml(Addresses.GetString(reader)).ToArray(),
				Trusted = Trusted.GetBoolean(reader),
				CustomData = CustomData.GetString(reader),
				Enabled = Enabled.GetBoolean(reader),
				DataCollectorId = DataCollectorId.GetInt32(reader),
				EndpointDefition = EndpointDefinitions.GetString(reader)
			};
		}
	}

	public RegisteredServer QueryRegisteredServer(Guid serverId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_registered_server_by_id");
			BindUniqueIdentifier("@ServiceId", serverId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RegisteredServer)null;
			}
			RegisteredServer result = RegisteredServerColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public RegisteredServer[] QueryRegisteredServersByType(Guid serviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_registered_server");
			BindUniqueIdentifier("@ServiceType", serviceType);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RegisteredServer> list = new List<RegisteredServer>();
			while (databaseDataReader.Read())
			{
				list.Add(RegisteredServerColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public RegisteredServer[] QueryAllRegisteredServers(bool includeDisabled)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_all_registered_servers");
			BindBoolean("@IncludeDisabled", includeDisabled);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RegisteredServer> list = new List<RegisteredServer>();
			while (databaseDataReader.Read())
			{
				list.Add(RegisteredServerColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RegisteredServer[] QueryRegisteredServersWithoutDataCollectorId(string[] ruleServiceTypes)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_registered_servers_without_data_collector");
			BindIdTable("@ServiceTypes", ruleServiceTypes);
			List<RegisteredServer> list = new List<RegisteredServer>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(RegisteredServerColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UpdateDataCollectors(Guid serverId, int dataCollectorId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_datacollectors_for_registered_servers");
			BindUniqueIdentifier("@ServerId", serverId);
			BindInt("@DataColletorId", dataCollectorId);
			ExecuteNonQuery();
		});
	}

	public void EnableService(Guid serverId, bool enabled)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_registered_server_enable");
			BindUniqueIdentifier("@ServiceId", serverId);
			BindBoolean("@Enabled", enabled);
			ExecuteNonQuery();
		});
	}

	public void RegisterService(Guid serviceType, Guid serverId, ServerAddress[] addresses, string name, string description, string customData, string endpointDefinition)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("register_registered_server");
			BindUniqueIdentifier("@ServiceId", serverId);
			BindUniqueIdentifier("@ServiceType", serviceType);
			BindStringMax("@Addresses", ServerAddresses.GetAsXml(addresses));
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			BindStringMax("@CustomData", customData);
			BindStringMax("@EndpointDefinition", endpointDefinition);
			ExecuteNonQuery();
		});
	}

	public void UnregisterService(Guid serverId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("unregister_registered_server");
			BindUniqueIdentifier("@ServiceId", serverId);
			ExecuteNonQuery();
		});
	}

	public void SetServiceData(Guid serverId, string customData)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_registered_server_custom_data");
			BindUniqueIdentifier("@ServiceId", serverId);
			BindStringMax("@CustomData", customData);
			ExecuteNonQuery();
		});
	}
}
