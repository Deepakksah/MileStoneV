using System.Collections.Generic;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class AccessControlEventTypeFilter : BaseEventTypeFilter
{
	private EventGroupType _eventGroupType;

	public AccessControlEventTypeFilter(IEnumerable<string> generatorTypes, EventGroupType eventGroupType)
		: base(generatorTypes)
	{
		_eventGroupType = eventGroupType;
		_idGenerator = SourceGuid.AccessControl.ToString();
	}

	public override bool Filter(EventTypeInfo eventTypeInfo)
	{
		return EventTypeProxy.GetMipEventGroupTypeFromDefinition(eventTypeInfo.Definition) == _eventGroupType;
	}
}
