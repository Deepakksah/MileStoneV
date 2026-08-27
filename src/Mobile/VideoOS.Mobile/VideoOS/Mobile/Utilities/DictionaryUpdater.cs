using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public static class DictionaryUpdater
{
	public class AddOption
	{
		public static readonly AddOption Add = new AddOption();

		public static readonly AddOption DoNothing = new AddOption();

		private AddOption()
		{
		}
	}

	public class DeleteOption
	{
		public static readonly DeleteOption Delete = new DeleteOption();

		public static readonly DeleteOption DoNothing = new DeleteOption();

		private DeleteOption()
		{
		}
	}

	public class UpdateOption
	{
		public static readonly UpdateOption UpdateItem = new UpdateOption();

		public static readonly UpdateOption DoNothing = new UpdateOption();

		private UpdateOption()
		{
		}
	}

	public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> target, IDictionary<TKey, TValue> source, Func<TKey, TValue, AddOption> addChecker, Func<TKey, TValue, DeleteOption> deleteChecker, Func<TValue, TValue, UpdateOption> updateChecker)
	{
		Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(source);
		foreach (KeyValuePair<TKey, TValue> item in new Dictionary<TKey, TValue>(target))
		{
			if (dictionary.ContainsKey(item.Key))
			{
				if (CallChecker(updateChecker, target[item.Key], source[item.Key], UpdateOption.UpdateItem))
				{
					target[item.Key] = source[item.Key];
				}
			}
			else if (CallChecker(deleteChecker, item.Key, item.Value, DeleteOption.Delete))
			{
				target.Remove(item.Key);
			}
			dictionary.Remove(item.Key);
		}
		foreach (KeyValuePair<TKey, TValue> item2 in dictionary)
		{
			if (CallChecker(addChecker, item2.Key, item2.Value, AddOption.Add))
			{
				target.Add(item2.Key, item2.Value);
			}
		}
	}

	public static bool CallChecker<T1, T2, TResult>(Func<T1, T2, TResult> checker, T1 param1, T2 param2, TResult expectedResult) where TResult : class
	{
		bool result = false;
		try
		{
			result = checker == null || expectedResult == checker(param1, param2);
		}
		catch (Exception ex)
		{
			Log.Error("", ex.Message);
		}
		return result;
	}

	public static bool IsTheSameWith<TKey, TValue>(this Dictionary<TKey, TValue> target, IDictionary<TKey, TValue> source) where TValue : IComparable
	{
		if (target.Count != source.Count)
		{
			return false;
		}
		foreach (KeyValuePair<TKey, TValue> item in source)
		{
			if (!target.ContainsKey(item.Key))
			{
				return false;
			}
			if (item.Value.CompareTo(target[item.Key]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsTheSameWithLinq<TKey, TValue>(this Dictionary<TKey, TValue> target, IDictionary<TKey, TValue> source) where TValue : IComparable
	{
		IEnumerable<KeyValuePair<TKey, TValue>> source2 = source.Where(delegate(KeyValuePair<TKey, TValue> sourceKvp)
		{
			Dictionary<TKey, TValue> dictionary = target;
			KeyValuePair<TKey, TValue> keyValuePair = sourceKvp;
			if (dictionary.ContainsKey(keyValuePair.Key))
			{
				keyValuePair = sourceKvp;
				TValue value = keyValuePair.Value;
				Dictionary<TKey, TValue> dictionary2 = target;
				keyValuePair = sourceKvp;
				return value.CompareTo(dictionary2[keyValuePair.Key]) == 0;
			}
			return false;
		});
		if (source.Count == source2.Count())
		{
			return target.Count == source2.Count();
		}
		return false;
	}
}
