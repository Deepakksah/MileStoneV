using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace VideoOS.LPR.Common.WCF;

public static class WebServiceSettings
{
	private const int _defaultWebServicePort = 22334;

	private const int _defaultConcurrentConnections = 100;

	private const int _defaultReceiveTimeout = 120000;

	private const int _messageBufferSize = 500000000;

	private const int _maxItemsInObjectGraph = 1000000;

	public static int ServerWebServicePort
	{
		get
		{
			if (!int.TryParse(ConfigurationManager.AppSettings["LPRWebServicePort"], out var result))
			{
				return 22334;
			}
			return result;
		}
	}

	public static int PluginWebServicePort => 22331;

	public static int ConcurrentConnections
	{
		get
		{
			if (!int.TryParse(ConfigurationManager.AppSettings["LPRWebServiceConcurrentConnections"], out var result))
			{
				return 100;
			}
			return result;
		}
	}

	public static int ReceiveTimeout
	{
		get
		{
			if (!int.TryParse(ConfigurationManager.AppSettings["LPRWebServiceReceiveTimeout"], out var result))
			{
				return 120000;
			}
			return result;
		}
	}

	public static WSHttpBinding CreateServerWSHttpBinding()
	{
		return new WSHttpBinding(SecurityMode.Message)
		{
			ReceiveTimeout = TimeSpan.FromMilliseconds(ReceiveTimeout),
			Security = 
			{
				Message = 
				{
					AlgorithmSuite = SecurityAlgorithmSuite.Default,
					ClientCredentialType = MessageCredentialType.Windows,
					NegotiateServiceCredential = true,
					EstablishSecurityContext = false
				}
			},
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

	public static BasicHttpBinding CreateServerBasicHttpBinding()
	{
		return new BasicHttpBinding
		{
			ReceiveTimeout = TimeSpan.FromMilliseconds(ReceiveTimeout),
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

	public static void SetMaxItemsInObjectGraph(IEnumerable<ServiceEndpoint> endpoints)
	{
		foreach (ServiceEndpoint endpoint in endpoints)
		{
			foreach (OperationDescription operation in endpoint.Contract.Operations)
			{
				DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
				if (dataContractSerializerOperationBehavior != null)
				{
					dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 1000000;
				}
			}
		}
	}
}
