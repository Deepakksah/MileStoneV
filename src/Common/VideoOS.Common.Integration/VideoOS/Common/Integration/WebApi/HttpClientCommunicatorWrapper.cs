using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.WebApi;

public class HttpClientCommunicatorWrapper : RefCountedObjectWrapper<ICommunicator>, ICommunicator, IDisposable
{
	public HttpClientCommunicatorWrapper(HttpClientCommunicator communicator)
		: base((ICommunicator)communicator)
	{
	}

	public HttpClientCommunicatorWrapper(ICommunicator communicator)
		: base(communicator)
	{
	}

	public HttpResponseMessage Patch<TObj>(string endpoint, TObj obj)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Patch(endpoint, obj));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public HttpResponseMessage Patch(string endpoint)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Patch(endpoint));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<HttpResponseMessage> PatchAsync<TObj>(string endpoint, TObj obj)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.PatchAsync(endpoint, obj));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<HttpResponseMessage> PatchAsync(string endpoint)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.PatchAsync(endpoint));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public TObj Receive<TObj>(string endpoint)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Receive<TObj>(endpoint));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<TObj> ReceiveAsync<TObj>(string endpoint)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.ReceiveAsync<TObj>(endpoint));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public TRet Send<TObj, TRet>(string endpoint, TObj obj, CancellationToken cancellationToken)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Send<TObj, TRet>(endpoint, obj, cancellationToken));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public HttpResponseMessage Send<TObj>(string endpoint, TObj obj, CancellationToken cancellationToken)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Send(endpoint, obj, cancellationToken));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public TRet Send<TRet>(string endpoint, CancellationToken cancellationToken)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Send<TRet>(endpoint, cancellationToken));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public HttpResponseMessage Send(string endpoint, CancellationToken cancellationToken)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.Send(endpoint, cancellationToken));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<TRet> SendAsync<TObj, TRet>(string endpoint, TObj obj)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.SendAsync<TObj, TRet>(endpoint, obj));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<HttpResponseMessage> SendAsync<TObj>(string endpoint, TObj obj)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.SendAsync(endpoint, obj));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<HttpResponseMessage> SendRawAsync(string endpoint, HttpContent message)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.SendRawAsync(endpoint, message));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<TRet> SendAsync<TRet>(string endpoint)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.SendAsync<TRet>(endpoint));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}

	public Task<HttpResponseMessage> SendAsync(string endpoint)
	{
		try
		{
			return ExecuteCallWithRefCount((ICommunicator communicator) => communicator.SendAsync(endpoint));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException("Communicator disposed");
		}
	}
}
