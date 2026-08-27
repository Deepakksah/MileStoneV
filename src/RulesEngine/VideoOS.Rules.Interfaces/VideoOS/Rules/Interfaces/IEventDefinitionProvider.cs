using System;
using VideoOS.Management.Common;

namespace VideoOS.Rules.Interfaces;

public interface IEventDefinitionProvider
{
	EventDefinition FindEventDefinitionFromEventTypeId(Guid eventTypeId);
}
