using System;

namespace VideoOS.Common.Integration;

public class RefCountedObjectWrapper<T> : IDisposable where T : class, IDisposable
{
	private object _refCountSync = new object();

	private int _refCount = 1;

	private volatile bool _disposed;

	public T Instance { get; private set; }

	public RefCountedObjectWrapper(T instance)
	{
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		Instance = instance;
	}

	public TResult ExecuteCallWithRefCount<TResult>(Func<T, TResult> func)
	{
		if (func == null)
		{
			throw new ArgumentNullException("func");
		}
		IncrementRefCount();
		try
		{
			return func(Instance);
		}
		finally
		{
			DecrementRefCount();
		}
	}

	public void ExecuteCallWithRefCount(Action<T> action)
	{
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		IncrementRefCount();
		try
		{
			action(Instance);
		}
		finally
		{
			DecrementRefCount();
		}
	}

	private void DecrementRefCount()
	{
		lock (_refCountSync)
		{
			_refCount--;
			if (_refCount == 0 && Instance != null)
			{
				Instance.Dispose();
				Instance = null;
			}
		}
	}

	private void IncrementRefCount()
	{
		lock (_refCountSync)
		{
			if (_refCount == 0 || _disposed)
			{
				throw new ObjectDeadException();
			}
			_refCount++;
		}
	}

	public void Dispose()
	{
		DecrementRefCount();
		_disposed = true;
	}
}
