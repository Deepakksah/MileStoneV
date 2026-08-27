using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class FailoverComponent : GeminiResourceComponent, IFailoverServerComponent, IDisposable
{
	private static class FailoverServerColumns
	{
		private static DatabaseColumnBinder IdFailoverServer = new DatabaseColumnBinder("IDFailoverServer");

		private static DatabaseColumnBinder HotStandbyRecorderId = new DatabaseColumnBinder("HotStandbyRecorderId");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Position = new DatabaseColumnBinder("GroupSequence");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("HostName");

		private static DatabaseColumnBinder UdpPort = new DatabaseColumnBinder("UdpPort");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder DatabaseLocation = new DatabaseColumnBinder("DatabaseLocation");

		private static DatabaseColumnBinder CertificateConfigured = new DatabaseColumnBinder("CertificateConfigured");

		private static DatabaseColumnBinder WebServerHostName = new DatabaseColumnBinder("WebServerHostName");

		private static DatabaseColumnBinder WebServerPort = new DatabaseColumnBinder("WebServerPort");

		private static DatabaseColumnBinder PublicAccessEnabled = new DatabaseColumnBinder("PublicAccessEnabled");

		private static DatabaseColumnBinder PublicWebServerHostName = new DatabaseColumnBinder("PublicWebServerHostName");

		private static DatabaseColumnBinder PublicWebServerPort = new DatabaseColumnBinder("PublicWebServerPort");

		private static DatabaseColumnBinder RecordingServerServiceId = new DatabaseColumnBinder("IDRecordingServerService");

		private static DatabaseColumnBinder RelationIds = new DatabaseColumnBinder("RelationIds");

		private static DatabaseColumnBinder MulticastServerAddress = new DatabaseColumnBinder("MulticastServerAddress");

		public static FailoverServer Bind(DatabaseDataReader reader)
		{
			return new FailoverServer
			{
				Id = IdFailoverServer.GetString(reader, allowNulls: false),
				HotStandbyRecorderId = (HotStandbyRecorderId.GetString(reader, allowNulls: true) ?? Guid.Empty.ToString()),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				Position = Position.GetInt16(reader),
				LastModified = LastModified.GetDateTime(reader),
				RelationIds = RelationIds.GetString(reader),
				HostName = HostName.GetString(reader, allowNulls: true),
				UdpPort = UdpPort.GetInt32(reader),
				DatabasePath = DatabaseLocation.GetString(reader, allowNulls: true, string.Empty),
				ClientEncryptionEnabled = CertificateConfigured.GetBoolean(reader),
				WebServerHostName = WebServerHostName.GetString(reader),
				WebServerPort = WebServerPort.GetInt32(reader),
				PublicAccessEnabled = PublicAccessEnabled.GetBoolean(reader),
				PublicWebServerHostName = PublicWebServerHostName.GetString(reader),
				PublicWebServerPort = PublicWebServerPort.GetInt32(reader),
				MulticastServerAddress = MulticastServerAddress.GetString(reader),
				Enabled = Enabled.GetBoolean(reader),
				ServiceId = RecordingServerServiceId.GetString(reader, allowNulls: false)
			};
		}
	}

	public Guid QueryAssociatedRecorderId(string failoverId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_failover_main_recorder");
			BindGuidType("@IDFailoverServer", failoverId);
			string text = (string)ExecuteScalar();
			return (text == null) ? Guid.Empty : new Guid(text);
		});
	}

	public FailoverServer[] QueryFailoverServers(string parentId, bool useServerGroupFilter)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_failover_servers");
			BindBoolean("@FilterOnServerGroup", useServerGroupFilter);
			BindGuidType("@IDServerGroup", parentId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<FailoverServer> list = new List<FailoverServer>();
			while (databaseDataReader.Read())
			{
				list.Add(FailoverServerColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public FailoverServer QueryFailoverServer(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_failover_server_by_id");
			BindGuidType("@IDFailoverServer", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (databaseDataReader.Read())
			{
				FailoverServer result = FailoverServerColumns.Bind(databaseDataReader);
				if (databaseDataReader.Read() || databaseDataReader.NextResult())
				{
					throw new DatabaseConfigExtraRowsException();
				}
				return result;
			}
			return (FailoverServer)null;
		});
	}

	public FailoverServer[] QueryFailoverServersByIds(string[] failoverServerIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_failover_server_by_ids");
			BindIdTable("@IDFailoverServerIds", failoverServerIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<FailoverServer> list = new List<FailoverServer>();
			int num = 0;
			while (databaseDataReader.Read())
			{
				list.Add(FailoverServerColumns.Bind(databaseDataReader));
				num++;
			}
			if (num != failoverServerIds.Length)
			{
				throw new FailoverServerNotFoundException();
			}
			return list.ToArray();
		});
	}

	public string CreateFailoverServer(string parentId, FailoverServer failoverServer)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			if (string.IsNullOrEmpty(failoverServer.Id))
			{
				failoverServer.Id = Guid.NewGuid().ToString();
			}
			PrepareStoredProcedure("create_failover_server");
			BindGuidType("@pkid", failoverServer.Id);
			BindGuidType("@ServiceId", failoverServer.ServiceId);
			BindGuidType("@parent_id", parentId);
			BindNameType("@Name", failoverServer.Name);
			BindDescription("@Description", failoverServer.Description);
			BindNameType("@HostName", failoverServer.HostName);
			BindInt("@GroupSequence", failoverServer.Position);
			BindInt("@UdpPort", failoverServer.UdpPort);
			BindDescription("@DatabaseLocation", failoverServer.DatabasePath);
			BindBoolean("@CertificateConfigured", failoverServer.ClientEncryptionEnabled);
			BindNameType("@WebServerHostName", failoverServer.WebServerHostName);
			BindInt("@WebServerPort", failoverServer.WebServerPort);
			ExecuteNonQuery();
			return failoverServer.Id;
		});
	}

	public void UpdateFailoverServer(FailoverServer failoverServer)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_failover_server");
			BindGuidType("@IDFailoverServer", failoverServer.Id);
			BindNameType("@Name", failoverServer.Name);
			BindDescription("@Description", failoverServer.Description);
			BindGuidType("@HotStandbyRecorderId", failoverServer.HotStandbyRecorderId);
			BindNameType("@HostName", failoverServer.HostName);
			BindInt("@UdpPort", failoverServer.UdpPort);
			BindDescription("@DatabaseLocation", failoverServer.DatabasePath);
			BindBoolean("@PublicAccessEnabled", failoverServer.PublicAccessEnabled);
			BindNameType("@PublicWebServerHostName", failoverServer.PublicWebServerHostName);
			BindInt("@PublicWebServerPort", failoverServer.PublicWebServerPort);
			BindNameType("@MulticastServerAddress", failoverServer.MulticastServerAddress);
			BindBoolean("@Enabled", failoverServer.Enabled);
			ExecuteNonQuery();
		});
	}

	public void DeleteFailoverServer(string failoverServerId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_failover_server");
			BindGuidType("@IDFailoverServer", failoverServerId);
			ExecuteNonQuery();
		});
	}

	public void MoveToGroup(string id, string targetGroupId, int insertAtPosition)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("move_failover_server");
			BindGuidType("@IDFailoverServer", id);
			BindGuidType("@IDTargetGroup", targetGroupId);
			BindShort("@InsertAtPosition", (short)insertAtPosition);
			ExecuteNonQuery();
		});
	}

	public void SetEnabled(string id, bool enabled)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("change_failover_server_enabled");
			BindGuidType("@IDFailoverServer", id);
			BindBoolean("@Enabled", enabled);
			ExecuteNonQuery();
		});
	}

	public void SetRecorderInformation(string id, bool clientEncryptionEnabled, string webserverHostName, int webserverPort, string version, string timeZone)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("set_failover_recorder_information");
			BindGuidType("@IDFailoverServer", id);
			BindBoolean("@CertificateConfigured", clientEncryptionEnabled);
			BindNameType("@WebServerHostName", webserverHostName);
			BindInt("@WebServerPort", webserverPort);
			BindNameType("@Version", version);
			BindNameType("@TimeZoneName", timeZone);
			ExecuteNonQuery();
		});
	}
}
