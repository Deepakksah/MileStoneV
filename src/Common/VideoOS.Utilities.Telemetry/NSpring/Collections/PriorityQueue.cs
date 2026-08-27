using System;
using System.Collections;
using System.Threading;

namespace NSpring.Collections;

public sealed class PriorityQueue : Queue
{
	private sealed class WaitingThreadNode
	{
		public object returnValue;

		public bool valueReturned;

		public WaitingThreadNode next;

		public WaitingThreadNode previous;
	}

	private sealed class Sublist : IComparable
	{
		private const int INITIAL_CAPACITY = 50;

		public int priority;

		public int count;

		public object[] elements = new object[50];

		public int capacity = 50;

		public int startIndex;

		public int endIndex;

		public void Clear()
		{
			if (count > 0)
			{
				Array.Clear(elements, 0, capacity);
				count = 0;
				startIndex = 0;
				endIndex = 0;
			}
		}

		public void AddLast(object _object)
		{
			if (count == capacity)
			{
				if (capacity == 50)
				{
					capacity *= 5;
				}
				else
				{
					capacity *= 2;
				}
				object[] destinationArray = new object[capacity];
				Array.Copy(elements, startIndex, destinationArray, 0, elements.Length - startIndex);
				if (startIndex > 0)
				{
					Array.Copy(elements, 0, destinationArray, elements.Length - startIndex, startIndex);
				}
				elements = destinationArray;
				startIndex = 0;
				endIndex = count - 1;
			}
			if (count > 0)
			{
				endIndex++;
				if (endIndex == capacity)
				{
					endIndex = 0;
				}
			}
			elements[endIndex] = _object;
			count++;
		}

		public object RemoveFirst()
		{
			object result = elements[startIndex++];
			if (startIndex == capacity)
			{
				startIndex = 0;
			}
			count--;
			return result;
		}

		public int CompareTo(object _object)
		{
			return priority - ((Sublist)_object).priority;
		}
	}

	public const int High = 20;

	public const int MediumHigh = 15;

	public const int Medium = 10;

	public const int MediumLow = 5;

	public const int Low = 0;

	private int peekWaitCount;

	private WaitingThreadNode peekWaitHeader;

	private int removeWaitCount;

	private WaitingThreadNode removeWaitHeader;

	private int waitNodeCacheCount;

	private WaitingThreadNode waitNodeCacheHeader;

	private Sublist[] sublistArray = new Sublist[51];

	private Hashtable sublistMap = new Hashtable(1000, 0.5f);

	private int sublistCount;

	private Sublist highestPrioritySublist;

	private int defaultPriority;

	private Sublist defaultPrioritySublist;

	public int DefaultPriority
	{
		get
		{
			lock (monitor)
			{
				return defaultPriority;
			}
		}
		set
		{
			lock (monitor)
			{
				if (value < 0)
				{
					throw new ArgumentException("Negative priorities are not allowed");
				}
				defaultPriority = value;
				if (defaultPriority > 20)
				{
					defaultPrioritySublist = GetSublist(defaultPriority);
				}
				else
				{
					defaultPrioritySublist = sublistArray[defaultPriority];
				}
			}
		}
	}

	public PriorityQueue()
	{
		peekWaitHeader = new WaitingThreadNode();
		peekWaitHeader.next = peekWaitHeader;
		peekWaitHeader.previous = peekWaitHeader;
		removeWaitHeader = new WaitingThreadNode();
		removeWaitHeader.next = removeWaitHeader;
		removeWaitHeader.previous = removeWaitHeader;
		waitNodeCacheHeader = new WaitingThreadNode();
		for (int i = 0; i <= 20; i++)
		{
			Sublist sublist = new Sublist
			{
				priority = i
			};
			sublistArray[i] = sublist;
			sublistMap.Add(i, sublist);
		}
		sublistCount = 21;
		highestPrioritySublist = null;
		defaultPrioritySublist = sublistArray[0];
	}

