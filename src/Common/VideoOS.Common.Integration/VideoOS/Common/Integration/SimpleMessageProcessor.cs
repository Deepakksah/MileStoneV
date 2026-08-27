using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace VideoOS.Common.Integration;

public class SimpleMessageProcessor<T> : ISimpleMessageProcessor<T>
{
	public delegate bool ProcessDelegate(T obj);

	private volatile bool _stopped;

	private volatile bool _enabled;

	private ProcessDelegate _processDelegate;

	private bool _ignoreSendErrors;

	private Queue<T> _messageQueue = new Queue<T>();

	private Thread _processingThread;

	public SimpleMessageProcessor(ProcessDelegate processDelegate, bool ignoreSendErrors)
	{
		_processDelegate = processDelegate;
		_ignoreSendErrors = ignoreSendErrors;
	}

	public void Start(bool enable)
	{
		if (_processingThread != null)
		{
			throw new InvalidOperationException("Processing thread is already started.");
		}
		_processingThread = new Thread(ProcessMessageThread, 262144);
		_processingThread.IsBackground = true;
		_processingThread.Name = string.Format(CultureInfo.InvariantCulture, "SimpleMessageSender<{0}>-WorkThread", typeof(T).Name);
		_processingThread.Start();
		if (enable)
		{
			EnableProcessing(enable: true);
		}
	}

	public void Stop()
	{
		lock (_messageQueue)
		{
			_stopped = true;
			_messageQueue.Clear();
			Monitor.Pulse(_messageQueue);
		}
		if (_processingThread != null)
		{
			_processingThread.Join();
		}
		_processingThread = null;
	}

	public void QueueMessage(T message)
	{
		lock (_messageQueue)
		{
			if (!_stopped)
			{
				_messageQueue.Enqueue(message);
				Monitor.Pulse(_messageQueue);
			}
		}
	}

	public void QueueMessages(T[] messages)
	{
		if (messages == null)
		{
			throw new ArgumentNullException("messages");
		}
		lock (_messageQueue)
		{
			if (!_stopped)
			{
				foreach (T item in messages)
				{
					_messageQueue.Enqueue(item);
				}
				Monitor.Pulse(_messageQueue);
			}
		}
	}

	public void EnableProcessing(bool enable)
	{
		lock (_messageQueue)
		{
			if (!_stopped)
			{
				_enabled = enable;
				if (enable)
				{
					Monitor.Pulse(_messageQueue);
				}
			}
		}
	}

	private void ProcessMessageThread()
	{
		try
		{
			bool flag = false;
			while (true)
			{
				T obj = default(T);
				lock (_messageQueue)
				{
					if (_stopped)
					{
						break;
					}
					if (_messageQueue.Count == 0 || !_enabled)
					{
						Monitor.Wait(_messageQueue);
						if (_stopped)
						{
							break;
						}
					}
					if (_enabled && _messageQueue.Count > 0)
					{
						obj = _messageQueue.Peek();
						flag = true;
					}
					else
					{
						obj = default(T);
						flag = false;
					}
				}
				if (!(_enabled && flag))
				{
					continue;
				}
				if (_processDelegate(obj) || _ignoreSendErrors)
				{
					lock (_messageQueue)
					{
						if (_messageQueue.Count > 0)
						{
							_messageQueue.Dequeue();
						}
					}
				}
				else
				{
					Thread.Sleep(1000);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}
}
