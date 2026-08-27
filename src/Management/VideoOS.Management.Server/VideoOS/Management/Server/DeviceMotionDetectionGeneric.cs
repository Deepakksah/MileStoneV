using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MotionDetection")]
public class DeviceMotionDetectionGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(DeviceMotionDetection.QueryMotionDetectionById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(DeviceMotionDetection.QueryMotionDetectionsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		DeviceMotionDetection deviceMotionDetection = ConvertToDeviceMotionDetection(genericObject.Properties);
		return ConvertToGenericObject2(DeviceMotionDetection.UpdateMotionDetection(requestContext, deviceMotionDetection.Id, deviceMotionDetection.MotionDetectionEnabled, deviceMotionDetection.ManualSensitivityEnabled, deviceMotionDetection.MotionPixThreshold, deviceMotionDetection.MotionPixCountThreshold, deviceMotionDetection.KeyFramesOnly, deviceMotionDetection.ProcessTime, deviceMotionDetection.DetectionMethod, deviceMotionDetection.HardwareAccelerationMode, deviceMotionDetection.GenerateMotionMetadata, deviceMotionDetection.UseExcludeRegions, deviceMotionDetection.GridSize, deviceMotionDetection.MotionDetectionExcludeRegions));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	private static GenericObject2[] ConvertToGenericObject2(DeviceMotionDetection[] motionDetections)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (DeviceMotionDetection motionDetection in motionDetections)
		{
			list.Add(ConvertToGenericObject2(motionDetection));
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToGenericObject2(DeviceMotionDetection motionDetection)
	{
		if (motionDetection != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", motionDetection.Id));
			list.Add(new ObjectProperty2("LastModified", motionDetection.LastModified));
			list.Add(new ObjectProperty2("Enabled", motionDetection.MotionDetectionEnabled));
			list.Add(new ObjectProperty2("ManualSensitivityEnabled", motionDetection.ManualSensitivityEnabled));
			list.Add(new ObjectProperty2("MotionPixCountThreshold", motionDetection.MotionPixCountThreshold));
			list.Add(new ObjectProperty2("MotionPixThreshold", motionDetection.MotionPixThreshold));
			list.Add(new ObjectProperty2("KeyframesOnly", motionDetection.KeyFramesOnly));
			list.Add(new ObjectProperty2("ProcessTime", motionDetection.ProcessTime.ToString()));
			list.Add(new ObjectProperty2("DetectionMethod", motionDetection.DetectionMethod.ToString()));
			list.Add(new ObjectProperty2("GenerateMotionMetadata", motionDetection.GenerateMotionMetadata));
			list.Add(new ObjectProperty2("UseExcludeRegions", motionDetection.UseExcludeRegions));
			list.Add(new ObjectProperty2("GridSize", motionDetection.GridSize.ToString()));
			list.Add(new ObjectProperty2("MotionDetectionExcludeRegions", motionDetection.MotionDetectionExcludeRegions));
			list.Add(new ObjectProperty2("HardwareAccelerationMode", motionDetection.HardwareAccelerationMode.ToString()));
			list.Add(new ObjectProperty2("DeviceId", motionDetection.Id));
			list.Add(new ObjectProperty2("HardwareId", motionDetection.HardwareId));
			list.Add(new ObjectProperty2("RecordingServerId", motionDetection.RecordingServerId));
			List<RelationProperty> list2 = new List<RelationProperty>();
			list2.Add(new RelationProperty("Device", motionDetection.Id));
			list2.Add(new RelationProperty("RecordingServer", motionDetection.RecordingServerId));
			list2.Add(new RelationProperty("Hardware", motionDetection.HardwareId));
			return new GenericObject2(motionDetection.Id, "MotionDetection", list.ToArray(), list2.ToArray());
		}
		return null;
	}

	private static DeviceMotionDetection ConvertToDeviceMotionDetection(ObjectProperty2[] properties)
	{
		DeviceMotionDetection deviceMotionDetection = new DeviceMotionDetection();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			deviceMotionDetection.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			deviceMotionDetection.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Enabled"))
		{
			deviceMotionDetection.MotionDetectionEnabled = GenericObjectHelper2.GetProperty<bool>(properties, "Enabled");
		}
		if (GenericObjectHelper2.Contains(properties, "ManualSensitivityEnabled"))
		{
			deviceMotionDetection.ManualSensitivityEnabled = GenericObjectHelper2.GetProperty<bool>(properties, "ManualSensitivityEnabled");
		}
		if (GenericObjectHelper2.Contains(properties, "MotionPixThreshold"))
		{
			deviceMotionDetection.MotionPixThreshold = GenericObjectHelper2.GetProperty<int>(properties, "MotionPixThreshold");
		}
		if (GenericObjectHelper2.Contains(properties, "MotionPixCountThreshold"))
		{
			deviceMotionDetection.MotionPixCountThreshold = GenericObjectHelper2.GetProperty<int>(properties, "MotionPixCountThreshold");
		}
		if (GenericObjectHelper2.Contains(properties, "KeyframesOnly"))
		{
			deviceMotionDetection.KeyFramesOnly = GenericObjectHelper2.GetProperty<bool>(properties, "KeyframesOnly");
		}
		if (GenericObjectHelper2.Contains(properties, "ProcessTime"))
		{
			deviceMotionDetection.ProcessTime = GenericObjectHelper2.GetProperty<ProcessTime.ProcessTimeType>(properties, "ProcessTime");
		}
		if (GenericObjectHelper2.Contains(properties, "DetectionMethod"))
		{
			deviceMotionDetection.DetectionMethod = GenericObjectHelper2.GetProperty<DetectionMethod.DetectionMethodType>(properties, "DetectionMethod");
		}
		if (GenericObjectHelper2.Contains(properties, "HardwareAccelerationMode"))
		{
			deviceMotionDetection.HardwareAccelerationMode = GenericObjectHelper2.GetProperty<HardwareAccelerationMode.HardwareAccelerationModeType>(properties, "HardwareAccelerationMode");
		}
		if (GenericObjectHelper2.Contains(properties, "GenerateMotionMetadata"))
		{
			deviceMotionDetection.GenerateMotionMetadata = GenericObjectHelper2.GetProperty<bool>(properties, "GenerateMotionMetadata");
		}
		if (GenericObjectHelper2.Contains(properties, "UseExcludeRegions"))
		{
			deviceMotionDetection.UseExcludeRegions = GenericObjectHelper2.GetProperty<bool>(properties, "UseExcludeRegions");
		}
		if (GenericObjectHelper2.Contains(properties, "GridSize"))
		{
			deviceMotionDetection.GridSize = GenericObjectHelper2.GetProperty<GridSize.GridSizeType>(properties, "GridSize");
		}
		if (GenericObjectHelper2.Contains(properties, "MotionDetectionExcludeRegions"))
		{
			deviceMotionDetection.MotionDetectionExcludeRegions = GenericObjectHelper2.GetProperty<string>(properties, "MotionDetectionExcludeRegions");
		}
		if (GenericObjectHelper2.Contains(properties, "HardwareId"))
		{
			deviceMotionDetection.HardwareId = GenericObjectHelper2.GetProperty<string>(properties, "HardwareId");
		}
		if (GenericObjectHelper2.Contains(properties, "RecordingServerId"))
		{
			deviceMotionDetection.RecordingServerId = GenericObjectHelper2.GetProperty<string>(properties, "RecordingServerId");
		}
		return deviceMotionDetection;
	}
}
