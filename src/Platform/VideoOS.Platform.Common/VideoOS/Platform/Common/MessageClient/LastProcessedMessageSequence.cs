using System;
using System.Threading;

namespace VideoOS.Platform.Common.MessageClient;

public class LastProcessedMessageSequence : ILastProcessedMessageSequence, IDisposable
{
	private int _lastProcessedMessageSequence = -1;

	private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

	public void Initialize()
	{
		_lock.EnterWriteLock();
		try
		{
			_lastProcessedMessageSequence = -1;
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	public int GetLastProcessedMessageSequence()
	{
		_lock.EnterReadLock();
		try
		{
			return _lastProcessedMessageSequence;
		}
		finally
		{
			_lock.ExitReadLock();
		}
	}

	public void UpdateLastProcessedMessageSequence(int newSequence)
	{
		_lock.EnterWriteLock();
		try
		{
			_lastProcessedMessageSequence = Math.Max(_lastProcessedMessageSequence, newSequence);
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	public void Dispose()
	{
		_lock?.Dispose();
	}
}
