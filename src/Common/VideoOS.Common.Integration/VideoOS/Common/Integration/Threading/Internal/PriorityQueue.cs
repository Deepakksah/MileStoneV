using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Threading.Internal;

public sealed class PriorityQueue : IEnumerable
{
	private class PriorityQueueEnumerator : IEnumerator
	{
		private readonly PriorityQueue _priorityQueue;

		private int _version;

		private int _queueIndex;

		private IEnumerator _enumerator;

		public object Current => _enumerator.Current;

		public PriorityQueueEnumerator(PriorityQueue priorityQueue)
		{
			_priorityQueue = priorityQueue;
			_version = _priorityQueue._version;
			_queueIndex = _priorityQueue.GetNextNonEmptyQueue(-1);
			if (_queueIndex >= 0)
			{
				_enumerator = _priorityQueue._queues[_queueIndex].GetEnumerator();
			}
			else
			{
				_enumerator = null;
			}
		}

		public void Reset()
		{
			_version = _priorityQueue._version;
			_queueIndex = _priorityQueue.GetNextNonEmptyQueue(-1);
			if (_queueIndex >= 0)
			{
				_enumerator = _priorityQueue._queues[_queueIndex].GetEnumerator();
			}
			else
			{
				_enumerator = null;
			}
		}

		public bool MoveNext()
		{
			if (_enumerator == null)
			{
				return false;
			}
			if (_version != _priorityQueue._version)
			{
				throw new InvalidOperationException("The collection has been modified");
			}
			if (!_enumerator.MoveNext())
			{
				_queueIndex = _priorityQueue.GetNextNonEmptyQueue(_queueIndex);
				if (-1 == _queueIndex)
				{
					return false;
				}
				_enumerator = _priorityQueue._queues[_queueIndex].GetEnumerator();
				_enumerator.MoveNext();
				return true;
			}
			return true;
		}
	}

	private const int _queuesCount = 5;

	private readonly LinkedList<IHasWorkItemPriority>[] _queues = new LinkedList<IHasWorkItemPriority>[5];

	private int _workItemsCount;

	private int _version;

	public int Count => _workItemsCount;

	public PriorityQueue()
	{
		for (int i = 0; i < _queues.Length; i++)
		{
			_queues[i] = new LinkedList<IHasWorkItemPriority>();
		}
	}

	public void Enqueue(IHasWorkItemPriority workItem)
	{
		if (workItem == null)
		{
			throw new ArgumentNullException("workItem");
		}
		int num = (int)(5 - workItem.WorkItemPriority - 1);
		_queues[num].AddLast(workItem);
		_workItemsCount++;
		_version++;
	}

	public IHasWorkItemPriority Dequeue()
	{
		IHasWorkItemPriority result = null;
		if (_workItemsCount > 0)
		{
			int nextNonEmptyQueue = GetNextNonEmptyQueue(-1);
			result = _queues[nextNonEmptyQueue].First.Value;
			_queues[nextNonEmptyQueue].RemoveFirst();
			_workItemsCount--;
			_version++;
		}
		return result;
	}

	private int GetNextNonEmptyQueue(int queueIndex)
	{
		for (int i = queueIndex + 1; i < 5; i++)
		{
			if (_queues[i].Count > 0)
			{
				return i;
			}
		}
		return -1;
	}

	public void Clear()
	{
		if (_workItemsCount > 0)
		{
			LinkedList<IHasWorkItemPriority>[] queues = _queues;
			for (int i = 0; i < queues.Length; i++)
			{
				queues[i].Clear();
			}
			_workItemsCount = 0;
			_version++;
		}
	}

	public IEnumerator GetEnumerator()
	{
		return new PriorityQueueEnumerator(this);
	}
}
