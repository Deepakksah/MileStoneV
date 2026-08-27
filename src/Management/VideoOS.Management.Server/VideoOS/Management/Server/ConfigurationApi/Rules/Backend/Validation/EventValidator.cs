using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class EventValidator : IValidator<RulePartModel>
{
	private readonly IEventTypeProxy _eventTypeProxy;

	private readonly IValueValidatorFactory _validatorFactory;

	private readonly EventTypeProvider _eventTypeProvider;

	public EventValidator(IEventTypeProxy eventTypeProxy, IValueValidatorFactory validatorFactory, EventTypeProvider eventTypeProvider)
	{
		_eventTypeProxy = eventTypeProxy;
		_validatorFactory = validatorFactory;
		_eventTypeProvider = eventTypeProvider;
	}

	public ValidationError[] Validate(IGeminiRequestContext requestContext, RulePartModel model)
	{
		model.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\EventValidator.cs", 34);
		if (model.TriggerType != TriggerType.Event)
		{
			return new ValidationError[0];
		}
		EventModel eventModel = model.EventModel;
		if (eventModel == null || eventModel.EventGroupType == EventGroupType.None)
		{
			return new ValidationError[1]
			{
				new ValidationError("Event group type is not set")
				{
					PropertyName = (model.IsStart ? "StartEventGroup" : "StopEventGroup"),
					ValidProperty = GetEmptyEventGroupProperty(requestContext, model)
				}
			};
		}
		if (model.EventModel.EventType == Guid.Empty)
		{
			return new ValidationError[1]
			{
				new ValidationError("Event type is not set")
				{
					PropertyName = (model.IsStart ? "StartEventType" : "StopEventType"),
					ValidProperty = GetEmptyEventTypeProperty(requestContext, model)
				}
			};
		}
		if (_eventTypeProxy.GetEventType(requestContext, model.EventModel.EventType) == null)
		{
			return new ValidationError[1]
			{
				new ValidationError("Selected event type is not valid: " + model.EventModel.EventType.ToString())
				{
					PropertyName = (model.IsStart ? "StartEventType" : "StopEventType"),
					ValidProperty = GetEmptyEventTypeProperty(requestContext, model)
				}
			};
		}
		EventGroupType eventGroupType = _eventTypeProxy.GetEventGroupType(requestContext, model.EventModel.EventType);
		if (model.EventModel.EventGroupType != eventGroupType && (model.EventModel.EventGroupType != EventGroupType.AnalyticsEvent || eventGroupType != EventGroupType.PluginEvent))
		{
			return new ValidationError[1]
			{
				new ValidationError("Event type " + model.EventModel.EventType.ToString() + " is not in the event group: " + model.EventModel.EventGroupType)
				{
					PropertyName = (model.IsStart ? "StartEventType" : "StopEventType"),
					ValidProperty = GetEmptyEventTypeProperty(requestContext, model)
				}
			};
		}
		if (model.EventModel.EventGroupType == EventGroupType.PluginEvent && model.EventModel.PluginEventGroupId != _eventTypeProxy.GetPluginEventGroupId(requestContext, model.EventModel.EventType))
		{
			return new ValidationError[1]
			{
				new ValidationError("Event type " + model.EventModel.EventType.ToString() + " is not in the event group: " + model.EventModel.PluginEventGroupId.ToString())
				{
					PropertyName = (model.IsStart ? "StartEventType" : "StopEventType"),
					ValidProperty = GetEmptyEventTypeProperty(requestContext, model)
				}
			};
		}
		if (model.EventModel.EventSources == null || !model.EventModel.EventSources.Any())
		{
			return new ValidationError[1]
			{
				new ValidationError("Event source is not set")
				{
					PropertyName = EventSourcePropertyName(model),
					ValidProperty = GetEmptyEventSourcesProperty(requestContext, model)
				}
			};
		}
		List<ValidationError> list = new List<ValidationError>();
		list.AddRange(ValidateSourceIds(requestContext, model));
		return list.ToArray();
	}

	private ValidationError[] ValidateSourceIds(IGeminiRequestContext requestContext, RulePartModel model)
	{
		EventSourceData[] sourcesData = _eventTypeProxy.GetEventSourceData(requestContext, model.EventModel.EventType);
		EventGroupType handlerEventGroupType = _eventTypeProxy.GetHandlerEventGroupType(requestContext, model.EventModel.EventGroupType, model.EventModel.EventType);
		IValueValidator sourceValidator = _validatorFactory.GetValidator(handlerEventGroupType, isGroup: false);
		IValueValidator groupValidator = _validatorFactory.GetValidator(handlerEventGroupType, isGroup: true);
		IEnumerable<EventSource> second = from x in model.EventModel.EventSources
			where !x.IsGroup
			where !IsValidEventSource(requestContext, sourceValidator, x.Id, sourcesData)
			select x;
		return (from s in (from x in model.EventModel.EventSources
				where x.IsGroup
				where !IsValidEventSource(requestContext, groupValidator, x.Id, sourcesData)
				select x).Concat(second)
			select new ValidationError("Invalid event {0}source: {1}", s.IsGroup ? "group " : "", s.Id)
			{
				PropertyName = EventSourcePropertyName(model)
			}).ToArray();
	}

	private static bool IsValidEventSource(IGeminiRequestContext requestContext, IValueValidator validator, Guid eventSource, IEnumerable<EventSourceData> allowedSources)
	{
		if (validator != null)
		{
			return allowedSources.Any((EventSourceData source) => source.ItemTypes.Any((string itemType) => validator.Validate(requestContext, eventSource, (itemType == "MIPItem") ? source.PluginType : itemType, CreateFilterParameters(source.Filters))));
		}
		return true;
	}

	private static IDictionary<string, RuleParameter> CreateFilterParameters(string[] filters)
	{
		if (filters == null || !filters.Any())
		{
			return null;
		}
		return new RuleParameter[1]
		{
			new RuleParameter
			{
				Name = "__Filters",
				Values = filters
			}
		}.ToDictionary((RuleParameter s) => s.Name);
	}

	private Property GetEmptyEventGroupProperty(IGeminiRequestContext requestContext, RulePartModel model)
	{
		Dictionary<string, string> eventGroups = _eventTypeProxy.GetEventGroups(requestContext);
		return GeneralPropertyDefinitions.GetEnumProperty(EventGroupTypePropertyName(model), "", EventGroupTypeDisplayName(model), EventGroupTypeTranslationId(model), string.Empty, eventGroups);
	}

	private Property GetEmptyEventTypeProperty(IGeminiRequestContext requestContext, RulePartModel model)
	{
		Dictionary<string, string> eventTypes = _eventTypeProvider.GetEventTypes(requestContext, model.EventModel.EventGroupType, model.EventModel.PluginEventGroupId);
		return GeneralPropertyDefinitions.GetEnumProperty(EventTypePropertyName(model), "", EventTypeDisplayName(model), EventTypeTranslationId(model), string.Empty, eventTypes);
	}

	private Property GetEmptyEventSourcesProperty(IGeminiRequestContext requestContext, RulePartModel model)
	{
		string[] eventSourceItemTypes = _eventTypeProxy.GetEventSourceItemTypes(requestContext, model.EventModel.EventType);
		return GeneralPropertyDefinitions.GetPathListProperty(EventSourcePropertyName(model), "", EventSourceDisplayName(model), EventSourceTranslationId(model), eventSourceItemTypes);
	}

	private static string EventGroupTypePropertyName(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return "StopEventGroup";
		}
		return "StartEventGroup";
	}

	private static string EventSourcePropertyName(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return "StopEventSources";
		}
		return "StartEventSources";
	}

	private static string EventTypePropertyName(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return "StopEventType";
		}
		return "StartEventType";
	}

	private static string EventGroupTypeDisplayName(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return ConfigurationApiResources.PropertyRuleStopEventGroup;
		}
		return ConfigurationApiResources.PropertyRuleStartEventGroup;
	}

	private static string EventSourceDisplayName(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return ConfigurationApiResources.PropertyRuleStopEventSources;
		}
		return ConfigurationApiResources.PropertyRuleStartEventSources;
	}

	private static string EventTypeDisplayName(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return ConfigurationApiResources.PropertyRuleStopEventType;
		}
		return ConfigurationApiResources.PropertyRuleStartEventType;
	}

	private static string EventSourceTranslationId(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return "PropertyRuleStopEventSources";
		}
		return "PropertyRuleStartEventSources";
	}

	private static string EventTypeTranslationId(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return "PropertyRuleStopEventType";
		}
		return "PropertyRuleStartEventType";
	}

	private static string EventGroupTypeTranslationId(RulePartModel model)
	{
		if (!model.IsStart)
		{
			return "PropertyRuleStopEventGroup";
		}
		return "PropertyRuleStartEventGroup";
	}
}
