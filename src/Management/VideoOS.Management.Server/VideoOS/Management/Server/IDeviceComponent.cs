using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IDeviceComponent : IDisposable
{
	Device QueryDeviceById(string deviceId);

	Device[] QueryDevicesByIds(IEnumerable<string> ids);

	Dictionary<string, string> QueryDeviceNamesByIds(IEnumerable<string> ids);

	Device[] QueryDevicesByIdsSorted(IEnumerable<string> ids);

	DeviceBasicInfoContract[] QueryDeviceInfoByIds(IEnumerable<string> ids);

	string[] QueryDeviceIdsGeneric(string recorderId, string hardwareId, DeviceTypeSearch deviceTypeSearch);

	string[] QueryDeviceIdsWithMediaOnRecorder(string recorderId);

	string[] QueryDevicesIdsByGroup(string deviceGroupId, DeviceTypeSearch deviceType);

	string[] QueryDeviceIdsByFilter(string[] roleIds, string deviceGroupId, DeviceTypeSearch deviceType, string filter, int maxResults, int startIndex, bool checkHardwareEnabled);

	string[] QueryDeviceIdsByTypeAndFilter(string[] roleIds, DeviceTypeSearch searchDeviceType, string filter);

	string[] QueryDeviceIdsBySearch(string deviceNameContains, string deviceShortNameContains, string deviceDescriptionContains, string hardwareAddressContains, IEnumerable<DeviceType> deviceTypes, string deviceIdEquals, string hardwareIdEquals, bool includeDisabledDevices);

	string[] QueryDevicesIdsByRecordingStorage(string recordingStorageId, DeviceTypeSearch deviceType);

	void UpdateDevice(string id, string name, string description, bool? enabled, bool? prebufferEnabled, bool? prebufferInMemory, int? prebufferSeconds, bool? recordingEnabled, double? recordingFramerate, bool? edgeStorageEnabled, bool? edgeStoragePlaybackEnabled, bool? recordKeyframesOnly, bool? manualTimeoutEnabled, int? manualTimeoutMinutes, bool? recordOnRelatedDevices, string gisPoint, string shortName, int? icon, double? coverageDirection, double? coverageDepth, double? coverageFieldOfView, CustomProperty[] customProperties, string shortcut, Guid sessionId, bool? multicastEnabled, int? ptzManualSessionTimeout, int? ptzReservedSessionTimeout, int? ptzExtendedManualSessionTimeout, bool? failoverLiveOnly, bool? failoverDisabled);

	DevicePtzPresets QueryPtzPresets(string deviceId);

	void UpdatePtzPresets(string deviceId, DateTime lastModified, string presetsXml, Guid sessionId, string presetId, bool createPreset);

	string DeletePtzPreset(string deviceId, string presetId, Guid sessionId);

	void UpdatePtzPresetsOrder(string deviceId, string[] presetIds, Guid sessionId);

	DeviceStreamsContract QueryDeviceStreams(string deviceId);

	void CreateDeviceTrack(DeviceTrackContract deviceTrackContract);

	void UpdateStreams(DeviceStreamsContract deviceStreamsContract, Guid sessionId);

	DeviceEvents QueryDeviceEvents(string deviceId);

	void UpdateEvents(string deviceId, DateTime lastModified, string eventsXml, Guid sessionId);

	DevicePatrollingProfile[] QueryPatrollingProfilesByPreset(string deviceId, string presetId);

	string[] QueryOtherRecordingServersWithMedia(string deviceId);

	string[] QueryOtherDevicesWithMediaOnRecorder(string recorderId);

	DeviceMotionDetectionDataContract QueryDeviceMotionDetection(string deviceId);

	DeviceMotionDetectionDataContract[] QueryDeviceMotionDetections(string[] deviceIds);

	void UpdateDeviceMotionDetection(string deviceId, DateTime lastModified, string motionDetectionXml, bool enabled, Guid sessionId);

	DevicePrivacyMaskDataContract QueryDevicePrivacyMask(string deviceId);

	void UpdateDevicePrivacyMask(string deviceId, string privacyMaskXml, bool? enabled, Guid sessionId);

	List<string> FilterOnEnabledDevices(HashSet<Guid> deviceList);

	bool QueryValidPoint(string gisPoint);
}
