using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.UI.Controls;

public class CollectionChangedInfo<TItem> : ICollectionChangedInfo
{
	public CollectionChange Change { get; }

	public ICollection<TItem> RemovedItems { get; }

	public ICollection<TItem> AddedItems { get; }

	public CollectionChangedInfo(CollectionChange change, IEnumerable<TItem> removedItems, IEnumerable<TItem> addedItems)
	{
		Change = change;
		RemovedItems = removedItems?.ToList() ?? Enumerable.Empty<TItem>().ToList();
		AddedItems = addedItems?.ToList() ?? Enumerable.Empty<TItem>().ToList();
	}
}
public class CollectionChangedInfo : ICollectionChangedInfo
{
	public CollectionChange Change { get; }

	public ICollection RemovedItems { get; }

	public ICollection AddedItems { get; }

	public CollectionChangedInfo(CollectionChange change, IEnumerable removedItems = null, IEnumerable addedItems = null)
	{
		Change = change;
		RemovedItems = removedItems?.OfType<object>().ToList() ?? Enumerable.Empty<object>().ToList();
		AddedItems = addedItems?.OfType<object>().ToList() ?? Enumerable.Empty<object>().ToList();
	}
}
