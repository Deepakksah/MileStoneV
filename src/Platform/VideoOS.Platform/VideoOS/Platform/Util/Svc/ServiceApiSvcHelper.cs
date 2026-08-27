using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.Util.Svc;

public class ServiceApiSvcHelper
{
	public static Binding GetBinding(ICredentials credentials, Uri serverAddress, bool basicUser)
	{
		if (basicUser)
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
				TextEncoding = Encoding.UTF8,
				UseDefaultWebProxy = true,
				AllowCookies = false,
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
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			BypassProxyOnLocal = false,
			TransactionFlow = false,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		};
	}

	public static Binding GetOAuthBinding(bool isHttps)
	{
		BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
		basicHttpBinding.Security.Mode = (isHttps ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
		basicHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
		basicHttpBinding.MaxReceivedMessageSize = 2147483647L;
		basicHttpBinding.MaxBufferPoolSize = 2147483647L;
		if (!Debugger.IsAttached)
		{
			basicHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10.0);
			basicHttpBinding.SendTimeout = TimeSpan.FromMinutes(10.0);
			basicHttpBinding.CloseTimeout = TimeSpan.FromMinutes(10.0);
		}
		basicHttpBinding.BypassProxyOnLocal = false;
		basicHttpBinding.TextEncoding = Encoding.UTF8;
		basicHttpBinding.UseDefaultWebProxy = true;
		basicHttpBinding.AllowCookies = false;
		basicHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
		return basicHttpBinding;
	}

	public static void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint)
	{
		foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
	}

	public static void SetCredentials(ClientCredentials clientCredentials, ICredentials credentials, Uri serverAddress, bool isBasicUser)
	{
		if (EnvironmentManager.Instance.TraceFunctionCalls)
		{
			EnvironmentManager.Instance.Log(error: false, "ServiceApiSvcHelper.SetCredentials", "Basic=" + isBasicUser);
		}
		if (isBasicUser)
		{
			Uri uri = new UriBuilder(serverAddress.Scheme, serverAddress.Host, serverAddress.Port).Uri;
			NetworkCredential credential = credentials.GetCredential(uri, "Basic");
			string userName = ((!string.IsNullOrEmpty(credential.Domain)) ? (credential.Domain + "\\" + credential.UserName) : credential.UserName);
			clientCredentials.UserName.UserName = userName;
			clientCredentials.UserName.Password = credential.Password;
		}
		else if (credentials is NetworkCredential)
		{
			clientCredentials.Windows.ClientCredential = credentials as NetworkCredential;
		}
		else
		{
			clientCredentials.Windows.ClientCredential = credentials.GetCredential(serverAddress, "Negotiate");
		}
	}
}
