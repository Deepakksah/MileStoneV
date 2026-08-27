using System;
using System.Net;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public abstract class DiscoveryResponseBase
{
	public bool IsError { get; }

	public HttpStatusCode StatusCode { get; }

	public string Error { get; }

	public Exception Exception { get; }

	public Uri AuthorizationServerUri { get; }

	public DiscoveryResponseBase[] ErrorResponses { get; }

	public DiscoveryResponseBase(HttpStatusCode statusCode, string reason)
	{
		IsError = true;
		StatusCode = statusCode;
		Error = reason;
	}

	public DiscoveryResponseBase(Exception exception, string reason)
	{
		IsError = true;
		Exception = exception;
		Error = reason;
	}

	public DiscoveryResponseBase(Uri identityProviderUri, string httpContent)
	{
		IsError = false;
		AuthorizationServerUri = identityProviderUri;
	}

	public DiscoveryResponseBase(string reason, DiscoveryResponseBase[] errorResponses)
	{
		IsError = true;
		Error = reason;
		ErrorResponses = errorResponses;
	}
}
