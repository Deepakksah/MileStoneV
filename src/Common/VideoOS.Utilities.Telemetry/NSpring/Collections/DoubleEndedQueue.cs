using System;
using System.Threading;

namespace NSpring.Collections;

public sealed class DoubleEndedQueue : Queue
{
	private sealed class WaitingThreadNode
	{
		public object returnValue;

		public bool valueReturned;

		public WaitingThreadNode next;

		public WaitingThreadNode previous;
	}

	private const float GROWTH_FACTOR = 3f;

	private const int INITIAL_CAPACITY = 1000;

	private int peekWaitCount;

	private WaitingThreadNode peekWaitHeader;

	private int removeWaitCount;

	private WaitingThreadNode removeWaitHeader;

	private int waitNodeCacheCount;

	private WaitingThreadNode waitNodeCacheHeader;

	private object[] elements = new object[1000];

	private int startIndex;

	private int endIndex;

	private int capacity = 1000;

	public DoubleEndedQueue()
	{
		peekWaitHeader = new WaitingThreadNode();
		peekWaitHeader.next = peekWaitHeader;
		peekWaitHeader.previous = peekWaitHeader;
		removeWaitHeader = new WaitingThreadNode();
		removeWaitHeader.next = removeWaitHeader;
		removeWaitHeader.previous = removeWaitHeader;
		waitNodeCacheHeader = new WaitingThreadNode();
	}

	public override void Clear()
	{
		lock (monitor)
		{
			Array.Clear(elements, 0, capacity);
			count = 0;
			startIndex = 0;
			endIndex = 0;
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
			if (count == capacity)
			{
				IncreaseArraySize();
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
	}

	public void AddLast(object _object)
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
			if (count == capacity)
			{
				IncreaseArraySize();
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
	}

	public void AddFirst(object _object)
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
			if (count == capacity)
			{
				IncreaseArraySize();
			}
			if (count > 0)
			{
				startIndex--;
				if (startIndex < 0)
				{
					startIndex = elements.Length - 1;
				}
			}
			elements[startIndex] = _object;
			count++;
		}
	}

	private void IncreaseArraySize()
	{
		capacity = (int)((float)elements.Length * 3f);
		object[] destinationArray = new object[capacity];
		Array.Copy(elements, startIndex, destinationArray, 0, elements.Length - startIndex);
		if (startIndex > 0)
		{
			Array.Copy(elements, 0, destinationArray, elements.Length - startIndex, startIndex);
		}
		startIndex = 0;
		endIndex = elements.Length - 1;
		elements = destinationArray;
	}

	public override object Peek()
	{
		lock (monitor)
		{
			return elements[startIndex];
		}
	}

	public object PeekFirst()
	{
		lock (monitor)
		{
			return elements[startIndex];
		}
	}

	public override object Peek(int _millisecondsTimeout)
	{
		return PeekFirst(_millisecondsTimeout);
	}

	public object PeekFirst(int _millisecondsTimeout)
	{
		lock (monitor)
		{
			if (count > 0)
			{
				return elements[startIndex];
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

	public object PeekLast()
	{
		lock (monitor)
		{
			return elements[endIndex];
		}
	}

	public object PeekLast(int _millisecondsTimeout)
	{
		lock (monitor)
		{
			if (count > 0)
			{
				return elements[endIndex];
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
				object result = elements[startIndex++];
				if (startIndex == capacity)
				{
					startIndex = 0;
				}
				count--;
				return result;
			}
		}
		return null;
	}

	public object RemoveFirst()
	{
		lock (monitor)
		{
			if (count > 0)
			{
				object result = elements[startIndex++];
				if (startIndex == capacity)
				{
					startIndex = 0;
				}
				count--;
				return result;
			}
		}
		return null;
	}

	public object RemoveLast()
	{
		lock (monitor)
		{
			if (count > 0)
			{
				object result = elements[endIndex--];
				if (endIndex < 0)
				{
					endIndex = capacity - 1;
				}
				count--;
				return result;
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
				result = elements[startIndex++];
				if (startIndex == capacity)
				{
					startIndex = 0;
				}
				count--;
				return result;
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

	public object RemoveFirst(int _millisecondsTimeout)
	{
		object result = null;
		lock (monitor)
		{
			if (count > 0)
			{
				result = elements[startIndex++];
				if (startIndex == capacity)
				{
					startIndex = 0;
				}
				count--;
				return result;
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

	public object RemoveLast(int _millisecondsTimeout)
	{
		object result = null;
		lock (monitor)
		{
			if (count > 0)
			{
				result = elements[endIndex--];
				if (endIndex < 0)
				{
					endIndex = capacity - 1;
				}
				count--;
				return result;
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
