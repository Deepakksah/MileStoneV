using System.Collections;

namespace VideoOS.Common.SharedComponent.Data;

public class PropertySortedList : SortedList
{
	public override void Add(object key, object value)
	{
		if (ContainsKey(key))
		{
			Remove(key);
		}
		base.Add(key, value);
	}
}
