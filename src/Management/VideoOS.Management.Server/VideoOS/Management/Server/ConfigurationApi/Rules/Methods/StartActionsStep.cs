using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class StartActionsStep : ActionsStepBase
{
	public StartActionsStep(IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
		: base(isStartAction: true, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy)
	{
	}

	public override bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (item.TryGetPropertyValue(ActionsPropertyKey, out var result))
		{
			IEnumerable<IActionDefinition> actionDefinitions = from at in StepBase.GetSelectedActionTypes(result)
				select _definitionsProvider.Get(requestContext, at);
			return AreActionsComplete(item, actionDefinitions);
		}
		return false;
	}
}
