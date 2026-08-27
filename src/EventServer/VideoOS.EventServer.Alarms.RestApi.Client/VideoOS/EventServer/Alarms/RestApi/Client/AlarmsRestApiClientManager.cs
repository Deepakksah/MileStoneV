using System;
using System.Net.Http;
using VideoOS.EventServer.Common.RestApi.Client;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

public class AlarmsRestApiClientManager : RestApiClientManagerBase<IClient, Client>
{
	public AlarmsRestApiClientManager()
		: base((Func<string, HttpClient, Client>)((string address, HttpClient httpClient) => new Client(address, httpClient)))
	{
	}
}
