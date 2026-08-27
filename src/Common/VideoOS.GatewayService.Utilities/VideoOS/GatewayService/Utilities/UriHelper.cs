using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.GatewayService.Utilities;

public class UriHelper : IUriHelper
{
	private IisHelper _iisHelper;

	public UriHelper()
	{
		_iisHelper = new IisHelper((ILogger)(object)NullLogger.Instance);
	}

	public UriHelper(ILogger logger)
	{
		_iisHelper = new IisHelper((ILogger)(object)new ILoggerWrapper(logger));
	}

	public UriHelper(ILogger logcomponent)
	{
		_iisHelper = new IisHelper(logcomponent);
	}

	public Uri GetGatewayServiceUri(string webSiteId, bool isSecure, string host)
	{
		int port = _iisHelper.GetHttpPort(webSiteId);
		string scheme = Uri.UriSchemeHttp;
		if (isSecure && _iisHelper.CheckSecureBindingsAndAssociatedCertificate(webSiteId))
		{
			port = _iisHelper.GetHttpsPort(webSiteId);
			scheme = Uri.UriSchemeHttps;
		}
		Uri uri = new UriBuilder(scheme, host, port, "/API/").Uri;
		if (uri.IsDefaultPort)
		{
			return new UriBuilder(scheme, host, -1, "/API/").Uri;
		}
		return uri;
	}

	public string GetHostName()
	{
		return Dns.GetHostName();
	}
}
