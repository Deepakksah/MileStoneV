using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Central.Util;

public class SubscribersHandler<T> : IDisposable
{
	private readonly HashSet<T> _subscribers = new HashSet<T>();

	private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

	private bool _disposed;

	public bool Subscribe(T subscriber)
	{
		if (_disposed)
		{
			return false;
		}
		_lock.EnterWriteLock();
		try
		{
			return _subscribers.Add(subscriber);
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	public bool Unsubscribe(T subscriber)
	{
		if (_disposed)
		{
			return false;
		}
		_lock.EnterWriteLock();
		try
		{
			return _subscribers.Remove(subscriber);
		}
		finally
		{
			_lock.ExitWriteLock();
		}
	}

	public void Notify(Action<T> action)
	{
		if (_disposed)
		{
			return;
		}
		_lock.EnterReadLock();
		try
		{
			foreach (T subscriber in _subscribers)
			{
				action(subscriber);
			}
		}
		finally
		{
			_lock.ExitReadLock();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (!_disposed)
			{
				_lock.Dispose();
			}
			_disposed = true;
		}
	}
}
