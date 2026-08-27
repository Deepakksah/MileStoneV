using System;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class Rule
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime LastModified { get; set; }

	public string Definition { get; set; }

	public bool Enabled { get; set; }

	private static string[] FilterRuleIdsBasedOnPermissions(IGeminiRequestContext requestContext, string[] ids)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.Rule, "GENERIC_READ", ids.Select((string id) => (id, id))).ToArray();
	}

	public static string[] QueryRuleIds(IGeminiRequestContext requestContext)
	{
		string[] ids;
		using (IRuleComponent ruleComponent = requestContext.CreateRuleComponent())
		{
			ids = ruleComponent.QueryRuleIds();
		}
		return FilterRuleIdsBasedOnPermissions(requestContext, ids);
	}

	public static Rule QueryRuleById(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "GENERIC_READ", id))
		{
			return null;
		}
		using IRuleComponent ruleComponent = requestContext.CreateRuleComponent();
		return ruleComponent.QueryRuleById(id);
	}

	public static Rule[] QueryRulesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		string[] array = ids;
		for (int i = 0; i < array.Length; i++)
		{
			Validation.CheckObjectId(array[i], "id", allowNullOrEmpty: false);
		}
		ids = FilterRuleIdsBasedOnPermissions(requestContext, ids);
		Rule[] result = new Rule[0];
		if (ids.Any())
		{
			using IRuleComponent ruleComponent = requestContext.CreateRuleComponent();
			result = ruleComponent.QueryRulesByIds(ids);
		}
		return result;
	}

	public static Rule CreateRule(IGeminiRequestContext requestContext, string id, string name, string description, string definition, bool enabled)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "CREATE_RULE", "/");
		id = GeminiResourceComponent.GetGuid(id);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description");
		Validation.CheckName(definition, "definition");
		Rule rule;
		using (IRuleComponent ruleComponent = requestContext.CreateRuleComponent())
		{
			ruleComponent.CreateRule(id, name, description, definition, enabled);
			rule = ruleComponent.QueryRuleById(id);
		}
		Integration.FireConfigurationChanged(requestContext, "Rule", id, EventAction.Added, name);
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditRuleCreated(rule.Id.ToGuid(), rule.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return rule;
	}

	public static Rule UpdateRule(IGeminiRequestContext requestContext, string id, string name, string description, string definition, bool enabled)
	{
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "GENERIC_WRITE", id);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "description", allowNull: false);
		Validation.CheckName(definition, "definition");
		Guid sessionId = Guid.NewGuid();
		Rule result;
		using (IRuleComponent ruleComponent = requestContext.CreateRuleComponent())
		{
			ruleComponent.UpdateRule(id, name, description, definition, enabled, sessionId);
			result = ruleComponent.QueryRuleById(id);
		}
		Integration.FireConfigurationChanged(requestContext, "Rule", id, EventAction.Updated, name);
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static void DeleteRule(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "DELETE", id);
		Guid sessionId = Guid.NewGuid();
		using (IRuleComponent ruleComponent = requestContext.CreateRuleComponent())
		{
			ruleComponent.DeleteRule(id, sessionId);
		}
		Integration.FireConfigurationChanged(requestContext, "Rule", id, EventAction.Deleted, string.Empty);
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}
}
