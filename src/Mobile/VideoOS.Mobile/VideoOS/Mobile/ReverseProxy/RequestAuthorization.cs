using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using VideoOS.Mobile.ReverseProxy.Authorization;

namespace VideoOS.Mobile.ReverseProxy;

public class RequestAuthorization
{
	private VideoOS.Mobile.ReverseProxy.Authorization.Authorization _authorization;

	private IAuthorizationProvider _authProvider;

	private HttpListenerRequest _request;

	private string _token;

	public RequestAuthorization(VideoOS.Mobile.ReverseProxy.Authorization.Authorization authorization, HttpListenerRequest request)
	{
		_authorization = authorization;
		_request = request;
	}

	public HttpStatusCode Authorize()
	{
		HttpStatusCode provider = _authorization.GetProvider(_request, out _authProvider);
		if (provider != HttpStatusCode.OK)
		{
			return provider;
		}
		_token = _authProvider.Authorize(_request);
		if (_token == null)
		{
			return HttpStatusCode.Unauthorized;
		}
		return HttpStatusCode.OK;
	}

	public void ApplyTokenHeader(HttpRequestMessage targetRequest)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		foreach (string authenticationHeader in _authProvider.AuthenticationHeaders)
		{
			if (((HttpHeaders)targetRequest.Headers).Contains(authenticationHeader))
			{
				((HttpHeaders)targetRequest.Headers).Remove(authenticationHeader);
			}
		}
		if (!string.IsNullOrEmpty(_token))
		{
			targetRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
		}
	}
}
