using System;

namespace VideoOS.Platform.UI.Automation.Serialization;

public abstract class TypeFilterBase
{
	private readonly Func<Type, bool> _filter;

	protected TypeFilterBase(Func<Type, bool> filter)
	{
		_filter = filter;
	}

	public bool Matches(Type type)
	{
		return _filter?.Invoke(type) ?? true;
	}
}
