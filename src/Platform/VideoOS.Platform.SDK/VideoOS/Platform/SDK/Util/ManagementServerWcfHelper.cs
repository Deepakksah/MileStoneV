using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.ServiceModel;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.OAuth;

namespace VideoOS.Platform.SDK.Util;

public class ManagementServerWcfHelper
{
	private class CustomSSLValidator : X509CertificateValidator
	{
		public override void Validate(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate", "Certificate was not supplied.");
			}
		}
	}

	public static Binding GetBinding(bool isHttps, string namespaceName)
	{
		Binding oAuthBinding = GetOAuthBinding(isHttps);
		oAuthBinding.Namespace = namespaceName;
		return oAuthBinding;
	}

	public static Binding GetOAuthBinding(bool isHttps)
	{
		return GetOAuthBinding(isHttps, TimeSpan.FromMinutes(10.0), TimeSpan.FromMinutes(10.0), TimeSpan.FromMinutes(1.0), TimeSpan.FromMinutes(1.0));
	}

	public static Binding GetOAuthBinding(bool isHttps, TimeSpan receiveTimeout, TimeSpan sendTimeout, TimeSpan openTimeout, TimeSpan closeTimeout)
	{
		BasicHttpBinding basicHttpBinding = CreateBasicHttpBinding(isHttps);
		basicHttpBinding.ReceiveTimeout = receiveTimeout;
		basicHttpBinding.SendTimeout = sendTimeout;
		basicHttpBinding.CloseTimeout = closeTimeout;
		basicHttpBinding.OpenTimeout = openTimeout;
		basicHttpBinding.BypassProxyOnLocal = false;
		basicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
		basicHttpBinding.TextEncoding = Encoding.UTF8;
		basicHttpBinding.UseDefaultWebProxy = true;
		basicHttpBinding.AllowCookies = false;
		basicHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
		return basicHttpBinding;
	}

	public static Binding GetNonOAuthBinding(bool isHttps)
	{
		BasicHttpBinding basicHttpBinding = CreateBasicHttpBinding(isHttps);
		basicHttpBinding.AllowCookies = true;
		return basicHttpBinding;
	}

	private static BasicHttpBinding CreateBasicHttpBinding(bool isHttps)
	{
		return new BasicHttpBinding
		{
			Security = 
			{
				Mode = (isHttps ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None)
			},
			ReaderQuotas = 
			{
				MaxStringContentLength = int.MaxValue
			},
			MaxReceivedMessageSize = 2147483647L,
			MaxBufferPoolSize = 2147483647L
		};
	}

	public static void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, IMipTokenCache mipTokenCache)
	{
		if (serviceEndpoint == null)
		{
			throw new ArgumentNullException("serviceEndpoint");
		}
		if (mipTokenCache == null)
		{
			throw new ArgumentNullException("mipTokenCache");
		}
		ITokenCache tokenCache = (mipTokenCache as MipTokenCacheInternal)?.TokenCache;
		if (tokenCache == null)
		{
			tokenCache = (mipTokenCache as MipTokenCache)?.TokenCache;
		}
		if (tokenCache == null)
		{
			tokenCache = (mipTokenCache as ManualMipTokenCache)?.TokenCache;
		}
		if (tokenCache == null)
		{
			throw new ArgumentNullException("mipTokenCache", "TokenCache could not be retrieved from given mipTokenCache");
		}
		serviceEndpoint.EndpointBehaviors.Add(new BearerTokenBehaviour(tokenCache));
		foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
	}

	public static EndpointAddress GetEndpointAddress(Uri serverUrl)
	{
		if (serverUrl != null)
		{
			return new EndpointAddress(serverUrl.AbsoluteUri);
		}
		throw new NotImplementedException("Authentication type not supported");
	}

	public static void SetupCertificateValidation<T>(ClientBase<T> soapClient) where T : class
	{
		soapClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
		soapClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
		{
			CertificateValidationMode = X509CertificateValidationMode.Custom,
			CustomCertificateValidator = new CustomSSLValidator()
		};
	}

	public static ClientBase<T> DisposeSoapClient<T>(ClientBase<T> soapClient) where T : class
	{
		try
		{
			soapClient.Close();
		}
		catch (CommunicationException)
		{
			soapClient.Abort();
		}
		catch (TimeoutException)
		{
			soapClient.Abort();
		}
		catch (Exception)
		{
			soapClient.Abort();
			throw;
		}
		soapClient = null;
		return soapClient;
	}
}
