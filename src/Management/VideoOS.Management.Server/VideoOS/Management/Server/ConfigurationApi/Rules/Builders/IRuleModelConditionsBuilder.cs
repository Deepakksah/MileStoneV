using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public interface IRuleModelConditionsBuilder
{
	IDictionary<ConditionType, ConditionModel> Build(ConfigurationItem item);
}
