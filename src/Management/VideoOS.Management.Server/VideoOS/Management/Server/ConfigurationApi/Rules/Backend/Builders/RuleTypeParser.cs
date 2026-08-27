using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class RuleTypeParser : IRuleTypeParser
{
	private static readonly List<TriggerType> _startRuleTriggerTypes = new List<TriggerType>
	{
		TriggerType.Event,
		TriggerType.TimeInterval,
		TriggerType.RecurringTime
	};

	private static readonly List<TriggerType> _stopRuleTriggerTypes = new List<TriggerType>
	{
		TriggerType.Event,
		TriggerType.TimeInterval,
		TriggerType.Timer
	};

	private readonly Dictionary<bool, List<TriggerType>> _ruleTriggerTypes = new Dictionary<bool, List<TriggerType>>
	{
		{ true, _startRuleTriggerTypes },
		{ false, _stopRuleTriggerTypes }
	};

	private readonly IEventTypeProxy _eventTypeProxy;

	private readonly IValueValidatorFactory _validatorFactory;

	public RuleTypeParser(IEventTypeProxy eventTypeProxy, IValueValidatorFactory validatorFactory)
	{
		_eventTypeProxy = eventTypeProxy;
		_validatorFactory = validatorFactory;
	}

	public bool TryParseRuleType(XmlNode ruleTypeNode, bool isStart, RulePartModel rulePartModel, IGeminiRequestContext requestContext)
	{
		if (ruleTypeNode?.Attributes == null)
		{
			return false;
		}
		TriggerType ruleTriggerTypeFromString = GetRuleTriggerTypeFromString(ruleTypeNode.Attributes["type"].Value);
		if (!_ruleTriggerTypes[isStart].Contains(ruleTriggerTypeFromString))
		{
			return false;
		}
		rulePartModel.TriggerType = ruleTriggerTypeFromString;
		switch (ruleTriggerTypeFromString)
		{
		case TriggerType.TimeInterval:
			return true;
		case TriggerType.Event:
			if (TryParseEventType(ruleTypeNode, rulePartModel))
			{
				return TryParseEventSources(ruleTypeNode, rulePartModel, requestContext);
			}
			return false;
		case TriggerType.RecurringTime:
			return ScheduleParser.TryParse(ruleTypeNode, rulePartModel);
		case TriggerType.Timer:
			return TryParseTimer(ruleTypeNode, rulePartModel);
		default:
			return false;
		}
	}

	private static TriggerType GetRuleTriggerTypeFromString(string ruleTypeString)
	{
		if (Enum.TryParse<TriggerType>(ruleTypeString, out var result))
		{
			return result;
		}
		return TriggerType.None;
	}

	private static bool TryParseEventType(XmlNode ruleTypeNode, RulePartModel theRule)
	{
		XmlNode nodeByTagName = ruleTypeNode.GetNodeByTagName("EventType");
		if (nodeByTagName == null)
		{
			return false;
		}
		string innerText = nodeByTagName.InnerText;
		theRule.EventModel = new EventModel
		{
			EventType = (string.IsNullOrEmpty(innerText) ? Guid.Empty : new Guid(innerText))
		};
		return true;
	}

	private static bool TryParseTimer(XmlNode ruleTypeNode, RulePartModel theRule)
	{
		XmlNode nodeByTagName = ruleTypeNode.GetNodeByTagName("TimerValue");
		if (nodeByTagName == null)
		{
			return false;
		}
		string innerText = nodeByTagName.InnerText;
		if (string.IsNullOrEmpty(innerText) || !int.TryParse(innerText, out var result))
		{
			return false;
		}
		theRule.Delay = result;
		return true;
	}

	private bool TryParseEventSources(XmlNode ruleTypeNode, RulePartModel theRule, IGeminiRequestContext requestContext)
	{
		XmlNode nodeByTagName = ruleTypeNode.GetNodeByTagName("Sources");
		if (nodeByTagName == null)
		{
			return false;
		}
		theRule.EventModel.EventGroupType = CreateEventGroupType(theRule.EventModel.EventType, requestContext);
		if (theRule.EventModel.EventGroupType == EventGroupType.NotSupported)
		{
			return false;
		}
		if (theRule.EventModel.EventGroupType == EventGroupType.PluginEvent)
		{
			theRule.EventModel.PluginEventGroupId = _eventTypeProxy.GetPluginEventGroupId(requestContext, theRule.EventModel.EventType);
			theRule.EventModel.EventGroupType = _eventTypeProxy.GetHandlerEventGroupType(requestContext, theRule.EventModel.EventGroupType, theRule.EventModel.EventType);
		}
		XmlNode nodeByTagName2 = nodeByTagName.GetNodeByTagName("SourceList");
		XmlNode nodeByTagName3 = nodeByTagName.GetNodeByTagName("GroupList");
		if (nodeByTagName2 == null || nodeByTagName3 == null)
		{
			return false;
		}
		string[] sources = nodeByTagName2.ChildNodes.GenerateArray();
		string[] groups = nodeByTagName3.ChildNodes.GenerateArray();
		theRule.EventModel.EventSources = CreateEventSources(requestContext, theRule.EventModel.EventGroupType, sources, groups);
		return true;
	}

	private EventGroupType CreateEventGroupType(Guid eventType, IGeminiRequestContext requestContext)
	{
		if (eventType == Guid.Empty)
		{
			return EventGroupType.None;
		}
		return _eventTypeProxy.GetEventGroupType(requestContext, eventType);
	}

	private EventSource[] CreateEventSources(IGeminiRequestContext requestContext, EventGroupType eventGroupType, string[] sources, string[] groups)
	{
		IEnumerable<EventSource> first = sources.Select((string s) => new EventSource
		{
			Id = new Guid(s.Trim()),
			ItemType = GetItemType(requestContext, eventGroupType, s.Trim(), isGroup: false),
			IsGroup = false
		});
		IEnumerable<EventSource> second = groups.Select((string s) => new EventSource
		{
			Id = new Guid(s.Trim()),
			ItemType = GetItemType(requestContext, eventGroupType, s.Trim(), isGroup: true),
			IsGroup = true
		});
		return first.Concat(second).ToArray();
	}

	private string GetItemType(IGeminiRequestContext requestContext, EventGroupType eventGroupType, string id, bool isGroup)
	{
		try
		{
			string text = _validatorFactory.GetValidator(eventGroupType, isGroup)?.GetItemType(requestContext, id, null);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		catch (InternalServerException)
		{
		}
		return "Unknown";
	}
}
