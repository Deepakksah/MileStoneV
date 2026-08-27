using System;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;

namespace VideoOS.Installer.Common.Utilities;

[CLSCompliant(false)]
public class ConfigurationApiServiceManager : WebServiceManagerBase<ConfigurationApiServiceClient, IConfigurationService>
{
	private int _servicePort;

	private string _serverName;

	public ConfigurationApiServiceManager(int servicePort)
		: this("localhost", servicePort)
	{
	}

	public ConfigurationApiServiceManager(string serverName, int servicePort)
	{
		_servicePort = servicePort;
		_serverName = serverName;
	}

	protected override ConfigurationApiServiceClient CreateClientObject()
	{
		Binding binding = WebServiceManagerBase<ConfigurationApiServiceClient, IConfigurationService>.CreateDefaultBinding();
		Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}/ManagementServer/ConfigurationApiService.svc", _serverName, _servicePort));
		string spn = SpnFactory.GetSpn(uri);
		EndpointAddress remoteAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(spn));
		ConfigurationApiServiceClient configurationApiServiceClient = new ConfigurationApiServiceClient(binding, remoteAddress);
		foreach (OperationDescription operation in configurationApiServiceClient.Endpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
		NetworkCredential clientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
		configurationApiServiceClient.ClientCredentials.Windows.ClientCredential = clientCredential;
		return configurationApiServiceClient;
	}
}
