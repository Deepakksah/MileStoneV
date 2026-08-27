using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public interface IRuleTypeXmlBuilder
{
	string GetXml(RulePartModel rulePart);
}
