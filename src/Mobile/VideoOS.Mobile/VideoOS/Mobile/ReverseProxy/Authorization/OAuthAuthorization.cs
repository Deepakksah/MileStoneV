using System;
using System.Collections.Generic;
using System.Net;

namespace VideoOS.Mobile.ReverseProxy.Authorization;

public class OAuthAuthorization : IAuthorizationProvider
{
	public const string ApiGatewayHeader = "Mobile-Connection-Id";

	private readonly ITokenProvider _tokenProvider;

	private static readonly IEnumerable<string> _authHeaders = new string[1] { "Mobile-Connection-Id" };

	public IEnumerable<string> AuthenticationHeaders => _authHeaders;

	public OAuthAuthorization(ITokenProvider tokenProvider)
	{
		_tokenProvider = tokenProvider ?? throw new ArgumentNullException("tokenProvider");
	}

	public string Authorize(HttpListenerRequest request)
	{
		string text = request.Headers["Mobile-Connection-Id"];
		if (!string.IsNullOrEmpty(text))
		{
			return _tokenProvider.GetOAuthToken(text);
		}
		return null;
	}

	public bool IsPresent(HttpListenerRequest request)
	{
		return !string.IsNullOrWhiteSpace(request.Headers["Mobile-Connection-Id"]);
	}
}
