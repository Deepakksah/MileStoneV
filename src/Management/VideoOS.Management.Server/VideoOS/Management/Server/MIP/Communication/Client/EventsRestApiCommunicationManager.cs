#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Services;
using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.Management.Common;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Management.Server.MIP.Communication.Client;

public class EventsRestApiCommunicationManager
{
	private RestApiCommunicationManager<IClient, VideoOS.EventServer.Events.RestApi.Client.Client> _communicationManager;

	private readonly ILogger _logger;

	private RestApiCommunicationManager<IClient, VideoOS.EventServer.Events.RestApi.Client.Client> CommunicationManager
	{
		get
		{
			if (_communicationManager == null)
			{
				_communicationManager = CreateCommunicationManager();
			}
			return _communicationManager;
		}
	}

	public IClient EventsClientRest => CommunicationManager?.RestClient;

	public EventsRestApiCommunicationManager()
	{
		_logger = VideoOS.Utilities.Telemetry.Telemetry.GetILogger("WebApi");
	}

	public void SentEventsToConsumer(IEnumerable<PostEventBody> bodies)
	{
		if (bodies == null)
		{
			throw new ArgumentNullException("bodies");
		}
		if (EventsClientRest == null)
		{
			Trace.WriteLine("EventsRestApiCommunicationManager: Event Server offline - SentEventsToConsumer call ignored");
			return;
		}
		try
		{
			ClientWithBearerToken.WithRetryAsync(async () => await EventsClientRest.Events_PostBulkAsync(bodies, false).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}
		catch (Exception arg)
		{
			_logger.LogError($"Error sending events Type: ex: {arg}");
		}
	}

	private RestApiCommunicationManager<IClient, VideoOS.EventServer.Events.RestApi.Client.Client> CreateCommunicationManager()
	{
		RegisteredServer registeredServer;
		using (GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext())
		{
			registeredServer = RegisteredServer.QueryRegisteredServersByType(requestContext, ServiceTypes.ServiceType_EventServerService).FirstOrDefault();
			if (registeredServer == null)
			{
				_logger.LogInformation("No registered instance of Event Server.");
				return null;
			}
			ServerAddress restServicesAddress = registeredServer.GetRestServicesAddress();
			if (restServicesAddress == null)
			{
				_logger.LogInformation("Event server has not any registered address.");
				return null;
			}
			restServicesAddress.URL = restServicesAddress.URL;
		}
		return new RestApiCommunicationManager<IClient, VideoOS.EventServer.Events.RestApi.Client.Client>((string address, HttpClient httpClient) => new VideoOS.EventServer.Events.RestApi.Client.Client(address, httpClient), TokenCacheService.ManagementServerTokenCacheInstance, registeredServer, _logger, "rest/events/v1/");
	}
}
