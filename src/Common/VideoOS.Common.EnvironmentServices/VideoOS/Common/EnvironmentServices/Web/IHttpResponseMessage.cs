using System;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpResponseMessage : IDisposable
{
	IHttpContent Content { get; }

	HttpStatusCode StatusCode { get; }

	string ReasonPhrase { get; }

	bool IsSuccessStatusCode { get; }

	IHttpResponseHeaders Headers { get; }

	void EnsureSuccessStatusCode();
}
