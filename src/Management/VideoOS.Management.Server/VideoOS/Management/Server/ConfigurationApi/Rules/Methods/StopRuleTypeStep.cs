using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class StopRuleTypeStep : RuleTypeStepBase
{
	public StopRuleTypeStep(IEventTypeProxy eventTypeProxy, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
		: base(eventTypeProxy, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy, "StopRuleType", "StopEventGroup", "StopEventType", "StopEventSources", ConfigurationApiResources.PropertyRuleStopRuleType, ConfigurationApiResources.PropertyRuleStopEventGroup, ConfigurationApiResources.PropertyRuleStopEventType, ConfigurationApiResources.PropertyRuleStopEventSources, "PropertyRuleStopRuleType", "PropertyRuleStopEventGroup", "PropertyRuleStopEventType", "PropertyRuleStopEventSources")
	{
	}

	public override bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		int result2;
		if (item.TryGetPropertyValue("StopRuleType", out TriggerType result) && result == TriggerType.Timer)
		{
			return item.TryGetPropertyValue("TimerTime", out result2);
		}
		return base.IsComplete(requestContext, item);
	}

	public override List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		lastPropertyKey = null;
		if (item.TryGetPropertyValue("StopRuleType", out TriggerType result) && result == TriggerType.Timer)
		{
			return GetTimerProperties(requestContext, item, result, out lastPropertyKey);
		}
		return base.GetCurrentProperties(requestContext, item, out lastPropertyKey);
	}

	public override List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		if (item.TryGetPropertyValue("StopRuleType", out TriggerType result) && result == TriggerType.RecurringTime)
		{
			return new List<Property>();
		}
		if (item.TryGetPropertyValue("StopRuleType", out result) && result == TriggerType.Timer)
		{
			List<Property> list = new List<Property>();
			if (lastPropertyKey == "StopRuleType")
			{
				CreateDelayProperty(list, 0);
				return list;
			}
		}
		return base.GetNextProperties(requestContext, item, lastPropertyKey);
	}

	protected override IDictionary<string, string> GetRuleTypeEnumValues(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (item.TryGetPropertyValueIfValid("StartRuleType", RuleTypeStepBase.StartRuleTypeEnumValues.Keys, out TriggerType result))
		{
			if (result == TriggerType.RecurringTime)
			{
				return new Dictionary<string, string> { { "None", "None" } };
			}
			if (item.TryGetPropertyValue("StartActions", out var result2))
			{
				return GetAllowedRuleTypes(requestContext, result2, result);
			}
		}
		return RuleTypeStepBase.StopRuleTypeEnumValues;
	}

	private Dictionary<string, string> GetAllowedRuleTypes(IGeminiRequestContext requestContext, string actionsProperty, TriggerType triggerType)
	{
		IEnumerable<IActionDefinition> source = from at in StepBase.GetSelectedActionTypes(actionsProperty)
			select _definitionsProvider.Get(requestContext, at);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (source.All((IActionDefinition action) => action == null || action.CounteractionActionType == ActionType.None.ToString()))
		{
			dictionary.Add("None", "None");
		}
		if (triggerType == TriggerType.TimeInterval)
		{
			dictionary.Add("TimeInterval", ConfigurationApiResources.RuleTypeInterval);
		}
		if (triggerType == TriggerType.Event)
		{
			dictionary.Add("Event", ConfigurationApiResources.RuleTypeEvent);
			dictionary.Add("Timer", ConfigurationApiResources.RuleTypeTimer);
		}
		return dictionary;
	}

	private List<Property> GetTimerProperties(IGeminiRequestContext requestContext, ConfigurationItem item, TriggerType triggerType, out string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		list.Add(CreateEnumProperty(RuleTypePropertyKey, triggerType.ToString(), RuleTypePropertyTranslationId, GetRuleTypeEnumValues(requestContext, item)));
		lastPropertyKey = "StopRuleType";
		if (!item.TryGetPropertyValue("TimerTime", out int result))
		{
			return list;
		}
		CreateDelayProperty(list, result);
		lastPropertyKey = "TimerTime";
		return list;
	}

	private static void CreateDelayProperty(List<Property> properties, int value)
	{
		properties.Add(GeneralPropertyDefinitions.GetIntProperty("TimerTime", ConfigurationApiResources.PropertyRuleTimerTime, value.ToString(CultureInfo.InvariantCulture), "PropertyRuleTimerTime", isSettable: true));
	}
}
