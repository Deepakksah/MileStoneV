using System;
using System.Net.Http;
using VideoOS.EventServer.Communication.Rest.Client;
using VideoOS.EventServer.Maps.RestApi.Client;

namespace VideoOS.EventServer.Communication.AlarmsRestApiClient;

public class MapsRestApiClientManager : RestApiClientManagerBase<IClient, Client>
{
	public MapsRestApiClientManager()
		: base((Func<string, HttpClient, Client>)((string address, HttpClient httpClient) => new Client(address, httpClient)))
	{
	}
}
