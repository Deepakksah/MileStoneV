using System;
using System.Collections.Generic;
using System.Net;

namespace VideoOS.Management.Server;

public class BasicAuthenticationErrorResponseData
{
	public HttpStatusCode StatusCode { get; }

	public IReadOnlyCollection<KeyValuePair<string, string>> Headers { get; }

	public BasicAuthenticationErrorResponseData(HttpStatusCode status)
		: this(status, Array.Empty<KeyValuePair<string, string>>())
	{
	}

	public BasicAuthenticationErrorResponseData(HttpStatusCode status, params KeyValuePair<string, string>[] headers)
	{
		StatusCode = status;
		Headers = headers;
	}
}
