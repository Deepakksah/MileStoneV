using System.Collections.Generic;

namespace System.Collections.ObjectModel;

public static class UpdateObservableCollection
{
	public static void Update<T>(this ObservableCollection<T> @this, IEnumerable<T> source, IComparer<T> comparer, int startPosition, ref int count)
	{
		using IEnumerator<T> enumerator = source.GetEnumerator();
		int num = 0;
		bool flag = !enumerator.MoveNext();
		while (!flag)
		{
			T current = enumerator.Current;
			if (num >= count)
			{
				@this.Insert(startPosition + num, current);
				flag = !enumerator.MoveNext();
				num++;
				count++;
				continue;
			}
			int num2 = comparer.Compare(@this[startPosition + num], current);
			if (num2 == 0)
			{
				num++;
				flag = !enumerator.MoveNext();
				continue;
			}
			if (num2 < 0)
			{
				@this.RemoveAt(startPosition + num);
				count--;
				continue;
			}
			@this.Insert(startPosition + num, current);
			flag = !enumerator.MoveNext();
			num++;
			count++;
		}
		while (num < count)
		{
			@this.RemoveAt(startPosition + num);
			count--;
		}
	}

	public static void Update<T>(this ObservableCollection<T> @this, IEnumerable<T> source, int startPosition, ref int count)
	{
		@this.Update(source, Comparer<T>.Default, startPosition, ref count);
	}

	public static void Update<T>(this ObservableCollection<T> @this, IEnumerable<T> source)
	{
		int count = @this.Count;
		@this.Update(source, Comparer<T>.Default, 0, ref count);
	}

	public static void Update<T>(this ObservableCollection<T> @this, IEnumerable<T> source, IComparer<T> comparer)
	{
		int count = @this.Count;
		@this.Update(source, comparer, 0, ref count);
	}
}
