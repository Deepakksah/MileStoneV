using System;
using System.Collections.Specialized;
using System.IO;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpListenerRequest
{
	bool IsWebSocketRequest { get; }

	string HttpMethod { get; }

	Uri Url { get; }

	string[] AcceptTypes { get; }

	string ContentType { get; }

	NameValueCollection Headers { get; }

	NameValueCollection QueryString { get; }

	Stream InputStream { get; }
}
