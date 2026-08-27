using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;

namespace VideoOS.Shared.ServerAPI;

public class ServiceApiSvcHelper
{
	public static Binding GetBinding(ICredentials credentials, Uri serverAddress)
	{
		Binding binding = ((!IsBasicUser(credentials, serverAddress)) ? ((Binding)new WSHttpBinding
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			},
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		}) : ((Binding)new BasicHttpBinding
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
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		}));
		TimeSpan sendTimeout = TimeSpan.FromSeconds(15.0);
		TimeSpan openTimeout = TimeSpan.FromSeconds(15.0);
		TimeSpan closeTimeout = TimeSpan.FromSeconds(15.0);
		TimeSpan receiveTimeout = TimeSpan.FromSeconds(15.0);
		binding.SendTimeout = sendTimeout;
		binding.OpenTimeout = openTimeout;
		binding.CloseTimeout = closeTimeout;
		binding.ReceiveTimeout = receiveTimeout;
		return binding;
	}

	public static bool IsBasicUser(ICredentials credentials, Uri serverAddress)
	{
		Uri uri = new UriBuilder(serverAddress.Scheme, serverAddress.Host, serverAddress.Port).Uri;
		if (credentials.GetCredential(uri, "Basic") != null)
		{
			return true;
		}
		return false;
	}

	public static void SetCredentials(ClientCredentials clientCredentials, ICredentials credentials, Uri serverAddress)
	{
		if (IsBasicUser(credentials, serverAddress))
		{
			Uri uri = new UriBuilder(serverAddress.Scheme, serverAddress.Host, serverAddress.Port).Uri;
			NetworkCredential credential = credentials.GetCredential(uri, "Basic");
			string userName = ((!string.IsNullOrEmpty(credential.Domain)) ? (credential.Domain + "\\" + credential.UserName) : credential.UserName);
			clientCredentials.UserName.UserName = userName;
			clientCredentials.UserName.Password = credential.Password;
			clientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
		}
		else
		{
			clientCredentials.Windows.ClientCredential = credentials.GetCredential(serverAddress, "Negotiate");
		}
	}
}
