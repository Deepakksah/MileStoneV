using System;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VideoOS.Management.Common;

namespace VideoOS.Server.Webservice.Shared;

public class ServerWebServiceManager : WebServiceManagerBase<ServerWebServiceClient, IServerWebService>
{
	private string _serverName;

	private int _port;

	public bool Active { get; set; }

	public ServerWebServiceManager(int port)
	{
		_serverName = "localhost";
		_port = port;
	}

	public ServerWebServiceManager(IWebServiceManagerExceptionHandler exceptionHandler, int port)
		: this(exceptionHandler, "localhost", port)
	{
	}

	public ServerWebServiceManager(IWebServiceManagerExceptionHandler exceptionHandler, string serverName, int port)
		: base(exceptionHandler)
	{
		_serverName = serverName;
		_port = port;
	}

	protected override ServerWebServiceClient CreateClientObject()
	{
		Binding binding = WebServiceManagerBase<ServerWebServiceClient, IServerWebService>.CreateDefaultBinding();
		Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}/ServerWebService", _serverName, _port));
		string spn = SpnFactory.GetSpn(uri);
		EndpointAddress remoteAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(spn));
		ServerWebServiceClient serverWebServiceClient = new ServerWebServiceClient(binding, remoteAddress);
		try
		{
			foreach (OperationDescription operation in serverWebServiceClient.Endpoint.Contract.Operations)
			{
				IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
				if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
				{
					dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
				}
			}
			NetworkCredential clientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
			serverWebServiceClient.ClientCredentials.Windows.ClientCredential = clientCredential;
			return serverWebServiceClient;
		}
		catch
		{
			serverWebServiceClient.Close();
			throw;
		}
	}
}
