using System;

namespace VideoOS.Management.Server;

public abstract class RequestContextBase : IDisposable
{
	public RequestContextBase()
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
