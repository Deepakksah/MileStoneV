using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

public static class SelectManyOrderedImplementation
{
	private class ManyOrderedEnumerable<T, TKey> : IEnumerable<T>, IEnumerable
	{
		private readonly IEnumerable<IEnumerable<T>> _orderedEnumerables;

		private readonly IComparer<TKey> _comparer;

		private readonly Func<T, TKey> _keySelector;

		public ManyOrderedEnumerable(IEnumerable<IEnumerable<T>> orderedEnumerables, Func<T, TKey> keySelector, IComparer<TKey> comparer)
		{
			_orderedEnumerables = orderedEnumerables;
			_comparer = comparer;
			_keySelector = keySelector;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new ManyOrderedEnumerator<T, TKey>(_orderedEnumerables, _keySelector, _comparer);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private class ManyOrderedEnumerator<T, TKey> : IEnumerator<T>, IDisposable, IEnumerator
	{
		private readonly IEnumerator<T>[] _enumerators;

		private readonly IComparer<IEnumerator<T>> _comparer;

		private SortedSet<IEnumerator<T>> _enumSet;

		public T Current => _enumSet.Min.Current;

		object IEnumerator.Current => Current;

		public ManyOrderedEnumerator(IEnumerable<IEnumerable<T>> orderedEnumerables, Func<T, TKey> keySelector, IComparer<TKey> comparer)
		{
			_enumerators = orderedEnumerables.Select((IEnumerable<T> e) => e.GetEnumerator()).ToArray();
			_comparer = Comparer<IEnumerator<T>>.Create(delegate(IEnumerator<T> e1, IEnumerator<T> e2)
			{
				int num = comparer.Compare(keySelector(e1.Current), keySelector(e2.Current));
				if (num == 0)
				{
					num = e1.GetHashCode().CompareTo(e2.GetHashCode());
				}
				return num;
			});
		}

		public void Dispose()
		{
			_enumerators.ForEach(delegate(IEnumerator<T> e)
			{
				e.Dispose();
			});
		}

		public bool MoveNext()
		{
			if (_enumSet == null)
			{
				_enumSet = new SortedSet<IEnumerator<T>>(_enumerators.Where((IEnumerator<T> e) => e.MoveNext()), _comparer);
			}
			else
			{
				IEnumerator<T> min = _enumSet.Min;
				_enumSet.Remove(min);
				if (min.MoveNext())
				{
					_enumSet.Add(min);
				}
			}
			return _enumSet.Count > 0;
		}

		public void Reset()
		{
			_enumerators.ForEach(delegate(IEnumerator<T> e)
			{
				e.Reset();
			});
			_enumSet = null;
		}
	}

	public static IEnumerable<TResult> SelectManyOrdered<TSource, TResult>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TResult>> selector, Comparison<TResult> comparison)
	{
		return @this.Select(selector).SelectManyOrdered(comparison);
	}

	public static IEnumerable<TResult> SelectManyOrdered<TSource, TKey, TResult>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TResult>> selector, Func<TResult, TKey> keySelector)
	{
		return @this.Select(selector).SelectManyOrdered(keySelector, Comparer<TKey>.Default);
	}

	public static IEnumerable<TResult> SelectManyOrdered<TSource, TKey, TResult>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TResult>> selector, Func<TResult, TKey> keySelector, Comparison<TKey> comparison)
	{
		return @this.Select(selector).SelectManyOrdered(keySelector, Comparer<TKey>.Create(comparison));
	}

	public static IEnumerable<TResult> SelectManyOrdered<TSource, TResult>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TResult>> selector)
	{
		return @this.Select(selector).SelectManyOrdered();
	}

	public static IEnumerable<T> SelectManyOrdered<T>(this IEnumerable<IEnumerable<T>> @this, Comparison<T> comparison)
	{
		return @this.SelectManyOrdered((T item) => item, Comparer<T>.Create(comparison));
	}

	public static IEnumerable<T> SelectManyOrdered<T>(this IEnumerable<IEnumerable<T>> @this)
	{
		return @this.SelectManyOrdered((T item) => item, Comparer<T>.Default);
	}

	public static IEnumerable<T> SelectManyOrdered<T, TKey>(this IEnumerable<IEnumerable<T>> @this, Func<T, TKey> keySelector)
	{
		return @this.SelectManyOrdered(keySelector, Comparer<TKey>.Default);
	}

	public static IEnumerable<T> SelectManyOrdered<T, TKey>(this IEnumerable<IEnumerable<T>> @this, Func<T, TKey> keySelector, IComparer<TKey> comparer)
	{
		return new ManyOrderedEnumerable<T, TKey>(@this, keySelector, comparer);
	}
}
