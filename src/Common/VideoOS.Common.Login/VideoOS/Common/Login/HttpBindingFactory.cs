using System;
using System.ServiceModel;

namespace VideoOS.Common.Login;

public sealed class HttpBindingFactory : IHttpBindingFactory
{
	private const int MaxCommValue = 500000000;

	public BasicHttpBinding CreateHttpBinding(string address)
	{
		BasicHttpSecurityMode mode = (IsHttps(address) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
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
			},
			Security = 
			{
				Mode = mode
			}
		};
	}

	public BasicHttpBinding CreateHttpBinding(string address, int millisecondsTimeout)
	{
		BasicHttpBinding basicHttpBinding = CreateHttpBinding(address);
		basicHttpBinding.SendTimeout = TimeSpan.FromMilliseconds(millisecondsTimeout);
		return basicHttpBinding;
	}

	public WSHttpBinding CreateHttpBindingCredentials(string address)
	{
		SecurityMode mode = (IsHttps(address) ? SecurityMode.TransportWithMessageCredential : SecurityMode.Message);
		return new WSHttpBinding
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
			},
			Security = 
			{
				Mode = mode,
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows,
					EstablishSecurityContext = false,
					NegotiateServiceCredential = true
				}
			}
		};
	}

	public WSHttpBinding CreateHttpBindingCredentials(string address, int millisecondsTimeout)
	{
		WSHttpBinding wSHttpBinding = CreateHttpBindingCredentials(address);
		wSHttpBinding.SendTimeout = TimeSpan.FromMilliseconds(millisecondsTimeout);
		return wSHttpBinding;
	}

	private bool IsHttps(string address)
	{
		return new Uri(address).Scheme == Uri.UriSchemeHttps;
	}
}
