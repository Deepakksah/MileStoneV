using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Plugins;
using VideoOS.Management.Common;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Common.Integration.Configuration;

public class HardwareDeviceMembers : IEnumerable<Device>, IEnumerable
{
	private ConfigurationManager configurationManager;

	private IPHardware hardware;

	private RollbackDictionary<Guid, Device> dictionary;

	private static readonly string _defaultGisPoint = CommonIds.GisPointEmpty;

	private static readonly string _defaultShortName = string.Empty;

	private const int _defaultIcon = 0;

	private const double _defaultCoverageDirection = 0.0;

	private const double _defaultCoverageDepth = 0.0;

	private const double _defaultCoverageFieldOfView = 0.0;

	public Device this[Guid id] => dictionary[id];

	public int Count => dictionary.Count;

	public HardwareDeviceMembers(ConfigurationManager configurationManager, IPHardware hardware)
	{
		if (configurationManager == null)
		{
			throw new ArgumentNullException("configurationManager");
		}
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		this.configurationManager = configurationManager;
		this.hardware = hardware;
		dictionary = new RollbackDictionary<Guid, Device>(configurationManager, this);
		Device[] devicesByHardwareId = this.configurationManager.DeviceItems.GetDevicesByHardwareId(hardware.ID, this.hardware.Recorder.DevicesLoaded);
		foreach (Device device in devicesByHardwareId)
		{
			dictionary.Add(device.ID, device);
		}
	}

	public Device AddNew(Guid id, string name, string description, string location, string deviceType, string settings, string presets, string shortcut, bool multicastEnabled, string streams, int deviceIndex, Guid recordingStorageId, bool failoverLiveOnly, bool disableOnFailover, int ptzManualSessionTimeout, int ptzReservedSessionTimeout, int ptzExtendedManualSessionTimeout, HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode)
	{
		bool enabled = RegisteredDeviceType.GetDeviceTypeEnum(deviceType) == DeviceTypeEnum.Camera;
		return AddNew(id, name, description, location, deviceType, settings, presets, shortcut, multicastEnabled, streams, deviceIndex, recordingStorageId, enabled, failoverLiveOnly, disableOnFailover, ptzManualSessionTimeout, ptzReservedSessionTimeout, ptzExtendedManualSessionTimeout, hardwareAccelerationMode);
	}

	public Device AddNew(Guid id, string name, string description, string location, string deviceType, string settings, string presets, string shortcut, bool multicastEnabled, string streams, int deviceIndex, Guid recordingStorageId, bool enabled, bool failoverLiveOnly, bool disableOnFailover, int ptzManualSessionTimeout, int ptzReservedSessionTimeout, int ptzExtendedManualSessionTimeout, HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode)
	{
		return AddNew(id, name, description, location, deviceType, settings, presets, shortcut, multicastEnabled, streams, deviceIndex, recordingStorageId, enabled, failoverLiveOnly, disableOnFailover, ptzManualSessionTimeout, ptzReservedSessionTimeout, ptzExtendedManualSessionTimeout, _defaultGisPoint, _defaultShortName, 0, 0.0, 0.0, 0.0, hardwareAccelerationMode);
	}

