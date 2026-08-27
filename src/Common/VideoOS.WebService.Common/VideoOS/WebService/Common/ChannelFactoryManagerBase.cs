using System;
using System.ServiceModel;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common;

public class ChannelFactoryManagerBase<TChannel> : ManagerBaseFramework<ChannelFactory<TChannel>, TChannel>, IDisposable, IChannelFactoryManager<TChannel>, IRetryChannel<TChannel> where TChannel : class
{
	public ChannelFactory<TChannel> Factory
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

	public new TChannel Channel
	{
		get
		{
			return base.Channel;
		}
		private set
		{
			base.Channel = value;
		}
	}

	public ChannelFactoryManagerBase(Uri serverUrl, string namespaceName, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, tokenCache, exceptionHandler)
	{
	}

	public ChannelFactoryManagerBase(Uri serverUrl, string namespaceName, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, exceptionHandler)
	{
	}

	public ChannelFactoryManagerBase(Uri serverUrl, string namespaceName, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, userName, password, domain, exceptionHandler)
	{
	}

	public ChannelFactoryManagerBase(Uri serverUrl, string namespaceName, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, userName, password, exceptionHandler)
	{
	}

	private void Disconnect()
	{
		CloseConnection();
		Channel = null;
		Factory = null;
	}

	public override void Initialize()
	{
		ChannelFactory<TChannel> channelFactory = new ChannelFactory<TChannel>(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(channelFactory.Endpoint);
		ConfigureCredentials(channelFactory.Credentials);
		Factory = channelFactory;
	}

	public override void Connect()
	{
		Initialize();
		Channel = Factory.CreateChannel();
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
