using System;

namespace VideoOS.Management.Server.RestApi;

public interface IRestApiRequestContext : IGeminiRequestContext, IDisposable
{
	IRestApiRequestParameters RestApiRequestParameters { get; }
}
