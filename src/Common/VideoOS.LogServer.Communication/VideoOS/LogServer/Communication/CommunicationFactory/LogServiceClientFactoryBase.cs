using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.MIP;
using VideoOS.Platform;
using VideoOS.Utilities;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public abstract class LogServiceClientFactoryBase<T> : IClientFactory<T> where T : ICommunicationObject
{
	protected readonly ILogger Logger;

	private static readonly Guid LogServerTypeId = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	protected LogServiceClientFactoryBase(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException("logger");
	}

	public abstract T Create();

	protected EndpointAddress LookupServerAddress(IEnvironmentWrapper environmentWrapper, string endpoint, bool isToken = false)
	{
		try
		{
			Uri logServerUri = null;
			string serviceData = null;
			IEnumerable<Configuration.ServiceURIInfo> registeredServiceUriInfo = environmentWrapper.GetRegisteredServiceUriInfo(LogServerTypeId);
			if (registeredServiceUriInfo != null && registeredServiceUriInfo.Count() > 0 && registeredServiceUriInfo.First().UriArray?.FirstOrDefault() != null)
			{
				serviceData = registeredServiceUriInfo.First().ServiceData;
				logServerUri = new Uri(registeredServiceUriInfo.First().UriArray?.First());
			}
			return LookupServerAddress(logServerUri, endpoint, isToken, serviceData);
		}
		catch (CommunicationMIPException ex)
		{
			LoggerExtensions.LogError(Logger, "Failed to obtain registered services: {0}", new object[1] { ex });
			throw new CommunicationServiceException(CommunicationExceptionMessage.FailedToObtainRegisteredServices);
		}
	}

	protected EndpointAddress LookupServerAddress(Uri logServerUri, string endpoint, bool isToken = false, string serviceData = null)
	{
		if (logServerUri == null)
		{
			LoggerExtensions.LogError(Logger, "Log Server not registered", Array.Empty<object>());
			throw new CommunicationServiceException(CommunicationExceptionMessage.LogServerNotRegistered);
		}
		try
		{
			if (isToken && !string.IsNullOrEmpty(serviceData))
			{
				try
				{
					X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(serviceData));
					return new EndpointAddress(new Uri(logServerUri, endpoint), CreateX509CertificateIdentity(certificate));
				}
				catch (CryptographicException)
				{
				}
			}
			return new EndpointAddress(new Uri(logServerUri, endpoint), CreateSpnIdentity(SpnFactory.GetSpn(logServerUri)));
		}
		catch (UriFormatException ex2)
		{
			LoggerExtensions.LogError(Logger, "Invalid Log Server address registered: {0}", new object[1] { ex2 });
			throw new CommunicationServiceException(CommunicationExceptionMessage.InvalidLogServerAddress);
		}
	}

	protected static Binding GetBinding(LogServiceClientBindingParameters bindingParameters)
	{
		return new WSHttpBinding(SecurityMode.Message)
		{
			BypassProxyOnLocal = false,
			TransactionFlow = false,
			HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			OpenTimeout = bindingParameters.OpenTimeout,
			CloseTimeout = bindingParameters.CloseTimeout,
			SendTimeout = bindingParameters.SendTimeout,
			ReceiveTimeout = bindingParameters.ReceiveTimeout,
			MaxReceivedMessageSize = 2147483647L,
			MaxBufferPoolSize = 2147483647L,
			ReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxArrayLength = int.MaxValue,
				MaxBytesPerRead = int.MaxValue,
				MaxNameTableCharCount = int.MaxValue,
				MaxDepth = int.MaxValue,
				MaxStringContentLength = int.MaxValue
			},
			Security = new WSHttpSecurity
			{
				Message = new NonDualMessageSecurityOverHttp
				{
					ClientCredentialType = MessageCredentialType.Windows,
					NegotiateServiceCredential = true,
					AlgorithmSuite = SecurityAlgorithmSuite.Default,
					EstablishSecurityContext = true
				}
			}
		};
	}

	protected static Binding GetTokenBinding()
	{
		return new WSHttpBinding(SecurityMode.Message)
		{
			BypassProxyOnLocal = false,
			TransactionFlow = false,
			HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			OpenTimeout = TimeSpan.FromMinutes(10.0),
			CloseTimeout = TimeSpan.FromMinutes(1.0),
			SendTimeout = TimeSpan.FromMinutes(10.0),
			ReceiveTimeout = TimeSpan.FromMinutes(10.0),
			MaxReceivedMessageSize = 2147483647L,
			MaxBufferPoolSize = 2147483647L,
			ReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxArrayLength = int.MaxValue,
				MaxBytesPerRead = int.MaxValue,
				MaxNameTableCharCount = int.MaxValue,
				MaxDepth = int.MaxValue,
				MaxStringContentLength = int.MaxValue
			},
			Security = new WSHttpSecurity
			{
				Message = new NonDualMessageSecurityOverHttp
				{
					ClientCredentialType = MessageCredentialType.None,
					NegotiateServiceCredential = false,
					AlgorithmSuite = SecurityAlgorithmSuite.Default,
					EstablishSecurityContext = false
				}
			}
		};
	}

	protected static void UpdateEndPoint(ServiceEndpoint endPoint)
	{
		foreach (OperationDescription operation in endPoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
	}

	protected static EndpointIdentity CreateX509CertificateIdentity(X509Certificate2 certificate)
	{
		return EndpointIdentity.CreateX509CertificateIdentity(certificate);
	}

	protected static EndpointIdentity CreateSpnIdentity(string spnName)
	{
		return EndpointIdentity.CreateSpnIdentity(spnName);
	}
}
