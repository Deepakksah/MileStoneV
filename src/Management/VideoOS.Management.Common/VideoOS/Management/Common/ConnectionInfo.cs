using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace VideoOS.Management.Common;

public class ConnectionInfo
{
	public Binding Binding { get; private set; }

	public NetworkCredential Credentials { get; private set; }

	public Uri ServerUri { get; private set; }

	public bool IsBasicAuthentication { get; private set; }

	public ConnectionInfo(Uri serverUri, Binding binding, NetworkCredential credentials)
	{
		bool isBasicAuthentication = binding.GetType() != typeof(WSHttpBinding);
		InitializeProperties(serverUri, isBasicAuthentication, binding, credentials);
	}

	public ConnectionInfo(Uri serverUri, bool isBasicAuthentication, NetworkCredential credentials)
	{
		Binding binding = GetBinding(isBasicAuthentication);
		InitializeProperties(serverUri, isBasicAuthentication, binding, credentials);
	}

	public ConnectionInfo(Uri serverUri, Binding binding, ClientCredentials credentials)
	{
		bool isBasicAuthentication = binding.GetType() != typeof(WSHttpBinding);
		NetworkCredential credentials2 = ConvertToNetworkCredential(isBasicAuthentication, credentials);
		InitializeProperties(serverUri, isBasicAuthentication, binding, credentials2);
	}

	public ConnectionInfo(Uri serverUri, bool isBasicAuthentication, ClientCredentials credentials)
	{
		Binding binding = GetBinding(isBasicAuthentication);
		NetworkCredential credentials2 = ConvertToNetworkCredential(isBasicAuthentication, credentials);
		InitializeProperties(serverUri, isBasicAuthentication, binding, credentials2);
	}

	private void InitializeProperties(Uri serverUri, bool isBasicAuthentication, Binding binding, NetworkCredential credentials)
	{
		ServerUri = serverUri;
		Binding = binding;
		Credentials = credentials;
		IsBasicAuthentication = isBasicAuthentication;
	}

	private static Binding GetBinding(bool isBasicBinding)
	{
		if (isBasicBinding)
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
				MaxBufferPoolSize = 524288L,
				MaxReceivedMessageSize = 2147483647L,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max
			};
		}
		return new WSHttpBinding
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			},
			MaxBufferPoolSize = 524288L,
			MaxReceivedMessageSize = 2147483647L,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		};
	}

	private static NetworkCredential ConvertToNetworkCredential(bool isBasicAuthentication, ClientCredentials credentials)
	{
		if (!isBasicAuthentication)
		{
			return credentials.Windows.ClientCredential;
		}
		if (!credentials.UserName.UserName.Contains("\\"))
		{
			return new NetworkCredential(credentials.UserName.UserName, credentials.UserName.Password);
		}
		string domain = credentials.UserName.UserName.Split('\\')[0];
		return new NetworkCredential(credentials.UserName.UserName.Split('\\')[1], credentials.UserName.Password, domain);
	}
}
