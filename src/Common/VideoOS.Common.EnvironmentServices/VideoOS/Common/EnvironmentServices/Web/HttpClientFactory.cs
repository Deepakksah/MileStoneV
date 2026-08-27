using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpClientFactory : IHttpClientFactory
{
	public IHttpClient Create()
	{
		return new HttpClient();
	}

	public IHttpClient Create(HttpMessageHandler httpMessageHandler)
	{
		return new HttpClient(httpMessageHandler);
	}

	public IHttpClient Create(Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> serverCertificateCustomValidationCallback)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		return new HttpClient((HttpMessageHandler)new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = serverCertificateCustomValidationCallback
		});
	}
}
