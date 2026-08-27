using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Event.Server.WebService.Action.Client;
using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.MIP.Communication.Client.EventConverter;

namespace VideoOS.Server.MIP;

public sealed class EventServerCommunicationManager : IDisposable, IEventConsumer
{
	private ActionClientCredentials _actionClient;

	private bool _disposed;

	private ILogger _logger;

	private readonly object _eventClientLock = new object();

	private Client _eventClient;

	private DateTime _tokenTimeout = DateTime.MinValue;

	private ITokenCache _tokenCache;

	private DateTime _lastLogEntryAboutError = DateTime.MinValue;

	private ActionClientCredentials ActionClient
	{
		get
		{
			if (_actionClient == null)
			{
				_actionClient = new ActionClientCredentials();
			}
			return _actionClient;
		}
	}

	private IClient EventClientRest
	{
		get
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			lock (_eventClientLock)
			{
				if (_eventClient == null)
				{
					RegisteredServer gatewayServerRegistration = AbstractEventServerClient.GetGatewayServerRegistration();
					if (gatewayServerRegistration != null && gatewayServerRegistration.Addresses.Length != 0)
					{
						string text = gatewayServerRegistration.Addresses[0].URL + "rest/v1/";
						_eventClient = new Client(text, new HttpClient());
						_tokenTimeout = DateTime.MinValue;
						Server.Instance.Logger.LogInformation("EventsClient created: {Address}", text);
					}
				}
				if (ShouldRefreshEventsClientToken())
				{
					JwtSecurityToken result = _tokenCache.RefreshToken().GetAwaiter().GetResult();
					_tokenTimeout = result.ValidTo;
					_eventClient.BearerToken = result.RawData;
					Server.Instance.Logger.LogInformation("EventsClient token updated");
				}
				return _eventClient;
			}
		}
	}

	private bool ShouldRefreshEventsClientToken()
	{
		if (_eventClient == null)
		{
			return false;
		}
		if (_eventClient.BearerToken == string.Empty)
		{
			return true;
		}
		if (_tokenTimeout == DateTime.MinValue)
		{
			return true;
		}
		if (_tokenTimeout.AddSeconds(-10.0) < DateTime.UtcNow)
		{
			return true;
		}
		return false;
	}

	public EventServerCommunicationManager(ILogger logger, ITokenCache tokenCache)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		_tokenCache = tokenCache;
		_logger = logger;
	}

	public void Close()
	{
		if (_actionClient != null)
		{
			_actionClient.Close();
			_actionClient = null;
		}
	}

	public void ActionExecutionRequest(Guid actionId, string actionName, Guid[] actionTargets, TriggerEvent triggerEvent)
	{
		try
		{
			if (actionTargets == null)
			{
				throw new ArgumentNullException("actionTargets");
			}
			if (triggerEvent == null)
			{
				throw new ArgumentNullException("triggerEvent");
			}
			_logger.LogInformation("Sending MIP action execution request Id: {ActionId}, Name: {ActionName} sourceId: {SourceId}, targetCount: {TargetCount}", actionId, actionName, triggerEvent.SourceId, actionTargets.Count());
			TriggerEventData triggerEventData = new TriggerEventData();
			triggerEventData.TriggerEventName = triggerEvent.Name;
			triggerEventData.TriggerEventTime = triggerEvent.Time;
			triggerEventData.TriggerSource = triggerEvent.SourceId;
			triggerEventData.TriggerEventType = triggerEvent.EventTypeId;
			if (triggerEvent.MetaData.TryGetValue("CustomData", out var value) && value.DataType == EventMetaData.DataTypeEnum.String)
			{
				triggerEventData.Metadata = value.Value;
			}
			ActionExecutionParams actionParameters = new ActionExecutionParams
			{
				ActionId = actionId,
				ActionTargets = actionTargets,
				TriggerEventData = triggerEventData
			};
			ActionClient.ExecuteAction(actionParameters);
		}
		catch (Exception ex)
		{
			_logger.LogError("Error sending MIP action Id: {ActionId}, Name: {ActionName} ex: {Exception}", actionId, actionName, ex);
		}
	}

	public void Action2ExecutionRequest(Guid actionId, string actionName, Guid[] actionTargets, Guid[] actionTargets2, TriggerEvent triggerEvent)
	{
		try
		{
			if (actionTargets == null)
			{
				throw new ArgumentNullException("actionTargets");
			}
			if (actionTargets2 == null)
			{
				throw new ArgumentNullException("actionTargets2");
			}
			if (triggerEvent == null)
			{
				throw new ArgumentNullException("triggerEvent");
			}
			_logger.LogInformation("Sending MIP action2 execution request Id: {ActionId}, Name: {ActionName} sourceId: {SourceId}, targetCount: {TargetCount}, target2Count: {Target2Count}", actionId, actionName, triggerEvent.SourceId, actionTargets.Count(), actionTargets2.Count());
			TriggerEventData triggerEventData = new TriggerEventData();
			triggerEventData.TriggerEventName = triggerEvent.Name;
			triggerEventData.TriggerEventTime = triggerEvent.Time;
			triggerEventData.TriggerSource = triggerEvent.SourceId;
			triggerEventData.TriggerEventType = triggerEvent.EventTypeId;
			if (triggerEvent.MetaData.TryGetValue("CustomData", out var value) && value.DataType == EventMetaData.DataTypeEnum.String)
			{
				triggerEventData.Metadata = value.Value;
			}
			ActionExecutionParams actionParameters = new ActionExecutionParams
			{
				ActionId = actionId,
				ActionTargets = actionTargets,
				ActionTargets2 = actionTargets2,
				TriggerEventData = triggerEventData
			};
			ActionClient.ExecuteAction(actionParameters);
		}
		catch (Exception ex)
		{
			_logger.LogError("Error sending MIP action Id: {ActionId}, Name: {ActionName} ex: {Exception}", actionId, actionName, ex);
		}
	}

	private static string GetTriggerEventSource(TriggerEvent e)
	{
		if (e.SourceId == Guid.Empty)
		{
			return null;
		}
		if (e.SourceId == SourceGuid.External)
		{
			return e.EventTypeId.ToString();
		}
		return e.SourceId.ToString();
	}

	private static DateTime GetTriggerEventDateTime(TriggerEvent e)
	{
		if (e.Time.Kind != DateTimeKind.Unspecified)
		{
			return e.Time;
		}
		return DateTime.SpecifyKind(e.Time, DateTimeKind.Utc);
	}

	private static DateTime GetTriggerEventDateTimeUTC(TriggerEvent e)
	{
		return GetTriggerEventDateTime(e).ToUniversalTime();
	}

	private PostEventBody PostEventBodyFromTriggerEvent(TriggerEvent e)
	{
		ParseMetadataResult parseMetadataResult = MetaDataParser.ParseMetadata(e.MetaData);
		if (parseMetadataResult.ReferenceIds.Any() || (parseMetadataResult.OtherMetadata != null && parseMetadataResult.OtherMetadata.Any()))
		{
			IEnumerable<AnalyticsObject> source = parseMetadataResult.OtherMetadata.Select((KeyValuePair<string, string> keyValuePair) => new AnalyticsObject
			{
				Id = Guid.NewGuid(),
				Name = keyValuePair.Key,
				Value = keyValuePair.Value
			});
			IEnumerable<string> source2 = parseMetadataResult.ReferenceIds.Select((Guid x) => x.ToString());
			AnalyticsData data = new AnalyticsData
			{
				References = source2.ToHashSet(),
				Objects = source.ToHashSet()
			};
			return new PostAnalyticsEventBody
			{
				Type = e.EventTypeId,
				Source = GetTriggerEventSource(e),
				Time = GetTriggerEventDateTimeUTC(e),
				Data = data
			};
		}
		return new PostEventBody
		{
			Type = e.EventTypeId,
			Source = GetTriggerEventSource(e),
			Time = GetTriggerEventDateTimeUTC(e)
		};
	}

	private async Task SentEventToConsumerAsync(TriggerEvent[] eventStatus)
	{
		if (eventStatus == null || eventStatus.Length == 0)
		{
			return;
		}
		try
		{
			List<PostEventBody> postEventBodies = eventStatus.Where((TriggerEvent x) => !x.HasBeenProcessedByEventServer).Select(PostEventBodyFromTriggerEvent).ToList();
			IEnumerable<PostBulkEventsResponseItem> enumerable = (await ClientWithBearerToken.WithRetryAsync(async () => await EventClientRest.Events_PostBulkAsync(postEventBodies, false), 2, 500)).Array.Where((PostBulkEventsResponseItem s) => s.Status >= 400);
			foreach (PostBulkEventsResponseItem item in enumerable)
			{
				_logger.LogError("Failed to send event (in bulk) of type {DataType} from source {DataSource}: {Status}", item.Data?.Type, item.Data?.Source, item.Status);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError("Error sending events with exception: {Exception}", ex.Message);
		}
	}

	public void SentEventToConsumer(TriggerEvent[] eventStatus)
	{
		if (eventStatus != null && eventStatus.Length != 0)
		{
			SentEventToConsumerAsync(eventStatus).Wait();
		}
	}

	public void SentEventToConsumer(PostEventBody body)
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		if (EventClientRest == null)
		{
			if (_lastLogEntryAboutError + TimeSpan.FromMinutes(1.0) < DateTime.UtcNow)
			{
				_logger.LogError("EventServerCommunicationManager: Unable to find event server");
				_lastLogEntryAboutError = DateTime.UtcNow;
			}
			return;
		}
		try
		{
			ClientWithBearerToken.WithRetryAsync(async () => await EventClientRest.Events_PostAsync(body, false).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}
		catch (ApiException ex)
		{
			_logger.LogError("Failed to send event of type {Type} from source {Source}: {ExceptionStatusCode}", body.Type, body.Source, ex.StatusCode);
		}
		catch (Exception ex2)
		{
			_logger.LogError("Error sending event with exception: {Exception}", ex2);
		}
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing && _actionClient != null)
		{
			_actionClient.Close();
			_actionClient.Dispose();
			_actionClient = null;
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
