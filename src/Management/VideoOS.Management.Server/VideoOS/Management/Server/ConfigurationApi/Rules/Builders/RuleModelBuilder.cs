using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class RuleModelBuilder : RuleModelBuilderBase, IRuleModelBuilder
{
	private readonly IRuleModelConditionsBuilder _conditionsBuilder;

	private readonly IRuleModelActionsBuilder _actionsBuilder;

	private readonly IServerInformationProxy _serverInformationProxy;

	private Guid _managementServerId = Guid.Empty;

	public RuleModelBuilder(IRuleModelConditionsBuilder conditionsBuilder, IServerInformationProxy serverInformationProxy, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		_conditionsBuilder = conditionsBuilder;
		_actionsBuilder = new RuleModelActionsBuilder(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
		_serverInformationProxy = serverInformationProxy;
	}

	public RuleModel Build(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		item.NullCheck("", "Build", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\RuleModel\\RuleModelBuilder.cs", 30);
		Guid result;
		string result2;
		RuleModel ruleModel = new RuleModel
		{
			Id = (item.TryGetPropertyValue("Id", out result) ? result : default(Guid)),
			Name = item.GetPropertyValue("Name"),
			Description = (item.TryGetPropertyValue("Description", out result2) ? result2 : ""),
			RuleStart = new RulePartModel
			{
				IsStart = true
			},
			RuleStop = new RulePartModel
			{
				IsStart = false
			},
			Enabled = item.EnableProperty.Enabled
		};
		SetRuleTrigger(requestContext, ruleModel.RuleStart, item, isStartRule: true);
		ruleModel.RuleStart.Conditions = _conditionsBuilder.Build(item);
		ruleModel.RuleStart.Actions = _actionsBuilder.Build(requestContext, item, "Start", isStartAction: true);
		SetRuleTrigger(requestContext, ruleModel.RuleStop, item, isStartRule: false);
		ruleModel.RuleStop.Actions = _actionsBuilder.Build(requestContext, item, "Stop", isStartAction: false);
		return ruleModel;
	}

	private void SetRuleTrigger(IGeminiRequestContext requestContext, RulePartModel rulePart, ConfigurationItem item, bool isStartRule)
	{
		string propertyName = (isStartRule ? "StartRuleType" : "StopRuleType");
		TriggerType result;
		TriggerType triggerType = (item.TryGetProperty(propertyName, out result) ? result : TriggerType.None);
		if ((isStartRule || triggerType != TriggerType.RecurringTime) && (!isStartRule || triggerType != TriggerType.Timer))
		{
			rulePart.TriggerType = triggerType;
			switch (triggerType)
			{
			case TriggerType.Event:
				SetEvent(requestContext, rulePart, item);
				break;
			case TriggerType.RecurringTime:
				rulePart.RecurringTime = RuleModelRecurringTimeBuilder.Build(item);
				break;
			case TriggerType.Timer:
				SetTimer(rulePart, item);
				break;
			}
		}
	}

	private static void SetTimer(RulePartModel rulePart, ConfigurationItem item)
	{
		rulePart.Delay = (item.TryGetPropertyValue("TimerTime", out int result) ? result : 0);
	}

	private void SetEvent(IGeminiRequestContext requestContext, RulePartModel rule, ConfigurationItem item)
	{
		rule.EventModel = new EventModel();
		EventGroupType result = EventGroupType.None;
		if (!item.TryGetPropertyValue(rule.IsStart ? "StartEventGroup" : "StopEventGroup", out result) && item.TryGetPropertyValue(rule.IsStart ? "StartEventGroup" : "StopEventGroup", out Guid result2))
		{
			result = EventGroupType.PluginEvent;
			rule.EventModel.PluginEventGroupId = result2;
		}
		rule.EventModel.EventGroupType = result;
		item.TryGetPropertyValue(rule.IsStart ? "StartEventType" : "StopEventType", out Guid result3);
		rule.EventModel.EventType = result3;
		string propertyName = (rule.IsStart ? "StartEventSources" : "StopEventSources");
		IEnumerable<ConfigurationItemPath> source = RuleModelBuilderBase.ExtractPaths(item, propertyName);
		rule.EventModel.EventSources = source.Select((ConfigurationItemPath s) => new EventSource
		{
			Id = ((string.IsNullOrEmpty(s.Id) && s.ItemType == "System") ? GetTopLevelId(requestContext, rule.EventModel.EventGroupType) : new Guid(s.Id)),
			ItemType = s.ItemType,
			IsGroup = RuleModelBuilderBase.IsGroup(s.ItemType)
		});
	}

	private Guid GetTopLevelId(IGeminiRequestContext requestContext, EventGroupType eventGroupType)
	{
		if (eventGroupType == EventGroupType.SystemDashboardServer)
		{
			if (_managementServerId == Guid.Empty)
			{
				_managementServerId = new Guid(_serverInformationProxy.GetServerInformation(requestContext).Id);
			}
			return _managementServerId;
		}
		return SourceGuid.Server;
	}
}
