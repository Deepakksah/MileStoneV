using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.EventServer.Communication;

public sealed class ClientPool<T> : IDisposable where T : class, ICommunicationObject
{
	private readonly HashSet<T> _clients = new HashSet<T>();

	private readonly Func<T> _createClient;

	private readonly int _defaultRetryCount;

	private bool _closed;

	public ClientPool(Func<T> createClient, int defaultRetryCount)
	{
		_createClient = createClient;
		_defaultRetryCount = defaultRetryCount;
	}

	public Task CallWithRetryAsync(Func<T, Task> method)
	{
		return CallWithRetryAsync(method, _defaultRetryCount);
	}

	public async Task CallWithRetryAsync(Func<T, Task> method, int retryCount)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (retryCount < 0)
		{
			throw new ArgumentOutOfRangeException("retryCount", "retryCount should be greater than or equal to 0");
		}
		int count = retryCount;
		while (true)
		{
			T client = null;
			try
			{
				client = TakeClient();
				await method(client).ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
			catch (LoginSettingsNotFoundException ex)
			{
				throw new CommunicationException(ex.Message, ex);
			}
			catch (CommunicationException)
			{
				if (ShouldRetry(count, client))
				{
					count--;
					continue;
				}
				throw;
			}
			finally
			{
				if (client != null)
				{
					PutClient(client);
				}
			}
		}
	}

	public Task<TRet> CallWithRetryAsync<TRet>(Func<T, Task<TRet>> method) where TRet : class
	{
		return CallWithRetryAsync(method, _defaultRetryCount);
	}

	public async Task<TRet> CallWithRetryAsync<TRet>(Func<T, Task<TRet>> method, int retryCount) where TRet : class
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (retryCount < 0)
		{
			throw new ArgumentOutOfRangeException("retryCount", "retryCount should be greater than or equal to 0");
		}
		int count = retryCount;
		while (true)
		{
			T client = null;
			try
			{
				client = TakeClient();
				return await method(client).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (LoginSettingsNotFoundException ex)
			{
				throw new CommunicationException(ex.Message, ex);
			}
			catch (CommunicationException)
			{
				if (ShouldRetry(count, client))
				{
					count--;
					continue;
				}
				throw;
			}
			finally
			{
				if (client != null)
				{
					PutClient(client);
				}
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
				val = TakeClient();
				return method(val);
			}
			catch (LoginSettingsNotFoundException ex)
			{
				throw new CommunicationException(ex.Message, ex);
			}
			catch (CommunicationException)
			{
				if (ShouldRetry(num, val))
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
				val = TakeClient();
				method(val);
				break;
			}
			catch (LoginSettingsNotFoundException ex)
			{
				throw new CommunicationException(ex.Message, ex);
			}
			catch (CommunicationException)
			{
				if (ShouldRetry(num, val))
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

	public Task<TRes> CallWithRetry<TRes>(Func<T, AsyncCallback, IAsyncResult> begin, Func<T, IAsyncResult, TRes> end, AsyncCallback callback, object asyncState)
	{
		TaskCompletionSource<TRes> tcs = new TaskCompletionSource<TRes>(asyncState);
		T client;
		try
		{
			client = TakeClient();
		}
		catch (LoginSettingsNotFoundException ex)
		{
			tcs.SetException(new CommunicationException(ex.Message, ex));
			callback?.Invoke(tcs.Task);
			return tcs.Task;
		}
		AsyncCallback arg = delegate(IAsyncResult ar)
		{
			try
			{
				TRes result = end(client, ar);
				tcs.SetResult(result);
				callback?.Invoke(tcs.Task);
			}
			catch (FaultException exception)
			{
				tcs.SetException(exception);
				callback?.Invoke(tcs.Task);
			}
			catch (CommunicationException)
			{
				T newClient = null;
				try
				{
					newClient = TakeClient();
				}
				catch (LoginSettingsNotFoundException ex3)
				{
					tcs.SetException(new CommunicationException(ex3.Message, ex3));
					callback?.Invoke(tcs.Task);
					return;
				}
				AsyncCallback arg2 = delegate(IAsyncResult arg3)
				{
					try
					{
						TRes result2 = end(newClient, arg3);
						tcs.SetResult(result2);
						callback?.Invoke(tcs.Task);
					}
					catch (Exception exception3)
					{
						tcs.SetException(exception3);
						callback?.Invoke(tcs.Task);
					}
					finally
					{
						PutClient(newClient);
					}
				};
				begin(newClient, arg2);
			}
			catch (Exception exception2)
			{
				tcs.SetException(exception2);
				callback?.Invoke(tcs.Task);
			}
			finally
			{
				PutClient(client);
			}
		};
		begin(client, arg);
		return tcs.Task;
	}

	public void Dispose()
	{
		_closed = true;
		lock (_clients)
		{
			foreach (T client in _clients)
			{
				ShutdownClient(client);
			}
			_clients.Clear();
		}
	}

	private static bool ShouldRetry(int count, T client)
	{
		if (count > 0 && client != null)
		{
			return client.State == CommunicationState.Faulted;
		}
		return false;
	}

	private T TakeClient()
	{
		lock (_clients)
		{
			if (_clients.Count == 0)
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
}
