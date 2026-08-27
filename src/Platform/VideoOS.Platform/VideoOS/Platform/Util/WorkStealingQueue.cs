using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Platform.Util;

public class WorkStealingQueue<T> where T : class
{
	private const int INITIAL_SIZE = 32;

	private T[] m_array = new T[32];

	private int m_mask = 31;

	private volatile int m_headIndex;

	private volatile int m_tailIndex;

	private object m_foreignLock = new object();

	public void LocalPush(T obj)
	{
		int num = m_tailIndex;
		if (num < m_headIndex + m_mask)
		{
			m_array[num & m_mask] = obj;
			m_tailIndex = num + 1;
			return;
		}
		lock (m_foreignLock)
		{
			int headIndex = m_headIndex;
			int num2 = m_tailIndex - m_headIndex;
			if (num2 >= m_mask)
			{
				T[] array = new T[m_array.Length << 1];
				for (int i = 0; i < m_array.Length; i++)
				{
					array[i] = m_array[(i + headIndex) & m_mask];
				}
				m_array = array;
				m_headIndex = 0;
				num = (m_tailIndex = num2);
				m_mask = (m_mask << 1) | 1;
			}
			m_array[num & m_mask] = obj;
			m_tailIndex = num + 1;
		}
	}

	public bool LocalPop(ref T obj)
	{
		int num;
		while (true)
		{
			int tailIndex = m_tailIndex;
			if (m_headIndex >= tailIndex)
			{
				obj = null;
				return false;
			}
			tailIndex--;
			Interlocked.Exchange(ref m_tailIndex, tailIndex);
			if (m_headIndex <= tailIndex)
			{
				num = tailIndex & m_mask;
				obj = m_array[num];
				if (obj != null)
				{
					break;
				}
				continue;
			}
			lock (m_foreignLock)
			{
				if (m_headIndex <= tailIndex)
				{
					int num2 = tailIndex & m_mask;
					obj = m_array[num2];
					if (obj != null)
					{
						m_array[num2] = null;
						return true;
					}
					continue;
				}
				m_tailIndex = tailIndex + 1;
				obj = null;
				return false;
			}
		}
		m_array[num] = null;
		return true;
	}

	public bool TrySteal(ref T obj)
	{
		obj = null;
		while (true)
		{
			if (m_headIndex >= m_tailIndex)
			{
				return false;
			}
			lock (m_foreignLock)
			{
				int headIndex = m_headIndex;
				Interlocked.Exchange(ref m_headIndex, headIndex + 1);
				if (headIndex < m_tailIndex)
				{
					int num = headIndex & m_mask;
					obj = m_array[num];
					if (obj == null)
					{
						continue;
					}
					m_array[num] = null;
					return true;
				}
				m_headIndex = headIndex;
				obj = null;
			}
			break;
		}
		return false;
	}

	public bool TryFindAndPop(T obj)
	{
		for (int num = m_tailIndex - 1; num >= m_headIndex; num--)
		{
			if (m_array[num & m_mask] == obj)
			{
				lock (m_foreignLock)
				{
					if (m_array[num & m_mask] == null)
					{
						return false;
					}
					m_array[num & m_mask] = null;
					if (num == m_tailIndex)
					{
						m_tailIndex--;
					}
					else if (num == m_headIndex)
					{
						m_headIndex++;
					}
					return true;
				}
			}
		}
		return false;
	}

	public T[] ToArray()
	{
		List<T> list = new List<T>();
		for (int num = m_tailIndex - 1; num >= m_headIndex; num--)
		{
			T val = m_array[num & m_mask];
			if (val != null)
			{
				list.Add(val);
			}
		}
		return list.ToArray();
	}
}
