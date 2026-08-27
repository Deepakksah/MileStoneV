using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using VideoOS.Utilities.Validation;

namespace VideoOS.ServiceHost.Common;

public sealed class WebServiceUtils : IWebServiceUtils
{
	private readonly IWebServiceSettings _webServiceSettings;

	private const int MAX_CONCURRENT_INSTANCES_MULTIPLIER = 2;

	public const int DEFAULT_MAX_MESSAGE_SIZE = 524288000;

	public WebServiceUtils(IWebServiceSettings webServiceSettings)
	{
		webServiceSettings.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\WebServiceUtils.cs", 19);
		_webServiceSettings = webServiceSettings;
	}

	public WSHttpBinding CreateWSHttpBinding(SecurityMode securityMode, TimeSpan receiveTimeout, WSMessageEncoding encoding, int maxMessageSize = 524288000)
	{
		WSHttpBinding wSHttpBinding = new WSHttpBinding(securityMode)
		{
			MessageEncoding = encoding
		};
		SetupCommonBinding(wSHttpBinding, receiveTimeout, maxMessageSize);
		if (securityMode == SecurityMode.Message || securityMode == SecurityMode.TransportWithMessageCredential)
		{
			wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			wSHttpBinding.Security.Message.NegotiateServiceCredential = true;
			wSHttpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
			wSHttpBinding.Security.Message.EstablishSecurityContext = false;
		}
		return wSHttpBinding;
	}

	public WSHttpBinding CreateWSHttpBinding(SecurityMode securityMode, TimeSpan receiveTimeout, MessageCredentialType encoding, int maxMessageSize = 524288000)
	{
		WSHttpBinding wSHttpBinding = new WSHttpBinding(securityMode);
		SetupCommonBinding(wSHttpBinding, receiveTimeout, maxMessageSize);
		wSHttpBinding.Security.Message.ClientCredentialType = encoding;
		return wSHttpBinding;
	}

	public BasicHttpBinding CreateBasicHttpBinding(TimeSpan receiveTimeout, int maxMessageSize = 524288000)
	{
		BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
		SetupCommonBinding(basicHttpBinding, receiveTimeout, maxMessageSize);
		return basicHttpBinding;
	}

	public ServiceThrottlingBehavior CreateServiceThrottlingBehavior()
	{
		return new ServiceThrottlingBehavior
		{
			MaxConcurrentCalls = _webServiceSettings.ConcurrentConnections,
			MaxConcurrentSessions = _webServiceSettings.ConcurrentConnections,
			MaxConcurrentInstances = _webServiceSettings.ConcurrentConnections * 2
		};
	}

	public ServiceMetadataBehavior CreateServiceMetadataBehavior(SecurityMode securityMode)
	{
		return new ServiceMetadataBehavior
		{
			HttpGetEnabled = (securityMode != SecurityMode.Transport && securityMode != SecurityMode.TransportWithMessageCredential),
			HttpsGetEnabled = (securityMode == SecurityMode.Transport || securityMode == SecurityMode.TransportWithMessageCredential)
		};
	}

	public ServiceMetadataBehavior CreateServiceMetadataBehavior(bool securityMode)
	{
		return new ServiceMetadataBehavior
		{
			HttpGetEnabled = !securityMode,
			HttpsGetEnabled = securityMode
		};
	}

	private static void SetupCommonBinding(WSHttpBinding wsHttpBinding, TimeSpan receiveTimeout, int maxMessageSize)
	{
		wsHttpBinding.MaxReceivedMessageSize = maxMessageSize;
		wsHttpBinding.MaxBufferPoolSize = maxMessageSize;
		wsHttpBinding.ReaderQuotas.MaxArrayLength = maxMessageSize;
		wsHttpBinding.ReaderQuotas.MaxBytesPerRead = maxMessageSize;
		wsHttpBinding.ReaderQuotas.MaxNameTableCharCount = maxMessageSize;
		wsHttpBinding.ReaderQuotas.MaxDepth = maxMessageSize;
		wsHttpBinding.ReaderQuotas.MaxStringContentLength = maxMessageSize;
		wsHttpBinding.ReceiveTimeout = receiveTimeout;
	}

	private static void SetupCommonBinding(BasicHttpBinding basicHttpBinding, TimeSpan receiveTimeout, int maxMessageSize)
	{
		basicHttpBinding.MaxReceivedMessageSize = maxMessageSize;
		basicHttpBinding.MaxBufferSize = maxMessageSize;
		basicHttpBinding.MaxBufferPoolSize = maxMessageSize;
		basicHttpBinding.ReaderQuotas.MaxArrayLength = maxMessageSize;
		basicHttpBinding.ReaderQuotas.MaxBytesPerRead = maxMessageSize;
		basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = maxMessageSize;
		basicHttpBinding.ReaderQuotas.MaxDepth = maxMessageSize;
		basicHttpBinding.ReaderQuotas.MaxStringContentLength = maxMessageSize;
		basicHttpBinding.ReceiveTimeout = receiveTimeout;
	}
}
