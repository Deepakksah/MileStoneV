using System;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class PropertyTypeFilter : TypeFilterBase
{
	public PropertyTypeFilter(Func<Type, bool> filter)
		: base(filter)
	{
	}
}
