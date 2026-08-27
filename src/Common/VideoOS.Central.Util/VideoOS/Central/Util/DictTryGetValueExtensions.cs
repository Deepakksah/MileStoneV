using System.Collections.Generic;

namespace VideoOS.Central.Util;

public static class DictTryGetValueExtensions
{
	public static bool TryGetValue<TKey, TValue>(this IEnumerable<IDictTryGetValue<TKey, TValue>> providers, TKey key, out TValue value)
	{
		bool flag = false;
		value = default(TValue);
		foreach (IDictTryGetValue<TKey, TValue> provider in providers)
		{
			flag = provider.TryGetValue(key, out value);
			if (flag)
			{
				break;
			}
		}
		return flag;
	}
}
