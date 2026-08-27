using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public class PerformanceCounterComponent : ReportingResourceComponent
{
	private static class HistoryDataColumns
	{
		private static DatabaseColumnBinder CalculatedValue = new DatabaseColumnBinder("calculated_value");

		private static DatabaseColumnBinder CollectionTime = new DatabaseColumnBinder("collection_time");

		public static HistoryData Bind(DatabaseDataReader reader, int offset)
		{
			double value = CalculatedValue.GetDouble(reader);
			DateTime time = CollectionTime.GetDateTime(reader).AddMinutes(offset);
			return new HistoryData
			{
				Value = value,
				Time = time
			};
		}
	}

	private static class DiskHistoryDataColumns
	{
		private static DatabaseColumnBinder MediaBytes = new DatabaseColumnBinder("MediaBytes");

		private static DatabaseColumnBinder FreeBytes = new DatabaseColumnBinder("FreeBytes");

		private static DatabaseColumnBinder OtherBytes = new DatabaseColumnBinder("OtherBytes");

		private static DatabaseColumnBinder CollectionTime = new DatabaseColumnBinder("collection_time");

		public static DiskHistoryData Bind(DatabaseDataReader reader, int offset)
		{
			double mediaBytes = MediaBytes.GetDouble(reader);
			double freeBytes = FreeBytes.GetDouble(reader);
			double otherBytes = OtherBytes.GetDouble(reader);
			DateTime time = CollectionTime.GetDateTime(reader).AddMinutes(offset);
			return new DiskHistoryData
			{
				MediaBytes = mediaBytes,
				FreeBytes = freeBytes,
				OtherBytes = otherBytes,
				Time = time
			};
		}
	}

	public int GetMaxPhysicalMemory(int datacollectorId)
	{
		try
		{
			PrepareStoredProcedure("reporting_web_max_physical_memory");
			BindInt("@data_collector_id", datacollectorId);
			return Convert.ToInt32(ExecuteScalar(), CultureInfo.InvariantCulture);
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public HistoryData[] GetHistoryData(Timescale timeScale, int instanceId, int offsetMinutes)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_history_performance_data");
				BindEnumType("@time_scale", typeof(Timescale), timeScale);
				BindInt("@instance_id", instanceId);
				BindInt("@offset_minutes", offsetMinutes);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<HistoryData> list = new List<HistoryData>();
				while (databaseDataReader.Read())
				{
					list.Add(HistoryDataColumns.Bind(databaseDataReader, offsetMinutes));
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

	public DiskHistoryData[] GetDiskHistoryData(Timescale timeScale, string driveName, Guid recorderId, int offsetMinutes, long maxValue)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_history_performance_data_disk");
				BindEnumType("@time_scale", typeof(Timescale), timeScale);
				BindString("@disk_name", driveName, 100, allowNull: false, SqlDbType.NVarChar);
				BindGuidType("@IDrecorder", recorderId);
				BindInt("@offset_minutes", offsetMinutes);
				BindLong("@max_value", maxValue);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<DiskHistoryData> list = new List<DiskHistoryData>();
				while (databaseDataReader.Read())
				{
					list.Add(DiskHistoryDataColumns.Bind(databaseDataReader, offsetMinutes));
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
