using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceRecorderPluginDataComponent : GeminiResourceComponent, IDeviceRecorderPluginDataComponent, IDisposable
{
	private static class RecorderColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDRecorderPlugin = new DatabaseColumnBinder("IDRecorderPlugin");

		private static DatabaseColumnBinder Settings = new DatabaseColumnBinder("Settings");

		public static DeviceRecorderPlugin Bind(DatabaseDataReader reader)
		{
			return new DeviceRecorderPlugin
			{
				DeviceId = new Guid(IDDevice.GetGuid(reader)),
				RecorderPluginId = new Guid(IDRecorderPlugin.GetGuid(reader)),
				Settings = Settings.GetString(reader)
			};
		}
	}

	public IEnumerable<DeviceRecorderPlugin> QueryEnabledPluginsByType(Guid pluginTypeId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recorder_plugin_device_enabled_by_type");
			BindGuidType("@IDRecorderPlugin", pluginTypeId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceRecorderPlugin> list = new List<DeviceRecorderPlugin>();
			while (databaseDataReader.Read())
			{
				list.Add(RecorderColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public DeviceRecorderPlugin QueryEnabledPluginsByTypeAndId(Guid pluginTypeId, Guid deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recorder_plugin_device_by_type_and_id");
			BindGuidType("@IDRecorderPlugin", pluginTypeId);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (DeviceRecorderPlugin)null;
			}
			DeviceRecorderPlugin result = RecorderColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateDeviceFisheyeSettings(Guid deviceId, string settings, bool enabled, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_fisheye_settings");
			BindGuidType("@IDDevice", deviceId);
			BindInt("@order", 1);
			BindString("@settings", settings, 32000, allowNull: false, SqlDbType.Text);
			BindBoolean("@enabled", enabled);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
