#define TRACE
using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using VideoOS.Mobile.CorporateBasicLogin;

namespace VideoOS.Mobile.Utilities;

public class BasicUserLoginClientLegacy : ILoginClient
{
	private const string BASIC_USER_CORPORATE_SERVICE_FORMAT = "https://{0}/ManagementServer/ServerCommandService.svc";

	public NetworkCredential Credentials { get; set; }

	public Uri Url { get; set; }

	public void Login(Guid instanceId, string currentToken)
	{
		Uri uri = new UriBuilder(Url).Uri;
		NetworkCredential credential = Credentials.GetCredential(uri, "Basic");
		if (credential == null)
		{
			Trace.WriteLine("Unable to find basic credentials for XPCO");
		}
		NetworkCredential networkCredential = new NetworkCredential(credential.UserName, credential.Password, "[BASIC]");
		Uri uri2 = new Uri($"https://{uri.Host}/ManagementServer/ServerCommandService.svc");
		ServerCommandServiceClient serverCommandServiceClient = new ServerCommandServiceClient(GetBasicBinding(), new EndpointAddress(uri2, new SpnEndpointIdentity("host/localhost")));
		string userName = ((!string.IsNullOrEmpty(networkCredential.Domain)) ? (networkCredential.Domain + "\\" + networkCredential.UserName) : networkCredential.UserName);
		serverCommandServiceClient.ClientCredentials.UserName.UserName = userName;
		serverCommandServiceClient.ClientCredentials.UserName.Password = networkCredential.Password;
		try
		{
			serverCommandServiceClient.Open();
			serverCommandServiceClient.Login(instanceId, currentToken);
		}
		finally
		{
			serverCommandServiceClient.Close();
		}
	}

	private static Binding GetBasicBinding()
	{
		return new BasicHttpBinding
		{
			Security = 
			{
				Mode = BasicHttpSecurityMode.Transport,
				Transport = 
				{
					ClientCredentialType = HttpClientCredentialType.Basic
				}
			},
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			BypassProxyOnLocal = false,
			HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		};
	}
}
