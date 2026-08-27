using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Log;
using VideoOS.Utilities;

namespace VideoOS.Management.Server;

public sealed class Device
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public DeviceType DeviceType { get; set; }

	public string SettingsXml { get; set; }

	public bool Enabled { get; set; }

	public int DeviceIndex { get; set; }

	public string RecordingStorageId { get; set; }

	public bool RecordingEnabled { get; set; }

	public double RecordingFramerate { get; set; }

	public bool PrebufferEnabled { get; set; }

	public bool PrebufferInMemory { get; set; }

	public int PrebufferSeconds { get; set; }

	public bool EdgeStorageEnabled { get; set; }

	public bool EdgeStoragePlayback { get; set; }

	public bool RecordKeyframesOnly { get; set; }

	public bool ManualTimeoutEnabled { get; set; }

	public int ManualTimeoutMinutes { get; set; }

	public bool RecordOnRelatedDevices { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }

	public string GisPoint { get; set; }

	public string ShortName { get; set; }

	public int Icon { get; set; }

	public double CoverageDirection { get; set; }

	public double CoverageDepth { get; set; }

	public double CoverageFieldOfView { get; set; }

	public string Shortcut { get; set; }

	public bool MulticastEnabled { get; set; }

	public int PtzManualSessionTimeout { get; set; }

	public int PtzReservedSessionTimeout { get; set; }

	public int PtzExtendedManualSessionTimeout { get; set; }

	public bool PtzEnabled { get; set; }

	public bool FailoverLiveOnly { get; set; }

	public bool DisableOnFailover { get; set; }

	public DeviceSettingUtilities SettingUtilities => new DeviceSettingUtilities(SettingsXml);

	public static Device QueryDeviceById(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Device device = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			device = deviceComponent.QueryDeviceById(id);
		}
		if (device != null)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, securityNamespace, "GENERIC_READ", id))
			{
				device = null;
			}
		}
		return device;
	}

	public static bool DeviceExists(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Guid namespaceId;
		try
		{
			namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, id);
		}
		catch (UnknownDeviceException)
		{
			return false;
		}
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, namespaceId, "GENERIC_READ", id))
		{
			return false;
		}
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		return deviceComponent.QueryDeviceById(id) != null;
	}

	public static Device[] QueryDevicesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		Validation.CheckObjectIdArray(ids, "ids", allowNullOrEmptyIds: false);
		Device[] devices = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			devices = deviceComponent.QueryDevicesByIds(ids);
		}
		return FilterDevices(requestContext, devices);
	}

	public static Device[] QueryDevicesByIdsNoSecurity(IGeminiRequestContext requestContext, string[] ids)
	{
		Validation.CheckObjectIdArray(ids, "ids", allowNullOrEmptyIds: false);
		Device[] array = null;
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		return deviceComponent.QueryDevicesByIds(ids);
	}

	public static string[] QueryDeviceIdsGeneric(IGeminiRequestContext requestContext, string recorderId, string hardwareId, DeviceTypeSearch deviceTypeSearch)
	{
		Validation.CheckObjectId(ref recorderId, "recorderId", allowNullOrEmpty: true);
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: true);
		Device[] devices;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			string[] ids = deviceComponent.QueryDeviceIdsGeneric(recorderId, hardwareId, deviceTypeSearch);
			devices = deviceComponent.QueryDevicesByIds(ids);
		}
		devices = FilterDevices(requestContext, devices);
		return devices.Select((Device f) => f.Id).ToArray();
	}

	public static string[] QueryDeviceIdsByDeviceGroup(IGeminiRequestContext requestContext, string deviceGroupId, DeviceTypeSearch deviceTypeSearch)
	{
		Validation.CheckObjectId(ref deviceGroupId, "deviceGroupId", allowNullOrEmpty: true);
		Device[] devices;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			string[] ids = deviceComponent.QueryDevicesIdsByGroup(deviceGroupId, deviceTypeSearch);
			devices = deviceComponent.QueryDevicesByIds(ids);
		}
		devices = FilterDevices(requestContext, devices);
		return devices.Select((Device f) => f.Id).ToArray();
	}

	public static Device[] QueryDevicesByFilter(IGeminiRequestContext requestContext, string deviceGroupId, DeviceTypeSearch deviceTypeSearch, string filter, int maxResults, int startIndex, bool checkDevicesEnabled)
	{
		Validation.CheckObjectId(ref deviceGroupId, "deviceGroupId", allowNullOrEmpty: true);
		string[] roleIds = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles.Select((Guid f) => f.ToString()).ToArray();
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		string[] ids = deviceComponent.QueryDeviceIdsByFilter(roleIds, deviceGroupId, deviceTypeSearch, filter, maxResults, startIndex, checkDevicesEnabled);
		return deviceComponent.QueryDevicesByIdsSorted(ids);
	}

	public static Device[] QueryDevicesByTypeAndFilter(IGeminiRequestContext requestContext, DeviceTypeSearch deviceTypeSearch, string filter)
	{
		string[] roleIds = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles.Select((Guid f) => f.ToString()).ToArray();
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		string[] ids = deviceComponent.QueryDeviceIdsByTypeAndFilter(roleIds, deviceTypeSearch, filter);
		return deviceComponent.QueryDevicesByIdsSorted(ids);
	}

	public static DeviceBasicInfoContract[] QueryDevicesInfoBySearch(IGeminiRequestContext requestContext, string deviceNameContains, string deviceShortNameContains, string deviceDescriptionContains, string hardwareAddressContains, string[] deviceTypes, string deviceIdEquals, string hardwareIdEquals, bool includeDisabledDevices)
	{
		deviceNameContains = deviceNameContains ?? string.Empty;
		deviceShortNameContains = deviceShortNameContains ?? string.Empty;
		deviceDescriptionContains = deviceDescriptionContains ?? string.Empty;
		hardwareAddressContains = hardwareAddressContains ?? string.Empty;
		deviceIdEquals = deviceIdEquals ?? string.Empty;
		hardwareIdEquals = hardwareIdEquals ?? string.Empty;
		deviceTypes = (from s in (deviceTypes ?? Array.Empty<string>()).Distinct(StringComparer.OrdinalIgnoreCase)
			where !string.Equals(s, DeviceType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase)
			select s).ToArray();
		string[] array = deviceTypes;
		for (int num = 0; num < array.Length; num++)
		{
			Validation.CheckEnumValue<DeviceType>(array[num], "deviceTypes");
		}
		IEnumerable<DeviceType> validatedDeviceTypes = deviceTypes.Select((string t) => (DeviceType)Enum.Parse(typeof(DeviceType), t));
		if (DeviceTypeInformation.DeviceTypeDictionary.All((KeyValuePair<DeviceType, DeviceTypeInformation> t) => validatedDeviceTypes.Contains(t.Key)))
		{
			validatedDeviceTypes = Enumerable.Empty<DeviceType>();
		}
		DeviceBasicInfoContract[] devices;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			string[] ids = deviceComponent.QueryDeviceIdsBySearch(deviceNameContains, deviceShortNameContains, deviceDescriptionContains, hardwareAddressContains, validatedDeviceTypes, deviceIdEquals, hardwareIdEquals, includeDisabledDevices);
			devices = deviceComponent.QueryDeviceInfoByIds(ids);
		}
		return FilterDevices(requestContext, devices);
	}

	public static string[] QueryDeviceIdsByRecordingStorage(IGeminiRequestContext requestContext, string recordingStorageId, DeviceTypeSearch deviceTypeSearch)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: true);
		Device[] devices;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			string[] ids = deviceComponent.QueryDevicesIdsByRecordingStorage(recordingStorageId, deviceTypeSearch);
			devices = deviceComponent.QueryDevicesByIds(ids);
		}
		devices = FilterDevices(requestContext, devices);
		return devices.Select((Device f) => f.Id).ToArray();
	}

	public static void UpdateDeviceValidation(IGeminiRequestContext requestContext, string id, string name, string description, bool? enabled, bool? recordingEnabled, bool? prebufferEnabled, bool? prebufferInMemory, int? prebufferSeconds, double? recordingFramerate, bool? edgeStorageEnabled, bool? edgeStoragePlaybackEnabled, bool? recordKeyframesOnly, bool? manualTimeoutEnabled, int? manualTimeoutMinutes, bool? recordOnRelatedDevices, string gisPoint, string shortName, int? icon, double? coverageDirection, double? coverageDepth, double? coverageFieldOfView, string shortcut)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref id, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		Validation.CheckBooleanValue(enabled, "Enabled", allowNull: true);
		Validation.CheckBooleanValue(recordingEnabled, "RecordingEnabled", allowNull: true);
		Validation.CheckBooleanValue(prebufferEnabled, "PrebufferEnabled", allowNull: true);
		Validation.CheckBooleanValue(prebufferInMemory, "PrebufferInMemory", allowNull: true);
		Validation.CheckIsPositive(prebufferSeconds, "PrebufferSeconds", allowNull: true);
		Validation.CheckBooleanValue(edgeStorageEnabled, "EdgeStorageEnabled", allowNull: true);
		Validation.CheckBooleanValue(edgeStoragePlaybackEnabled, "EdgeStoragePlaybackEnabled", allowNull: true);
		Validation.CheckBooleanValue(recordKeyframesOnly, "RecordKeyframesOnly", allowNull: true);
		Validation.CheckBooleanValue(recordOnRelatedDevices, "RecordOnRelatedDevices", allowNull: true);
		Validation.CheckRecordingFramerate(recordingFramerate, "RecordingFramerate", allowNull: true);
		Validation.CheckBooleanValue(manualTimeoutEnabled, "ManualRecordingTimeoutEnabled", allowNull: true);
		Validation.CheckIsPositive(manualTimeoutMinutes, "ManualRecordingTimeoutMinutes", allowNull: true);
		Validation.CheckIsWKTPoint(gisPoint, "GisPoint", requestContext, allowNull: true);
		Validation.CheckShortName(shortName, "ShortName", allowNull: true);
		Validation.ValidateInt32(icon, "Icon", 0, int.MaxValue, allowNull: true);
		Validation.ValidateDouble(coverageDirection, "CoverageDirection", -1.0, 1.0, allowNull: true);
		Validation.ValidateDouble(coverageDepth, "CoverageDepth", 0.0, 1000000000.0, allowNull: true);
		Validation.ValidateDouble(coverageFieldOfView, "CoverageFieldOfView", 0.0, 1.0, allowNull: true);
		Validation.CheckShortcut(shortcut, "Shortcut", allowNull: true);
		Guid namespaceId;
		try
		{
			namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, id);
		}
		catch (UnknownDeviceException)
		{
			throw;
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", id);
	}

	public static Device UpdateDevice(IGeminiRequestContext requestContext, string id, string name, string description, bool? enabled, bool? prebufferEnabled, bool? prebufferInMemory, int? prebufferSeconds, bool? recordingEnabled, double? recordingFramerate, bool? edgeStorageEnabled, bool? edgeStoragePlaybackEnabled, bool? recordKeyframesOnly, bool? manualTimeoutEnabled, int? manualTimeoutMinutes, bool? recordOnRelatedDevices, string gisPoint, string shortName, int? icon, double? coverageDirection, double? coverageDepth, double? coverageFieldOfView, CustomProperty[] customProperties, string shortcut, bool? multicastEnabled, int? ptzManualSessionTimeout, int? ptzReservedSessionTimeout, int? ptzExtendedManualSessionTimeout, bool? failoverLiveOnly, bool? failoverDisabled)
	{
		try
		{
			UpdateDeviceValidation(requestContext, id, name, description, enabled, recordingEnabled, prebufferEnabled, prebufferInMemory, prebufferSeconds, recordingFramerate, edgeStorageEnabled, edgeStoragePlaybackEnabled, recordKeyframesOnly, manualTimeoutEnabled, manualTimeoutMinutes, recordOnRelatedDevices, gisPoint, shortName, icon, coverageDirection, coverageDepth, coverageFieldOfView, shortcut);
		}
		catch (UnknownDeviceException)
		{
			throw new DeviceUpdateException();
		}
		Guid sessionId = Guid.NewGuid();
		Device device;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			deviceComponent.UpdateDevice(id, Character.RemoveInvalidXmlChars(name), Character.RemoveInvalidXmlChars(description), enabled, prebufferEnabled, prebufferInMemory, prebufferSeconds, recordingEnabled, recordingFramerate, edgeStorageEnabled, edgeStoragePlaybackEnabled, recordKeyframesOnly, manualTimeoutEnabled, manualTimeoutMinutes, recordOnRelatedDevices, gisPoint, shortName, icon, coverageDirection, coverageDepth, coverageFieldOfView, customProperties, shortcut, sessionId, multicastEnabled, ptzManualSessionTimeout, ptzReservedSessionTimeout, ptzExtendedManualSessionTimeout, failoverLiveOnly, failoverDisabled);
			device = deviceComponent.QueryDeviceById(id);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		if (device != null)
		{
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device.DeviceType);
			Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.ItemType, device.Id, EventAction.Updated, device.Name);
		}
		return device;
	}

	public static string[] QueryOtherRecordingServersWithMedia(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Guid namespaceId;
		try
		{
			namespaceId = requestContext.DeviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		}
		catch (UnknownDeviceException)
		{
			throw new DeviceNotFoundException();
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
		string[] recordingServerIds;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			recordingServerIds = deviceComponent.QueryOtherRecordingServersWithMedia(deviceId);
		}
		return FilterRecordingServerIds(requestContext, recordingServerIds);
	}

	public static string[] QueryOtherDevicesWithMediaOnRecorder(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId);
		Device[] devices;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			string[] ids = deviceComponent.QueryOtherDevicesWithMediaOnRecorder(recordingServerId);
			devices = deviceComponent.QueryDevicesByIds(ids);
		}
		devices = FilterDevices(requestContext, devices);
		return devices.Select((Device f) => f.Id).ToArray();
	}

	private static Device[] FilterDevices(IGeminiRequestContext requestContext, Device[] devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		return FilterDeviceObjects(requestContext, devices.Select((Device d) => (d: d, Id: d.Id, DeviceType: d.DeviceType)));
	}

	private static DeviceBasicInfoContract[] FilterDevices(IGeminiRequestContext requestContext, DeviceBasicInfoContract[] devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		return FilterDeviceObjects(requestContext, devices.Select((DeviceBasicInfoContract d) => (d: d, Id: d.Id, DeviceType: d.DeviceType)));
	}

	private static T[] FilterDeviceObjects<T>(IGeminiRequestContext requestContext, IEnumerable<(T instance, string id, DeviceType type)> devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		(T, string, DeviceType)[] array = devices.ToArray();
		HashSet<string> invalidIds = new HashSet<string>(array.Length, StringComparer.Ordinal);
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		Parallel.ForEach(array, delegate((T instance, string id, DeviceType type) device)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.type].SecurityNamespace;
			try
			{
				if (!securityManagerService.HasObjectPermission(requestContext, securityNamespace, "GENERIC_READ", device.id))
				{
					lock (invalidIds)
					{
						invalidIds.Add(device.id);
						return;
					}
				}
			}
			catch (SecurityObjectDoesNotExistException)
			{
				lock (invalidIds)
				{
					invalidIds.Add(device.id);
				}
			}
		});
		return (from d in array
			where !invalidIds.Contains(d.id)
			select d.instance).ToArray();
	}

	private static string[] FilterRecordingServerIds(IGeminiRequestContext requestContext, IEnumerable<string> recordingServerIds)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerIds.Select((string r) => (r, r))).ToArray();
	}
}
