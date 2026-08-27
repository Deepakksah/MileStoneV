using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class FailoverServerComponent : VmoResourceComponent
{
	private static class FailoverServerColumns
	{
		private static DatabaseColumnBinder IDFailoverServer = new DatabaseColumnBinder("IDFailoverServer");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("HostName");

		private static DatabaseColumnBinder UdpPort = new DatabaseColumnBinder("UdpPort");

		private static DatabaseColumnBinder GroupSequence = new DatabaseColumnBinder("GroupSequence");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder DatabaseLocation = new DatabaseColumnBinder("DatabaseLocation");

		private static DatabaseColumnBinder CertificateConfigured = new DatabaseColumnBinder("CertificateConfigured");

		private static DatabaseColumnBinder WebServerHostName = new DatabaseColumnBinder("WebServerHostName");

		private static DatabaseColumnBinder WebServerPort = new DatabaseColumnBinder("WebServerPort");

		private static DatabaseColumnBinder PublicAccessEnabled = new DatabaseColumnBinder("PublicAccessEnabled");

		private static DatabaseColumnBinder PublicWebServerHostName = new DatabaseColumnBinder("PublicWebServerHostName");

		private static DatabaseColumnBinder PublicWebServerPort = new DatabaseColumnBinder("PublicWebServerPort");

		public static FailoverServer Bind(DatabaseDataReader reader)
		{
			FailoverServer failoverServer = new FailoverServer();
			failoverServer.Id = new Guid(IDFailoverServer.GetString(reader));
			failoverServer.Name = Name.GetString(reader);
			failoverServer.Description = Description.GetString(reader, allowNulls: true);
			failoverServer.HostName = HostName.GetString(reader, allowNulls: true);
			failoverServer.UdpPort = (ushort)UdpPort.GetInt32(reader);
			failoverServer.GroupSequence = GroupSequence.GetInt16(reader);
			failoverServer.Enabled = Enabled.GetBoolean(reader);
			failoverServer.DatabaseLocation = DatabaseLocation.GetString(reader, allowNulls: true, "");
			failoverServer.LastModified = LastModified.GetDateTime(reader);
			failoverServer.CertificateConfigured = CertificateConfigured.GetBoolean(reader);
			failoverServer.WebServerHostName = WebServerHostName.GetString(reader);
			failoverServer.WebServerPort = WebServerPort.GetInt32(reader);
			failoverServer.PublicAccessEnabled = PublicAccessEnabled.GetBoolean(reader);
			failoverServer.PublicWebServerHostName = PublicWebServerHostName.GetString(reader);
			failoverServer.PublicWebServerPort = PublicWebServerPort.GetInt32(reader);
			return failoverServer;
		}
	}

	public FailoverServer[] QueryFailoverServers()
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_failover_servers");
				BindBoolean("@FilterOnServerGroup", parameterValue: false);
				BindGuidType("@IDServerGroup", Guid.Empty);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<FailoverServer> list = new List<FailoverServer>();
				while (databaseDataReader.Read())
				{
					list.Add(FailoverServerColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
