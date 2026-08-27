namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class RuleProxy : IRuleProxy
{
	public string[] QueryRuleIds(IGeminiRequestContext requestContext)
	{
		return Rule.QueryRuleIds(requestContext);
	}

	public Rule QueryRuleById(IGeminiRequestContext requestContext, string id)
	{
		return Rule.QueryRuleById(requestContext, id);
	}

	public Rule[] QueryRulesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		return Rule.QueryRulesByIds(requestContext, ids);
	}

	public Rule CreateRule(IGeminiRequestContext requestContext, string id, string name, string description, string definition, bool enabled)
	{
		return Rule.CreateRule(requestContext, id, name, description, definition, enabled);
	}

	public Rule UpdateRule(IGeminiRequestContext requestContext, string id, string name, string description, string definition, bool enabled)
	{
		return Rule.UpdateRule(requestContext, id, name, description, definition, enabled);
	}

	public void DeleteRule(IGeminiRequestContext requestContext, string id)
	{
		Rule.DeleteRule(requestContext, id);
	}
}
