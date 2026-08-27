using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration;

public class MemoryMessagePersister<T> : IMessagePersister<T>
{
	private List<T> _persistedMessages = new List<T>();

	public bool HasMessages
	{
		get
		{
			lock (_persistedMessages)
			{
				return _persistedMessages.Count > 0;
			}
		}
	}

	public void Open()
	{
	}

	public void Close()
	{
		_persistedMessages.Clear();
	}

	public void PersistMessages(T[] messages)
	{
		lock (_persistedMessages)
		{
			_persistedMessages.AddRange(messages);
		}
	}

	public T[] GetRecentMessages(int maxCount)
	{
		lock (_persistedMessages)
		{
			int num = Math.Min(maxCount, _persistedMessages.Count);
			T[] result = _persistedMessages.GetRange(_persistedMessages.Count - num, num).ToArray();
			_persistedMessages.RemoveRange(_persistedMessages.Count - num, num);
			return result;
		}
	}
}
