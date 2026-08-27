using System;
using System.Collections.Generic;
using VideoOS.Management.Server.BusinessObjects.EventTypes;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class BaseEventTypeFilter
{
	protected string _idGenerator;

	public IEnumerable<string> GeneratorTypes { get; }

	public BaseEventTypeFilter(IEnumerable<string> generatorTypes)
	{
		GeneratorTypes = generatorTypes;
	}

	public virtual bool Filter(EventTypeInfo eventTypeInfo)
	{
		return true;
	}

	public virtual string GetIdGenerator(IGeminiRequestContext requestContext, Guid eventGroupId)
	{
		return _idGenerator;
	}
}
