using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceDataComponent : GeminiResourceComponent, IDeviceDataComponent, IDisposable
{
	private static class DeviceColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDHardware = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Streams = new DatabaseColumnBinder("Streams");

		private static DatabaseColumnBinder Shortcut = new DatabaseColumnBinder("Shortcut");

		private static DatabaseColumnBinder Settings = new DatabaseColumnBinder("Settings");

		private static DatabaseColumnBinder Presets = new DatabaseColumnBinder("Presets");

		private static DatabaseColumnBinder MulticastEnabled = new DatabaseColumnBinder("MulticastEnabled");

		private static DatabaseColumnBinder DeviceIndex = new DatabaseColumnBinder("DeviceIndex");

		private static DatabaseColumnBinder EdgestoragePlayback = new DatabaseColumnBinder("edgestorage_playback");

		private static DatabaseColumnBinder ManualTimeoutEnabled = new DatabaseColumnBinder("manual_timeout_enabled");

		private static DatabaseColumnBinder ManualTimeoutMinutes = new DatabaseColumnBinder("manual_timeout");

		private static DatabaseColumnBinder GisPoint = new DatabaseColumnBinder("GisPointString");

		private static DatabaseColumnBinder ShortName = new DatabaseColumnBinder("ShortName");

		private static DatabaseColumnBinder Icon = new DatabaseColumnBinder("Icon");

		private static DatabaseColumnBinder CoverageDirection = new DatabaseColumnBinder("CoverageDirection");

		private static DatabaseColumnBinder CoverageDepth = new DatabaseColumnBinder("CoverageDepth");

		private static DatabaseColumnBinder CoverageFieldOfView = new DatabaseColumnBinder("CoverageFieldOfView");

		public static DeviceData Bind(DatabaseDataReader reader)
		{
			return new DeviceData
			{
				Id = new Guid(IDDevice.GetGuid(reader)),
				DeviceType = DeviceType.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				Streams = Streams.GetString(reader, allowNulls: true),
				Shortcut = Shortcut.GetString(reader),
				Settings = Settings.GetString(reader, allowNulls: false),
				Presets = Presets.GetString(reader, allowNulls: true),
				MulticastEnabled = MulticastEnabled.GetBoolean(reader),
				HardwareId = new Guid(IDHardware.GetGuid(reader)),
				DeviceIndex = DeviceIndex.GetInt32(reader),
				EdgestoragePlaybackEnabled = EdgestoragePlayback.GetBoolean(reader, allowNulls: true, defaultValue: false),
				ManualTimeoutEnabled = ManualTimeoutEnabled.GetBoolean(reader, allowNulls: true, defaultValue: false),
				ManualTimeoutMinutes = ManualTimeoutMinutes.GetInt32(reader, allowNulls: true, 0),
				GisPoint = GisPoint.GetGeographyString(reader),
				ShortName = ShortName.GetString(reader),
				Icon = Icon.GetInt32(reader),
				CoverageDirection = CoverageDirection.GetDouble(reader),
				CoverageDepth = CoverageDepth.GetDouble(reader),
				CoverageFieldOfView = CoverageFieldOfView.GetDouble(reader)
			};
		}
	}

	private static class RecorderColumns
	{
		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		public static Guid Bind(DatabaseDataReader reader)
		{
			return new Guid(IDRecorder.GetGuid(reader));
		}
	}

	public IEnumerable<DeviceData> QueryEnabledDevicesByRecorderAdmin(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_devices_enabled_by_recorder");
			BindGuidType("@IDRecorder", id.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceData> list = new List<DeviceData>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public Dictionary<Guid, IEnumerable<DeviceData>> QueryEnabledDevicesByRoleId(string[] roleIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_devices_enabled_by_roleids");
			BindIdTable("@RoleIds", roleIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<Guid, IEnumerable<DeviceData>> dictionary = new Dictionary<Guid, IEnumerable<DeviceData>>();
			while (databaseDataReader.Read())
			{
				Guid key = RecorderColumns.Bind(databaseDataReader);
				DeviceData item = DeviceColumns.Bind(databaseDataReader);
				if (dictionary.TryGetValue(key, out var value))
				{
					((List<DeviceData>)value).Add(item);
				}
				else
				{
					dictionary.Add(key, new List<DeviceData> { item });
				}
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return dictionary;
		});
	}

	public IEnumerable<Guid> QueryDeviceIdsByHardware(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_ids_by_hardware");
			BindGuidType("@IDHardware", id.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Guid> list = new List<Guid>();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}
}
