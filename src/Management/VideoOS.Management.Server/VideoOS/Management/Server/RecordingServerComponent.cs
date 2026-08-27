using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RecordingServerComponent : GeminiResourceComponent, IRecordingServerComponent, IDisposable
{
	private static class RecordingServerServiceInfoColumns
	{
		private static DatabaseColumnBinder Pkid = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder ServerType = new DatabaseColumnBinder("server_type");

		private static DatabaseColumnBinder ServerId = new DatabaseColumnBinder("server_id");

		public static RecordingServerServiceInfo Bind(DatabaseDataReader reader)
		{
			RecordingServerServiceInfo obj = new RecordingServerServiceInfo
			{
				Id = Pkid.GetGuid(reader)
			};
			string enumName = ServerType.GetString(reader);
			obj.ServerType = StringToEnum<RecordingServerServiceInfo.ServiceServerType>(enumName);
			obj.ServerId = ServerId.GetGuid(reader);
			return obj;
		}

		private static T StringToEnum<T>(string enumName)
		{
			return (T)Enum.Parse(typeof(T), enumName, ignoreCase: true);
		}
	}

	private static class RecordingServerColumns
	{
		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder SynchronizationTime = new DatabaseColumnBinder("SynchronizationTime");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("host_name");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder CertificateConfigured = new DatabaseColumnBinder("certificate_configured");

		private static DatabaseColumnBinder WebServerHostName = new DatabaseColumnBinder("webserver_hostname");

		private static DatabaseColumnBinder WebServerPort = new DatabaseColumnBinder("webserver_port");

		private static DatabaseColumnBinder PublicAccessEnabled = new DatabaseColumnBinder("PublicAccessEnabled");

		private static DatabaseColumnBinder PublicWebServerHostName = new DatabaseColumnBinder("PublicWebServerHostName");

		private static DatabaseColumnBinder PublicWebServerPort = new DatabaseColumnBinder("PublicWebServerPort");

		private static DatabaseColumnBinder ScanRanges = new DatabaseColumnBinder("scan_ranges");

		private static DatabaseColumnBinder TimeZoneName = new DatabaseColumnBinder("time_zone_name");

		private static DatabaseColumnBinder IDPrimaryFailoverGroup = new DatabaseColumnBinder("IDFailoverPrimaryGroup");

		private static DatabaseColumnBinder IDSecondaryFailoverGroup = new DatabaseColumnBinder("IDFailoverSecondaryGroup");

		private static DatabaseColumnBinder FailoverPort = new DatabaseColumnBinder("failover_udp_port");

		private static DatabaseColumnBinder FailoverHostName = new DatabaseColumnBinder("failover_host_name");

		private static DatabaseColumnBinder MulticastSettings = new DatabaseColumnBinder("MulticastSettings");

		private static DatabaseColumnBinder DevicePackInfo = new DatabaseColumnBinder("devicepack_info");

		private static DatabaseColumnBinder X64 = new DatabaseColumnBinder("x64");

		private static DatabaseColumnBinder FailoverId = new DatabaseColumnBinder("failover_server_id");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder MulticastServerAddress = new DatabaseColumnBinder("MulticastServerAddress");

		private static DatabaseColumnBinder ShutdownOnStorageFailure = new DatabaseColumnBinder("ShutdownOnStorageFailure");

		private static DatabaseColumnBinder ActivePublicAccessEnabled = new DatabaseColumnBinder("ActivePublicAccessEnabled");

		private static DatabaseColumnBinder ActivePublicWebServerHostName = new DatabaseColumnBinder("ActivePublicWebServerHostName");

		private static DatabaseColumnBinder ActivePublicWebServerPort = new DatabaseColumnBinder("ActivePublicWebServerPort");

		private static DatabaseColumnBinder Version = new DatabaseColumnBinder("version");

		public static RecordingServer Bind(DatabaseDataReader reader)
		{
			return new RecordingServer
			{
				Id = IDRecorder.GetString(reader, allowNulls: false),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				SynchronizationTime = SynchronizationTime.GetInt32(reader, allowNulls: false, -1),
				LastModified = LastModified.GetDateTime(reader),
				Enabled = Enabled.GetBoolean(reader),
				TimeZoneName = TimeZoneName.GetString(reader, allowNulls: true),
				HostName = HostName.GetString(reader),
				MulticastSettings = MulticastSettings.GetString(reader, allowNulls: true),
				DevicePackInfo = DevicePackInfo.GetString(reader, allowNulls: true),
				X64 = X64.GetBoolean(reader, allowNulls: false, defaultValue: true),
				FailoverHostName = FailoverHostName.GetString(reader, allowNulls: true),
				FailoverPort = FailoverPort.GetInt32(reader, allowNulls: true, 11000),
				PrimaryFailoverGroup = IDPrimaryFailoverGroup.GetString(reader, allowNulls: true),
				SecondaryFailoverGroup = IDSecondaryFailoverGroup.GetString(reader, allowNulls: true),
				FailoverId = FailoverId.GetString(reader, allowNulls: true),
				ScanRanges = ScanRanges.GetString(reader, allowNulls: true),
				ScanRangeWarning = 255,
				CertificateConfigured = CertificateConfigured.GetBoolean(reader),
				WebServerHostName = WebServerHostName.GetString(reader),
				WebServerPort = WebServerPort.GetInt32(reader),
				ActivePublicAccessEnabled = ActivePublicAccessEnabled.GetBoolean(reader),
				ActivePublicWebServerHostName = ActivePublicWebServerHostName.GetString(reader),
				ActivePublicWebServerPort = ActivePublicWebServerPort.GetInt32(reader),
				PublicAccessEnabled = PublicAccessEnabled.GetBoolean(reader),
				PublicWebServerHostName = PublicWebServerHostName.GetString(reader),
				PublicWebServerPort = PublicWebServerPort.GetInt32(reader),
				MulticastServerAddress = MulticastServerAddress.GetString(reader),
				ShutdownOnStorageFailure = ShutdownOnStorageFailure.GetBoolean(reader),
				Version = Version.GetString(reader)
			};
		}
	}

	public string CreateRecordingServer(string id, string name, string hostName, string webserverHostName, int webserverPort, string timeZoneName, string scanRanges, bool isMulticastLicensed, int failoverUdpPort)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("recording_server_create_or_update");
			BindGuidType("@RecorderId", id);
			BindGuidType("@ServiceId", id);
			BindInt("@Type", 0);
			BindNameType("@Name", name);
			BindNameType("@HostName", hostName);
			BindBoolean("@CertificateConfigured", parameterValue: false);
			BindBoolean("@IsMulticastLicensed", isMulticastLicensed);
			BindNameType("@WebServerHostName", webserverHostName);
			BindInt("@WebServerPort", webserverPort);
			BindText("@ScanRanges", scanRanges, allowNull: false);
			BindNameType("@TimeZoneName", timeZoneName);
			BindText("@DevicePackInfo", "", allowNull: false);
			BindInt("@FailoverUdpPort", failoverUdpPort);
			BindBoolean("@authorized", parameterValue: true);
			BindInt("@Databaseport", 5210);
			BindGuidType("@FailoverId", Guid.Empty.ToString());
			BindBoolean("@FailOnUpdate", parameterValue: true);
			ExecuteScalar();
			return id;
		});
	}

	public RecordingServer QueryRecordingServer(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_by_id");
			BindGuidType("@IDRecorder", recordingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RecordingServer)null;
			}
			RecordingServer result = RecordingServerColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public RecordingServer[] QueryRecordingServersByIds(string[] recorderIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_servers_by_ids");
			BindIdTable("@IDRecorderIds", recorderIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RecordingServer> list = new List<RecordingServer>();
			while (databaseDataReader.Read())
			{
				list.Add(RecordingServerColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryRecordingServerIds(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_servers_ids");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRecorder");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public int QueryEnabledRecordingServersCount()
	{
		string[] value = new string[3] { "SELECT COUNT([IDRecorder])", "FROM   [Recorders]", "WHERE  [Enabled] = 1" };
		string sql = string.Join("\r\n", value);
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			return (int)ExecuteScalar();
		});
	}

	public string[] QueryRecordingServerIds()
	{
		return QueryRecordingServerIds("909749AE-88C4-4780-A381-383D691E98FA");
	}

	public void UpdateRecordingServer(string recordingServerId, string name, string description, int? synchronizationTime, CustomProperty[] customProperties, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_server");
			BindGuidType("@IDRecorder", recordingServerId);
			BindNameType("@Name", name, allowNull: true);
			BindDescription("@Description", description, allowNull: true);
			BindInt("@SynchronizationTime", synchronizationTime);
			BindCustomProperties("@custom_properties", customProperties);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void UpdateRecordingServerMulticastSettings(string recordingServerId, string multicastSettings)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_server_multicast");
			BindGuidType("@IDRecorder", recordingServerId);
			BindXml("@multicastSettings", multicastSettings, allowNull: false);
			ExecuteNonQuery();
		});
	}

	public void UpdateRecordingServerFailoverSettings(string recordingServerId, string primaryFailoverGroupId, string secondaryFailoverGroupId, int failoverPorts)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_server_failover");
			BindGuidType("@IDRecorder", recordingServerId);
			BindGuidType("@IDPrimary", primaryFailoverGroupId);
			BindGuidType("@IDSecondary", secondaryFailoverGroupId);
			BindInt("@Port", failoverPorts);
			ExecuteNonQuery();
		});
	}

	public void UpdateRecordingServerServiceConfigurable(string recordingServerId, bool? publicAccessEnabled, string publicWebAddress, int? publicWebPort, bool? shutdownOnStorageFailure, string multicastServerAddress)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("recording_server_service_configurable_update");
			BindGuidType("@IDRecorder", recordingServerId);
			BindBoolean("@EnableAccess", publicAccessEnabled);
			BindNameType("@WebAddress", publicWebAddress, allowNull: true);
			BindInt("@WebPort", publicWebPort);
			BindBoolean("@ShutdownOnStorageFailure", shutdownOnStorageFailure);
			BindNameType("@MulticastServerAddress", multicastServerAddress);
			ExecuteNonQuery();
		});
	}

	public string QueryRecordingServerHostName(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT host_name FROM [RecordingServerService] rss");
			AddStatement("INNER JOIN [RecordingServerServiceRecorder] rssr ON rss.pkid = rssr.[recording_server_service_id]");
			AddStatement("WHERE rssr.[recorder_id] = @recordingServerId", 1);
			BindGuidType("@recordingServerId", recordingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new RecordingServerNotFoundException();
			}
			string result = databaseDataReader.GetString(0);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string GetIdFromDeviceId(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT h.[IDRecorder] ");
			stringBuilder.Append("FROM [Hardware] h ");
			stringBuilder.Append("INNER JOIN [Devices] d ON d.[IDHardware] = h.[IDHardware] ");
			stringBuilder.Append("WHERE d.[IDDevice] = @IDDevice");
			PrepareSqlBatch();
			AddStatement(stringBuilder.ToString());
			BindGuidType("@IDDevice", deviceId);
			return (string)ExecuteScalar();
		});
	}

	public Guid[] GetIdsFromHostingServerId(int hostingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_servers_by_hosting_server");
			BindInt("@pkid", hostingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Guid> list = new List<Guid>();
			while (databaseDataReader.Read())
			{
				string g = databaseDataReader.GetString(0);
				Guid item = new Guid(g);
				list.Add(item);
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid? GetIdFromhostedHardwareId(int hostedHardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_by_hosted_hardware");
			BindInt("@pkid", hostedHardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Guid?)null;
			}
			string g = databaseDataReader.GetString(0);
			return new Guid(g);
		});
	}

	public string QueryRecordingServerDiskUsage(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_statusdata");
			BindGuidType("@IDRecorder", recordingServerId);
			BindGuidType("@Type", "DiskUsage");
			return (string)ExecuteScalar();
		});
	}

	public string QueryRecordingServerIdFromRecordingServerServiceId(string recordingServerServiceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_id_by_service_id");
			BindGuidType("@recording_server_service_id", recordingServerServiceId);
			return (string)ExecuteScalar();
		});
	}

	public RecordingServerServiceInfo QueryRecordingServerServiceInfo(string recordingServerServiceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_service_info");
			BindGuidType("@recording_server_service_id", recordingServerServiceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RecordingServerServiceInfo)null;
			}
			RecordingServerServiceInfo result = RecordingServerServiceInfoColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
