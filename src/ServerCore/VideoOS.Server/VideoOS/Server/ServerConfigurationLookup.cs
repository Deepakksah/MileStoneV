using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.Properties;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server;

public sealed class ServerConfigurationLookup : IServerConfigurationLookup
{
	private Server server;

	public ServerConfigurationLookup(Server server)
	{
		this.server = server;
	}

	public bool IsRecorderRegistered(Guid recorderId)
	{
		try
		{
			return RecorderConfigData.RecorderExists(server.DataComponentFactory, recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return false;
	}

	public bool RecorderExists(Guid recorderId)
	{
		try
		{
			return RecorderConfigData.RecorderExists(server.DataComponentFactory, recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
	}

	public bool IsRecorderEnabled(Guid recorderId)
	{
		try
		{
			return RecorderConfigData.RecorderEnabled(server.DataComponentFactory, recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return false;
	}

	public bool RecorderHasLockedDeviceDataDistribution(Guid recorderId)
	{
		using IEvidenceLockComponent evidenceLockComponent = server.DataComponentFactory.CreateEvidenceLockComponent();
		return evidenceLockComponent.RecorderHasLockedDeviceDataDistribution(recorderId);
	}

	public string FindRecorderDisplayName(Guid recorderId)
	{
		try
		{
			return ConfigurationDataManager.FindRecorderNameByRecorderId(recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	public Guid FindRecorderIdFromServiceId(Guid serviceId)
	{
		return ConfigurationDataManager.FindRecorderIdFromServiceId(serviceId);
	}

	public string FindDeviceDisplayName(Guid deviceId)
	{
		try
		{
			return ConfigurationDataManager.FindDeviceNameByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	public bool DeviceHasEvidenceLock(string deviceId)
	{
		using IEvidenceLockComponent evidenceLockComponent = server.DataComponentFactory.CreateEvidenceLockComponent();
		return evidenceLockComponent.DeviceHasEvidenceLock(deviceId);
	}

	public Guid QueryDeviceEdgeTrackIdByDeviceId(Guid deviceId)
	{
		return ConfigurationDataManager.QueryDeviceEdgeTrackIdByDeviceId(deviceId);
	}

	public DeviceDataDistribution[] QueryDeviceDataDistribution(Guid deviceId, Guid recorderId)
	{
		return DeviceDataDistribution.QueryDeviceDataDistribution(new ServerEnvironment().DataComponentFactory, deviceId, recorderId);
	}

	public string FindSourceDisplayName(Guid sourceId, SourceType sourceType)
	{
		string result = Resources.EventGeneratorSourceUnknown;
		switch (sourceType)
		{
		case SourceType.Device:
			try
			{
				result = ConfigurationDataManager.FindDeviceNameByDeviceId(sourceId);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			break;
		case SourceType.Recorder:
			try
			{
				result = ConfigurationDataManager.FindRecorderNameByRecorderId(sourceId);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			break;
		case SourceType.External:
			result = Resources.EventGeneratorSourceExternal;
			break;
		case SourceType.System:
			result = Resources.EventGeneratorSourceSystem;
			break;
		case SourceType.Server:
			result = Resources.EventGeneratorSourceServer;
			break;
		}
		return result;
	}

	public SourceType FindEventSourceTypeFromEventTypeID(Guid eventTypeId)
	{
		string text = string.Empty;
		try
		{
			text = ConfigurationDataManager.FindEventTypeGeneratorTypeByEventTypeId(eventTypeId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		if (!string.IsNullOrEmpty(text))
		{
			return ConvertEventGeneratorTypeToSourceType(text);
		}
		return SourceType.External;
	}

	public Subtype FindEventSourceSubtypeFromEventTypeID(Guid eventTypeId)
	{
		string text = string.Empty;
		try
		{
			text = ConfigurationDataManager.FindEventTypeGeneratorSubtypeByEventTypeId(eventTypeId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (ConfigurationDataSubsystemException)
		{
		}
		if (!string.IsNullOrEmpty(text))
		{
			return ConvertEventGeneratorSubtypeToSubType(text);
		}
		return Subtype.Unknown;
	}

	public string FindEventDisplayNameFromEventTypeID(Guid eventTypeId, string defaultValue)
	{
		string result = defaultValue;
		string text = null;
		try
		{
			text = ConfigurationDataManager.FindEventTypeDefinitionByEventTypeId(eventTypeId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		if (!string.IsNullOrEmpty(text))
		{
			DefinitionValues definitionValues = EventDefinition.GetDefinitionValues(text);
			result = definitionValues.DisplayName;
		}
		return result;
	}

	public Guid QuerydDefaultStorageIdByRecorder(Guid recorderId)
	{
		return ConfigurationDataManager.FindDefaultStorageIdByRecorder(recorderId);
	}

	public string FindAVICodec()
	{
		using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(server.Data));
		SystemSetting systemSetting = configurationManager.SystemSettings[SettingList.AVICodec];
		if (systemSetting == null)
		{
			return string.Empty;
		}
		return systemSetting.Value;
	}

	public IPHardware FindIPHardware(Guid hardwareId)
	{
		using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(Server.Instance.Data));
		return configurationManager.FindHardwareByHardwareId(hardwareId);
	}

	private static SourceType ConvertEventGeneratorTypeToSourceType(string eventGeneratorType)
	{
		switch (eventGeneratorType)
		{
		case "System":
		case "Timer":
		case "SystemDashboard":
			return SourceType.System;
		case "Server":
			return SourceType.Server;
		case "External":
		case "MIP":
			return SourceType.External;
		case "Recorder":
			return SourceType.Recorder;
		case "Device":
		case "RecorderPlugin":
		case "DriverDevice":
		case "MIPDevice":
			return SourceType.Device;
		case "Hardware":
		case "DriverHardware":
			return SourceType.Hardware;
		default:
			return SourceType.Unknown;
		}
	}

	private static Subtype ConvertEventGeneratorSubtypeToSubType(string eventGeneratorSubtype)
	{
		return eventGeneratorSubtype switch
		{
			"Device" => Subtype.Device, 
			"Server" => Subtype.Server, 
			"Disk" => Subtype.Disk, 
			"Storage" => Subtype.Storage, 
			_ => Subtype.Unknown, 
		};
	}
}
