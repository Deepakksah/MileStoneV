namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IRuleProxy
{
	string[] QueryRuleIds(IGeminiRequestContext requestContext);

	Rule QueryRuleById(IGeminiRequestContext requestContext, string id);

	Rule[] QueryRulesByIds(IGeminiRequestContext requestContext, string[] ids);

	Rule CreateRule(IGeminiRequestContext requestContext, string id, string name, string description, string definition, bool enabled);

	Rule UpdateRule(IGeminiRequestContext requestContext, string id, string name, string description, string definition, bool enabled);

	void DeleteRule(IGeminiRequestContext requestContext, string id);
}
