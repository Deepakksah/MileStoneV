using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common;

public abstract class WebServiceManagerBase<TClient, TChannel> : ManagerBaseFramework<TClient, TChannel>, IDisposable, IWebServiceManager<TClient, TChannel>, IRetryChannel<TChannel> where TClient : ClientBase<TChannel> where TChannel : class
{
	public TClient Instance
	{
		get
		{
			return base.CommunicationObject;
		}
		protected set
		{
			base.CommunicationObject = value;
		}
	}

	protected WebServiceManagerBase(Uri serverUrl, string namespaceName, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, tokenCache, exceptionHandler)
	{
	}

	protected WebServiceManagerBase(Uri serverUrl, string namespaceName, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, exceptionHandler)
	{
	}

	protected WebServiceManagerBase(Uri serverUrl, string namespaceName, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, userName, password, domain, exceptionHandler)
	{
	}

	protected WebServiceManagerBase(Uri serverUrl, string namespaceName, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, userName, password, exceptionHandler)
	{
	}

	private void Disconnect()
	{
		CloseConnection();
		Instance = null;
	}

	protected abstract TClient CreateClientObject(Binding binding, EndpointAddress remoteAddress);

	public override void Initialize()
	{
		TClient val = CreateClientObject(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(val.Endpoint);
		ConfigureCredentials(val.ClientCredentials);
		Instance = val;
	}

	public override void Connect()
	{
		Initialize();
		Open();
		if (base.CommunicationObject is TChannel channel)
		{
			base.Channel = channel;
			return;
		}
		ClientBase<TChannel> communicationObject = base.CommunicationObject;
		base.Channel = (TChannel)communicationObject.InnerChannel;
	}

	public virtual void Open()
	{
		Instance.Open();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Disconnect();
		}
	}
}
