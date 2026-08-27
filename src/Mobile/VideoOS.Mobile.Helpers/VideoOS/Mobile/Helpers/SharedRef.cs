using System;

namespace VideoOS.Mobile.Helpers;

public class SharedRef<T> : IDisposable where T : class, IDisposable
{
	private T _reference;

	private RefCounter _refCounter;

	private bool _disposed;

	public T Reference => _reference;

	public SharedRef(T reference)
	{
		Initialize(reference, new RefCounter());
	}

	public SharedRef(SharedRef<T> sharedRef)
	{
		Initialize(sharedRef.Reference, sharedRef._refCounter);
	}

	private void Initialize(T reference, RefCounter refCounter)
	{
		if (reference == null || refCounter == null)
		{
			throw new ArgumentNullException("reference");
		}
		_reference = reference;
		_refCounter = refCounter;
		_refCounter.AddRef();
	}

	public SharedRef<T> Clone()
	{
		return new SharedRef<T>(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_refCounter.RemoveRef();
			if (_refCounter.RefCount == 0)
			{
				_reference?.Dispose();
			}
			_disposed = true;
			_reference = null;
			_refCounter = null;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~SharedRef()
	{
		Dispose(disposing: false);
	}
}
