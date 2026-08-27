using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public interface IConfigurationPropertiesBuilder<TModel>
{
	IEnumerable<Property> Build(IGeminiRequestContext requestContext, TModel rule);
}
