using System;

namespace VideoOS.Mobile.Portable.Utilities.ProxyPattern;

public abstract class ProxyItem<TI> : IProxyItem<TI>, IDisposable
{
	private bool _disposed;

	public Action<ProxyItem<TI>> OnDestruction;

	public abstract TI GetProxy { get; }

	~ProxyItem()
	{
		Dispose(disposing: true);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				OnDestruction?.Invoke(this);
			}
			_disposed = true;
		}
	}
}
