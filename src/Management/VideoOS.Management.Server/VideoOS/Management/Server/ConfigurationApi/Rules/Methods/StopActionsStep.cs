using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class StopActionsStep : ActionsStepBase
{
	private const string StopRuleTypeKey = "StopRuleType";

	public StopActionsStep(IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
		: base(isStartAction: false, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy)
	{
	}

	public override bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!item.TryGetPropertyValue("StopRuleType", out TriggerType result))
		{
			return false;
		}
		if (result == TriggerType.None)
		{
			return true;
		}
		if (!item.TryGetPropertyValue(ActionsPropertyKey, out var result2))
		{
			return false;
		}
		IEnumerable<IActionDefinition> actionDefinitions = from sat in StepBase.GetSelectedActionTypes(result2)
			select _definitionsProvider.Get(requestContext, sat);
		return AreActionsComplete(item, actionDefinitions);
	}
}
