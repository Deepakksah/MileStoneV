using System;
using System.Collections.Generic;

namespace VideoOS.Rules.Interfaces;

public interface IRuleTrigger
{
	Guid EventTypeId { get; }

	List<Guid> GetSources();

	List<Guid> GetSourceTargetGroupIds();
}
