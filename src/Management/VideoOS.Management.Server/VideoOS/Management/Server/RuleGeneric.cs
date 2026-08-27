using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("Rule")]
public class RuleGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(Rule.QueryRuleById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToObject2s(Rule.QueryRulesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "Rule")
		{
			return Rule.QueryRuleIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		Rule rule = ConvertToRule(properties);
		rule = Rule.CreateRule(requestContext, rule.Id, rule.Name, rule.Description, rule.Definition, rule.Enabled);
		return ConvertToGenericObject2(rule);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		Rule rule = ConvertToRule(genericObject);
		rule = Rule.UpdateRule(requestContext, rule.Id, rule.Name, rule.Description, rule.Definition, rule.Enabled);
		return ConvertToGenericObject2(rule);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		Rule.DeleteRule(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(Rule rule)
	{
		if (rule == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", rule.Id));
		list.Add(new ObjectProperty2("Name", rule.Name));
		list.Add(new ObjectProperty2("Description", rule.Description));
		list.Add(new ObjectProperty2("LastModified", rule.LastModified));
		list.Add(new ObjectProperty2("RuleXml", rule.Definition));
		list.Add(new ObjectProperty2("Enabled", rule.Enabled));
		return new GenericObject2(rule.Id, "Rule", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToObject2s(Rule[] rules)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (Rule rule in rules)
		{
			list.Add(ConvertToGenericObject2(rule));
		}
		return list.ToArray();
	}

	private static Rule ConvertToRule(GenericObject2 genericObject)
	{
		return ConvertToRule(genericObject.Properties);
	}

	private static Rule ConvertToRule(ObjectProperty2[] properties)
	{
		Rule rule = new Rule();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			rule.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			rule.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			rule.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			rule.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "RuleXml"))
		{
			rule.Definition = GenericObjectHelper2.GetProperty<string>(properties, "RuleXml");
		}
		if (GenericObjectHelper2.Contains(properties, "Enabled"))
		{
			rule.Enabled = GenericObjectHelper2.GetProperty<bool>(properties, "Enabled");
		}
		return rule;
	}
}
