using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("Device")]
[EntityName("RecordingDevice")]
[EntityName("CameraDevice")]
[EntityName("OutputDevice")]
[EntityName("MicrophoneDevice")]
[EntityName("MetadataDevice")]
[EntityName("SpeakerDevice")]
[EntityName("InputDevice")]
[EntityName("OutputDevice")]
[FunctionHandler("QueryOtherRecordingServersWithMedia")]
[FunctionHandler("QueryOtherDevicesWithMediaOnRecorder")]
public class DeviceGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(Device.QueryDeviceById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(Device.QueryDevicesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		DeviceTypeSearch deviceTypeSearch = ResolveDeviceTypeSearchFromEntityName(entityName);
		if (string.Equals(parentEntityName, "ManagementServer", StringComparison.Ordinal))
		{
			string recorderId = null;
			string hardwareId = null;
			return Device.QueryDeviceIdsGeneric(requestContext, recorderId, hardwareId, deviceTypeSearch);
		}
		if (string.Equals(parentEntityName, "RecordingServer", StringComparison.Ordinal))
		{
			string hardwareId2 = null;
			return Device.QueryDeviceIdsGeneric(requestContext, parentId, hardwareId2, deviceTypeSearch);
		}
		if (string.Equals(parentEntityName, "Hardware", StringComparison.Ordinal))
		{
			string recorderId2 = null;
			return Device.QueryDeviceIdsGeneric(requestContext, recorderId2, parentId, deviceTypeSearch);
		}
		if (string.Equals(parentEntityName, "CameraDeviceGroup", StringComparison.Ordinal) || string.Equals(parentEntityName, "MicrophoneDeviceGroup", StringComparison.Ordinal) || string.Equals(parentEntityName, "SpeakerDeviceGroup", StringComparison.Ordinal) || string.Equals(parentEntityName, "InputDeviceGroup", StringComparison.Ordinal) || string.Equals(parentEntityName, "OutputDeviceGroup", StringComparison.Ordinal) || string.Equals(parentEntityName, "MetadataDeviceGroup", StringComparison.Ordinal))
		{
			return Device.QueryDeviceIdsByDeviceGroup(requestContext, parentId, deviceTypeSearch);
		}
		if (string.Equals(parentEntityName, "RecordingStorage", StringComparison.Ordinal))
		{
			return Device.QueryDeviceIdsByRecordingStorage(requestContext, parentId, deviceTypeSearch);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		try
		{
			DeviceTypeUtilities.ResolveDeviceTypeFromDeviceEntityName(genericObject.EntityName);
		}
		catch (ArgumentException)
		{
			throw;
		}
		genericObject.Properties.TryGetProperty<string>("Id", out var value);
		genericObject.Properties.TryGetProperty<string>("Name", out var value2);
		genericObject.Properties.TryGetProperty<string>("Description", out var value3);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("Enabled", out bool? value4);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("RecordingEnabled", out bool? value5);
		genericObject.Properties.TryGetProperty<string>("RecordingStorageId", out var _);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<double>("RecordingFramerate", out double? value7);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("PrebufferEnabled", out bool? value8);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("PrebufferInMemory", out bool? value9);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<int>("PrebufferSeconds", out int? value10);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("EdgeStorageEnabled", out bool? value11);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("EdgeStoragePlaybackEnabled", out bool? value12);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("RecordKeyframesOnly", out bool? value13);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("ManualRecordingTimeoutEnabled", out bool? value14);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<int>("ManualRecordingTimeoutMinutes", out int? value15);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<bool>("RecordOnRelatedDevices", out bool? value16);
		genericObject.Properties.TryGetProperty<string>("GisPoint", out var value17);
		genericObject.Properties.TryGetProperty<string>("ShortName", out var value18);
		genericObject.Properties.TryGetProperty<string>("Shortcut", out var value19);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<int>("Icon", out int? value20);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<double>("CoverageDirection", out double? value21);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<double>("CoverageDepth", out double? value22);
		((IEnumerable<ObjectProperty2>)genericObject.Properties).TryGetProperty<double>("CoverageFieldOfView", out double? value23);
		return ConvertToGenericObject2(Device.UpdateDevice(requestContext, value, value2, value3, value4, value8, value9, value10, value5, value7, value11, value12, value13, value14, value15, value16, value17, value18, value20, value21, value22, value23, customProperties, value19, null, null, null, null, null, null));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	private static GenericObject2 ConvertToGenericObject2(Device device)
	{
		if (device != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", device.Id));
			list.Add(new ObjectProperty2("Name", device.Name));
			list.Add(new ObjectProperty2("Description", device.Description));
			list.Add(new ObjectProperty2("CreatedDate", device.CreatedDate));
			list.Add(new ObjectProperty2("LastModified", device.LastModified));
			list.Add(new ObjectProperty2("DeviceType", device.DeviceType.ToString()));
			list.Add(new ObjectProperty2("Enabled", device.Enabled));
			list.Add(new ObjectProperty2("DeviceIndex", device.DeviceIndex));
			list.Add(new ObjectProperty2("GisPoint", device.GisPoint));
			list.Add(new ObjectProperty2("ShortName", device.ShortName));
			list.Add(new ObjectProperty2("Icon", device.Icon));
			list.Add(new ObjectProperty2("CoverageDirection", device.CoverageDirection));
			list.Add(new ObjectProperty2("CoverageDepth", device.CoverageDepth));
			list.Add(new ObjectProperty2("CoverageFieldOfView", device.CoverageFieldOfView));
			if (DeviceTypeUtilities.IsRecordingDevice(device.DeviceType))
			{
				list.Add(new ObjectProperty2("RecordingEnabled", device.RecordingEnabled));
				list.Add(new ObjectProperty2("RecordingStorageId", device.RecordingStorageId));
				list.Add(new ObjectProperty2("RecordingFramerate", device.RecordingFramerate));
				list.Add(new ObjectProperty2("PrebufferEnabled", device.PrebufferEnabled));
				list.Add(new ObjectProperty2("PrebufferInMemory", device.PrebufferInMemory));
				list.Add(new ObjectProperty2("PrebufferSeconds", device.PrebufferSeconds));
				list.Add(new ObjectProperty2("EdgeStorageEnabled", device.EdgeStorageEnabled));
				list.Add(new ObjectProperty2("EdgeStoragePlaybackEnabled", device.EdgeStoragePlayback));
				list.Add(new ObjectProperty2("RecordKeyframesOnly", device.RecordKeyframesOnly));
				list.Add(new ObjectProperty2("ManualRecordingTimeoutEnabled", device.ManualTimeoutEnabled));
				list.Add(new ObjectProperty2("ManualRecordingTimeoutMinutes", device.ManualTimeoutMinutes));
				list.Add(new ObjectProperty2("RecordOnRelatedDevices", device.RecordOnRelatedDevices));
			}
			list.Add(new ObjectProperty2("HardwareId", device.HardwareId));
			list.Add(new ObjectProperty2("RecordingServerId", device.RecordingServerId));
			List<RelationProperty> list2 = new List<RelationProperty>();
			list2.Add(new RelationProperty("RecordingServer", device.RecordingServerId));
			list2.Add(new RelationProperty("Hardware", device.HardwareId));
			if (device.DeviceType != DeviceType.Unknown)
			{
				string entityName = DeviceTypeUtilities.ResolveDeviceEntityNameFromDeviceType(device.DeviceType);
				return new GenericObject2(device.Id, entityName, list.ToArray(), list2.ToArray());
			}
		}
		return null;
	}

	private static GenericObject2[] ConvertToGenericObject2(Device[] devices)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (Device device in devices)
		{
			list.Add(ConvertToGenericObject2(device));
		}
		return list.ToArray();
	}

	public static DeviceTypeSearch ResolveDeviceTypeSearchFromEntityName(string entityName)
	{
		if (entityName == null)
		{
			throw new ArgumentNullException("entityName");
		}
		if (string.Equals(entityName, "Device", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.All;
		}
		if (string.Equals(entityName, "RecordingDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.RecordingDevice;
		}
		if (string.Equals(entityName, "CameraDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.Camera;
		}
		if (string.Equals(entityName, "MicrophoneDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.Microphone;
		}
		if (string.Equals(entityName, "MetadataDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.Metadata;
		}
		if (string.Equals(entityName, "SpeakerDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.Speaker;
		}
		if (string.Equals(entityName, "InputDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.Input;
		}
		if (string.Equals(entityName, "OutputDevice", StringComparison.Ordinal))
		{
			return DeviceTypeSearch.Output;
		}
		throw new ArgumentException("The entityName '" + entityName + "' is not supported", "entityName");
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (!(functionName == "QueryOtherRecordingServersWithMedia"))
		{
			if (functionName == "QueryOtherDevicesWithMediaOnRecorder")
			{
				return QueryOtherDevicesWithMediaOnRecorder(requestContext, parameters);
			}
			throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
		}
		return QueryOtherRecordingServersWithMedia(requestContext, parameters);
	}

	private static GenericObject2[] QueryOtherRecordingServersWithMedia(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		string property = GenericObjectHelper2.GetProperty<string>(parameters.FirstOrDefault().Properties, "Id");
		return (from id in Device.QueryOtherRecordingServersWithMedia(requestContext, property)
			select new GenericObject2(id, null, new ObjectProperty2[0], null)).ToArray();
	}

	private static GenericObject2[] QueryOtherDevicesWithMediaOnRecorder(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string property = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		return (from id in Device.QueryOtherDevicesWithMediaOnRecorder(requestContext, property)
			select new GenericObject2(id, null, new ObjectProperty2[0], null)).ToArray();
	}
}
