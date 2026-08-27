using System;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public interface IXmlRuleModelBuilder
{
	RuleModel Build(Guid id, string name, string description, string ruleXml, bool enabled, IGeminiRequestContext requestContext);
}
