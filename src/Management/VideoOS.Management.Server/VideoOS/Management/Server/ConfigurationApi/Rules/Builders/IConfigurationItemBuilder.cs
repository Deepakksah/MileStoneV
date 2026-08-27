using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public interface IConfigurationItemBuilder
{
	ConfigurationItem Build(IGeminiRequestContext requestContext, RuleModel rule);
}
