using System;

namespace VideoOS.Management.Server;

public abstract class ApplicationHost<T> : IDisposable where T : RequestContextBase
{
	public abstract Guid InstanceId { get; }

	public bool IsDisposed { get; set; }

	public abstract T CreateRequest(RequestContextType requestKind, HostRequestType requestType, params object[] additionalParameters);

	public abstract void Start(T requestContext);

	public abstract void Stop(T requestContext);

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
