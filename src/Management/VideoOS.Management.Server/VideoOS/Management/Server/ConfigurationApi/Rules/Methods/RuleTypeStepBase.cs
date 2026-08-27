using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public abstract class RuleTypeStepBase : StepBase
{
	private readonly IEventTypeProxy _eventTypeProxy;

	private readonly EventTypeProvider _eventTypeProvider;

	private readonly string _eventGroupPropertyKey;

	private readonly string _eventTypePropertyKey;

	private readonly string _eventSourcesPropertyKey;

	private readonly List<string> _eventRuleTypePropertyKeys;

	private readonly Dictionary<string, Func<IGeminiRequestContext, ConfigurationItem, Property>> _eventPropertyKeyToMethodMapper;

	private readonly Dictionary<string, Func<IGeminiRequestContext, ConfigurationItem, string, Property>> _lastPropertyToMethodMapper;

	private readonly Dictionary<string, string> _propertyKeyToResourceMapper;

	protected readonly string RuleTypePropertyKey;

	private readonly string _eventTypePropertyTranslationId;

	private readonly string _eventGroupPropertyTranslationId;

	private readonly string _eventSourcesPropertyTranslationId;

	protected readonly string RuleTypePropertyTranslationId;

	protected static readonly IDictionary<string, string> StopRuleTypeEnumValues = new Dictionary<string, string>
	{
		{ "None", "None" },
		{
			"TimeInterval",
			ConfigurationApiResources.RuleTypeInterval
		},
		{
			"Event",
			ConfigurationApiResources.RuleTypeEvent
		},
		{
			"Timer",
			ConfigurationApiResources.RuleTypeTimer
		}
	};

	protected static readonly IDictionary<string, string> StartRuleTypeEnumValues = new Dictionary<string, string>
	{
		{
			"TimeInterval",
			ConfigurationApiResources.RuleTypeInterval
		},
		{
			"Event",
			ConfigurationApiResources.RuleTypeEvent
		},
		{
			"RecurringTime",
			ConfigurationApiResources.RuleTypeRecurringTime
		}
	};

	protected RuleTypeStepBase(IEventTypeProxy eventTypeProxy, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy, string ruleTypePropertyKey, string eventGroupPropertyKey, string eventTypePropertyKey, string eventSourcesPropertyKey, string ruleTypePropertyResource, string eventGroupPropertyResource, string eventTypePropertyResource, string eventSourcesPropertyResource, string ruleTypePropertyTranslationId, string eventGroupPropertyTranslationId, string eventTypePropertyTranslationId, string eventSourcesPropertyTranslationId)
		: base(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy)
	{
		_eventTypeProxy = eventTypeProxy;
		_eventTypeProvider = new EventTypeProvider(_eventTypeProxy);
		RuleTypePropertyKey = ruleTypePropertyKey;
		_eventGroupPropertyKey = eventGroupPropertyKey;
		_eventTypePropertyKey = eventTypePropertyKey;
		_eventSourcesPropertyKey = eventSourcesPropertyKey;
		RuleTypePropertyTranslationId = ruleTypePropertyTranslationId;
		_eventTypePropertyTranslationId = eventTypePropertyTranslationId;
		_eventGroupPropertyTranslationId = eventGroupPropertyTranslationId;
		_eventSourcesPropertyTranslationId = eventSourcesPropertyTranslationId;
		_eventRuleTypePropertyKeys = new List<string> { _eventGroupPropertyKey, _eventTypePropertyKey, _eventSourcesPropertyKey };
		_eventPropertyKeyToMethodMapper = new Dictionary<string, Func<IGeminiRequestContext, ConfigurationItem, Property>>
		{
			{ _eventGroupPropertyKey, GetEventGroupProperty },
			{ _eventTypePropertyKey, GetEventTypeProperty },
			{ _eventSourcesPropertyKey, GetEventSourcesProperty }
		};
		_propertyKeyToResourceMapper = new Dictionary<string, string>
		{
			{ RuleTypePropertyKey, ruleTypePropertyResource },
			{ _eventGroupPropertyKey, eventGroupPropertyResource },
			{ _eventTypePropertyKey, eventTypePropertyResource },
			{ _eventSourcesPropertyKey, eventSourcesPropertyResource }
		};
		_lastPropertyToMethodMapper = new Dictionary<string, Func<IGeminiRequestContext, ConfigurationItem, string, Property>>
		{
			{ _eventTypePropertyKey, MakeEventSourcesProperty },
			{ _eventGroupPropertyKey, MakeEventTypeProperty },
			{ RuleTypePropertyKey, MakeEventGroupProperty },
			{
				string.Empty,
				MakeRuleTypeProperty
			}
		};
	}

	public override bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (item.TryGetPropertyValueIfValid(RuleTypePropertyKey, GetRuleTypeEnumValues(requestContext, item).Keys, out TriggerType result))
		{
			return CheckRuleTypeSpecificProperties(item, result.ToString(), mustHaveValue: true);
		}
		return false;
	}

	public override List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		List<Property> list = new List<Property>();
		lastPropertyKey = null;
		if (item.TryGetPropertyValueIfValid(RuleTypePropertyKey, GetRuleTypeEnumValues(requestContext, item).Keys, out TriggerType result))
		{
			list.Add(MakeRuleTypeProperty(requestContext, item, result.ToString()));
			lastPropertyKey = RuleTypePropertyKey;
			if (IsEventRuleType(item))
			{
				foreach (string eventRuleTypePropertyKey in _eventRuleTypePropertyKeys)
				{
					Property item2;
					if ((item2 = _eventPropertyKeyToMethodMapper[eventRuleTypePropertyKey](requestContext, item)) == null)
					{
						break;
					}
					list.Add(item2);
					lastPropertyKey = eventRuleTypePropertyKey;
				}
			}
		}
		return list;
	}

	public override List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		if (_lastPropertyToMethodMapper.TryGetValue(string.IsNullOrEmpty(lastPropertyKey) ? string.Empty : lastPropertyKey, out var value))
		{
			return new List<Property> { value(requestContext, item, string.Empty) };
		}
		return new List<Property>();
	}

	protected abstract IDictionary<string, string> GetRuleTypeEnumValues(IGeminiRequestContext requestContext, ConfigurationItem item);

	private bool CheckRuleTypeSpecificProperties(ConfigurationItem item, string ruleType, bool mustHaveValue)
	{
		Func<ConfigurationItem, string, bool> checkFunc = (ConfigurationItem a, string b) => (!mustHaveValue) ? a.IsPropertyPresent(b) : a.IsPropertyValuePresent(b);
		if (IsEventTriggerType(ruleType))
		{
			return _eventRuleTypePropertyKeys.All((string p) => checkFunc(item, p));
		}
		return true;
	}

	private static bool IsEventTriggerType(string ruleType)
	{
		return ruleType == TriggerType.Event.ToString();
	}

	private bool IsEventRuleType(ConfigurationItem item)
	{
		if (item.TryGetPropertyValue(RuleTypePropertyKey, out var result))
		{
			return IsEventTriggerType(result);
		}
		return false;
	}

	private Property GetEventGroupProperty(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		string text = null;
		Guid result2;
		if (item.TryGetPropertyValue(_eventGroupPropertyKey, out EventGroupType result))
		{
			text = result.ToString();
		}
		else if (item.TryGetPropertyValue(_eventGroupPropertyKey, out result2))
		{
			text = result2.ToString();
		}
		if (text == null)
		{
			return null;
		}
		return MakeEventGroupProperty(requestContext, item, text);
	}

	private Property GetEventTypeProperty(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		Guid eventGroupId = Guid.Empty;
		EventGroupType eventGroupType = EventGroupType.None;
		try
		{
			eventGroupType = item.GetPropertyValue<EventGroupType>(_eventGroupPropertyKey);
		}
		catch (ArgumentException)
		{
			eventGroupId = item.GetPropertyValue<Guid>(_eventGroupPropertyKey);
			eventGroupType = EventGroupType.PluginEvent;
		}
		Dictionary<string, string> eventTypes = _eventTypeProvider.GetEventTypes(requestContext, eventGroupType, eventGroupId);
		if (!item.TryGetPropertyValue(_eventTypePropertyKey, out Guid result) || !eventTypes.Any())
		{
			return null;
		}
		return CreateEnumProperty(_eventTypePropertyKey, result.ToString(), _eventTypePropertyTranslationId, eventTypes);
	}

	private Property GetEventSourcesProperty(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!item.TryGetPropertyValue(_eventSourcesPropertyKey, out var result))
		{
			return null;
		}
		return MakeEventSourcesProperty(requestContext, item, result);
	}

	private Property MakeRuleTypeProperty(IGeminiRequestContext requestContext, ConfigurationItem item, string value)
	{
		IDictionary<string, string> ruleTypeEnumValues = GetRuleTypeEnumValues(requestContext, item);
		return CreateEnumProperty(RuleTypePropertyKey, value, RuleTypePropertyTranslationId, ruleTypeEnumValues);
	}

	private Property MakeEventGroupProperty(IGeminiRequestContext requestContext, ConfigurationItem item, string value)
	{
		return CreateEnumProperty(_eventGroupPropertyKey, value, _eventGroupPropertyTranslationId, _eventTypeProxy.GetEventGroups(requestContext));
	}

	private Property MakeEventTypeProperty(IGeminiRequestContext requestContext, ConfigurationItem item, string value)
	{
		Guid eventGroupId = Guid.Empty;
		EventGroupType eventGroupType = EventGroupType.None;
		try
		{
			eventGroupType = item.GetPropertyValue<EventGroupType>(_eventGroupPropertyKey);
		}
		catch (ArgumentException)
		{
			eventGroupId = item.GetPropertyValue<Guid>(_eventGroupPropertyKey);
			eventGroupType = EventGroupType.PluginEvent;
		}
		return CreateEnumProperty(_eventTypePropertyKey, value, _eventTypePropertyTranslationId, _eventTypeProvider.GetEventTypes(requestContext, eventGroupType, eventGroupId));
	}

	private Property MakeEventSourcesProperty(IGeminiRequestContext requestContext, ConfigurationItem item, string value)
	{
		Guid propertyValue = item.GetPropertyValue<Guid>(_eventTypePropertyKey);
		return CreatePathListProperty(_eventSourcesPropertyKey, value, _eventSourcesPropertyTranslationId, _eventTypeProxy.GetEventSourceItemTypes(requestContext, propertyValue));
	}

	protected Property CreateEnumProperty(string propertyName, string value, string translationId, IDictionary<string, string> members)
	{
		string displayName = _propertyKeyToResourceMapper[propertyName];
		return GeneralPropertyDefinitions.GetEnumProperty(propertyName, value, displayName, translationId, string.Empty, members);
	}

	private Property CreatePathListProperty(string propertyName, string value, string translationId, IEnumerable<string> itemTypes)
	{
		string displayName = _propertyKeyToResourceMapper[propertyName];
		return GeneralPropertyDefinitions.GetPathListProperty(propertyName, value, displayName, translationId, itemTypes);
	}
}
