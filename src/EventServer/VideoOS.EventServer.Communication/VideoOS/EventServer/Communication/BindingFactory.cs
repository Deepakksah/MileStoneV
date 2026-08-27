using System.ServiceModel;

namespace VideoOS.EventServer.Communication;

public static class BindingFactory
{
	private const int MessageBufferSize = 500000000;

	public static BasicHttpBinding CreateBasicHttpBindingCredentials()
	{
		BasicHttpBinding basicHttpBinding = CreateBasicHttpBindingPlain();
		basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
		basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
		return basicHttpBinding;
	}

	public static BasicHttpBinding CreateBasicHttpBindingToken(bool isHttps)
	{
		BasicHttpBinding basicHttpBinding = CreateBasicHttpBindingPlain();
		if (isHttps)
		{
			basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
		}
		return basicHttpBinding;
	}

	private static BasicHttpBinding CreateBasicHttpBindingPlain()
	{
		return new BasicHttpBinding
		{
			MaxReceivedMessageSize = 500000000L,
			MaxBufferSize = 500000000,
			MaxBufferPoolSize = 500000000L,
			ReaderQuotas = 
			{
				MaxArrayLength = 500000000,
				MaxBytesPerRead = 500000000,
				MaxNameTableCharCount = 500000000,
				MaxDepth = 500000000,
				MaxStringContentLength = 500000000
			}
		};
	}

	public static WSHttpBinding CreateWSHttpBindingCredentials(bool isHttps)
	{
		WSHttpBinding wSHttpBinding = CreateWSHttpBindingPlain();
		wSHttpBinding.Security.Mode = (isHttps ? SecurityMode.TransportWithMessageCredential : SecurityMode.Message);
		wSHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
		wSHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
		wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
		wSHttpBinding.Security.Message.EstablishSecurityContext = false;
		wSHttpBinding.Security.Message.NegotiateServiceCredential = true;
		return wSHttpBinding;
	}

	private static WSHttpBinding CreateWSHttpBindingPlain()
	{
		return new WSHttpBinding(SecurityMode.None)
		{
			MaxReceivedMessageSize = 500000000L,
			MaxBufferPoolSize = 500000000L,
			ReaderQuotas = 
			{
				MaxArrayLength = 500000000,
				MaxBytesPerRead = 500000000,
				MaxNameTableCharCount = 500000000,
				MaxDepth = 500000000,
				MaxStringContentLength = 500000000
			}
		};
	}
}
