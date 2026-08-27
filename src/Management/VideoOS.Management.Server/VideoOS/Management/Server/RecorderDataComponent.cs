using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RecorderDataComponent : GeminiResourceComponent, IRecorderDataComponent, IDisposable
{
	private static class RecorderColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder IDService = new DatabaseColumnBinder("IDService");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder SynchronizationTime = new DatabaseColumnBinder("SynchronizationTime");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("host_name");

		private static DatabaseColumnBinder CertificateConfigured = new DatabaseColumnBinder("certificate_configured");

		private static DatabaseColumnBinder WebServerHostName = new DatabaseColumnBinder("webserver_hostname");

		private static DatabaseColumnBinder WebServerPort = new DatabaseColumnBinder("webserver_port");

		private static DatabaseColumnBinder PublicAccessEnabled = new DatabaseColumnBinder("PublicAccessEnabled");

		private static DatabaseColumnBinder PublicWebServerHostName = new DatabaseColumnBinder("PublicWebServerHostName");

		private static DatabaseColumnBinder PublicWebServerPort = new DatabaseColumnBinder("PublicWebServerPort");

		private static DatabaseColumnBinder TimeZoneName = new DatabaseColumnBinder("time_zone_name");

		public static RecorderData Bind(DatabaseDataReader reader)
		{
			return new RecorderData
			{
				Id = new Guid(IDRecorder.GetGuid(reader)),
				ServiceId = new Guid(IDService.GetGuid(reader)),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				SynchronizationTime = SynchronizationTime.GetInt32(reader, allowNulls: false, -1),
				HostName = HostName.GetString(reader),
				CertificateConfigured = CertificateConfigured.GetBoolean(reader),
				WebServerHostName = WebServerHostName.GetString(reader),
				WebServerPort = WebServerPort.GetInt32(reader),
				PublicAccessEnabled = PublicAccessEnabled.GetBoolean(reader),
				PublicWebServerHostName = PublicWebServerHostName.GetString(reader),
				PublicWebServerPort = PublicWebServerPort.GetInt32(reader),
				TimeZoneName = TimeZoneName.GetString(reader),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	private static class RecorderModifiedColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		public static RecorderModifiedData Bind(DatabaseDataReader reader)
		{
			return new RecorderModifiedData
			{
				Id = new Guid(IDRecorder.GetGuid(reader)),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	public IEnumerable<RecorderData> QueryEnabledRecorders(Guid[] recorderIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_servers_enabled2");
			BindIdTable("@IDRecorders", recorderIds?.Select((Guid f) => f.ToString()).ToArray());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RecorderData> list = new List<RecorderData>();
			while (databaseDataReader.Read())
			{
				list.Add(RecorderColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public RecorderData QueryActiveRecorder(Guid recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_active2");
			BindGuidType("@IDRecorder", recorderId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DatabaseConfigMissingRowException();
			}
			RecorderData result = RecorderColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public IEnumerable<RecorderModifiedData> QueryRecordersChangedAfter(DateTime changesFrom)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recorder_ids_changed_after");
			BindDateTime("@changedAfter", changesFrom);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RecorderModifiedData> list = new List<RecorderModifiedData>();
			while (databaseDataReader.Read())
			{
				list.Add(RecorderModifiedColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}
}
