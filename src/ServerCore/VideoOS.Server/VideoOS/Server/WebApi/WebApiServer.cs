using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Hosting;
using Owin;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.WebApi;

public sealed class WebApiServer
{
	private ILogger Logger;

	private IDisposable _webApiServer;

	private WebApiStatus _webApiStatus;

	public int Port { get; private set; }

	public string Scheme { get; private set; }

	public WebApiServer(int port, string scheme)
		: this(port, scheme, Telemetry.GetILogger("WebApi"))
	{
	}

	public WebApiServer(int port, string scheme, ILogger logger)
	{
		Port = port;
		Logger = logger;
		Scheme = scheme;
	}

	public void Start()
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}://+:{1}/", Scheme, Port);
		Logger.LogInformation("Starting WebApi communication with baseaddress : {BaseAddress}", text);
		_webApiStatus = new WebApiStatus();
		_webApiServer = WebApp.Start(text, delegate(IAppBuilder builder)
		{
			Startup.Configuration(builder, ServerConfig.Instance, _webApiStatus);
		});
	}

	public void Stop()
	{
		Logger.LogInformation("Stopping WebApi communication");
		if (_webApiServer != null)
		{
			_webApiStatus.IsWebApiStopping = true;
			_webApiServer.Dispose();
		}
	}
}
