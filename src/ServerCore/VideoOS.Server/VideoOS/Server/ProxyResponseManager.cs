#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public sealed class ProxyResponseManager : IResponseHandler
{
	private class QueueEntry
	{
		public Queue<ResponseBase> ResponseQueue;

		public Timer ActivityTimer;
	}

	private readonly int _activityTimeout = 60000;

	private Hashtable _responseQueues = new Hashtable();

	public event EventHandler<ResponseSendTimeoutEventArgs> ResponseSendTimeout;

	private QueueEntry GetQueueEntry(Guid proxyId)
	{
		QueueEntry queueEntry = null;
		lock (_responseQueues)
		{
			queueEntry = _responseQueues[proxyId] as QueueEntry;
			if (queueEntry == null)
			{
				queueEntry = new QueueEntry();
				queueEntry.ResponseQueue = new Queue<ResponseBase>();
				queueEntry.ActivityTimer = new Timer(ActivityTimerCallback, proxyId, _activityTimeout, -1);
				_responseQueues.Add(proxyId, queueEntry);
			}
		}
		return queueEntry;
	}

	private void OnResponseSendTimeout(ResponseSendTimeoutEventArgs e)
	{
		if (this.ResponseSendTimeout != null)
		{
			this.ResponseSendTimeout(this, e);
		}
	}

	private void ActivityTimerCallback(object target)
	{
		try
		{
			if (!(target is Guid guid))
			{
				return;
			}
			lock (_responseQueues)
			{
				QueueEntry queueEntry = GetQueueEntry(guid);
				ResponseBase[] array = queueEntry.ResponseQueue.ToArray();
				_responseQueues.Remove(guid);
				if (array.Length != 0)
				{
					OnResponseSendTimeout(new ResponseSendTimeoutEventArgs(array));
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

	public bool IsProxyRegistered(Guid proxyId)
	{
		lock (_responseQueues)
		{
			return _responseQueues.Contains(proxyId);
		}
	}

	public ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout)
	{
		QueueEntry queueEntry = GetQueueEntry(proxyId);
		lock (queueEntry)
		{
			queueEntry.ActivityTimer.Change((uint)timeout.TotalMilliseconds + _activityTimeout, -1L);
			if (queueEntry.ResponseQueue.Count == 0)
			{
				if (!Monitor.Wait(queueEntry, timeout))
				{
					return null;
				}
				if (queueEntry.ResponseQueue.Count == 0)
				{
					return null;
				}
			}
			ResponseBase[] array = new ResponseBase[queueEntry.ResponseQueue.Count];
			queueEntry.ResponseQueue.CopyTo(array, 0);
			queueEntry.ResponseQueue.Clear();
			if (array.Any((ResponseBase f) => f is ExceptionResponse))
			{
				List<ResponseBase> list = new List<ResponseBase>();
				ResponseBase[] array2 = array;
				foreach (ResponseBase responseBase in array2)
				{
					if (responseBase is ExceptionResponse exceptionResponse)
					{
						list.Add(new ExceptionResponseDataContract(exceptionResponse));
					}
					else
					{
						list.Add(responseBase);
					}
				}
				return list.ToArray();
			}
			return array;
		}
	}

	public void HandleResponse(CommandBase command, ResponseBase response)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		Guid proxyId = (Guid)command.InvocationInfo;
		Trace.WriteLine("ProxyResponseManager: Queuing response, proxyId=" + proxyId.ToString() + " type=" + response.GetType().ToString() + " token=" + response.Token.ToString());
		QueueEntry queueEntry = GetQueueEntry(proxyId);
		lock (queueEntry)
		{
			queueEntry.ResponseQueue.Enqueue(response);
			Monitor.Pulse(queueEntry);
		}
	}

	public void ReleaseGetResponses(Guid proxyId)
	{
		QueueEntry queueEntry = GetQueueEntry(proxyId);
		lock (queueEntry)
		{
			Monitor.Pulse(queueEntry);
		}
	}
}
