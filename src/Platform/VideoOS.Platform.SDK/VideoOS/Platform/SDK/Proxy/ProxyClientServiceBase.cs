using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Proxy;

public abstract class ProxyClientServiceBase<TClient> : ProxyClientBase, IDisposable where TClient : class, new()
{
	private TClient _serviceClient;

	protected TClient ServiceClient
	{
		[DebuggerStepThrough]
		get
		{
			return _serviceClient;
		}
		[DebuggerStepThrough]
		set
		{
			_serviceClient = value;
		}
	}

	public ProxyClientServiceBase(LoginSettings loginSettings, Binding binding)
		: base(loginSettings, binding)
	{
	}

	public void Initialize(TClient serviceClient)
	{
		_serviceClient = serviceClient;
	}

	protected void CallWithRetry(Action call)
	{
		try
		{
			call();
		}
		catch (FaultException<ExceptionDetail>)
		{
			throw;
		}
		catch (CommunicationException)
		{
			Initialize();
			call();
		}
	}

	protected TResult CallWithRetry<TResult>(Func<TResult> call)
	{
		try
		{
			return call();
		}
		catch (FaultException<ExceptionDetail>)
		{
			throw;
		}
		catch (CommunicationException)
		{
			Initialize();
			return call();
		}
		catch (ObjectDisposedException)
		{
			Initialize();
			return call();
		}
	}
}