	private Sublist GetSublist(int _priority)
	{
		Sublist sublist = (Sublist)sublistMap[_priority];
		if (sublist == null)
		{
			sublist = new Sublist();
			sublist.priority = _priority;
			sublistMap.Add(_priority, sublist);
			if (sublistCount == sublistArray.Length)
			{
				Sublist[] destinationArray = new Sublist[sublistArray.Length * 2];
				Array.Copy(sublistArray, 0, destinationArray, 0, sublistArray.Length);
				sublistArray = destinationArray;
			}
			sublistArray[sublistCount++] = sublist;
			Array.Sort(sublistArray, 0, sublistCount);
		}
		return sublist;
	}

	public override void Clear()
	{
		lock (monitor)
		{
			if (count <= 0)
			{
				return;
			}
			for (int i = 0; i < sublistCount; i++)
			{
				Sublist sublist = sublistArray[i];
				if (sublist.count > 0)
				{
					count -= sublist.count;
					sublist.Clear();
					if (count == 0)
					{
						break;
					}
				}
			}
		}
	}

	public override void Add(object _object)
	{
		lock (monitor)
		{
			if (!isOpen)
			{
				throw new InvalidOperationException("This instance is closed, and cannot accept input");
			}
			if (!isNullAllowed && _object == null)
			{
				throw new ArgumentNullException("This instance does not allow null input");
			}
			if (peekWaitCount > 0)
			{
				lock (peekWaitHeader)
				{
					if (peekWaitCount > 0)
					{
						WaitingThreadNode next = peekWaitHeader.next;
						while (next != peekWaitHeader)
						{
							next.valueReturned = true;
							next.returnValue = _object;
							next.previous = null;
							next = next.next;
							next.previous.next = null;
						}
						peekWaitCount = 0;
						peekWaitHeader.next = null;
						Monitor.PulseAll(peekWaitHeader);
					}
				}
			}
			if (removeWaitCount > 0)
			{
				lock (removeWaitHeader)
				{
					if (removeWaitCount > 0)
					{
						WaitingThreadNode next2 = removeWaitHeader.next;
						removeWaitHeader.next = next2.next;
						removeWaitHeader.next.previous = removeWaitHeader;
						next2.next = null;
						next2.previous = null;
						removeWaitCount--;
						lock (next2)
						{
							next2.valueReturned = true;
							next2.returnValue = _object;
							Monitor.Pulse(next2);
							return;
						}
					}
					return;
				}
			}
			defaultPrioritySublist.AddLast(_object);
			if (highestPrioritySublist == null || highestPrioritySublist.priority < defaultPriority)
			{
				highestPrioritySublist = defaultPrioritySublist;
			}
			count++;
		}
	}

	public void Add(object _object, int _priority)
	{
		if (_priority < 0)
		{
			throw new ArgumentException("Negative priorities are not allowed");
		}
		lock (monitor)
		{
			if (!isOpen)
			{
				throw new InvalidOperationException("This instance is closed, and cannot accept input");
			}
			if (!isNullAllowed && _object == null)
			{
				throw new ArgumentNullException("This instance does not allow null input");
			}
			if (peekWaitCount > 0)
			{
				lock (peekWaitHeader)
				{
					if (peekWaitCount > 0)
					{
						WaitingThreadNode next = peekWaitHeader.next;
						while (next != peekWaitHeader)
						{
							next.valueReturned = true;
							next.returnValue = _object;
							next.previous = null;
							next = next.next;
							next.previous.next = null;
						}
						peekWaitCount = 0;
						peekWaitHeader.next = null;
						Monitor.PulseAll(peekWaitHeader);
					}
				}
			}
			if (removeWaitCount > 0)
			{
				lock (removeWaitHeader)
				{
					if (removeWaitCount > 0)
					{
						WaitingThreadNode next = removeWaitHeader.next;
						removeWaitHeader.next = next.next;
						removeWaitHeader.next.previous = removeWaitHeader;
						next.next = null;
						next.previous = null;
						removeWaitCount--;
						lock (next)
						{
							next.valueReturned = true;
							next.returnValue = _object;
							Monitor.Pulse(next);
							return;
						}
					}
					return;
				}
			}
			Sublist sublist = ((_priority > 20) ? GetSublist(_priority) : sublistArray[_priority]);
			sublist.AddLast(_object);
			if (highestPrioritySublist == null || highestPrioritySublist.priority < _priority)
			{
				highestPrioritySublist = sublist;
			}
			count++;
		}
	}

