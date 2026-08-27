using System.Collections.Generic;
using System.Net;

namespace VideoOS.Mobile.ReverseProxy.Authorization;

public interface IAuthorizationProvider
{
	IEnumerable<string> AuthenticationHeaders { get; }

	string Authorize(HttpListenerRequest request);

	bool IsPresent(HttpListenerRequest request);
}
