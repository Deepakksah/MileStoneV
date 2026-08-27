using System;

namespace VideoOS.GatewayService.Utilities;

public interface IUriHelper
{
	Uri GetGatewayServiceUri(string webSiteId, bool isSecure, string host);

	string GetHostName();
}
