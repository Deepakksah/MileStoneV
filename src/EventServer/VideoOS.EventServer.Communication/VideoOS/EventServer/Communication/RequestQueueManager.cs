using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.EventServer.Communication;

public sealed class RequestQueueManager
{
	public delegate void HandleRequestObject(RequestObject requestObject);

	public delegate void FaultExceptionHandler(Exception error, int requestId);

	private Queue<RequestObject> _requests = new Queue<RequestObject>();

	private Thread _thread;

	private object _queueLock = new object();

	private object _threadLock = new object();

	private bool _close;

	private string _eventServerAddress;

	private HandleRequestObject _handleRequestObject;

	private FaultExceptionHandler _faultExceptionHandler;

	public RequestQueueManager(string eventServerAddress, HandleRequestObject handleRequestObject, FaultExceptionHandler faultExceptionHandler)
	{
		_eventServerAddress = eventServerAddress;
		_handleRequestObject = handleRequestObject;
		_faultExceptionHandler = faultExceptionHandler;
	}

	public void Open()
	{
		_close = false;
	}

	public void Close()
	{
		_close = true;
		lock (_threadLock)
		{
			if (_thread != null)
			{
				_thread.Join();
				_thread = null;
			}
		}
	}

	public void Enqueue(RequestObject requestObject)
	{
		if (requestObject == null)
		{
			throw new ArgumentException("RequestObject cannot be null");
		}
		if (_thread == null && !_close)
		{
			lock (_threadLock)
			{
				if (_thread == null && !_close)
				{
					_thread = new Thread(ThreadLoop);
					_thread.IsBackground = true;
					_thread.Name = "RequestQueueManagerThread (" + _eventServerAddress + ")";
					_thread.Start();
				}
			}
		}
		lock (_queueLock)
		{
			_requests.Enqueue(requestObject);
		}
	}

	private void ThreadLoop()
	{
		try
		{
			while (!_close)
			{
				RequestObject requestObject = null;
				try
				{
					lock (_queueLock)
					{
						if (_requests.Count > 0)
						{
							requestObject = _requests.Dequeue();
						}
					}
					if (requestObject != null)
					{
						_handleRequestObject(requestObject);
					}
				}
				catch (Exception error)
				{
					try
					{
						_faultExceptionHandler(error, requestObject?.RequestId ?? (-1));
					}
					catch (Exception)
					{
					}
				}
				if (_requests.Count == 0)
				{
					Thread.Sleep(50);
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}
}
