using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.EventsAndState;

public abstract class InternalSpecifier<T>
{
	public IEnumerable<T> Ids { get; }

	public bool IsAny => Ids == null;

	protected InternalSpecifier()
	{
	}

	protected InternalSpecifier(IEnumerable<T> ids)
	{
		if (ids == null || !ids.Any())
		{
			throw new ArgumentException("The collection 'ids' must contain at least one element.", "ids");
		}
		Ids = ids;
	}
}
