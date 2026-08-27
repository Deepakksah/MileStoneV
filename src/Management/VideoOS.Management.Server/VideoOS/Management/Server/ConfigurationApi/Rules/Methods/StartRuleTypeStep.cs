using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class StartRuleTypeStep(IEventTypeProxy eventTypeProxy, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy) : RuleTypeStepBase(eventTypeProxy, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy, "StartRuleType", "StartEventGroup", "StartEventType", "StartEventSources", ConfigurationApiResources.PropertyRuleStartRuleType, ConfigurationApiResources.PropertyRuleStartEventGroup, ConfigurationApiResources.PropertyRuleStartEventType, ConfigurationApiResources.PropertyRuleStartEventSources, "PropertyRuleStartRuleType", "PropertyRuleStartEventGroup", "PropertyRuleStartEventType", "PropertyRuleStartEventType")
{
	private readonly RecurringTimeSubstep _recurringTimeSubstep = new RecurringTimeSubstep();

	public override bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (item.TryGetPropertyValue("StartRuleType", out TriggerType result) && result == TriggerType.RecurringTime)
		{
			return _recurringTimeSubstep.IsComplete(requestContext, item);
		}
		if (item.TryGetPropertyValue("StartRuleType", out result) && result == TriggerType.Timer)
		{
			throw new ArgumentException("Timer is not valid for start rule type");
		}
		return base.IsComplete(requestContext, item);
	}

	public override List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		lastPropertyKey = null;
		if (item.TryGetPropertyValue("StartRuleType", out TriggerType result) && result == TriggerType.RecurringTime)
		{
			List<Property> list = new List<Property>();
			list.Add(CreateEnumProperty(RuleTypePropertyKey, result.ToString(), RuleTypePropertyTranslationId, GetRuleTypeEnumValues(requestContext, item)));
			list.AddRange(_recurringTimeSubstep.GetCurrentProperties(requestContext, item, out lastPropertyKey));
			lastPropertyKey = lastPropertyKey ?? "StartRuleType";
			return list;
		}
		return base.GetCurrentProperties(requestContext, item, out lastPropertyKey);
	}

	public override List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		if (item.TryGetPropertyValue("StartRuleType", out TriggerType result) && result == TriggerType.RecurringTime)
		{
			return _recurringTimeSubstep.GetNextProperties(requestContext, item, lastPropertyKey);
		}
		return base.GetNextProperties(requestContext, item, lastPropertyKey);
	}

	protected override IDictionary<string, string> GetRuleTypeEnumValues(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		return RuleTypeStepBase.StartRuleTypeEnumValues;
	}
}
