using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Utilities.Collections;

public class SynchronizedList<TSource, T, TKey> : IEnumerable<T>, IEnumerable where TKey : IComparable
{
	private List<T> _data = new List<T>();

	private Func<T, TKey> _targetKeySelector;

	private Func<TSource, TKey> _sourceKeySelector;

	private Func<TSource, T> _convertFunction;

	private Action<T> _disposeAction;

	private object _syncRoot = new object();

	public SynchronizedList(Func<T, TKey> targetKeySelector, Func<TSource, TKey> sourceKeySelector, Func<TSource, T> convertFunction, Action<T> disposeAction)
	{
		_targetKeySelector = targetKeySelector;
		_sourceKeySelector = sourceKeySelector;
		_convertFunction = convertFunction;
		_disposeAction = disposeAction;
	}

	public SynchronizedList(Func<T, TKey> targetKeySelector, Func<TSource, TKey> sourceKeySelector, Func<TSource, T> convertFunction)
		: this(targetKeySelector, sourceKeySelector, convertFunction, (Action<T>)null)
	{
	}

	public void Update(IEnumerable<TSource> source)
	{
		lock (_syncRoot)
		{
			int num = 0;
			IEnumerator<TSource> enumerator = source.OrderBy(_sourceKeySelector).GetEnumerator();
			bool flag = enumerator.MoveNext();
			while (flag || num < _data.Count)
			{
				if (num == _data.Count)
				{
					_data.Add(_convertFunction(enumerator.Current));
					num++;
					flag = enumerator.MoveNext();
				}
				else if (flag)
				{
					int num2 = _targetKeySelector(_data[num]).CompareTo(_sourceKeySelector(enumerator.Current));
					if (num2 < 0)
					{
						DisposeItem(num);
						_data.RemoveAt(num);
					}
					else if (num2 > 0)
					{
						_data.Insert(num, _convertFunction(enumerator.Current));
					}
					else
					{
						num++;
						flag = enumerator.MoveNext();
					}
				}
				else
				{
					DisposeItem(num);
					_data.RemoveAt(num);
				}
			}
		}
	}

	public void Clear(Action<T> disposeAction)
	{
		lock (_syncRoot)
		{
			foreach (T datum in _data)
			{
				disposeAction(datum);
			}
			_data.Clear();
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		lock (_syncRoot)
		{
			T[] array = new T[_data.Count];
			_data.CopyTo(array, 0);
			return array.AsEnumerable().GetEnumerator();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void DisposeItem(int index)
	{
		if (_disposeAction != null)
		{
			_disposeAction(_data[index]);
		}
	}
}
