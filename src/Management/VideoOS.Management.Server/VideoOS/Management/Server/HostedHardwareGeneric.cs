using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("AxisOneClickCamera")]
[EntityName("HostedHardware")]
public sealed class HostedHardwareGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(HostedHardware.QueryHostedHardwareById(requestContext, int.Parse(id, CultureInfo.InvariantCulture)), entityName);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(HostedHardware.QueryHostedHardwareByIds(requestContext, ids.Select((string f) => int.Parse(f, CultureInfo.InvariantCulture)).ToArray()));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		return FilterBasedOnEntities(HostedHardware.QueryHostedHardwareIds(requestContext, int.Parse(parentId, CultureInfo.InvariantCulture)), parentEntityName, entityName);
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		if (string.Equals(entityName, "AxisOneClickCamera", StringComparison.OrdinalIgnoreCase))
		{
			HostedHardware hostedHardware = ConvertToHostedHardware(properties);
			hostedHardware.CustomProperties = customProperties;
			hostedHardware = HostedHardware.CreateHostedHardware(requestContext, hostedHardware, HostingTypes.AxisOneClickCameraType);
			return ConvertToGenericObject2(hostedHardware, entityName);
		}
		throw new HostedHardwareCreationException(entityName);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		HostedHardware hostedHardware = ConvertToHostedHardware(genericObject);
		hostedHardware.CustomProperties = customProperties;
		hostedHardware = HostedHardware.UpdateHostedHardware(requestContext, hostedHardware);
		return ConvertToGenericObject2(hostedHardware, genericObject.EntityName);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		HostedHardware.DeleteHostedHardware(requestContext, int.Parse(id, CultureInfo.InvariantCulture));
	}

	private static GenericObject2[] ConvertToGenericObject2s(HostedHardware[] HostedHardwares)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (HostedHardware hostedHardware in HostedHardwares)
		{
			list.Add(ConvertToGenericObject2(hostedHardware, null));
		}
		return list.ToArray();
	}

	private static HostedHardware ConvertToHostedHardware(GenericObject2 genericObject)
	{
		return ConvertToHostedHardware(genericObject.Properties);
	}

	private static GenericObject2 ConvertToGenericObject2(HostedHardware hostedHardware, string entityName)
	{
		if (hostedHardware == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", hostedHardware.Id));
		list.Add(new ObjectProperty2("Name", hostedHardware.Name));
		list.Add(new ObjectProperty2("Description", hostedHardware.Description));
		list.Add(new ObjectProperty2("CreatedDate", hostedHardware.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", hostedHardware.LastModified));
		list.Add(new ObjectProperty2("Type", hostedHardware.Type));
		list.Add(new ObjectProperty2("UserName", hostedHardware.UserName));
		list.Add(new ObjectProperty2("Password", hostedHardware.Password));
		list.Add(new ObjectProperty2("HostingInfo", hostedHardware.HostingInfo));
		list.Add(new ObjectProperty2("ManufacturerId", hostedHardware.ManufacturerId));
		list.Add(new ObjectProperty2("HostingServerId", hostedHardware.HostingServerId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		list2.Add(new RelationProperty("AxisTunnelServer", hostedHardware.HostingServerId.ToString(CultureInfo.InvariantCulture)));
		if (string.Equals(hostedHardware.Type, HostingTypes.AxisOneClickCameraType, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(entityName) || string.Equals(entityName, "HostedHardware", StringComparison.OrdinalIgnoreCase) || string.Equals(entityName, "AxisOneClickCamera", StringComparison.OrdinalIgnoreCase)))
		{
			return new GenericObject2(hostedHardware.Id.ToString(CultureInfo.InvariantCulture), "AxisOneClickCamera", list.ToArray(), list2.ToArray());
		}
		return null;
	}

	private static HostedHardware ConvertToHostedHardware(ObjectProperty2[] properties)
	{
		HostedHardware hostedHardware = new HostedHardware();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			hostedHardware.Id = GenericObjectHelper2.GetProperty<int>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			hostedHardware.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			hostedHardware.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			hostedHardware.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			hostedHardware.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Type"))
		{
			hostedHardware.Type = GenericObjectHelper2.GetProperty<string>(properties, "Type");
		}
		if (GenericObjectHelper2.Contains(properties, "UserName"))
		{
			hostedHardware.UserName = GenericObjectHelper2.GetProperty<string>(properties, "UserName");
		}
		if (GenericObjectHelper2.Contains(properties, "Password"))
		{
			hostedHardware.Password = GenericObjectHelper2.GetProperty<string>(properties, "Password");
		}
		if (GenericObjectHelper2.Contains(properties, "HostingInfo"))
		{
			hostedHardware.HostingInfo = GenericObjectHelper2.GetProperty<string>(properties, "HostingInfo");
		}
		if (GenericObjectHelper2.Contains(properties, "ManufacturerId"))
		{
			hostedHardware.ManufacturerId = GenericObjectHelper2.GetProperty<string>(properties, "ManufacturerId");
		}
		if (GenericObjectHelper2.Contains(properties, "HostingServerId"))
		{
			hostedHardware.HostingServerId = GenericObjectHelper2.GetProperty<int>(properties, "HostingServerId");
		}
		return hostedHardware;
	}

	private static string[] FilterBasedOnEntities(string[] ids, string parentEntityName, string entityName)
	{
		if ((string.IsNullOrEmpty(entityName) || string.Equals(entityName, "HostedHardware", StringComparison.OrdinalIgnoreCase) || string.Equals(entityName, "AxisOneClickCamera", StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrEmpty(parentEntityName) || string.Equals(parentEntityName, "HostingServer", StringComparison.OrdinalIgnoreCase) || string.Equals(parentEntityName, "AxisTunnelServer", StringComparison.OrdinalIgnoreCase)))
		{
			return ids;
		}
		return new List<string>().ToArray();
	}
}