	public Device AddNew(Guid id, string name, string description, string location, string deviceType, string settings, string presets, string shortcut, bool multicastEnabled, string streams, int deviceIndex, Guid recordingStorageId, bool enabled, bool failoverLiveOnly, bool disableOnFailover, int ptzManualSessionTimeout, int ptzReservedSessionTimeout, int ptzExtendedManualSessionTimeout, string gisPoint, string shortName, int icon, double coverageDirection, double coverageDepth, double coverageFieldOfView, HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode)
	{
		Device device = configurationManager.DeviceItems.AddNew(id, name, description, location, deviceType, enabled, hardware, settings, presets, shortcut, multicastEnabled, streams, deviceIndex, failoverLiveOnly, disableOnFailover, ptzManualSessionTimeout, ptzReservedSessionTimeout, ptzExtendedManualSessionTimeout, gisPoint, shortName, icon, coverageDirection, coverageDepth, coverageFieldOfView);
		dictionary.Add(device.ID, device);
		if (RegisteredDeviceType.IsRecordingDeviceType(deviceType))
		{
			configurationManager.DeviceTrackItems.AddNew(id);
		}
		if (recordingStorageId != Guid.Empty)
		{
			double recording_framerate = (RegisteredDeviceType.IsCamera(deviceType) ? 5.0 : 999.0);
			bool record_on_related_devices = RegisteredDeviceType.IsCamera(deviceType);
			configurationManager.Dataset.RecordingStorageDevice.AddRecordingStorageDeviceRow(recordingStorageId.ToString(), device.ID.ToString(), recording_enabled: true, recording_framerate, prebuffer_enabled: true, 3, prebuffer_in_memory: true, edgestorage_enabled: false, edgestorage_playback: false, record_keyframes: false, DateTime.UtcNow, manual_timeout_enabled: true, 5, record_on_related_devices);
		}
		Guid deviceGroupId = Guid.Empty;
		if (RegisteredDeviceType.IsCamera(deviceType))
		{
			deviceGroupId = WellKnownDevicesGroupIds.AllCameras;
		}
		else if (RegisteredDeviceType.IsAudioInput(deviceType))
		{
			deviceGroupId = WellKnownDevicesGroupIds.AllMicrophones;
		}
		else if (RegisteredDeviceType.IsAudioOutput(deviceType))
		{
			deviceGroupId = WellKnownDevicesGroupIds.AllSpeakers;
		}
		else if (RegisteredDeviceType.IsMetadata(deviceType))
		{
			deviceGroupId = WellKnownDevicesGroupIds.AllMetadata;
		}
		else if (RegisteredDeviceType.IsInput(deviceType))
		{
			deviceGroupId = WellKnownDevicesGroupIds.AllInputs;
		}
		else if (RegisteredDeviceType.IsOutput(deviceType))
		{
			deviceGroupId = WellKnownDevicesGroupIds.AllOutputs;
		}
		DeviceGroup deviceGroup = configurationManager.FindDeviceGroupByDeviceGroupId(deviceGroupId);
		if (!deviceGroup.IsDevicesLoaded)
		{
			configurationManager.DeviceGroupMemberItems.AddNew(deviceGroupId, device.ID);
		}
		else
		{
			deviceGroup.Devices.Add(device);
		}
		if (RegisteredDeviceType.IsCamera(deviceType))
		{
			PluginSettings motionDetectionPluginSettings = PluginSettings.GetMotionDetectionPluginSettings();
			motionDetectionPluginSettings.DefaultSettings.Set("HardwareAccelerationMode", hardwareAccelerationMode.ToString());
			motionDetectionPluginSettings.DefaultSettings.Set("GenerateSearchData", configurationManager.SystemSettings[SettingList.AdministratorGenerateSearchData].Value);
			string activeSettings = motionDetectionPluginSettings.DefaultSettings.SaveAsXml();
			bool valueAsBool = configurationManager.SystemSettings[SettingList.AdministratorMotionDetection].ValueAsBool;
			device.DeviceRecorderPlugins.Add(WellKnownPluginIds.MotionPluginId, activeSettings, valueAsBool);
			bool valueAsBool2 = configurationManager.SystemSettings[SettingList.AdministratorLiveMulticast].ValueAsBool;
			device.MulticastEnabled = valueAsBool2;
			configurationManager.DevicePrivacyMaskItems.AddNew(device.ID);
		}
		return device;
	}

	public bool TryGet(Guid id, out Device device)
	{
		return dictionary.TryGetValue(id, out device);
	}

	public bool TryGet(DeviceHwid hwid, out Device device)
	{
		foreach (Device value in dictionary.Values)
		{
			if (value.Hwid == hwid)
			{
				device = value;
				return true;
			}
		}
		device = null;
		return false;
	}

	public void Remove(Device device)
	{
		if (device == null)
		{
			throw new ArgumentNullException("device");
		}
		configurationManager.DeviceDataDistributionEntries.RemoveByDevice(device);
		DeviceGroupMember[] deviceGroupMembersByDevice = configurationManager.DeviceGroupMemberItems.GetDeviceGroupMembersByDevice(device.ID);
		foreach (DeviceGroupMember deviceGroupMember in deviceGroupMembersByDevice)
		{
			DeviceGroup deviceGroupByDeviceGroupId = configurationManager.DeviceGroupItems.GetDeviceGroupByDeviceGroupId(deviceGroupMember.DeviceGroupId);
			if (deviceGroupByDeviceGroupId.IsDevicesLoaded)
			{
				deviceGroupByDeviceGroupId.Devices.Remove(device);
			}
			else
			{
				configurationManager.DeviceGroupMemberItems.Remove(deviceGroupMember.DeviceGroupId, deviceGroupMember.DeviceId);
			}
		}
		if (RegisteredDeviceType.IsCamera(device.DeviceType))
		{
			configurationManager.PatrollingProfiles.RemoveProfilesForDevice(device.ID);
			configurationManager.DevicePrivacyMaskItems.RemoveByDevice(device.ID);
		}
		device.DeviceRecorderPlugins.Clear();
		configurationManager.DeviceLinks.DeviceDeleted(device.ID);
		if (device.IsRecordingDevice)
		{
			configurationManager.DeviceTrackItems.RemoveByDevice(device.ID);
		}
		dictionary.Remove(device.ID);
		configurationManager.DeviceItems.Remove(device.ID);
	}

	public void Clear()
	{
		Device[] array = ToArray();
		foreach (Device device in array)
		{
			Remove(device);
		}
	}

	public Device FindFirstByName(string name)
	{
		foreach (Device value in dictionary.Values)
		{
			if (string.Equals(name, value.Name, StringComparison.CurrentCultureIgnoreCase))
			{
				return value;
			}
		}
		return null;
	}

	public IEnumerator<Device> GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	public Device[] ToArray()
	{
		Device[] array = new Device[dictionary.Values.Count];
		dictionary.Values.CopyTo(array, 0);
		return array;
	}
}
