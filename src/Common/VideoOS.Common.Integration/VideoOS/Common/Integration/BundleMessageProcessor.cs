using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace VideoOS.Common.Integration;

public class BundleMessageProcessor<T> : IBundleMessageProcessor<T>
{
	public delegate bool ProcessDelegate(T[] obj);

	private volatile bool _stopped;

	private volatile bool _enabled;

	private ProcessDelegate _processDelegate;

	private int _maxMessagesPerCall = 100;

	private int _milliSecondsBetweenCalls = 5000;

	private Queue<T> _messageQueue = new Queue<T>();

	private IMessagePersister<T> _messagePersister;

	private Thread _processingThread;

	public int MaxMessagesPerCall
	{
		get
		{
			return _maxMessagesPerCall;
		}
		set
		{
			_maxMessagesPerCall = value;
		}
	}

	public int MillisecondsBetweenCalls
	{
		get
		{
			return _milliSecondsBetweenCalls;
		}
		set
		{
			_milliSecondsBetweenCalls = value;
		}
	}

	public BundleMessageProcessor(ProcessDelegate processDelegate)
		: this(processDelegate, (IMessagePersister<T>)null)
	{
	}

	public BundleMessageProcessor(ProcessDelegate processDelegate, IMessagePersister<T> messagePersister)
	{
		_processDelegate = processDelegate;
		_messagePersister = messagePersister;
	}

	public void Start(bool enable)
	{
		if (_processingThread != null)
		{
			throw new InvalidOperationException("Processing thread is already started.");
		}
		try
		{
			if (_messagePersister != null)
			{
				_messagePersister.Open();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			_messagePersister = null;
		}
		_processingThread = new Thread(ProcessMessageThread, 262144);
		_processingThread.IsBackground = true;
		_processingThread.Name = string.Format(CultureInfo.InvariantCulture, "BundleMessageProcessor<{0}>-WorkThread", typeof(T).Name);
		_processingThread.Start();
		if (enable)
		{
			EnableProcessing(enable: true);
		}
	}

	public void Stop()
	{
		_stopped = true;
		lock (_messageQueue)
		{
			Monitor.Pulse(_messageQueue);
		}
		if (_processingThread != null)
		{
			_processingThread.Join();
		}
		_processingThread = null;
		if (_messagePersister != null)
		{
			_messagePersister.Close();
		}
	}

	public void QueueMessage(T message)
	{
		if (_stopped)
		{
			return;
		}
		lock (_messageQueue)
		{
			_messageQueue.Enqueue(message);
			if (_messageQueue.Count >= _maxMessagesPerCall)
			{
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
		if (_stopped)
		{
			return;
		}
		lock (_messageQueue)
		{
			foreach (T item in messages)
			{
				_messageQueue.Enqueue(item);
			}
			if (_messageQueue.Count >= _maxMessagesPerCall)
			{
				Monitor.Pulse(_messageQueue);
			}
		}
	}

	public void EnableProcessing(bool enable)
	{
		if (_stopped)
		{
			return;
		}
		lock (_messageQueue)
		{
			_enabled = enable;
			if (_enabled)
			{
				Monitor.Pulse(_messageQueue);
			}
		}
	}

	private void ProcessMessageThread()
	{
		try
		{
			while (true)
			{
				bool persist = false;
				T[] array = null;
				lock (_messageQueue)
				{
					if (_stopped)
					{
						break;
					}
					Monitor.Wait(_messageQueue, _milliSecondsBetweenCalls);
					if (_stopped)
					{
						break;
					}
					int num = Math.Min(_messageQueue.Count, _maxMessagesPerCall);
					if (num > 0)
					{
						array = new T[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = _messageQueue.Dequeue();
						}
					}
					persist = !_enabled;
				}
				try
				{
					ProcessMessages(array, persist);
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
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void ProcessMessages(T[] messages, bool persist)
	{
		if (messages == null)
		{
			return;
		}
		try
		{
			if (!persist)
			{
				if (_messagePersister != null && _messagePersister.HasMessages)
				{
					try
					{
						int num = _maxMessagesPerCall - messages.Length;
						if (num <= 0)
						{
							num = messages.Length / 5;
						}
						T[] recentMessages = _messagePersister.GetRecentMessages(num);
						T[] array = new T[messages.Length + recentMessages.Length];
						recentMessages.CopyTo(array, 0);
						messages.CopyTo(array, recentMessages.Length);
						messages = array;
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception)
					{
					}
				}
				if (_processDelegate(messages))
				{
					return;
				}
				try
				{
					if (_messagePersister != null)
					{
						_messagePersister.PersistMessages(messages);
					}
					return;
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception)
				{
					return;
				}
			}
			try
			{
				if (_messagePersister != null)
				{
					_messagePersister.PersistMessages(messages);
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
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}
}
