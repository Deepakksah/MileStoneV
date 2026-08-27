using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public class ThreadSafeFrameQueue<T> where T : class
{
	public delegate void OnThrownFrame(T frame);

	private readonly List<T> _listFramesQueue = new List<T>();

	public int FrameQueueSize { get; set; }

	public OnThrownFrame FrameDisposer { get; set; }

	private int AddInternal(T newFrame, Action<T> addInternal, Predicate<T> predict)
	{
		if (newFrame == null || addInternal == null)
		{
			return 0;
		}
		int num = 0;
		lock (_listFramesQueue)
		{
			addInternal(newFrame);
			if (_listFramesQueue.Count > FrameQueueSize)
			{
				num = ((predict != null) ? _listFramesQueue.FindLastIndex(predict) : (_listFramesQueue.Count - FrameQueueSize));
				num = System.Math.Max(num, 0);
			}
			int index = 0;
			for (int i = 0; i < num; i++)
			{
				T frame = _listFramesQueue[index];
				_listFramesQueue.RemoveAt(index);
				FrameDisposer?.Invoke(frame);
			}
			return num;
		}
	}

	public int Add(T newFrame, Predicate<T> predict)
	{
		return AddInternal(newFrame, delegate
		{
			_listFramesQueue.Add(newFrame);
		}, predict);
	}

	public int Insert(T newFrame, Predicate<T> predict)
	{
		return AddInternal(newFrame, delegate(T frame)
		{
			_listFramesQueue.Insert(0, frame);
		}, predict);
	}

	public int Add(int newSize, T newFrame, Predicate<T> predict = null)
	{
		int num = 0;
		lock (_listFramesQueue)
		{
			if (newFrame != null)
			{
				_listFramesQueue.Add(newFrame);
			}
			num = _listFramesQueue.Count - newSize;
			num = System.Math.Max(num, 0);
			if (num > 0 && predict != null && !_listFramesQueue.Any((T item) => predict(item)))
			{
				predict = null;
			}
			bool flag = num > 0;
			int num2 = 0;
			int num3 = 0;
			while (flag && num3 < _listFramesQueue.Count)
			{
				T val = _listFramesQueue[num3];
				if ((predict != null) ? predict(val) : (num2 < num))
				{
					_listFramesQueue.RemoveAt(num3);
					FrameDisposer?.Invoke(val);
					num2++;
				}
				else
				{
					num3++;
					if (num2 >= num)
					{
						flag = false;
					}
				}
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	public T Remove()
	{
		T result = null;
		lock (_listFramesQueue)
		{
			if (_listFramesQueue.Count > 0)
			{
				result = _listFramesQueue[0];
				_listFramesQueue.RemoveAt(0);
			}
		}
		return result;
	}

	public bool Insert(int newSize, T newFrame)
	{
		bool result = false;
		lock (_listFramesQueue)
		{
			if (newFrame != null)
			{
				if (newSize > _listFramesQueue.Count)
				{
					_listFramesQueue.Insert(0, newFrame);
					result = true;
				}
				else if (FrameDisposer != null)
				{
					FrameDisposer(newFrame);
				}
			}
		}
		return result;
	}

	public void Clear()
	{
		Add(0, null);
	}
}
