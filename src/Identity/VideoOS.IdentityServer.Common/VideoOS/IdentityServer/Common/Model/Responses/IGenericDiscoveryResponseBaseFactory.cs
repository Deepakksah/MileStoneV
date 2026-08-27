using System;
using System.Net;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public interface IGenericDiscoveryResponseBaseFactory<T> where T : DiscoveryResponseBase
{
	T Create(HttpStatusCode statusCode, string reason);

	T Create(Exception exception, string reason);

	T Create(Uri identityProviderUri, string httpContent);

	T Create(string reason, T[] errorResponses);
}
