using System.Net.Http;
using Microsoft.Extensions.Logging;
using VideoOS.EventServer.Alarms.RestApi.Client;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server.MIP.Communication.Client;

public class AlarmsRestApiCommunicationManager
{
	private readonly RestApiCommunicationManager<IClient, VideoOS.EventServer.Alarms.RestApi.Client.Client> _communicationManager;

	public IClient AlarmsClientRest => _communicationManager.RestClient;

	public AlarmsRestApiCommunicationManager(ITokenCache tokenCache, RegisteredServer gatewayServerAddress, ILogger logger)
	{
		_communicationManager = new RestApiCommunicationManager<IClient, VideoOS.EventServer.Alarms.RestApi.Client.Client>((string address, HttpClient httpClient) => new VideoOS.EventServer.Alarms.RestApi.Client.Client(address, httpClient), tokenCache, gatewayServerAddress, logger, "rest/alarms/v1/");
	}
}
