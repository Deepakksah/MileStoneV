using System;
using System.Net;

namespace VideoOS.Common.WebSockets;

public class HttpResponseException : Exception
{
	public HttpStatusCode HttpStatusCode { get; }

	public HttpResponseException(HttpStatusCode httpStatusCode, string message)
		: base(message)
	{
		HttpStatusCode = httpStatusCode;
	}

	public HttpResponseException(HttpStatusCode httpStatusCode, string message, Exception innerException)
		: base(message, innerException)
	{
		HttpStatusCode = httpStatusCode;
	}

	public override string ToString()
	{
		return $"HttpStatus = {HttpStatusCode} {Environment.NewLine} {base.ToString()}";
	}
}
