using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq;

public static class IEnumerableExtension
{
	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		foreach (T item in enumerable)
		{
			action(item);
		}
	}

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, bool> action)
	{
		int num = 0;
		foreach (T item in enumerable)
		{
			action(item, num++ == 0);
		}
	}

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
	{
		int num = 0;
		foreach (T item in enumerable)
		{
			action(item, num++);
		}
	}

	public static Task<T[]> ToArrayAsync<T>(this IEnumerable<T> @this)
	{
		return Task.Factory.StartNew(() => @this.ToArray());
	}

	public static Task<T[]> ToArrayAsync<T>(this IEnumerable<T> @this, CancellationToken cancellationToken)
	{
		return Task.Factory.StartNew(() => @this.ToArray(), cancellationToken);
	}

	public static IEnumerable<T> DistinctOrdered<T>(this IEnumerable<T> @this)
	{
		return @this.DistinctOrdered(Comparer<T>.Default);
	}

	public static IEnumerable<T> DistinctOrdered<T>(this IEnumerable<T> @this, Comparison<T> comparison)
	{
		return @this.DistinctOrdered(Comparer<T>.Create(comparison));
	}

	public static IEnumerable<T> DistinctOrdered<T>(this IEnumerable<T> @this, IComparer<T> comparer)
	{
		IEnumerator<T> enumerator = @this.GetEnumerator();
		try
		{
			if (!enumerator.MoveNext())
			{
				yield break;
			}
			yield return enumerator.Current;
			T current = enumerator.Current;
			while (enumerator.MoveNext())
			{
				if (comparer.Compare(current, enumerator.Current) != 0)
				{
					yield return enumerator.Current;
					current = enumerator.Current;
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public static bool SequenceEqualIgnoreOrder<T>(this IEnumerable<T> enumeration, IEnumerable<T> otherEnumeration)
	{
		List<T> list = enumeration.ToList();
		List<T> list2 = otherEnumeration?.ToList();
		if (list2 == null || list.Count != list2.Count)
		{
			return false;
		}
		return new HashSet<T>(list).SetEquals(new HashSet<T>(list2));
	}

	public static bool IsSingleItem<T>(this IEnumerable<T> e)
	{
		if (e.Any())
		{
			return !e.Skip(1).Any();
		}
		return false;
	}
}
