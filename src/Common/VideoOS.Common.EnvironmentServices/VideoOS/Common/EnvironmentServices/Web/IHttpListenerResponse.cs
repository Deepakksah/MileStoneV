using System;
using System.IO;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpListenerResponse : IDisposable
{
	string ContentType { get; set; }

	long ContentLength64 { get; set; }

	WebHeaderCollection Headers { get; set; }

	Stream OutputStream { get; }

	int StatusCode { get; set; }

	string StatusDescription { get; set; }

	void Close();
}
