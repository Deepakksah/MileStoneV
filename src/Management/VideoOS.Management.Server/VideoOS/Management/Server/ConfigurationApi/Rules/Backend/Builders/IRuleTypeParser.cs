using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public interface IRuleTypeParser
{
	bool TryParseRuleType(XmlNode rulePartNode, bool isStart, RulePartModel rulePartModel, IGeminiRequestContext requestContext);
}
