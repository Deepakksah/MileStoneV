using System;
using System.Data;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public class ServerOverviewComponent : ReportingResourceComponent
{
	private static class ServerOverviewColumns
	{
		private static DatabaseColumnBinder IDServer = new DatabaseColumnBinder("IDServer");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("ServerType");

		private static DatabaseColumnBinder ServerName = new DatabaseColumnBinder("ServerName");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("HostName");

		private static DatabaseColumnBinder CpuInstanceId = new DatabaseColumnBinder("CpuInstanceId");

		private static DatabaseColumnBinder MemoryInstanceId = new DatabaseColumnBinder("MemoryInstanceId");

		public static ServerOverview Bind(DatabaseDataReader reader)
		{
			return new ServerOverview
			{
				ServerId = IDServer.GetStringAsGuid(reader),
				DataCollectorId = DataCollectorId.GetInt32(reader),
				ServerType = Type.GetString(reader),
				ServerName = ServerName.GetString(reader),
				HostName = HostName.GetString(reader),
				CpuInstanceId = CpuInstanceId.GetInt32(reader, allowNulls: true, 0),
				MemoryInstanceId = MemoryInstanceId.GetInt32(reader, allowNulls: true, 0)
			};
		}
	}

	public ServerOverview GetServerOverview(Guid serverId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_non_recorder_overview");
				BindString("@server_id", serverId.ToString(), 100, allowNull: false, SqlDbType.NVarChar);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				return (!databaseDataReader.Read()) ? null : ServerOverviewColumns.Bind(databaseDataReader);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
