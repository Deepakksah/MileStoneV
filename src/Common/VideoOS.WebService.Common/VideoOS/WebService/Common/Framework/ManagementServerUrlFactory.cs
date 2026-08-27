using System;

namespace VideoOS.WebService.Common.Framework;

public class ManagementServerUrlFactory
{
	public static Uri OauthEndpoint(string hostName, int port, string serviceName, bool useHttps)
	{
		return new UriBuilder(useHttps ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port, "ManagementServer/" + serviceName + "OAuth.svc").Uri;
	}

	public static Uri LegacyEndpoint(string hostName, int port, string serviceName, bool useHttps)
	{
		return new UriBuilder(useHttps ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port, "ManagementServer/" + serviceName + ".svc").Uri;
	}
}
