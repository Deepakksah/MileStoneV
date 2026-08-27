using System;
using VideoOS.Server.WebApi;

namespace VideoOS.Server.RecorderCommunication;

public sealed class WebApiCommunicationProvider : ICommunicationProvider
{
	private WebApiServer _webApiServer;

	private EventHandler<ConnectionChangedEventArgs> _connectionChanged;

	event EventHandler<ConnectionChangedEventArgs> ICommunicationProvider.ConnectionChanged
	{
		add
		{
			_connectionChanged = (EventHandler<ConnectionChangedEventArgs>)Delegate.Combine(_connectionChanged, value);
		}
		remove
		{
			_connectionChanged = (EventHandler<ConnectionChangedEventArgs>)Delegate.Remove(_connectionChanged, value);
		}
	}

	public void Close()
	{
		if (_webApiServer != null)
		{
			_webApiServer.Stop();
		}
	}

	public void Initialize(ServerConfig config)
	{
		_webApiServer = new WebApiServer(config.WebApiConfig.Port, config.WebApiConfig.Scheme);
		_webApiServer.Start();
	}
}
