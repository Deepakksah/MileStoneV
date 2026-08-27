using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public interface IRuleModelActionsBuilder
{
	IEnumerable<ActionModel> Build(IGeminiRequestContext requestContext, ConfigurationItem item, string prefix, bool isStartAction);
}
