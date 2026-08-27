using System;
using System.ServiceModel;
using System.Threading;

namespace VideoOS.EventServer.Communication;

public abstract class AbstractClient
{
	protected const int DefaultRetryCount = 2;

	private readonly RequestQueueManager _requestQueueManager;

	private static int _requestNumber;

	protected Uri _uri;

	public virtual CommunicationState State => CommunicationState.Opened;

	public int EventServerVersion { get; private set; }

	public Version EventServerVersion2 { get; private set; }

	protected AbstractClient(string eventServerAddress, Version eventServerVersion, Uri uri)
	{
		EventServerVersion2 = eventServerVersion;
		EventServerVersion = eventServerVersion.Major;
		_uri = uri;
		_requestQueueManager = new RequestQueueManager(eventServerAddress, HandleRequestObject, FireFaultException);
		_requestQueueManager.Open();
	}

	protected abstract void HandleRequestObject(RequestObject requestObject);

	public void Open()
	{
	}

	public virtual void Close()
	{
		_requestQueueManager.Close();
	}

	public int NextRequestId()
	{
		return Interlocked.Increment(ref _requestNumber);
	}

	protected void Enqueue(RequestObject requestObject)
	{
		_requestQueueManager.Enqueue(requestObject);
	}

	protected abstract void FireFaultException(Exception exception, int requestId);

	protected static EndpointIdentity CreateSpnIdentity(string spnName)
	{
		return EndpointIdentity.CreateSpnIdentity(spnName);
	}
}
