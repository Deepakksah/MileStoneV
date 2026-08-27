using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class RecordingServerComponent : VmoResourceComponent, IRecordingServerComponent, IDisposable
{
	private static class RecordingServerCreatedColumns
	{
		private static DatabaseColumnBinder IsCreated = new DatabaseColumnBinder("is_created");

		private static DatabaseColumnBinder DiffgramCreated = new DatabaseColumnBinder("diffgram_created");

		public static Tuple<bool, bool> Bind(DatabaseDataReader reader)
		{
			return new Tuple<bool, bool>(IsCreated.GetBoolean(reader), DiffgramCreated.GetBoolean(reader));
		}
	}

	private static class RecordingServerColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("recorder_id");

		private static DatabaseColumnBinder ServiceId = new DatabaseColumnBinder("recording_server_service_id");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("type");

		private static DatabaseColumnBinder Active = new DatabaseColumnBinder("active");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder SynchronizationTime = new DatabaseColumnBinder("SynchronizationTime");

		private static DatabaseColumnBinder CertificateConfigured = new DatabaseColumnBinder("certificate_configured");

		private static DatabaseColumnBinder WebServerHostName = new DatabaseColumnBinder("webserver_hostname");

		private static DatabaseColumnBinder WebServerPort = new DatabaseColumnBinder("webserver_port");

		private static DatabaseColumnBinder PublicAccessEnabled = new DatabaseColumnBinder("PublicAccessEnabled");

		private static DatabaseColumnBinder PublicWebServerHostName = new DatabaseColumnBinder("PublicWebServerHostName");

		private static DatabaseColumnBinder PublicWebServerPort = new DatabaseColumnBinder("PublicWebServerPort");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("host_name");

		private static DatabaseColumnBinder ScanRanges = new DatabaseColumnBinder("scan_ranges");

		private static DatabaseColumnBinder TimeZoneName = new DatabaseColumnBinder("time_zone_name");

		private static DatabaseColumnBinder FailoverPrimaryGroupId = new DatabaseColumnBinder("IdFailoverPrimaryGroup");

		private static DatabaseColumnBinder FailoverSecondaryGroupId = new DatabaseColumnBinder("IdFailoverSecondaryGroup");

		private static DatabaseColumnBinder FailoverUdpPort = new DatabaseColumnBinder("failover_udp_port");

		private static DatabaseColumnBinder MulticastSettings = new DatabaseColumnBinder("MulticastSettings");

		private static DatabaseColumnBinder DevicePackInfo = new DatabaseColumnBinder("devicepack_info");

		private static DatabaseColumnBinder X64 = new DatabaseColumnBinder("x64");

		private static DatabaseColumnBinder DatabasePort = new DatabaseColumnBinder("database_port");

		private static DatabaseColumnBinder Version = new DatabaseColumnBinder("version");

		public static RecordingServer Bind(DatabaseDataReader reader)
		{
			RecordingServer recordingServer = new RecordingServer();
			recordingServer.LastModified = LastModified.GetDateTime(reader);
			recordingServer.RecorderId = RecorderId.GetStringAsGuid(reader);
			recordingServer.ServiceId = ServiceId.GetStringAsGuid(reader);
			recordingServer.Type = (RecordingServerServiceType)Type.GetInt32(reader);
			recordingServer.Active = Active.GetBoolean(reader);
			recordingServer.Enabled = Enabled.GetBoolean(reader);
			recordingServer.Name = Name.GetString(reader);
			recordingServer.Description = Description.GetString(reader);
			recordingServer.SynchronizationTime = SynchronizationTime.GetInt32(reader, allowNulls: false, -1);
			recordingServer.CertificateConfigured = CertificateConfigured.GetBoolean(reader);
			recordingServer.WebServerHostName = WebServerHostName.GetString(reader);
			recordingServer.WebServerPort = WebServerPort.GetInt32(reader);
			recordingServer.PublicAccessEnabled = PublicAccessEnabled.GetBoolean(reader);
			recordingServer.PublicWebServerHostName = PublicWebServerHostName.GetString(reader);
			recordingServer.PublicWebServerPort = PublicWebServerPort.GetInt32(reader);
			recordingServer.HostName = HostName.GetString(reader);
			recordingServer.ScanRanges = ScanRanges.GetString(reader);
			recordingServer.TimeZoneName = TimeZoneName.GetString(reader);
			recordingServer.FailoverPrimaryGroupId = FailoverPrimaryGroupId.GetStringAsGuid(reader, allowNulls: true, Guid.Empty);
			recordingServer.FailoverSecondaryGroupId = FailoverSecondaryGroupId.GetStringAsGuid(reader, allowNulls: true, Guid.Empty);
			recordingServer.FailoverUdpPort = Convert.ToUInt16(FailoverUdpPort.GetInt32(reader, allowNulls: true, 11000));
			recordingServer.MulticastSettings = MulticastSettings.GetString(reader);
			recordingServer.DevicePackInfo = DevicePackInfo.GetString(reader);
			recordingServer.X64 = X64.GetBoolean(reader);
			recordingServer.DatabasePort = DatabasePort.GetInt32(reader);
			recordingServer.Version = Version.GetString(reader);
			return recordingServer;
		}
	}

	private static class RecorderServiceVersionColumns
	{
		private static DatabaseColumnBinder ServiceId = new DatabaseColumnBinder("recording_server_service_id");

		private static DatabaseColumnBinder Version = new DatabaseColumnBinder("version");

		public static RecorderServiceVersion Bind(DatabaseDataReader reader)
		{
			RecorderServiceVersion recorderServiceVersion = new RecorderServiceVersion();
			recorderServiceVersion.ServiceId = ServiceId.GetStringAsGuid(reader);
			Version version = System.Version.Parse(Version.GetString(reader));
			recorderServiceVersion.Version = new Version(version.Major, version.Minor, 0, version.Revision);
			return recorderServiceVersion;
		}
	}

	public RecordingServer QueryRecordingServerActive(Guid recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_active");
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? null : RecordingServerColumns.Bind(databaseDataReader);
		});
	}

	public RecorderServiceVersion[] QueryRecorderServiceVersions()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recorder_service_versions");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RecorderServiceVersion> list = new List<RecorderServiceVersion>();
			while (databaseDataReader.Read())
			{
				list.Add(RecorderServiceVersionColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public Tuple<bool, bool> CreateOrUpdateRecorder(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, string name, string hostName, Uri webServerUri, string scanRanges, string timeZoneName, string devicePackInfo, ushort failoverServerPort, int databasePort, Guid failoverId, Version version, bool isMulticastLicensed)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("recording_server_create_or_update");
				BindGuidType("@RecorderId", recorderId);
				BindGuidType("@ServiceId", serviceId);
				BindInt("@Type", (int)serviceType);
				BindNameType("@Name", name);
				BindNameType("@HostName", hostName);
				BindBoolean("@CertificateConfigured", string.Equals(webServerUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));
				BindBoolean("@IsMulticastLicensed", isMulticastLicensed);
				BindNameType("@WebServerHostName", webServerUri.Host);
				BindInt("@WebServerPort", webServerUri.Port);
				BindText("@ScanRanges", scanRanges, allowNull: false);
				BindNameType("@TimeZoneName", timeZoneName);
				BindText("@DevicePackInfo", devicePackInfo, allowNull: false);
				BindInt("@FailoverUdpPort", failoverServerPort);
				BindBoolean("@authorized", parameterValue: false);
				BindInt("@DatabasePort", databasePort);
				BindGuidType("@FailoverId", failoverId);
				BindNameType("@Version", version.ToString());
				DatabaseDataReader databaseDataReader = ExecuteReader();
				return (!databaseDataReader.Read()) ? null : RecordingServerCreatedColumns.Bind(databaseDataReader);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
