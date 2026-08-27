using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using VideoOS.Platform.Util.Security;

namespace VideoOS.Platform.Util.Svc;

public sealed class ClientPool<T> : IDisposable where T : class, ICommunicationObject
{
	private readonly Func<T> _createClient;

	private readonly int _defaultRetryCount;

	private bool _closed;

	private HashSet<T> _clients = new HashSet<T>();

	public ClientPool(Func<T> createClient, int defaultRetryCount)
	{
		_createClient = createClient;
		_defaultRetryCount = defaultRetryCount;
	}

	private T TakeClient(bool forceNew)
	{
		lock (_clients)
		{
			if (forceNew || _clients.Count == 0)
			{
				return _createClient();
			}
			T val = _clients.First();
			_clients.Remove(val);
			return val;
		}
	}

	private void PutClient(T client)
	{
		if (_closed || client.State == CommunicationState.Closing || client.State == CommunicationState.Closed || client.State == CommunicationState.Faulted)
		{
			ShutdownClient(client);
			return;
		}
		lock (_clients)
		{
			_clients.Add(client);
		}
	}

	private static void ShutdownClient(T client)
	{
		if (client == null)
		{
			return;
		}
		try
		{
			if (client.State != CommunicationState.Faulted)
			{
				client.Close();
			}
		}
		catch (Exception)
		{
			client.Abort();
		}
		finally
		{
			if (client.State != CommunicationState.Closed)
			{
				client.Abort();
			}
		}
	}

	public TRet CallWithRetry<TRet>(Func<T, TRet> method)
	{
		return CallWithRetry(method, _defaultRetryCount);
	}

	public TRet CallWithRetry<TRet>(Func<T, TRet> method, int retryCount)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (retryCount < 0)
		{
			throw new ArgumentOutOfRangeException("retryCount", "retryCount should be greater than or equal to 0");
		}
		int num = retryCount;
		while (true)
		{
			T val = null;
			try
			{
				val = TakeClient(num != retryCount);
				return method(val);
			}
			catch (CommunicationException ex)
			{
				if (ex is FaultException<VMOExceptionDetail>)
				{
					throw;
				}
				ShutdownClient(val);
				val = null;
				if (num > 0)
				{
					num--;
					continue;
				}
				throw;
			}
			finally
			{
				if (val != null)
				{
					PutClient(val);
				}
			}
		}
	}

	public void CallWithRetry(Action<T> method)
	{
		CallWithRetry(method, _defaultRetryCount);
	}

	public void CallWithRetry(Action<T> method, int retryCount)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (retryCount < 0)
		{
			throw new ArgumentOutOfRangeException("retryCount", "retryCount should be greater than or equal to 0");
		}
		int num = retryCount;
		while (true)
		{
			T val = null;
			try
			{
				val = TakeClient(num != retryCount);
				method(val);
				break;
			}
			catch (CommunicationException ex)
			{
				if (ex is FaultException<VMOExceptionDetail>)
				{
					throw;
				}
				ShutdownClient(val);
				val = null;
				if (num > 0)
				{
					num--;
					continue;
				}
				throw;
			}
			finally
			{
				if (val != null)
				{
					PutClient(val);
				}
			}
		}
	}

	public void Logoff()
	{
		lock (_clients)
		{
			foreach (T client in _clients)
			{
				ShutdownClient(client);
			}
			_clients.Clear();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			_closed = true;
			Logoff();
		}
	}
}
