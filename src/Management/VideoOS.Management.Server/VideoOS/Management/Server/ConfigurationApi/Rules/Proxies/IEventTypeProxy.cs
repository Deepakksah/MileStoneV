using System;
using System.Collections.Generic;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IEventTypeProxy
{
	EventTypeInfo GetEventType(IGeminiRequestContext requestContext, Guid eventTypeId);

	IEnumerable<EventTypeInfo> GetEventTypes(IGeminiRequestContext requestContext, string[] generatorTypes, string idGenerator, Guid eventGroupId);

	EventGroupType GetEventGroupType(IGeminiRequestContext requestContext, Guid eventType);

	EventSourceData[] GetEventSourceData(IGeminiRequestContext requestContext, Guid eventType);

	string[] GetEventSourceItemTypes(IGeminiRequestContext requestContext, Guid eventType);

	Dictionary<string, string> GetEventGroups(IGeminiRequestContext requestContext);

	Guid GetPluginEventTypeIdGenerator(IGeminiRequestContext requestContext, Guid eventGroupId);

	Guid GetPluginEventGroupId(IGeminiRequestContext requestContext, Guid eventType);

	EventGroupType GetHandlerEventGroupType(IGeminiRequestContext requestContext, EventGroupType eventGroupType, Guid eventType);
}
