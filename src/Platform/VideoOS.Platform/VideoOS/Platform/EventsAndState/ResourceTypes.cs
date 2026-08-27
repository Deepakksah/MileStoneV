using System.Collections.Generic;

namespace VideoOS.Platform.EventsAndState;

public class ResourceTypes : InternalSpecifier<string>
{
	public static ResourceTypes Any { get; } = new ResourceTypes();

	private ResourceTypes()
	{
	}

	public ResourceTypes(IEnumerable<string> ids)
		: base(ids)
	{
	}
}
