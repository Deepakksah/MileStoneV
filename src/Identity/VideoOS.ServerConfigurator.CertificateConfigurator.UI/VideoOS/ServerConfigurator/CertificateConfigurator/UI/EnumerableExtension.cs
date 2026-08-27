using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public static class EnumerableExtension
{
	private class ConcatDistinctHelper<T, TKey> : IEnumerable<T>, IEnumerable
	{
		private Dictionary<TKey, T> _original;

		private Dictionary<TKey, T> _additional;

		public ConcatDistinctHelper(IEnumerable<T> original, IEnumerable<T> additional, Func<T, TKey> key)
		{
			_original = original.ToDictionary(key);
			_additional = additional.ToDictionary(key);
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (TKey item in _original.Keys.Concat(_additional.Keys).Distinct())
			{
				if (_original.ContainsKey(item))
				{
					yield return _original[item];
				}
				else
				{
					yield return _additional[item];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public static IEnumerable<T> ConcatDistinct<T, TKey>(this IEnumerable<T> @this, IEnumerable<T> additional, Func<T, TKey> key)
	{
		return new ConcatDistinctHelper<T, TKey>(@this, additional, key);
	}
}
