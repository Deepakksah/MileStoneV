using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceDisabledDataComponent : GeminiResourceComponent, IDeviceDisabledDataComponent, IDisposable
{
	private static class DeviceDisabledColumns
	{
		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder IDHardware = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		private static DatabaseColumnBinder DeviceName = new DatabaseColumnBinder("Name");

		public static DeviceDisabledData Bind(DatabaseDataReader reader)
		{
			return new DeviceDisabledData
			{
				RecorderId = new Guid(IDRecorder.GetGuid(reader)),
				HardwareId = new Guid(IDHardware.GetGuid(reader)),
				Id = new Guid(IDDevice.GetGuid(reader)),
				Type = DeviceType.GetString(reader),
				Name = DeviceName.GetString(reader)
			};
		}
	}

	public IEnumerable<DeviceDisabledData> QueryDisabledDevices()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_devices_disabled");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceDisabledData> list = new List<DeviceDisabledData>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceDisabledColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}
}
