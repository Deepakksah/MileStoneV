using System;
using System.Collections.Generic;

namespace VideoOS.Platform.EventsAndState;

public class SourceIds : InternalSpecifier<Guid>
{
	public static SourceIds Any { get; } = new SourceIds();

	private SourceIds()
	{
	}

	public SourceIds(IEnumerable<Guid> ids)
		: base(ids)
	{
	}
}
