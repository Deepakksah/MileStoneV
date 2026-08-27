using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("AxisTunnelServer")]
[EntityName("HostingServer")]
public sealed class HostingServerGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(HostingServer.QueryHostingServerById(requestContext, int.Parse(id, CultureInfo.InvariantCulture)), entityName);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(HostingServer.QueryHostingServerByIds(requestContext, ids.Select((string f) => int.Parse(f, CultureInfo.InvariantCulture)).ToArray()));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		return FilterBasedOnEntity(HostingServer.QueryHostingServerIds(requestContext), entityName);
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		if (string.Equals(entityName, "AxisTunnelServer", StringComparison.OrdinalIgnoreCase))
		{
			HostingServer hostingServer = ConvertToHostingServer(properties);
			hostingServer.CustomProperties = customProperties;
			hostingServer = HostingServer.CreateHostingServer(requestContext, hostingServer, HostingTypes.AxisTunnelServerType);
			return ConvertToGenericObject2(hostingServer, entityName);
		}
		throw new HostingServerCreationException(entityName);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		if (string.Equals(genericObject.EntityName, "AxisTunnelServer", StringComparison.OrdinalIgnoreCase))
		{
			HostingServer hostingServer = ConvertToHostingServer(genericObject);
			hostingServer.CustomProperties = customProperties;
			hostingServer = HostingServer.UpdateHostingServer(requestContext, hostingServer);
			return ConvertToGenericObject2(hostingServer, genericObject.EntityName);
		}
		throw new HostingServerUpdateException(genericObject.EntityName);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		HostingServer.DeleteHostingServer(requestContext, int.Parse(id, CultureInfo.InvariantCulture));
	}

	private static GenericObject2[] ConvertToGenericObject2s(HostingServer[] hostingServers)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		for (int i = 0; i < hostingServers.Length; i++)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(hostingServers[i], null);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}

	private static HostingServer ConvertToHostingServer(GenericObject2 genericObject)
	{
		return ConvertToHostingServer(genericObject.Properties);
	}

	private static GenericObject2 ConvertToGenericObject2(HostingServer hostingServer, string entityName)
	{
		if (hostingServer == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", hostingServer.Id));
		list.Add(new ObjectProperty2("Name", hostingServer.Name));
		list.Add(new ObjectProperty2("Description", hostingServer.Description));
		list.Add(new ObjectProperty2("CreatedDate", hostingServer.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", hostingServer.LastModified));
		list.Add(new ObjectProperty2("Type", hostingServer.Type));
		list.Add(new ObjectProperty2("UserName", hostingServer.UserName));
		list.Add(new ObjectProperty2("Password", hostingServer.Password));
		list.Add(new ObjectProperty2("InternalAddress", hostingServer.InternalAddress));
		list.Add(new ObjectProperty2("ExternalAddress", hostingServer.ExternalAddress));
		if (string.Equals(hostingServer.Type, HostingTypes.AxisTunnelServerType, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(entityName) || string.Equals(entityName, "HostingServer", StringComparison.OrdinalIgnoreCase) || string.Equals(entityName, "AxisTunnelServer", StringComparison.OrdinalIgnoreCase)))
		{
			return new GenericObject2(hostingServer.Id.ToString(CultureInfo.InvariantCulture), "AxisTunnelServer", list.ToArray(), null);
		}
		return null;
	}

	private static HostingServer ConvertToHostingServer(ObjectProperty2[] properties)
	{
		HostingServer hostingServer = new HostingServer();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			hostingServer.Id = GenericObjectHelper2.GetProperty<int>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			hostingServer.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			hostingServer.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			hostingServer.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			hostingServer.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Type"))
		{
			hostingServer.Type = GenericObjectHelper2.GetProperty<string>(properties, "Type");
		}
		if (GenericObjectHelper2.Contains(properties, "UserName"))
		{
			hostingServer.UserName = GenericObjectHelper2.GetProperty<string>(properties, "UserName");
		}
		if (GenericObjectHelper2.Contains(properties, "Password"))
		{
			hostingServer.Password = GenericObjectHelper2.GetProperty<string>(properties, "Password");
		}
		if (GenericObjectHelper2.Contains(properties, "InternalAddress"))
		{
			hostingServer.InternalAddress = GenericObjectHelper2.GetProperty<string>(properties, "InternalAddress");
		}
		if (GenericObjectHelper2.Contains(properties, "ExternalAddress"))
		{
			hostingServer.ExternalAddress = GenericObjectHelper2.GetProperty<string>(properties, "ExternalAddress");
		}
		return hostingServer;
	}

	private static string[] FilterBasedOnEntity(string[] ids, string entityName)
	{
		if (string.IsNullOrEmpty(entityName) || string.Equals(entityName, "HostingServer", StringComparison.OrdinalIgnoreCase) || string.Equals(entityName, "AxisTunnelServer", StringComparison.OrdinalIgnoreCase))
		{
			return ids;
		}
		return new List<string>().ToArray();
	}
}
