using System;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication.QueueingClient;

public class Entry
{
	private readonly DateTime _scheduleTime;

	private readonly int _retries;

	public Func<Task> Callback { get; }

	public bool IsFutureWork => _scheduleTime > DateTime.Now;

	public int NextRetries => _retries + 1;

	public Entry(int retries, Func<Task> callback, DateTime scheduleTime)
	{
		_retries = retries;
		Callback = callback;
		_scheduleTime = scheduleTime;
	}

	public bool NeedsRetry(int maxRetries)
	{
		return _retries < maxRetries;
	}
}
