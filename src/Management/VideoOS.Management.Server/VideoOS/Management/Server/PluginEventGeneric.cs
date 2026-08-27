using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("PluginEvent")]
[EntityName("MIPEvent")]
[EntityName("MIPDeviceEvent")]
public class PluginEventGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToUserGenericObject2(PluginEvent.QueryPluginEvent(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToUserGenericObject2s(PluginEvent.QueryPluginEvents(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "PluginEvent")
		{
			return PluginEvent.QueryPluginEventIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		PluginEvent pluginEvent = ConvertToPluginEvent(entityName, properties, customProperties);
		pluginEvent.CustomProperties = customProperties;
		return ConvertToUserGenericObject2(PluginEvent.CreatePluginEvent(requestContext, pluginEvent));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		PluginEvent pluginEvent = ConvertToPluginEvent(genericObject.EntityName, genericObject.Properties, customProperties);
		return ConvertToUserGenericObject2(PluginEvent.UpdatePluginEvent(requestContext, pluginEvent));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		PluginEvent.DeletePluginEvent(requestContext, id);
	}

	private static GenericObject2 ConvertToUserGenericObject2(PluginEvent pluginEvent)
	{
		if (pluginEvent != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", pluginEvent.Id));
			list.Add(new ObjectProperty2("Name", pluginEvent.Name));
			list.Add(new ObjectProperty2("CreatedDate", pluginEvent.CreatedDate));
			list.Add(new ObjectProperty2("LastModified", pluginEvent.LastModified));
			list.Add(new ObjectProperty2("Definition", pluginEvent.Definition));
			if (pluginEvent.GeneratorType == PluginEventType.MIP)
			{
				return new GenericObject2(pluginEvent.Id, "MIPEvent", list.ToArray(), null);
			}
			if (pluginEvent.GeneratorType == PluginEventType.MIPDevice)
			{
				return new GenericObject2(pluginEvent.Id, "MIPDeviceEvent", list.ToArray(), null);
			}
		}
		return null;
	}

	private static GenericObject2[] ConvertToUserGenericObject2s(PluginEvent[] pluginEvents)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (PluginEvent pluginEvent in pluginEvents)
		{
			list.Add(ConvertToUserGenericObject2(pluginEvent));
		}
		return list.ToArray();
	}

	private static PluginEvent ConvertToPluginEvent(string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties)
	{
		PluginEvent pluginEvent = new PluginEvent();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			pluginEvent.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			pluginEvent.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			pluginEvent.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			pluginEvent.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Definition"))
		{
			pluginEvent.Definition = GenericObjectHelper2.GetProperty<string>(properties, "Definition");
		}
		pluginEvent.CustomProperties = customProperties;
		if (!(entityName == "MIPEvent"))
		{
			if (!(entityName == "MIPDeviceEvent"))
			{
				throw new ArgumentException("EntityName must be either MIPEvent or MIPDeviceEvent.");
			}
			pluginEvent.GeneratorType = PluginEventType.MIPDevice;
		}
		else
		{
			pluginEvent.GeneratorType = PluginEventType.MIP;
		}
		return pluginEvent;
	}
}
