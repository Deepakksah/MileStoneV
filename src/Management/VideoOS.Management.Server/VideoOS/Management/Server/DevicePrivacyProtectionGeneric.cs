using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("DevicePrivacyProtection")]
public class DevicePrivacyProtectionGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(DevicePrivacyProtection.QueryPrivacyProtectionById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		throw new NotSupportedException();
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
		DevicePrivacyProtection devicePrivacyProtection = ConvertToPrivacyProtection(genericObject.Properties);
		return ConvertToGenericObject2(DevicePrivacyProtection.UpdatePrivacyProtection(requestContext, devicePrivacyProtection.Id, devicePrivacyProtection.Enabled, devicePrivacyProtection.PrivacyMaskXml));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	private static GenericObject2 ConvertToGenericObject2(DevicePrivacyProtection privacyProtection)
	{
		if (privacyProtection != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", privacyProtection.Id));
			list.Add(new ObjectProperty2("LastModified", privacyProtection.LastModified));
			list.Add(new ObjectProperty2("Enabled", privacyProtection.Enabled));
			list.Add(new ObjectProperty2("MaskXml", privacyProtection.PrivacyMaskXml));
			list.Add(new ObjectProperty2("DeviceId", privacyProtection.Id));
			list.Add(new ObjectProperty2("HardwareId", privacyProtection.HardwareId));
			list.Add(new ObjectProperty2("RecordingServerId", privacyProtection.RecordingServerId));
			List<RelationProperty> list2 = new List<RelationProperty>();
			list2.Add(new RelationProperty("Device", privacyProtection.Id));
			list2.Add(new RelationProperty("RecordingServer", privacyProtection.RecordingServerId));
			list2.Add(new RelationProperty("Hardware", privacyProtection.HardwareId));
			return new GenericObject2(privacyProtection.Id, "DevicePrivacyProtection", list.ToArray(), list2.ToArray());
		}
		return null;
	}

	private static DevicePrivacyProtection ConvertToPrivacyProtection(ObjectProperty2[] properties)
	{
		DevicePrivacyProtection devicePrivacyProtection = new DevicePrivacyProtection();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			devicePrivacyProtection.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Enabled"))
		{
			devicePrivacyProtection.Enabled = GenericObjectHelper2.GetProperty<bool>(properties, "Enabled");
		}
		if (GenericObjectHelper2.Contains(properties, "MaskXml"))
		{
			devicePrivacyProtection.PrivacyMaskXml = GenericObjectHelper2.GetProperty<string>(properties, "MaskXml");
		}
		return devicePrivacyProtection;
	}
}
