using System;
using System.Collections.Generic;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public class FailoverIncidenceComponent : ReportingResourceComponent
{
	private static class FailoverIncidentColumns
	{
		private static DatabaseColumnBinder RecorderName = new DatabaseColumnBinder("RecorderName");

		private static DatabaseColumnBinder StartTime = new DatabaseColumnBinder("StartTime");

		private static DatabaseColumnBinder EndTime = new DatabaseColumnBinder("EndTime");

		public static FailoverIncident Bind(DatabaseDataReader reader)
		{
			return new FailoverIncident
			{
				RecorderName = RecorderName.GetString(reader, allowNulls: true, "Not found"),
				StartTime = StartTime.GetDateTime(reader, allowNulls: true, DateTime.MinValue).ToLocalTime(),
				EndTime = EndTime.GetDateTime(reader, allowNulls: true, DateTime.MinValue).ToLocalTime()
			};
		}
	}

	public FailoverIncident[] GetFailoverIncidentsByFailoverServer(Guid failoverServerId, Timescale timeScale)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_query_failover_incidence");
				BindGuidType("@IDFailover", failoverServerId);
				BindStringMax("@time_scale", TranslateTimescale(timeScale), allowNull: false);
				BindInt("@time_offset", 0);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<FailoverIncident> list = new List<FailoverIncident>();
				while (databaseDataReader.Read())
				{
					list.Add(FailoverIncidentColumns.Bind(databaseDataReader));
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

	private static string TranslateTimescale(Timescale timescale)
	{
		return timescale switch
		{
			Timescale.Day => "Day", 
			Timescale.Month => "Month", 
			Timescale.Week => "Week", 
			_ => throw new NotImplementedException("This timescale is not implemented"), 
		};
	}
}
