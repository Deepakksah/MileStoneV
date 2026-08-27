using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("PerformanceStateRuleConfiguration")]
public class PerformanceStateRuleConfigGeneric : IGenericObjectSupport2
{
	private static GenericObject2[] ConvertToPerformanceStateRuleConfigGenericObject2s(PerformanceStateRuleConfig[] PerformanceStateRuleConfig)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (PerformanceStateRuleConfig performanceStateRuleConfig in PerformanceStateRuleConfig)
		{
			list.Add(ConvertToPerformanceStateRuleConfigGenericObject2(performanceStateRuleConfig));
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToPerformanceStateRuleConfigGenericObject2(PerformanceStateRuleConfig performanceStateRuleConfig)
	{
		if (performanceStateRuleConfig == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", performanceStateRuleConfig.CounterRuleId));
		list.Add(new ObjectProperty2("Name", performanceStateRuleConfig.RuleName));
		list.Add(new ObjectProperty2("CriticalThreshold", performanceStateRuleConfig.CriticalThreshold));
		list.Add(new ObjectProperty2("WarningThreshold", performanceStateRuleConfig.WarningThreshold));
		list.Add(new ObjectProperty2("TimeWindow", performanceStateRuleConfig.TimeWindow));
		list.Add(new ObjectProperty2("RuleType", performanceStateRuleConfig.RuleType));
		list.Add(new ObjectProperty2("Disabled", performanceStateRuleConfig.Disabled));
		list.Add(new ObjectProperty2("Unit", performanceStateRuleConfig.Unit));
		list.Add(new ObjectProperty2("Direction", performanceStateRuleConfig.Direction));
		return new GenericObject2(performanceStateRuleConfig.CounterRuleId, "PerformanceStateRuleConfiguration", list.ToArray(), null);
	}

	private static PerformanceStateRuleConfig ConvertToPerformanceStateRuleConfig(ObjectProperty2[] properties)
	{
		PerformanceStateRuleConfig performanceStateRuleConfig = new PerformanceStateRuleConfig();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			performanceStateRuleConfig.CounterRuleId = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			performanceStateRuleConfig.RuleName = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "CriticalThreshold"))
		{
			performanceStateRuleConfig.CriticalThreshold = GenericObjectHelper2.GetProperty<float>(properties, "CriticalThreshold");
		}
		if (GenericObjectHelper2.Contains(properties, "WarningThreshold"))
		{
			performanceStateRuleConfig.WarningThreshold = GenericObjectHelper2.GetProperty<float>(properties, "WarningThreshold");
		}
		if (GenericObjectHelper2.Contains(properties, "TimeWindow"))
		{
			performanceStateRuleConfig.TimeWindow = GenericObjectHelper2.GetProperty<int>(properties, "TimeWindow");
		}
		if (GenericObjectHelper2.Contains(properties, "RuleType"))
		{
			performanceStateRuleConfig.RuleType = GenericObjectHelper2.GetProperty<int>(properties, "RuleType");
		}
		if (GenericObjectHelper2.Contains(properties, "Disabled"))
		{
			performanceStateRuleConfig.Disabled = GenericObjectHelper2.GetProperty<bool>(properties, "Disabled");
		}
		if (GenericObjectHelper2.Contains(properties, "Direction"))
		{
			performanceStateRuleConfig.Direction = GenericObjectHelper2.GetProperty<bool>(properties, "Direction");
		}
		if (GenericObjectHelper2.Contains(properties, "Unit"))
		{
			performanceStateRuleConfig.Unit = GenericObjectHelper2.GetProperty<long>(properties, "Unit");
		}
		return performanceStateRuleConfig;
	}

	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToPerformanceStateRuleConfigGenericObject2(PerformanceStateRuleConfig.QueryPerformanceStateRuleConfigById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToPerformanceStateRuleConfigGenericObject2s(PerformanceStateRuleConfig.QueryPerformanceStateRuleConfigByIds(requestContext, -1, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "PerformanceStateRuleConfiguration")
		{
			return QueryPerformanceStateRuleConfigIds(requestContext);
		}
		return null;
	}

	private static string[] QueryPerformanceStateRuleConfigIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetService<SecurityManagerService>().CheckApplicationReadPermission(requestContext);
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		return performanceStateRuleConfigComponent.QueryCountersStateRuleIds();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		PerformanceStateRuleConfig performanceStateRuleConfig = ConvertToPerformanceStateRuleConfig(properties);
		performanceStateRuleConfig = PerformanceStateRuleConfig.CreatPerformanceStateRuleConfig(requestContext, performanceStateRuleConfig.CounterRuleId, performanceStateRuleConfig.CriticalThreshold, performanceStateRuleConfig.WarningThreshold, performanceStateRuleConfig.TimeWindow, performanceStateRuleConfig.RuleName, performanceStateRuleConfig.RuleType);
		return ConvertToPerformanceStateRuleConfigGenericObject2(performanceStateRuleConfig);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		PerformanceStateRuleConfig performanceStateRuleConfig = ConvertToPerformanceStateRuleConfig(genericObject.Properties);
		performanceStateRuleConfig = PerformanceStateRuleConfig.UpdatePerformanceStateRuleConfig(requestContext, performanceStateRuleConfig.CounterRuleId, performanceStateRuleConfig.CriticalThreshold, performanceStateRuleConfig.WarningThreshold, performanceStateRuleConfig.TimeWindow, performanceStateRuleConfig.RuleName, performanceStateRuleConfig.RuleType, performanceStateRuleConfig.Disabled);
		return ConvertToPerformanceStateRuleConfigGenericObject2(performanceStateRuleConfig);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		PerformanceStateRuleConfig.DeletePerformanceStateRuleConfig(requestContext, id);
	}
}
