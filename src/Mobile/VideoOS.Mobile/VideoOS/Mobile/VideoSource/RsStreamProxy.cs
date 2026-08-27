using System;

namespace VideoOS.Mobile.VideoSource;

public class RsStreamProxy<TData> : IRsStream<TData>, IRsStreamSource<TData>, IDisposable
{
	private IRsStream<TData> _proxy;

	private bool _disposed;

	public virtual OnStreamData<TData> OnData
	{
		get
		{
			return _proxy.OnData;
		}
		set
		{
			_proxy.OnData = value;
		}
	}

	public OnEventData OnEvent
	{
		get
		{
			return _proxy.OnEvent;
		}
		set
		{
			_proxy.OnEvent = value;
		}
	}

	public OnFailCallback OnFail
	{
		get
		{
			return _proxy.OnFail;
		}
		set
		{
			_proxy.OnFail = value;
		}
	}

	public bool IsPaused
	{
		get
		{
			return _proxy.IsPaused;
		}
		set
		{
			_proxy.IsPaused = value;
		}
	}

	public RsStreamProxy(IRsStream<TData> proxy)
		: this()
	{
		SetSource(proxy);
	}

	public RsStreamProxy()
	{
	}

	public virtual void SetSource(IRsStream<TData> proxy)
	{
		SetSourceInternal(proxy);
	}

	protected void SetSourceInternal(IRsStream<TData> proxy)
	{
		_proxy = proxy;
	}

	~RsStreamProxy()
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
			_disposed = true;
		}
	}
}
