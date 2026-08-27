using System.Collections.Generic;
using System.Linq;

namespace System.Collections.ObjectModel;

public static class ObservableCollectionExtensions
{
	public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
	{
		List<T> list = new List<T>(collection);
		list.Sort(comparison);
		for (int i = 0; i < list.Count; i++)
		{
			collection.Move(collection.IndexOf(list[i]), i);
		}
	}

	public static bool SequenceEquals<T>(this ObservableCollection<T> collection, ObservableCollection<T> otherCollection, bool ignoreOrder)
	{
		if (otherCollection == null || collection.Count != otherCollection.Count)
		{
			return false;
		}
		if (ignoreOrder)
		{
			return new HashSet<T>(collection).SetEquals(new HashSet<T>(otherCollection));
		}
		return collection.SequenceEqual(otherCollection);
	}
}
