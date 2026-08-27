using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public interface IWizardStep
{
	bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item);

	List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey);

	List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey);
}
