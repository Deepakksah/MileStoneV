using System;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpWebResponse : IWebResponse, IDisposable
{
	HttpStatusCode StatusCode { get; }
}
