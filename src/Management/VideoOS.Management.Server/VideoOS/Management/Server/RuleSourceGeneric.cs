using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("RuleSource")]
public class RuleSourceGeneric : IGenericObjectSupport2
{
	private static GenericObject2[] ConvertToRuleSourceGenericObject2s(RuleSource[] ruleSource)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (RuleSource ruleSource2 in ruleSource)
		{
			list.Add(ConvertToRuleSourceGenericObject2(ruleSource2));
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToRuleSourceGenericObject2(RuleSource ruleSource)
	{
		if (ruleSource == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", ruleSource.SourceId));
		list.Add(new ObjectProperty2("Name", ruleSource.Name));
		list.Add(new ObjectProperty2("CriticalThreshold", ruleSource.CriticalThreshold));
		list.Add(new ObjectProperty2("WarningThreshold", ruleSource.WarningThreshold));
		list.Add(new ObjectProperty2("TimeWindow", ruleSource.TimeWindow));
		list.Add(new ObjectProperty2("RuleType", ruleSource.RuleType));
		list.Add(new ObjectProperty2("CounterRuleId", ruleSource.CounterRuleId));
		list.Add(new ObjectProperty2("Configurable", ruleSource.Configurable));
		list.Add(new ObjectProperty2("CounterId", ruleSource.CounterId));
		list.Add(new ObjectProperty2("DataCollectorId", ruleSource.DataCollectorId));
		list.Add(new ObjectProperty2("CounterPath", ruleSource.CounterPath));
		list.Add(new ObjectProperty2("ParentId", ruleSource.ParentId));
		return new GenericObject2(ruleSource.SourceId, "RuleSource", list.ToArray(), null);
	}

	private static RuleSource ConvertToRuleSource(ObjectProperty2[] properties)
	{
		RuleSource ruleSource = new RuleSource();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			ruleSource.SourceId = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			ruleSource.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "CriticalThreshold"))
		{
			ruleSource.CriticalThreshold = GenericObjectHelper2.GetProperty<float>(properties, "CriticalThreshold");
		}
		if (GenericObjectHelper2.Contains(properties, "WarningThreshold"))
		{
			ruleSource.WarningThreshold = GenericObjectHelper2.GetProperty<float>(properties, "WarningThreshold");
		}
		if (GenericObjectHelper2.Contains(properties, "TimeWindow"))
		{
			ruleSource.TimeWindow = GenericObjectHelper2.GetProperty<int>(properties, "TimeWindow");
		}
		if (GenericObjectHelper2.Contains(properties, "RuleType"))
		{
			ruleSource.RuleType = GenericObjectHelper2.GetProperty<int>(properties, "RuleType");
		}
		if (GenericObjectHelper2.Contains(properties, "CounterRuleId"))
		{
			ruleSource.CounterRuleId = GenericObjectHelper2.GetProperty<string>(properties, "CounterRuleId");
		}
		if (GenericObjectHelper2.Contains(properties, "Configurable"))
		{
			ruleSource.Configurable = GenericObjectHelper2.GetProperty<bool>(properties, "Configurable");
		}
		if (GenericObjectHelper2.Contains(properties, "CounterId"))
		{
			ruleSource.CounterId = GenericObjectHelper2.GetProperty<int>(properties, "CounterId");
		}
		if (GenericObjectHelper2.Contains(properties, "DataCollectorId"))
		{
			ruleSource.DataCollectorId = GenericObjectHelper2.GetProperty<int>(properties, "DataCollectorId");
		}
		if (GenericObjectHelper2.Contains(properties, "CounterPath"))
		{
			ruleSource.CounterPath = GenericObjectHelper2.GetProperty<string>(properties, "CounterPath");
		}
		if (GenericObjectHelper2.Contains(properties, "ParentId"))
		{
			ruleSource.ParentId = GenericObjectHelper2.GetProperty<string>(properties, "ParentId");
		}
		return ruleSource;
	}

	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToRuleSourceGenericObject2(RuleSource.QueryRuleSourceById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToRuleSourceGenericObject2s(RuleSource.QueryRuleSourceByIds(requestContext, -1, ids, generateSources: true));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "RuleSource")
		{
			return QueryRuleSourceIds(requestContext);
		}
		return null;
	}

	public static GenericObject2[] QueryAllRuleSources(IGeminiRequestContext requestContext)
	{
		return ConvertToRuleSourceGenericObject2s(RuleSource.QueryRuleSourceByIds(requestContext, -1, null, generateSources: true));
	}

	private static string[] QueryRuleSourceIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetService<SecurityManagerService>().CheckApplicationReadPermission(requestContext);
		using RuleSourceComponent ruleSourceComponent = requestContext.CreateComponent<RuleSourceComponent>();
		return ruleSourceComponent.QueryCountersStateRuleIds();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		RuleSource ruleSource = ConvertToRuleSource(properties);
		ruleSource = RuleSource.CreateRuleSource(requestContext, ruleSource.SourceId, ruleSource.CriticalThreshold, ruleSource.WarningThreshold, ruleSource.TimeWindow, ruleSource.CounterRuleId, ruleSource.DataCollectorId, ruleSource.CounterId, ruleSource.ParentId, ruleSource.Name);
		return ConvertToRuleSourceGenericObject2(ruleSource);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		RuleSource ruleSource = ConvertToRuleSource(genericObject.Properties);
		ruleSource = RuleSource.UpdateRuleSource(requestContext, ruleSource.SourceId, ruleSource.CounterRuleId, ruleSource.CriticalThreshold, ruleSource.WarningThreshold, ruleSource.TimeWindow, ruleSource.Name, ruleSource.ParentId);
		return ConvertToRuleSourceGenericObject2(ruleSource);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		RuleSource.DeleteRuleSource(requestContext, id);
	}
}
