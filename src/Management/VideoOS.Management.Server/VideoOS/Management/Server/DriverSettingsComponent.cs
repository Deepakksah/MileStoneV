using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DriverSettingsComponent : GeminiResourceComponent, IDriverSettingsComponent, IDisposable
{
	private static class HardwareDriverSettingsInfoColumns
	{
		private static DatabaseColumnBinder SettingsXml = new DatabaseColumnBinder("settings_xml");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("hardware_id");

		private static DatabaseColumnBinder DefinitionDigest = new DatabaseColumnBinder("definition_digest");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		public static HardwareDriverSettingsInfo Bind(DatabaseDataReader reader)
		{
			return new HardwareDriverSettingsInfo
			{
				HardwareId = HardwareId.GetGuid(reader),
				DefinitionDigest = DefinitionDigest.GetString(reader),
				SettingsXml = SettingsXml.GetString(reader),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	private static class DeviceDriverSettingsInfoColumns
	{
		private static DatabaseColumnBinder DeviceIndex = new DatabaseColumnBinder("device_index");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("device_type");

		private static DatabaseColumnBinder SettingsXml = new DatabaseColumnBinder("settings_xml");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("hardware_id");

		private static DatabaseColumnBinder DefinitionDigest = new DatabaseColumnBinder("definition_digest");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		public static DeviceDriverSettingsInfo Bind(DatabaseDataReader reader)
		{
			return new DeviceDriverSettingsInfo
			{
				HardwareId = HardwareId.GetGuid(reader),
				DefinitionDigest = DefinitionDigest.GetString(reader),
				DeviceIndex = DeviceIndex.GetInt32(reader),
				DeviceType = DatabaseDeviceTypeStrings.GetDeviceTypeEnum(DeviceType.GetString(reader)),
				SettingsXml = SettingsXml.GetString(reader),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	string[] IDriverSettingsComponent.QueryAllSettingsByHardware(string hardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_all_hardware_settings");
			BindGuidType("@hardware_id", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				string item = databaseDataReader.GetString(0);
				list.Add(item);
			}
			if (list.Count == 0)
			{
				throw new DriverSettingsHardwareNotFoundException();
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.Select((string settings) => SettingsXmlTransformer.Instance.Decrypt(settings, (string setting) => RequestContext.GetEncryptionService().Decrypt(setting))).ToArray();
		});
	}

	public HardwareDriverSettingsInfo QueryHardwareDriverSettingsInfo(string hardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_driver_settings_info");
			BindGuidType("@hardware_id", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new HardwareDriverSettingsHardwareNotFoundException();
			}
			HardwareDriverSettingsInfo hardwareDriverSettingsInfo = HardwareDriverSettingsInfoColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			if (!string.IsNullOrEmpty(hardwareDriverSettingsInfo.SettingsXml))
			{
				hardwareDriverSettingsInfo.SettingsXml = SettingsXmlTransformer.Instance.Decrypt(hardwareDriverSettingsInfo.SettingsXml, (string setting) => RequestContext.GetEncryptionService().Decrypt(setting));
			}
			return hardwareDriverSettingsInfo;
		});
	}

	public void UpdateHardwareSettings(string hardwareId, DateTime dateTime, string settingsXml)
	{
		settingsXml = SettingsXmlTransformer.Instance.Encrypt(settingsXml, (string setting) => RequestContext.GetEncryptionService().Encrypt(setting));
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_hardware_settings");
			BindGuidType("@hardware_id", hardwareId);
			BindDateTime("@last_modified", dateTime);
			BindXml("@settings", settingsXml);
			ExecuteNonQuery();
		});
	}

	public DeviceDriverSettingsInfo QueryDeviceDriverSettingsInfo(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_driver_settings_info");
			BindGuidType("@device_id", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DeviceDriverSettingsDeviceNotFoundException();
			}
			DeviceDriverSettingsInfo result = DeviceDriverSettingsInfoColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateDeviceSettings(string deviceId, DateTime dateTime, string settingsXml, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_settings");
			BindGuidType("@device_id", deviceId);
			BindDateTime("@last_modified", dateTime);
			BindXml("@settings", settingsXml);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