	public override object Peek()
	{
		lock (monitor)
		{
			if (count > 0)
			{
				if (highestPrioritySublist != null)
				{
					return highestPrioritySublist.elements[highestPrioritySublist.startIndex];
				}
				for (int num = sublistCount - 1; num > 0; num--)
				{
					if (sublistArray[num].count > 0)
					{
						highestPrioritySublist = sublistArray[num];
						return highestPrioritySublist.elements[highestPrioritySublist.startIndex];
					}
				}
			}
		}
		return null;
	}

	public override object Peek(int _millisecondsTimeout)
	{
		lock (monitor)
		{
			if (count > 0)
			{
				if (highestPrioritySublist != null)
				{
					return highestPrioritySublist.elements[highestPrioritySublist.startIndex];
				}
				for (int num = sublistCount - 1; num > 0; num--)
				{
					if (sublistArray[num].count > 0)
					{
						highestPrioritySublist = sublistArray[num];
						return highestPrioritySublist.elements[highestPrioritySublist.startIndex];
					}
				}
			}
			if (_millisecondsTimeout <= 0 || !isOpen)
			{
				return null;
			}
		}
		object result = null;
		lock (peekWaitHeader)
		{
			peekWaitCount++;
			WaitingThreadNode waitingThreadNode;
			if (waitNodeCacheCount > 0)
			{
				waitingThreadNode = waitNodeCacheHeader.next;
				waitNodeCacheHeader.next = waitingThreadNode.next;
				waitNodeCacheCount--;
			}
			else
			{
				waitingThreadNode = new WaitingThreadNode();
			}
			peekWaitHeader.previous.next = waitingThreadNode;
			waitingThreadNode.previous = peekWaitHeader.previous;
			peekWaitHeader.previous = waitingThreadNode;
			waitingThreadNode.next = peekWaitHeader;
			try
			{
				Monitor.Wait(peekWaitHeader, _millisecondsTimeout);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			if (waitingThreadNode.valueReturned)
			{
				result = waitingThreadNode.returnValue;
				waitingThreadNode.returnValue = null;
				waitingThreadNode.valueReturned = false;
			}
			else
			{
				waitingThreadNode.previous.next = waitingThreadNode.next;
				waitingThreadNode.next.previous = waitingThreadNode.previous;
				waitingThreadNode.previous = null;
				peekWaitCount--;
			}
			waitingThreadNode.next = waitNodeCacheHeader.next;
			waitNodeCacheHeader.next = waitingThreadNode;
			waitNodeCacheCount++;
			return result;
		}
	}

	public override object Remove()
	{
		lock (monitor)
		{
			if (count > 0)
			{
				if (highestPrioritySublist != null)
				{
					object result = highestPrioritySublist.RemoveFirst();
					if (highestPrioritySublist.count == 0)
					{
						highestPrioritySublist = null;
					}
					count--;
					return result;
				}
				for (int num = sublistCount - 1; num >= 0; num--)
				{
					Sublist sublist = sublistArray[num];
					if (sublist.count > 0)
					{
						object result2 = sublist.RemoveFirst();
						if (sublist.count > 0)
						{
							highestPrioritySublist = sublist;
						}
						count--;
						return result2;
					}
				}
			}
		}
		return null;
	}

	public override object Remove(int _millisecondsTimeout)
	{
		object result = null;
		lock (monitor)
		{
			if (count > 0)
			{
				if (highestPrioritySublist != null)
				{
					result = highestPrioritySublist.RemoveFirst();
					if (highestPrioritySublist.count == 0)
					{
						highestPrioritySublist = null;
					}
					count--;
					return result;
				}
				for (int num = sublistCount - 1; num >= 0; num--)
				{
					Sublist sublist = sublistArray[num];
					if (sublist.count > 0)
					{
						result = sublist.RemoveFirst();
						if (sublist.count > 0)
						{
							highestPrioritySublist = sublist;
						}
						count--;
						return result;
					}
				}
			}
			if (_millisecondsTimeout <= 0 || !isOpen)
			{
				return null;
			}
		}
		WaitingThreadNode waitingThreadNode;
		lock (removeWaitHeader)
		{
			peekWaitCount++;
			if (waitNodeCacheCount > 0)
			{
				waitingThreadNode = waitNodeCacheHeader.next;
				waitNodeCacheHeader.next = waitingThreadNode.next;
				waitNodeCacheCount--;
			}
			else
			{
				waitingThreadNode = new WaitingThreadNode();
			}
			removeWaitHeader.previous.next = waitingThreadNode;
			waitingThreadNode.previous = removeWaitHeader.previous;
			removeWaitHeader.previous = waitingThreadNode;
			waitingThreadNode.next = removeWaitHeader;
		}
		lock (waitingThreadNode)
		{
			if (!waitingThreadNode.valueReturned)
			{
				try
				{
					Monitor.Wait(waitingThreadNode, _millisecondsTimeout);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
		}
		lock (removeWaitHeader)
		{
			if (waitingThreadNode.valueReturned)
			{
				result = waitingThreadNode.returnValue;
				waitingThreadNode.returnValue = null;
				waitingThreadNode.valueReturned = false;
			}
			else
			{
				waitingThreadNode.previous.next = waitingThreadNode.next;
				waitingThreadNode.next.previous = waitingThreadNode.previous;
				waitingThreadNode.previous = null;
				peekWaitCount--;
			}
			waitingThreadNode.next = waitNodeCacheHeader.next;
			waitNodeCacheHeader.next = waitingThreadNode;
			waitNodeCacheCount++;
			return result;
		}
	}

	public override void Close()
	{
		lock (monitor)
		{
			if (!isOpen)
			{
				return;
			}
			if (peekWaitCount > 0)
			{
				lock (peekWaitHeader)
				{
					if (peekWaitCount > 0)
					{
						WaitingThreadNode next = peekWaitHeader.next;
						while (next != peekWaitHeader)
						{
							next.valueReturned = true;
							next.returnValue = null;
							next.previous = null;
							next = next.next;
							next.previous.next = null;
						}
						peekWaitCount = 0;
						peekWaitHeader.next = null;
						Monitor.PulseAll(peekWaitHeader);
					}
				}
			}
			if (removeWaitCount <= 0)
			{
				return;
			}
			lock (removeWaitHeader)
			{
				if (removeWaitCount <= 0)
				{
					return;
				}
				WaitingThreadNode next = removeWaitHeader.next;
				WaitingThreadNode[] array = new WaitingThreadNode[removeWaitCount];
				for (int i = 0; i < removeWaitCount; i++)
				{
					array[i] = next;
					next.valueReturned = true;
					next.returnValue = null;
					next.previous = null;
					next = next.next;
					next.previous.next = null;
				}
				for (int j = 0; j < removeWaitCount; j++)
				{
					lock (array[j])
					{
						Monitor.Pulse(array[j]);
					}
				}
				removeWaitHeader.next = removeWaitHeader;
				removeWaitHeader.previous = removeWaitHeader;
				removeWaitCount = 0;
			}
		}
	}
}
