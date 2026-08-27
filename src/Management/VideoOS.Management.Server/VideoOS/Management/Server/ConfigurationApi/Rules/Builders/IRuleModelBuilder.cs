using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public interface IRuleModelBuilder
{
	RuleModel Build(IGeminiRequestContext requestContext, ConfigurationItem item);
}
