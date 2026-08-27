using System;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class CollectionItemFilter : TypeFilterBase
{
	private readonly Func<Type, bool> _itemFilter;

	public CollectionItemFilter(Func<Type, bool> filter, Func<Type, bool> itemFilter)
		: base(filter)
	{
		_itemFilter = itemFilter;
	}

	public bool MatchesItem(Type itemType)
	{
		return _itemFilter?.Invoke(itemType) ?? true;
	}
}
