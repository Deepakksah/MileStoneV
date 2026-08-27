using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("SystemDashboardConfiguration")]
public class SystemDashboardConfigurationGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToSystemDashboardConfigGenericObject2(SystemDashboardConfiguration.QuerySystemDashboardConfigById(requestContext, Convert.ToInt32(id, CultureInfo.InvariantCulture)));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToSystemDashboardConfigGenericObject2s(SystemDashboardConfiguration.QuerySystemDashboardConfigurationByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "SystemDashboardConfiguration")
		{
			return QuerySystemDashboardConfigIds(requestContext);
		}
		return null;
	}

	private static string[] QuerySystemDashboardConfigIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, "GENERIC_READ");
		using SystemDashboardConfigurationComponent systemDashboardConfigurationComponent = requestContext.CreateComponent<SystemDashboardConfigurationComponent>();
		return systemDashboardConfigurationComponent.QuerySystemDashboardConfigIds();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		SystemDashboardConfiguration systemDashboardConfiguration = ConvertToSystemDashboardConfig(properties);
		systemDashboardConfiguration = SystemDashboardConfiguration.CreateSystemDashboardConfig(requestContext, systemDashboardConfiguration.DashboardTileId, systemDashboardConfiguration.EventTypes, systemDashboardConfiguration.StateSourceType, systemDashboardConfiguration.SingleStateSourceId, systemDashboardConfiguration.DashboardTileName, systemDashboardConfiguration.Position);
		return ConvertToSystemDashboardConfigGenericObject2(systemDashboardConfiguration);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		SystemDashboardConfiguration systemDashboardConfiguration = ConvertToSystemDashboardConfig(genericObject.Properties);
		systemDashboardConfiguration = SystemDashboardConfiguration.UpdateSystemDashboardConfiguration(requestContext, systemDashboardConfiguration.DashboardTileId, systemDashboardConfiguration.EventTypes, systemDashboardConfiguration.StateSourceType, systemDashboardConfiguration.SingleStateSourceId, systemDashboardConfiguration.DashboardTileName, systemDashboardConfiguration.Position);
		return ConvertToSystemDashboardConfigGenericObject2(systemDashboardConfiguration);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		SystemDashboardConfiguration.DeleteSystemDashboardConfig(requestContext, Convert.ToInt32(id, CultureInfo.InvariantCulture));
	}

	private static GenericObject2[] ConvertToSystemDashboardConfigGenericObject2s(SystemDashboardConfiguration[] config)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (SystemDashboardConfiguration config2 in config)
		{
			list.Add(ConvertToSystemDashboardConfigGenericObject2(config2));
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToSystemDashboardConfigGenericObject2(SystemDashboardConfiguration config)
	{
		if (config == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", config.DashboardTileId));
		list.Add(new ObjectProperty2("Name", config.DashboardTileName));
		list.Add(new ObjectProperty2("DashboardId", config.DashboardTileId));
		list.Add(new ObjectProperty2("SingleStateSourceId", config.SingleStateSourceId));
		list.Add(new ObjectProperty2("StateSourceType", config.StateSourceType));
		list.Add(new ObjectProperty2("Position", config.Position));
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int eventType in config.EventTypes)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0};", eventType);
		}
		list.Add(new ObjectProperty2("EventTypes", stringBuilder.ToString()));
		return new GenericObject2(config.DashboardTileId.ToString(CultureInfo.InvariantCulture), "SystemDashboardConfiguration", list.ToArray(), null);
	}

	private static SystemDashboardConfiguration ConvertToSystemDashboardConfig(ObjectProperty2[] properties)
	{
		SystemDashboardConfiguration systemDashboardConfiguration = new SystemDashboardConfiguration();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			systemDashboardConfiguration.DashboardTileId = GenericObjectHelper2.GetProperty<int>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			systemDashboardConfiguration.DashboardTileName = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "SingleStateSourceId"))
		{
			systemDashboardConfiguration.SingleStateSourceId = GenericObjectHelper2.GetProperty<string>(properties, "SingleStateSourceId");
		}
		if (GenericObjectHelper2.Contains(properties, "StateSourceType"))
		{
			systemDashboardConfiguration.StateSourceType = GenericObjectHelper2.GetProperty<string>(properties, "StateSourceType");
		}
		if (GenericObjectHelper2.Contains(properties, "EventTypes"))
		{
			string[] array = GenericObjectHelper2.GetProperty<string>(properties, "EventTypes").Split(';');
			systemDashboardConfiguration.EventTypes = new List<int>();
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result))
				{
					systemDashboardConfiguration.EventTypes.Add(result);
				}
			}
		}
		if (GenericObjectHelper2.Contains(properties, "Position"))
		{
			systemDashboardConfiguration.Position = GenericObjectHelper2.GetProperty<int>(properties, "Position");
		}
		return systemDashboardConfiguration;
	}
}
