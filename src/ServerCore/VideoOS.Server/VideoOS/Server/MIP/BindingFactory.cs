using System.ServiceModel;

namespace VideoOS.Server.MIP;

public static class BindingFactory
{
	private const int MESSAGE_BUFFER_SIZE = 500000000;

	public static BasicHttpBinding CreateBasicHttpBindingCredentials()
	{
		BasicHttpBinding basicHttpBinding = CreateBasicHttpBindingPlain();
		basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
		basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
		return basicHttpBinding;
	}

	private static BasicHttpBinding CreateBasicHttpBindingPlain()
	{
		BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
		basicHttpBinding.MaxReceivedMessageSize = 500000000L;
		basicHttpBinding.MaxBufferSize = 500000000;
		basicHttpBinding.MaxBufferPoolSize = 500000000L;
		basicHttpBinding.ReaderQuotas.MaxArrayLength = 500000000;
		basicHttpBinding.ReaderQuotas.MaxBytesPerRead = 500000000;
		basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = 500000000;
		basicHttpBinding.ReaderQuotas.MaxDepth = 500000000;
		basicHttpBinding.ReaderQuotas.MaxStringContentLength = 500000000;
		return basicHttpBinding;
	}

	public static WSHttpBinding CreateWSHttpBindingCredentials()
	{
		WSHttpBinding wSHttpBinding = CreateWSHttpBindingPlain();
		wSHttpBinding.Security.Mode = SecurityMode.Message;
		wSHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
		wSHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
		wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
		wSHttpBinding.Security.Message.EstablishSecurityContext = false;
		wSHttpBinding.Security.Message.NegotiateServiceCredential = true;
		return wSHttpBinding;
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

	public static WSHttpBinding CreateWSHttpBindingPlain()
	{
		WSHttpBinding wSHttpBinding = new WSHttpBinding(SecurityMode.None);
		wSHttpBinding.MaxReceivedMessageSize = 500000000L;
		wSHttpBinding.MaxBufferPoolSize = 500000000L;
		wSHttpBinding.ReaderQuotas.MaxArrayLength = 500000000;
		wSHttpBinding.ReaderQuotas.MaxBytesPerRead = 500000000;
		wSHttpBinding.ReaderQuotas.MaxNameTableCharCount = 500000000;
		wSHttpBinding.ReaderQuotas.MaxDepth = 500000000;
		wSHttpBinding.ReaderQuotas.MaxStringContentLength = 500000000;
		return wSHttpBinding;
	}
}
