using System;
using System.Collections.Generic;
using VideoOS.Server.DataComponents;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public class DeviceDataDistribution
{
	public sealed class DeviceDataDistributionComponent : VmoResourceComponent, IDeviceDataDistributionComponent, IDisposable
	{
		public DeviceDataDistribution[] QueryDeviceDataDistribution(Guid? deviceId, Guid? recorderId, Guid? trackId)
		{
			try
			{
				return base.Connection.ExecuteRetryable(delegate
				{
					PrepareStoredProcedure("query_device_data_distribution");
					BindGuidType("device_id", deviceId, allowNullOrEmpty: true);
					BindGuidType("recorder_id", recorderId, allowNullOrEmpty: true);
					BindUniqueIdentifier("track_id", trackId);
					List<DeviceDataDistribution> list = new List<DeviceDataDistribution>();
					DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("device_id");
					DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("recorder_id");
					DatabaseColumnBinder databaseColumnBinder3 = new DatabaseColumnBinder("track_id");
					DatabaseDataReader databaseDataReader = ExecuteReader();
					while (databaseDataReader.Read())
					{
						DeviceDataDistribution item = new DeviceDataDistribution
						{
							DeviceId = new Guid(databaseColumnBinder.GetString(databaseDataReader)),
							RecorderId = new Guid(databaseColumnBinder2.GetString(databaseDataReader)),
							TrackId = databaseColumnBinder3.GetGuid2(databaseDataReader)
						};
						list.Add(item);
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

		public void DeleteDeviceDataDistribution(Guid recorderId, Guid trackId)
		{
			try
			{
				base.Connection.ExecuteRetryable(delegate
				{
					PrepareStoredProcedure("delete_device_data_distribution");
					BindGuidType("recorder_id", recorderId);
					BindUniqueIdentifier("track_id", trackId);
					ExecuteNonQuery();
				});
			}
			catch (DatabaseException ex)
			{
				MapException(ex);
				throw;
			}
		}
	}

	public Guid DeviceId { get; set; }

	public Guid TrackId { get; set; }

	public Guid RecorderId { get; set; }

	public static DeviceDataDistribution[] QueryDeviceDataDistribution(IDataComponentFactory componentFactory, Guid deviceId, Guid recorderId)
	{
		using IDeviceDataDistributionComponent deviceDataDistributionComponent = componentFactory.CreateDeviceDataDistributionComponent();
		return deviceDataDistributionComponent.QueryDeviceDataDistribution(deviceId, recorderId, null);
	}

	public static DeviceDataDistribution[] QueryDeviceDataDistribution(IDataComponentFactory componentFactory)
	{
		using IDeviceDataDistributionComponent deviceDataDistributionComponent = componentFactory.CreateDeviceDataDistributionComponent();
		return deviceDataDistributionComponent.QueryDeviceDataDistribution(null, null, null);
	}

	public static void DeleteDeviceDataDistribution(IDataComponentFactory componentFactory, Guid recorderId, Guid trackId)
	{
		using IDeviceDataDistributionComponent deviceDataDistributionComponent = componentFactory.CreateDeviceDataDistributionComponent();
		deviceDataDistributionComponent.DeleteDeviceDataDistribution(recorderId, trackId);
	}
}
