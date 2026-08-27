using System;
using System.Threading;

namespace VideoOS.Common.Integration;

public abstract class StatusHandlerBase
{
	protected Guid statusHandleId;

	protected bool isDisposed;

	protected DateTime lastRequestTime;

	protected object queueLock;

	protected long sequenceNo;

	public DateTime LastRequestTime => lastRequestTime;

	public Guid StatusHandleId => statusHandleId;

	public StatusHandlerBase(Guid statusHandleId)
	{
		this.statusHandleId = statusHandleId;
		isDisposed = false;
		lastRequestTime = DateTime.UtcNow;
		queueLock = new object();
	}

	public void Dispose()
	{
		isDisposed = true;
		lock (queueLock)
		{
			Monitor.Pulse(queueLock);
		}
	}

	public abstract StatusBase[] GetStatus(int timeoutMs);
}
