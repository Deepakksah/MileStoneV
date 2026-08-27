using System;
using System.Collections.Generic;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public class DeviceOverviewComponent : ReportingResourceComponent
{
	private static class DeviceOverviewColumns
	{
		private static DatabaseColumnBinder DeviceId = new DatabaseColumnBinder("DeviceId");

		private static DatabaseColumnBinder DeviceName = new DatabaseColumnBinder("DeviceName");

		private static DatabaseColumnBinder RecorderWebServerUri = new DatabaseColumnBinder("RecorderWebServerUri");

		private static DatabaseColumnBinder RecordingStorageName = new DatabaseColumnBinder("RecordingStorageName");

		private static DatabaseColumnBinder RecorderName = new DatabaseColumnBinder("RecorderName");

		private static DatabaseColumnBinder CameraHostName = new DatabaseColumnBinder("CameraHostName");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("RecorderId");

		private static DatabaseColumnBinder CollectorId = new DatabaseColumnBinder("CollectorId");

		public static DeviceOverview Bind(DatabaseDataReader reader)
		{
			return new DeviceOverview
			{
				DeviceId = DeviceId.GetStringAsGuid(reader),
				DeviceName = DeviceName.GetString(reader),
				RecorderWebserverUri = RecorderWebServerUri.GetString(reader),
				RecordingStorageName = RecordingStorageName.GetString(reader),
				RecordingServerName = RecorderName.GetString(reader),
				DeviceHostName = CameraHostName.GetString(reader),
				RecorderId = RecorderId.GetStringAsGuid(reader),
				DataCollectorId = CollectorId.GetInt32(reader)
			};
		}
	}

	private static class DeviceInstancesOverviewColumns
	{
		private static DatabaseColumnBinder NetworkLoadInstanceId = new DatabaseColumnBinder("NetworkLoadInstanceId");

		private static DatabaseColumnBinder StorageUsageInstanceId = new DatabaseColumnBinder("StorageUsageInstanceId");

		public static DeviceInstancesOverview Bind(DatabaseDataReader reader)
		{
			return new DeviceInstancesOverview
			{
				NetworkLoadInstanceId = NetworkLoadInstanceId.GetInt32(reader, allowNulls: true, 0),
				StorageUsageInstanceId = StorageUsageInstanceId.GetInt32(reader, allowNulls: true, 0)
			};
		}
	}

	public DeviceOverview[] GetDeviceOverviewByRecorder(Guid recorderId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_device_overview_by_recorder_id");
				BindGuidType("@recorder_id", recorderId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<DeviceOverview> list = new List<DeviceOverview>();
				while (databaseDataReader.Read())
				{
					list.Add(DeviceOverviewColumns.Bind(databaseDataReader));
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

	public DeviceOverview[] GetDeviceOverviewByGroup(Guid groupId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_device_overview_by_group_id");
				BindGuidType("@device_group_id ", groupId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<DeviceOverview> list = new List<DeviceOverview>();
				while (databaseDataReader.Read())
				{
					list.Add(DeviceOverviewColumns.Bind(databaseDataReader));
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

	public DeviceOverview[] GetDeviceOverview(Guid deviceId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_device_overview_by_device_id");
				BindGuidType("@device_id", deviceId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<DeviceOverview> list = new List<DeviceOverview>();
				while (databaseDataReader.Read())
				{
					list.Add(DeviceOverviewColumns.Bind(databaseDataReader));
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

	public DeviceInstancesOverview GetDeviceInstancesOverview(Guid deviceId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("reporting_web_device_instances");
				BindGuidType("@device_id", deviceId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				return (!databaseDataReader.Read()) ? null : DeviceInstancesOverviewColumns.Bind(databaseDataReader);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
