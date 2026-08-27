using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public static class ObservableCollectionExtensions
{
	public static void UpdateKeyedItems<T>(this ObservableCollection<T> @this, IEnumerable<T> newItems, Func<T, object> key)
	{
		@this.UpdateKeyedItems(newItems, key, null);
	}

	public static void UpdateKeyedItems<T>(this ObservableCollection<T> @this, IEnumerable<T> newItems, Func<T, object> key, Action<T, T> update)
	{
		if (@this == null)
		{
			throw new ArgumentNullException("this");
		}
		if (newItems == null)
		{
			throw new ArgumentNullException("newItems");
		}
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		Dictionary<object, T> oldDict = @this.ToDictionary((T item) => key(item));
		Dictionary<object, T> newDict = newItems.ToDictionary((T item) => key(item));
		foreach (KeyValuePair<object, T> item in oldDict.Where((KeyValuePair<object, T> item) => !newDict.ContainsKey(item.Key)))
		{
			@this.Remove(item.Value);
		}
		foreach (KeyValuePair<object, T> item2 in newDict.Where((KeyValuePair<object, T> item) => !oldDict.ContainsKey(item.Key)))
		{
			@this.Add(item2.Value);
		}
		foreach (KeyValuePair<object, T> item3 in oldDict.Where((KeyValuePair<object, T> item) => newDict.ContainsKey(item.Key)))
		{
			update?.Invoke(item3.Value, newDict[item3.Key]);
		}
		int num = 0;
		foreach (T newItem in newItems)
		{
			object key2 = key(newItem);
			if (!oldDict.TryGetValue(key2, out var value))
			{
				value = newDict[key2];
			}
			int num2 = @this.IndexOf(value);
			if (num2 != num)
			{
				@this.Move(num2, num);
			}
			num++;
		}
	}
}
