using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpClientFactory
{
	IHttpClient Create();

	IHttpClient Create(HttpMessageHandler httpMessageHandler);

	IHttpClient Create(Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> serverCertificateCustomValidationCallback);
}
