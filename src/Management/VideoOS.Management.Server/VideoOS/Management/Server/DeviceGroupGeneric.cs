using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("CameraDeviceGroup")]
[EntityName("OutputDeviceGroup")]
[EntityName("MicrophoneDeviceGroup")]
[EntityName("SpeakerDeviceGroup")]
[EntityName("InputDeviceGroup")]
[EntityName("MetadataDeviceGroup")]
[EntityName("DeviceGroup")]
public class DeviceGroupGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(DeviceGroup.QueryDeviceGroupById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(DeviceGroup.QueryDeviceGroups(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		DeviceType deviceType = DeviceTypeUtilities.ResolveDeviceGroupDeviceTypeFromEntityName(entityName);
		if ((parentEntityName == "ManagementServer" || parentEntityName == entityName) && deviceType != DeviceType.Unknown)
		{
			return DeviceGroup.QueryDeviceGroupIds(requestContext, parentId, deviceType);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	private static GenericObject2 ConvertToGenericObject2(DeviceGroup deviceGroup)
	{
		GenericObject2 result = null;
		if (deviceGroup != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", deviceGroup.Id));
			list.Add(new ObjectProperty2("Name", deviceGroup.Name));
			list.Add(new ObjectProperty2("Description", deviceGroup.Description));
			list.Add(new ObjectProperty2("LastModified", deviceGroup.LastModified));
			list.Add(new ObjectProperty2("BuiltIn", deviceGroup.BuiltIn));
			list.Add(new ObjectProperty2("ParentGroupId", deviceGroup.ParentGroupId));
			string entityName = DeviceTypeUtilities.ResolveDeviceGroupEntityNameFromDeviceType(deviceGroup.DeviceType);
			List<RelationProperty> list2 = new List<RelationProperty>();
			if (!string.IsNullOrEmpty(deviceGroup.ParentGroupId))
			{
				list2.Add(new RelationProperty(entityName, deviceGroup.ParentGroupId));
			}
			if (!string.IsNullOrEmpty(deviceGroup.RelationIds))
			{
				string[] array = deviceGroup.RelationIds.Split(';');
				foreach (string id in array)
				{
					list2.Add(new RelationProperty(entityName, id));
				}
			}
			result = new GenericObject2(deviceGroup.Id, entityName, list.ToArray(), list2.ToArray());
		}
		return result;
	}

	private static GenericObject2[] ConvertToGenericObject2s(DeviceGroup[] deviceGroups)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		if (deviceGroups != null)
		{
			foreach (DeviceGroup deviceGroup in deviceGroups)
			{
				list.Add(ConvertToGenericObject2(deviceGroup));
			}
		}
		return list.ToArray();
	}
}
