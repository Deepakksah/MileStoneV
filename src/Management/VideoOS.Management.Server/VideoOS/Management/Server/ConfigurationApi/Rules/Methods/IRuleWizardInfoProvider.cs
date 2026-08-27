using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public interface IRuleWizardInfoProvider
{
	bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item);

	Property[] GetProperties(IGeminiRequestContext requestContext, ConfigurationItem item);
}
