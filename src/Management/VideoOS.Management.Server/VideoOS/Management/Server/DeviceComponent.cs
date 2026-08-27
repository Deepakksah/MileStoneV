using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceComponent : GeminiResourceComponent, IDeviceComponent, IDisposable
{
	private static class DeviceColumns
	{
		public static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		public static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder DeviceIndex = new DatabaseColumnBinder("DeviceIndex");

		private static DatabaseColumnBinder Settings = new DatabaseColumnBinder("Settings");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder RecordingStorageId = new DatabaseColumnBinder("recordingstorage_id");

		private static DatabaseColumnBinder RecordingEnabled = new DatabaseColumnBinder("recording_enabled");

		private static DatabaseColumnBinder RecordingFramerate = new DatabaseColumnBinder("recording_framerate");

		private static DatabaseColumnBinder PrebufferEnabled = new DatabaseColumnBinder("prebuffer_enabled");

		private static DatabaseColumnBinder PrebufferInMemory = new DatabaseColumnBinder("prebuffer_in_memory");

		private static DatabaseColumnBinder PrebufferSeconds = new DatabaseColumnBinder("prebuffer_seconds");

		private static DatabaseColumnBinder EdgeStorageEnabled = new DatabaseColumnBinder("edgestorage_enabled");

		private static DatabaseColumnBinder EdgeStoragePlayback = new DatabaseColumnBinder("edgestorage_playback");

		private static DatabaseColumnBinder RecordKeyframes = new DatabaseColumnBinder("record_keyframes");

		private static DatabaseColumnBinder ManualTimeoutEnabled = new DatabaseColumnBinder("manual_timeout_enabled");

		private static DatabaseColumnBinder ManualTimeout = new DatabaseColumnBinder("manual_timeout");

		private static DatabaseColumnBinder RecordOnRelatedDevices = new DatabaseColumnBinder("record_on_related_devices");

		private static DatabaseColumnBinder GisPoint = new DatabaseColumnBinder("GisPointString");

		private static DatabaseColumnBinder ShortName = new DatabaseColumnBinder("ShortName");

		private static DatabaseColumnBinder Shortcut = new DatabaseColumnBinder("Shortcut");

		private static DatabaseColumnBinder Icon = new DatabaseColumnBinder("Icon");

		private static DatabaseColumnBinder CoverageDirection = new DatabaseColumnBinder("CoverageDirection");

		private static DatabaseColumnBinder CoverageDepth = new DatabaseColumnBinder("CoverageDepth");

		private static DatabaseColumnBinder CoverageFieldOfView = new DatabaseColumnBinder("CoverageFieldOfView");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder MulticastEnabledBinder = new DatabaseColumnBinder("MulticastEnabled");

		private static DatabaseColumnBinder PtzManualSessionTimeoutBinder = new DatabaseColumnBinder("PtzManualSessionTimeout");

		private static DatabaseColumnBinder PtzReservedSessionTimeoutBinder = new DatabaseColumnBinder("PtzReservedSessionTimeout");

		private static DatabaseColumnBinder PtzExtendedManualSessionTimeoutBinder = new DatabaseColumnBinder("PtzExtendedManualSessionTimeout");

		private static DatabaseColumnBinder PtzEnabledBinder = new DatabaseColumnBinder("PtzEnabled");

		private static DatabaseColumnBinder FailoverLiveOnly = new DatabaseColumnBinder("FailoverLiveOnly");

		private static DatabaseColumnBinder DisableOnFailover = new DatabaseColumnBinder("DisableOnFailover");

		public static Device Bind(DatabaseDataReader reader)
		{
			GisPoint.GetGeographyString(reader, allowNulls: true, null);
			return new Device
			{
				Id = IDDevice.GetGuid(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				CreatedDate = DateTime.MinValue,
				LastModified = LastModified.GetDateTime(reader),
				DeviceType = DatabaseDeviceTypeStrings.GetDeviceTypeEnum(DeviceType.GetString(reader)),
				SettingsXml = Settings.GetString(reader),
				Enabled = Enabled.GetBoolean(reader),
				DeviceIndex = DeviceIndex.GetInt32(reader),
				RecordingStorageId = RecordingStorageId.GetGuid(reader),
				RecordingEnabled = RecordingEnabled.GetBoolean(reader),
				RecordingFramerate = RecordingFramerate.GetDouble(reader),
				PrebufferEnabled = PrebufferEnabled.GetBoolean(reader),
				PrebufferInMemory = PrebufferInMemory.GetBoolean(reader),
				PrebufferSeconds = PrebufferSeconds.GetInt32(reader),
				EdgeStorageEnabled = EdgeStorageEnabled.GetBoolean(reader),
				EdgeStoragePlayback = EdgeStoragePlayback.GetBoolean(reader),
				RecordKeyframesOnly = RecordKeyframes.GetBoolean(reader),
				ManualTimeoutEnabled = ManualTimeoutEnabled.GetBoolean(reader),
				ManualTimeoutMinutes = ManualTimeout.GetInt32(reader),
				RecordOnRelatedDevices = RecordOnRelatedDevices.GetBoolean(reader),
				GisPoint = GisPoint.GetGeographyString(reader),
				ShortName = ShortName.GetString(reader),
				Shortcut = Shortcut.GetString(reader),
				Icon = Icon.GetInt32(reader),
				CoverageDirection = CoverageDirection.GetDouble(reader),
				CoverageDepth = CoverageDepth.GetDouble(reader),
				CoverageFieldOfView = CoverageFieldOfView.GetDouble(reader),
				HardwareId = HardwareId.GetGuid(reader),
				RecordingServerId = RecorderId.GetGuid(reader),
				MulticastEnabled = MulticastEnabledBinder.GetBoolean(reader),
				PtzManualSessionTimeout = PtzManualSessionTimeoutBinder.GetInt32(reader),
				PtzReservedSessionTimeout = PtzReservedSessionTimeoutBinder.GetInt32(reader),
				PtzExtendedManualSessionTimeout = PtzExtendedManualSessionTimeoutBinder.GetInt32(reader),
				PtzEnabled = ToBool(PtzEnabledBinder.GetString(reader, allowNulls: true)),
				FailoverLiveOnly = FailoverLiveOnly.GetBoolean(reader),
				DisableOnFailover = DisableOnFailover.GetBoolean(reader)
			};
		}

		private static bool ToBool(string val)
		{
			if (bool.TryParse(val, out var result))
			{
				return result;
			}
			return false;
		}
	}

	private static class DeviceBasicInfoColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		public static DeviceBasicInfoContract Bind(DatabaseDataReader reader)
		{
			return new DeviceBasicInfoContract
			{
				Id = IDDevice.GetGuid(reader),
				Name = Name.GetString(reader),
				DeviceType = DatabaseDeviceTypeStrings.GetDeviceTypeEnum(DeviceType.GetString(reader)),
				HardwareId = HardwareId.GetGuid(reader)
			};
		}
	}

	private static class DevicePresetsColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder Presets = new DatabaseColumnBinder("Presets");

		private static DatabaseColumnBinder Settings = new DatabaseColumnBinder("Settings");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("IDRecorder");

		public static DevicePtzPresets Bind(DatabaseDataReader reader)
		{
			return new DevicePtzPresets
			{
				Id = IDDevice.GetGuid(reader),
				PresetsXml = Presets.GetString(reader),
				SettingsXml = Settings.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				HardwareId = HardwareId.GetGuid(reader),
				RecordingServerId = RecorderId.GetGuid(reader)
			};
		}
	}

	private static class DevicePatrollingProfileColumns
	{
		private static DatabaseColumnBinder IDPatrollingProfile = new DatabaseColumnBinder("IDPatrollingProfile");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("Definition");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("IDRecorder");

		public static DevicePatrollingProfile Bind(DatabaseDataReader reader)
		{
			return new DevicePatrollingProfile
			{
				Id = IDPatrollingProfile.GetGuid(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				DefinitionXml = Definition.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				DeviceId = IDDevice.GetGuid(reader),
				HardwareId = HardwareId.GetGuid(reader),
				RecordingServerId = RecorderId.GetGuid(reader)
			};
		}
	}

	private static class DeviceTrackDataContractColumns
	{
		private static DatabaseColumnBinder DeviceId = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder TrackId = new DatabaseColumnBinder("IdTrack");

		private static DatabaseColumnBinder TrackDefinitionId = new DatabaseColumnBinder("IdTrackDefinition");

		private static DatabaseColumnBinder MergeEdge = new DatabaseColumnBinder("MergeEdge");

		private static DatabaseColumnBinder PlaybackDefault = new DatabaseColumnBinder("DefaultPlayback");

		private static DatabaseColumnBinder InUse = new DatabaseColumnBinder("InUse");

		public static DeviceTrackContract Bind(DatabaseDataReader reader)
		{
			return new DeviceTrackContract(DeviceId.GetGuid(reader), TrackId.GetGuid2(reader), TrackDefinitionId.GetGuid2(reader), PlaybackDefault.GetBoolean(reader), MergeEdge.GetBoolean(reader), InUse.GetBoolean(reader), null);
		}
	}

	private static class DeviceStreamsDataColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		private static DatabaseColumnBinder DeviceIndex = new DatabaseColumnBinder("DeviceIndex");

		private static DatabaseColumnBinder StreamsXml = new DatabaseColumnBinder("Streams");

		private static DatabaseColumnBinder HardwareDefinitionDigest = new DatabaseColumnBinder("HardwareDefinitionDigest");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("IDRecorder");

		public static DeviceStreamsContract Bind(DatabaseDataReader reader)
		{
			return new DeviceStreamsContract
			{
				Id = IDDevice.GetGuid(reader),
				DeviceType = DatabaseDeviceTypeStrings.GetDeviceTypeEnum(DeviceType.GetString(reader)),
				DeviceIndex = DeviceIndex.GetInt32(reader),
				StreamsXml = StreamsXml.GetString(reader),
				HardwareDefinitionDigest = HardwareDefinitionDigest.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				HardwareId = HardwareId.GetGuid(reader),
				RecordingServerId = RecorderId.GetGuid(reader)
			};
		}
	}

	private static class DeviceEventsColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder DeviceIndex = new DatabaseColumnBinder("DeviceIndex");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder HardwareDefinitionDigest = new DatabaseColumnBinder("HardwareDefinitionDigest");

		private static DatabaseColumnBinder EventSettings = new DatabaseColumnBinder("EventSettings");

		private static DatabaseColumnBinder HardwareDefinition = new DatabaseColumnBinder("HardwareDefinition");

		public static DeviceEvents Bind(DatabaseDataReader reader)
		{
			return new DeviceEvents
			{
				Id = IDDevice.GetGuid(reader),
				DeviceIndex = DeviceIndex.GetInt32(reader),
				LastModified = LastModified.GetDateTime(reader),
				HardwareId = HardwareId.GetGuid(reader),
				HardwareDefinitionDigest = HardwareDefinitionDigest.GetString(reader),
				EventSettings = EventSettings.GetString(reader),
				HardwareDefinition = HardwareDefinition.GetString(reader)
			};
		}
	}

	private static class DeviceMotionDetectionColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder MotionDetectionXml = new DatabaseColumnBinder("Settings");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("IDRecorder");

		public static DeviceMotionDetectionDataContract Bind(DatabaseDataReader reader)
		{
			return new DeviceMotionDetectionDataContract
			{
				Id = IDDevice.GetGuid(reader),
				MotionDetectionEnabled = Enabled.GetBoolean(reader),
				MotionDetectionXml = MotionDetectionXml.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				HardwareId = HardwareId.GetGuid(reader),
				RecordingServerId = RecorderId.GetGuid(reader)
			};
		}
	}

	private static class DevicePrivacyMaskColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder PrivacyMaskXml = new DatabaseColumnBinder("PrivacyMask");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder RecorderId = new DatabaseColumnBinder("IDRecorder");

		public static DevicePrivacyMaskDataContract Bind(DatabaseDataReader reader)
		{
			return new DevicePrivacyMaskDataContract
			{
				LastModified = LastModified.GetDateTime(reader),
				Id = IDDevice.GetGuid(reader),
				PrivacyMaskXml = PrivacyMaskXml.GetString(reader, allowNulls: true),
				Enabled = Enabled.GetBoolean(reader),
				HardwareId = HardwareId.GetGuid(reader),
				RecordingServerId = RecorderId.GetGuid(reader)
			};
		}
	}

	public DeviceComponent()
	{
	}

	public DeviceComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public Device QueryDeviceById(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Device)null;
			}
			Device result = DeviceColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Device[] QueryDevicesByIds(IEnumerable<string> ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_by_ids");
			BindIdTable("@ids", ids.Distinct());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Device> list = new List<Device>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Dictionary<string, string> QueryDeviceNamesByIds(IEnumerable<string> ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_names_by_ids");
			BindIdTable("@ids", ids.Distinct());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			while (databaseDataReader.Read())
			{
				dictionary.Add(DeviceColumns.IDDevice.GetString(databaseDataReader), DeviceColumns.Name.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return dictionary;
		});
	}

	public Device[] QueryDevicesByIdsSorted(IEnumerable<string> ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_by_ids_sorted");
			BindIdTable("@ids", ids.Distinct());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Device> list = new List<Device>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public DeviceBasicInfoContract[] QueryDeviceInfoByIds(IEnumerable<string> ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("device_info_query_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceBasicInfoContract> list = new List<DeviceBasicInfoContract>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceBasicInfoColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDeviceIdsGeneric(string recorderId, string hardwareId, DeviceTypeSearch deviceTypeSearch)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_ids_generic");
			BindGuidType("@IDRecorder", recorderId, allowNullOrEmpty: true);
			BindGuidType("@IDHardware", hardwareId, allowNullOrEmpty: true);
			BindEnumType("@DeviceTypeSearch", typeof(DeviceTypeSearch), deviceTypeSearch);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDeviceIdsWithMediaOnRecorder(string recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_ids_with_media_on_recorder");
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDevicesIdsByGroup(string deviceGroupId, DeviceTypeSearch deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_ids_by_device_group");
			BindGuidType("@IDDeviceGroup", deviceGroupId);
			BindEnumType("@DeviceTypeSearch", typeof(DeviceTypeSearch), deviceType);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDeviceIdsByFilter(string[] roleIds, string deviceGroupId, DeviceTypeSearch searchDeviceType, string filter, int maxResults, int startIndex, bool checkDevicesEnabled)
	{
		string deviceTypeString = DatabaseDeviceTypeStrings.GetDeviceTypeString(searchDeviceType);
		string value = "1f30d14b-ba91-4e8a-8838-eda17931f8fc";
		bool num = roleIds.Contains(value, StringComparer.OrdinalIgnoreCase);
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		if (!num)
		{
			text = $" INNER JOIN [SecurityAvailableDevicesTable](@RoleIds, 'GENERIC_READ', {(checkDevicesEnabled ? 1 : 0)}, null) ad ON d.IDDevice = ad.IDDevice ";
		}
		else if (checkDevicesEnabled)
		{
			text2 = " JOIN [Hardware] hw on hw.[IDHardware] = d.[IDHardware] AND hw.[Enabled] = 1";
			text3 = " AND d.[Enabled] = 1";
		}
		string sqlStatement;
		if (string.IsNullOrEmpty(filter))
		{
			sqlStatement = string.Format(CultureInfo.InvariantCulture, "SELECT d.[IDDevice],d.[Name] FROM dbo.Devices d" + text + " LEFT JOIN [RecordingStorageDevice] rs ON rs.[device_id] = d.[IDDevice] JOIN [DeviceGroupMembers] dg on dg.[IDDeviceGroup] = '{2}' AND dg.IDDevice = d.[IDDevice] " + text2 + " WHERE d.[DeviceType] = '{0}'" + text3 + " ORDER BY d.[Name] OFFSET {3} ROWS FETCH NEXT {1} ROWS ONLY", deviceTypeString, maxResults, deviceGroupId, startIndex);
		}
		else
		{
			sqlStatement = string.Format(CultureInfo.InvariantCulture, "SELECT d.[IDDevice],d.[Name] FROM dbo.Devices d" + text + " LEFT JOIN [RecordingStorageDevice] rs ON rs.[device_id] = d.[IDDevice] JOIN [DeviceGroupMembers] dg on dg.[IDDeviceGroup] = '{3}' AND dg.IDDevice = d.[IDDevice] " + text2 + " WHERE d.[DeviceType] = '{0}'" + text3 + " AND {1} ORDER BY d.[Name] OFFSET {4} ROWS FETCH NEXT {2} ROWS ONLY", deviceTypeString, filter, maxResults, deviceGroupId, startIndex);
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sqlStatement.Length);
			BindIdTable("@RoleIds", roleIds);
			AddStatement(sqlStatement);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDeviceIdsByTypeAndFilter(string[] roleIds, DeviceTypeSearch searchDeviceType, string filter)
	{
		string deviceTypeString = DatabaseDeviceTypeStrings.GetDeviceTypeString(searchDeviceType);
		string value = "1f30d14b-ba91-4e8a-8838-eda17931f8fc";
		bool num = roleIds.Contains(value, StringComparer.OrdinalIgnoreCase);
		string empty = string.Empty;
		string text = string.Empty;
		if (!num)
		{
			empty = " INNER JOIN [SecurityAvailableDevicesTable](@RoleIds, 'GENERIC_READ', 1, null) ad ON d.IDDevice = ad.IDDevice ";
		}
		else
		{
			empty = " JOIN [Hardware] hw on hw.[IDHardware] = d.[IDHardware] AND hw.[Enabled] = 1";
			text = "AND d.[Enabled] = 1 ";
		}
		string sqlStatement;
		if (string.IsNullOrEmpty(filter))
		{
			sqlStatement = string.Format(CultureInfo.InvariantCulture, "SELECT d.[IDDevice],d.[Name] FROM dbo.Devices d" + empty + " WHERE d.[DeviceType] = '{0}' " + text, deviceTypeString);
		}
		else
		{
			sqlStatement = string.Format(CultureInfo.InvariantCulture, "SELECT d.[IDDevice],d.[Name] FROM dbo.Devices d" + empty + " WHERE d.[DeviceType] = '{0}' AND {1} " + text, deviceTypeString, filter);
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sqlStatement.Length);
			BindIdTable("@RoleIds", roleIds);
			AddStatement(sqlStatement);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDeviceIdsBySearch(string deviceNameContains, string deviceShortNameContains, string deviceDescriptionContains, string hardwareAddressContains, IEnumerable<DeviceType> deviceTypes, string deviceIdEquals, string hardwareIdEquals, bool includeDisabledDevices)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			IEnumerable<string> idArray = deviceTypes.Select((DeviceType t) => DatabaseDeviceTypeStrings.GetDeviceTypeString(t));
			PrepareStoredProcedure("device_ids_search");
			BindString("@DeviceNameContains", EscapeForLikePredicate(deviceNameContains), 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@DeviceShortNameContains", EscapeForLikePredicate(deviceShortNameContains), 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@DeviceDescriptionContains", EscapeForLikePredicate(deviceDescriptionContains), 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@HardwareAddressContains", EscapeForLikePredicate(hardwareAddressContains), 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@IDDeviceEquals", deviceIdEquals, 37, allowNull: false, SqlDbType.VarChar);
			BindString("@IDHardwareEquals", hardwareIdEquals, 37, allowNull: false, SqlDbType.VarChar);
			BindResultTable("@DeviceTypes", idArray);
			BindBoolean("@IncludeDisabledDevices", includeDisabledDevices);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
		static string EscapeForLikePredicate(string value)
		{
			return value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
		}
	}

	public string[] QueryDevicesIdsByRecordingStorage(string recordingStorageId, DeviceTypeSearch deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_ids_by_recording_storage");
			BindGuidType("@IDRecordingStorage", recordingStorageId);
			BindEnumType("@DeviceTypeSearch", typeof(DeviceTypeSearch), deviceType);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UpdateDevice(string id, string name, string description, bool? enabled, bool? prebufferEnabled, bool? prebufferInMemory, int? prebufferSeconds, bool? recordingEnabled, double? recordingFramerate, bool? edgeStorageEnabled, bool? edgestoragePlaybackEnabled, bool? recordKeyframesOnly, bool? manualTimeoutEnabled, int? manualTimeoutMinutes, bool? recordOnRelatedDevices, string gisPoint, string shortName, int? icon, double? coverageDirection, double? coverageDepth, double? coverageFieldOfView, CustomProperty[] customProperties, string shortcut, Guid sessionId, bool? multicastEnabled, int? ptzManualSessionTimeout, int? ptzReservedSessionTimeout, int? ptzExtendedManualSessionTimeout, bool? failoverLiveOnly, bool? failoverDisabled)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device");
			BindGuidType("@IDDevice", id);
			BindNameType("@name", name, allowNull: true);
			BindDescription("@description", description, allowNull: true);
			BindBoolean("@enabled", enabled);
			BindBoolean("@prebuffer_enabled", prebufferEnabled);
			BindBoolean("@prebuffer_in_memory", prebufferInMemory);
			BindInt("@prebuffer_seconds", prebufferSeconds);
			BindBoolean("@recording_enabled", recordingEnabled);
			BindFloat("@recording_framerate", recordingFramerate);
			BindBoolean("@edgestorage_enabled", edgeStorageEnabled);
			BindBoolean("@edgestorage_playback_enabled", edgestoragePlaybackEnabled);
			BindBoolean("@record_keyframes", recordKeyframesOnly);
			BindBoolean("@manual_timeout_enabled", manualTimeoutEnabled);
			BindInt("@manual_timeout", manualTimeoutMinutes);
			BindBoolean("@record_on_related_devices", recordOnRelatedDevices);
			BindText("@gis_point_string", gisPoint, allowNull: true);
			BindShortNameType("@short_name", shortName, allowNull: true);
			BindInt("@icon", icon);
			BindFloat("@coverage_direction", coverageDirection);
			BindFloat("@coverage_depth", coverageDepth);
			BindFloat("@coverage_field_of_view", coverageFieldOfView);
			BindShortcutType("@shortcut", shortcut, allowNull: true);
			BindCustomProperties("@custom_properties", customProperties);
			BindGuidType("@session_id", sessionId.ToString());
			BindBoolean("@multicastEnabled", multicastEnabled);
			BindInt("@ptzManualSessionTimeout", ptzManualSessionTimeout);
			BindInt("@ptzReservedSessionTimeout", ptzReservedSessionTimeout);
			BindInt("@ptzExtendedManualSessionTimeout", ptzExtendedManualSessionTimeout);
			BindBoolean("@failoverLiveOnly", failoverLiveOnly);
			BindBoolean("@failoverDisabled", failoverDisabled);
			ExecuteNonQuery();
		});
	}

	public DevicePtzPresets QueryPtzPresets(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_presets");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? null : DevicePresetsColumns.Bind(databaseDataReader);
		});
	}

	public DevicePatrollingProfile[] QueryPatrollingProfilesByPreset(string deviceId, string presetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_patrolling_profiles_by_preset");
			BindGuidType("@device_id", deviceId);
			BindGuidType("@preset_id", presetId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DevicePatrollingProfile> list = new List<DevicePatrollingProfile>();
			while (databaseDataReader.Read())
			{
				list.Add(DevicePatrollingProfileColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public void UpdatePtzPresets(string deviceId, DateTime dateTime, string presetsXml, Guid sessionId, string presetId, bool createPreset)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_presets");
			BindGuidType("@device_id", deviceId);
			BindDateTime("@last_modified", dateTime);
			BindXml("@presets_xml", presetsXml);
			BindGuidType("@session_id", sessionId.ToString());
			BindGuidType("@preset_id", presetId);
			BindBoolean("@create_preset", createPreset);
			ExecuteNonQuery();
		});
	}

	public string DeletePtzPreset(string deviceId, string presetId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_device_preset");
			BindGuidType("@device_id", deviceId);
			BindGuidType("@preset_id", presetId);
			BindGuidType("@session_id", sessionId.ToString());
			return (string)ExecuteScalar();
		});
	}

	public void UpdatePtzPresetsOrder(string deviceId, string[] presetIds, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_preset_order");
			BindGuidType("@device_id", deviceId);
			BindArrayAsXml("@ids", presetIds);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void CreateDeviceTrack(DeviceTrackContract deviceTrackContract)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("device_track_create");
			BindGuidType("@IDDevice", deviceTrackContract.DeviceId);
			BindGuidType("@IDTrackDefinition", deviceTrackContract.TrackDefinitionId);
			BindGuidType("@IDTrack", deviceTrackContract.TrackId);
			BindBoolean("@MergeEdge", deviceTrackContract.MergeEdge);
			BindBoolean("@DefaultPlayback", deviceTrackContract.DefaultPlayback);
			BindBoolean("@InUse", deviceTrackContract.InUse);
			ExecuteNonQuery();
		});
	}

	public DeviceStreamsContract QueryDeviceStreams(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.Snapshot);
				PrepareStoredProcedure("query_device_streams");
				BindGuidType("@IDDevice", deviceId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				if (!databaseDataReader.Read())
				{
					return (DeviceStreamsContract)null;
				}
				DeviceStreamsContract deviceStreamsContract = DeviceStreamsDataColumns.Bind(databaseDataReader);
				if (databaseDataReader.NextResult())
				{
					throw new DatabaseConfigExtraRowsException();
				}
				PrepareStoredProcedure("device_tracks_query_by_device");
				BindGuidType("@IDDevice", deviceId);
				databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					deviceStreamsContract.DeviceTracks.Add(DeviceTrackDataContractColumns.Bind(databaseDataReader));
				}
				if (databaseDataReader.NextResult())
				{
					throw new DatabaseConfigExtraRowsException();
				}
				CommitTransaction();
				return deviceStreamsContract;
			}
			catch (DatabaseException)
			{
				RollbackTransaction();
				throw;
			}
			catch (VmoServerException)
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public void UpdateStreams(DeviceStreamsContract deviceStreamsContract, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				PrepareStoredProcedure("update_device_streams");
				BindGuidType("@device_id", deviceStreamsContract.Id);
				BindDateTime("@last_modified", deviceStreamsContract.LastModified);
				BindXml("@streams_xml", deviceStreamsContract.StreamsXml);
				BindGuidType("@session_id", sessionId);
				ExecuteNonQuery();
				foreach (DeviceTrackContract deviceTrack in deviceStreamsContract.DeviceTracks)
				{
					PrepareStoredProcedure("device_track_update");
					BindGuidType("@IDTrack", deviceTrack.TrackId);
					BindBoolean("@MergeEdge", deviceTrack.MergeEdge);
					BindBoolean("@DefaultPlayback", deviceTrack.DefaultPlayback);
					BindBoolean("@InUse", deviceTrack.InUse);
					BindDateTime2("@OnlyUseMediaAfter", deviceTrack.OnlyUseMediaAfter);
					ExecuteNonQuery();
				}
				CommitTransaction();
			}
			catch (DatabaseException)
			{
				RollbackTransaction();
				throw;
			}
			catch (VmoServerException)
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public DeviceEvents QueryDeviceEvents(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_events_and_definition_by_device_id");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? null : DeviceEventsColumns.Bind(databaseDataReader);
		});
	}

	public void UpdateEvents(string hardwareId, DateTime lastModified, string eventsXml, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_hardware_events");
			BindGuidType("@hardware_id", hardwareId);
			BindDateTime("@last_modified", lastModified);
			BindXml("@events_xml", eventsXml);
			BindGuidType("@session_id", sessionId);
			ExecuteNonQuery();
		});
	}

	public DeviceMotionDetectionDataContract QueryDeviceMotionDetection(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_motion_detection");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? null : DeviceMotionDetectionColumns.Bind(databaseDataReader);
		});
	}

	public DeviceMotionDetectionDataContract[] QueryDeviceMotionDetections(string[] deviceIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_motion_detection_by_ids");
			BindIdTable("@IDDevices", deviceIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceMotionDetectionDataContract> list = new List<DeviceMotionDetectionDataContract>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceMotionDetectionColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UpdateDeviceMotionDetection(string deviceId, DateTime lastModified, string motionDetectionXml, bool enabled, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_motion_detection");
			BindGuidType("@device_id", deviceId);
			BindDateTime("@last_modified", lastModified);
			BindXml("@motion_detection_xml", motionDetectionXml);
			BindBoolean("@enabled", enabled);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public DevicePrivacyMaskDataContract QueryDevicePrivacyMask(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_privacy_mask");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			new DevicePrivacyMaskDataContract();
			return (!databaseDataReader.Read()) ? null : DevicePrivacyMaskColumns.Bind(databaseDataReader);
		});
	}

	public void UpdateDevicePrivacyMask(string deviceId, string privacyMaskXml, bool? enabled, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_privacy_mask");
			BindGuidType("@device_id", deviceId);
			BindXml("@privacy_mask_xml", privacyMaskXml, allowNull: true);
			BindBoolean("@enabled", enabled);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public string[] QueryOtherRecordingServersWithMedia(string deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[query_device_data_distribution]");
			BindGuidType("@device_id", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("recorder_id");
			while (databaseDataReader.Read())
			{
				hashSet.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			return hashSet.ToArray();
		});
	}

	public string[] QueryOtherDevicesWithMediaOnRecorder(string recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[query_device_data_distribution]");
			BindGuidType("@recorder_id", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("device_id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public List<string> FilterOnEnabledDevices(HashSet<Guid> deviceList)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[filter_on_enabled_devices]");
			BindIdTable("@object_ids", deviceList.Select((Guid id) => id.ToString()));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("device_id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			return list;
		});
	}

	public bool QueryValidPoint(string gisPoint)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_valid_point");
			BindStringMax("@valid_point", gisPoint);
			DatabaseParameter databaseParameter = BindReturnValue();
			ExecuteNonQuery();
			return databaseParameter.Value.ToString().Equals("1", StringComparison.Ordinal);
		});
	}
}
