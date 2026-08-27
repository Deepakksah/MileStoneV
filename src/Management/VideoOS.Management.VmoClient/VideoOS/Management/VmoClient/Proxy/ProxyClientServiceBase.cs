using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient.Proxy;

public abstract class ProxyClientServiceBase<TClient> : ProxyClientBase where TClient : class, new()
{
	protected TClient ServiceClient { get; set; }

	public ProxyClientServiceBase(ConnectionSettings connectionSettings)
		: base(connectionSettings)
	{
	}

	public void Initialize(TClient serviceClient)
	{
		ServiceClient = serviceClient;
	}

	protected void CallWithRetry(Action call)
	{
		CallWithRetry(delegate
		{
			call();
			return false;
		});
	}

	protected TResult CallWithRetry<TResult>(Func<TResult> call)
	{
		if (call == null)
		{
			throw new ArgumentNullException("call");
		}
		try
		{
			return call();
		}
		catch (FaultException)
		{
			throw;
		}
		catch (SecurityNegotiationException)
		{
			throw;
		}
		catch (CommunicationException ex3)
		{
			if (ex3.InnerException != null && (ex3.InnerException is IdpException || ex3.InnerException is IdentityServerTokenException))
			{
				throw;
			}
			Initialize();
			return call();
		}
		catch (ConnectionFailureException)
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
