using System;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Definition;

public class StreamCombinationsCollection : KeyedCollection<Guid, StreamCombinations>
{
	public bool Supported => base.Count > 0;

	protected override Guid GetKeyForItem(StreamCombinations item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.StreamTypeId;
	}
}
