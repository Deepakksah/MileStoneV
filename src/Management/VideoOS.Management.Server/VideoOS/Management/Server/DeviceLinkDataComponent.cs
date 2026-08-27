using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceLinkDataComponent : GeminiResourceComponent, IDeviceLinkDataComponent, IDisposable
{
	private static class DeviceLinkColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDDeviceLinked = new DatabaseColumnBinder("IDDeviceLinked");

		private static DatabaseColumnBinder Priority = new DatabaseColumnBinder("Priority");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static DeviceLinkData Bind(DatabaseDataReader reader)
		{
			DeviceLinkData deviceLinkData = new DeviceLinkData();
			string g = IDDevice.GetString(reader);
			deviceLinkData.IdDevice = new Guid(g);
			string g2 = IDDeviceLinked.GetString(reader);
			deviceLinkData.IdDeviceLink = new Guid(g2);
			deviceLinkData.Priority = Priority.GetInt32(reader);
			deviceLinkData.LastModified = LastModified.GetDateTime(reader);
			return deviceLinkData;
		}
	}

	public IEnumerable<DeviceLinkData> QueryDeviceLinks()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_links");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceLinkData> list = new List<DeviceLinkData>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceLinkColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public IEnumerable<DeviceLinkData> QueryDeviceLinksFromId(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_links_from_id");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceLinkData> list = new List<DeviceLinkData>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceLinkColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public IEnumerable<DeviceLinkData> QueryDeviceLinksFromLinkedId(string linkedDeviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_links_from_linkedid");
			BindGuidType("@IDDeviceLinked", linkedDeviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceLinkData> list = new List<DeviceLinkData>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceLinkColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void AddDeviceLink(string deviceId, string relatedDeviceId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("insert_devicelink");
			BindGuidType("@IDDevice", deviceId);
			BindGuidType("@IDDeviceLinked", relatedDeviceId);
			BindInt("@Priority", 0);
			ExecuteNonQuery();
		});
	}

	public void RemoveDeviceLink(string deviceId, string relatedDeviceId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("remove_devicelink");
			BindGuidType("@IDDevice", deviceId);
			BindGuidType("@IDDeviceLinked", relatedDeviceId);
			ExecuteNonQuery();
		});
	}
}
