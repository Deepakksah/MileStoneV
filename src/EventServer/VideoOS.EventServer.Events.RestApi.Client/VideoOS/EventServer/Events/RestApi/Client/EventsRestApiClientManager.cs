using System;
using System.Net.Http;
using VideoOS.EventServer.Common.RestApi.Client;

namespace VideoOS.EventServer.Events.RestApi.Client;

public class EventsRestApiClientManager : RestApiClientManagerBase<IClient, Client>
{
	public EventsRestApiClientManager()
		: base((Func<string, HttpClient, Client>)((string address, HttpClient httpClient) => new Client(address, httpClient)))
	{
	}
}
