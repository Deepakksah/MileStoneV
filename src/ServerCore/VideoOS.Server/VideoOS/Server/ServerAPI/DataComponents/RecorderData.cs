using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.ServerAPI.DataComponents;

public sealed class RecorderData
{
	private sealed class RecorderDataComponent : VmoResourceComponent
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
				RecorderData recorderData = new RecorderData();
				recorderData.Id = IDRecorder.GetStringAsGuid(reader);
				recorderData.ServiceId = IDService.GetStringAsGuid(reader);
				recorderData.Name = Name.GetString(reader);
				recorderData.Description = Description.GetString(reader, allowNulls: true);
				recorderData.SynchronizationTime = SynchronizationTime.GetInt32(reader, allowNulls: false, -1);
				recorderData.HostName = HostName.GetString(reader);
				recorderData.CertificateConfigured = CertificateConfigured.GetBoolean(reader);
				recorderData.WebServerHostName = WebServerHostName.GetString(reader);
				recorderData.WebServerPort = WebServerPort.GetInt32(reader);
				recorderData.PublicAccessEnabled = PublicAccessEnabled.GetBoolean(reader);
				recorderData.PublicWebServerHostName = PublicWebServerHostName.GetString(reader);
				recorderData.PublicWebServerPort = PublicWebServerPort.GetInt32(reader);
				recorderData.TimeZoneName = TimeZoneName.GetString(reader);
				recorderData.LastModified = LastModified.GetDateTime(reader);
				return recorderData;
			}
		}

		public RecorderDataComponent()
		{
		}

		public RecorderData[] QueryAllRecorders()
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_recording_servers_enabled");
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
				return list.ToArray();
			});
		}
	}

	public Guid Id { get; private set; }

	public Guid ServiceId { get; private set; }

	public string Name { get; private set; }

	public string Description { get; private set; }

	public int SynchronizationTime { get; set; }

	public string HostName { get; private set; }

	public bool CertificateConfigured { get; private set; }

	public string WebServerHostName { get; private set; }

	public int WebServerPort { get; private set; }

	public bool PublicAccessEnabled { get; private set; }

	public string PublicWebServerHostName { get; private set; }

	public int PublicWebServerPort { get; private set; }

	public string TimeZoneName { get; private set; }

	public DateTime LastModified { get; private set; }

	public static RecorderData[] QueryAllRecorders()
	{
		using RecorderDataComponent recorderDataComponent = new RecorderDataComponent();
		return recorderDataComponent.QueryAllRecorders();
	}
}
