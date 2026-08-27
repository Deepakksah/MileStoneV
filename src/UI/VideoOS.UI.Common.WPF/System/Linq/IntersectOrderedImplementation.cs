using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

public static class IntersectOrderedImplementation
{
	private class IntersectOrderedEnumerable<T, TKey> : IEnumerable<T>, IEnumerable
	{
		private readonly IEnumerable<T> _firstEnumerable;

		private readonly IEnumerable<T> _secondEnumerable;

		private readonly IComparer<TKey> _comparer;

		private readonly Func<T, TKey> _keySelector;

		public IntersectOrderedEnumerable(IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable, Func<T, TKey> keySelector, IComparer<TKey> comparer)
		{
			_firstEnumerable = firstEnumerable;
			_secondEnumerable = secondEnumerable;
			_comparer = comparer;
			_keySelector = keySelector;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new IntersectOrderedEnumerator<T, TKey>(_firstEnumerable, _secondEnumerable, _keySelector, _comparer);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private class IntersectOrderedEnumerator<T, TKey> : IEnumerator<T>, IDisposable, IEnumerator
	{
		private readonly IEnumerator<T> _firstEnumerator;

		private readonly IEnumerator<T> _secondEnumerator;

		private readonly IComparer<TKey> _comparer;

		private readonly Func<T, TKey> _keySelector;

		private T _current;

		public T Current => _current;

		object IEnumerator.Current => Current;

		public IntersectOrderedEnumerator(IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable, Func<T, TKey> keySelector, IComparer<TKey> comparer)
		{
			_firstEnumerator = firstEnumerable.GetEnumerator();
			_secondEnumerator = secondEnumerable.GetEnumerator();
			_comparer = comparer;
			_keySelector = keySelector;
		}

		public void Dispose()
		{
			_firstEnumerator.Dispose();
			_secondEnumerator.Dispose();
		}

		public bool MoveNext()
		{
			bool? flag = null;
			bool flag2 = _firstEnumerator.MoveNext() && _secondEnumerator.MoveNext();
			while (flag2 && !flag.HasValue)
			{
				switch (Math.Sign(_comparer.Compare(_keySelector(_firstEnumerator.Current), _keySelector(_secondEnumerator.Current))))
				{
				case -1:
					flag2 = _firstEnumerator.MoveNext();
					break;
				case 1:
					flag2 = _secondEnumerator.MoveNext();
					break;
				case 0:
					_current = _firstEnumerator.Current;
					flag = true;
					break;
				}
			}
			if (flag.HasValue)
			{
				return flag.Value;
			}
			return false;
		}

		public void Reset()
		{
			_firstEnumerator.Reset();
			_secondEnumerator.Reset();
			_current = default(T);
		}
	}

	public static IEnumerable<T> IntersectOrdered<T>(this IEnumerable<T> @this, IEnumerable<T> other)
	{
		return @this.IntersectOrdered(other, (T item) => item, Comparer<T>.Default);
	}

	public static IEnumerable<T> IntersectOrdered<T>(this IEnumerable<T> @this, IEnumerable<T> other, Comparison<T> comparison)
	{
		return @this.IntersectOrdered(other, (T item) => item, Comparer<T>.Create(comparison));
	}

	public static IEnumerable<T> IntersectOrdered<T, TKey>(this IEnumerable<T> @this, IEnumerable<T> other, Func<T, TKey> keySelector)
	{
		return new IntersectOrderedEnumerable<T, TKey>(@this, other, keySelector, Comparer<TKey>.Default);
	}

	public static IEnumerable<T> IntersectOrdered<T, TKey>(this IEnumerable<T> @this, IEnumerable<T> other, Func<T, TKey> keySelector, IComparer<TKey> comparer)
	{
		return new IntersectOrderedEnumerable<T, TKey>(@this, other, keySelector, comparer);
	}
}
