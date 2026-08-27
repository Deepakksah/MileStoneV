using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.UI.Common.WPF.Utils;

public class ObservableCollectionEx<T> : ObservableCollection<T>, IObservableCollectionEx, IList, ICollection, IEnumerable
{
	private class ComparableComparer : IComparer<T>
	{
		public static bool CanUseIComparable => typeof(IComparable<T>).IsAssignableFrom(typeof(T));

		public int Compare(T x, T y)
		{
			return ((IComparable<T>)(object)x).CompareTo(y);
		}
	}

	private class FunctionComparer : IComparer<T>
	{
		private Func<T, T, int> _comparer;

		public FunctionComparer(Func<T, T, int> comparer)
		{
			_comparer = comparer;
		}

		public int Compare(T x, T y)
		{
			return _comparer(x, y);
		}
	}

	public void Sort(IComparer<T> comparer)
	{
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		for (int i = 0; i < base.Count; i++)
		{
			int num = i;
			for (int j = i + 1; j < base.Count; j++)
			{
				T x = base[num];
				T y = base[j];
				if (comparer.Compare(x, y) > 0)
				{
					num = j;
				}
			}
			if (num > i)
			{
				Move(num, i);
			}
		}
	}

	public void Sort()
	{
		Sort(new ComparableComparer());
	}

	public void Sort(Func<T, T, int> comparer)
	{
		Sort(new FunctionComparer(comparer));
	}

	void IObservableCollectionEx.Move(int oldIndex, int newIndex)
	{
		Move(oldIndex, newIndex);
	}
}
