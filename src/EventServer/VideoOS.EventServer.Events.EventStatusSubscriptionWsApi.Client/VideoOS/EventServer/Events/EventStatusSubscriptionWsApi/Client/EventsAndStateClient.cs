using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.Common.WebSockets;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;
using VideoOS.Platform;
using VideoOS.Platform.EventsAndState;
using VideoOS.Platform.Util;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class EventsAndStateClient : IEventsAndStateClient
{
	private static long _clientId;

	private readonly ICommandHandler _commandHandler;

	private readonly IEventHandler _eventHandler;

	private readonly IEventReceiver _eventReceiver;

	private readonly IFactory<IWebSocketClient<string>> _webSocketClientfactory;

	private IWebSocketClient<string> _doNotAccessDirectlyWebSocketClient;

	private IWebSocketClient<string> WebSocketClient
	{
		get
		{
			return _doNotAccessDirectlyWebSocketClient;
		}
		set
		{
			_doNotAccessDirectlyWebSocketClient = value;
			_commandHandler.SetOutgoingMessageProcessor(value);
		}
	}

	private bool Connected => WebSocketClient != null;

	public Task ConnectionTask { get; private set; }

	public long ClientId => WebSocketClient?.Id ?? (-1);

	public static long GetUpdateClientId()
	{
		return ++_clientId;
	}

	public EventsAndStateClient()
		: this((ILogger)(object)NullLogger.Instance)
	{
	}

	public EventsAndStateClient(ILogger logger)
		: this(new IocReplacement(logger))
	{
	}

	private EventsAndStateClient(IocReplacement iocReplacement)
		: this(iocReplacement.CommandHandler, iocReplacement.EventHandler, iocReplacement.EventReceiver, iocReplacement.WebSocketClientFactory)
	{
	}

	public EventsAndStateClient(ICommandHandler commandHandler, IEventHandler eventHandler, IEventReceiver eventReceiver, IFactory<IWebSocketClient<string>> webSocketClientfactory)
	{
		_commandHandler = commandHandler;
		_eventHandler = eventHandler;
		_eventReceiver = eventReceiver;
		_webSocketClientfactory = webSocketClientfactory;
	}

	public void AddEventReceiver(IEventReceiver eventReceiver)
	{
		_eventHandler.AddEventReceiver(eventReceiver);
	}

	public async Task<Guid> AddSubscriptionAsync(IEnumerable<SubscriptionRule> subscriptionRules, CancellationToken cancellationToken)
	{
		if (subscriptionRules == null)
		{
			throw new ArgumentNullMIPException("subscriptionRules");
		}
		return ResponseBase.FromJson<SubscriptionUpdateResponse>(await _commandHandler.SendCommand(new AddSubscriptionCommand
		{
			Filters = subscriptionRules.Select((SubscriptionRule x) => new SubscriptionFilter(x)).ToList()
		}, cancellationToken)).SubscriptionId;
	}

	public async Task ConnectAsync(Uri serverAddress, string jwt, CancellationToken cancellationToken)
	{
		if (Connected)
		{
			throw new InvalidOperationException("Client already connected, disconnect before attempting to establish a new connection");
		}
		if (serverAddress == null)
		{
			throw new ArgumentNullMIPException("serverAddress");
		}
		if (jwt == null)
		{
			throw new ArgumentNullMIPException("jwt");
		}
		string[] schemes = new string[2]
		{
			Uri.UriSchemeHttp,
			Uri.UriSchemeHttps
		};
		string[] schemes2 = new string[2] { "ws", "wss" };
		Uri uri;
		if (SchemeIsAnyOf(serverAddress.Scheme, schemes))
		{
			uri = new UriBuilder(SchemeIsAnyOf(serverAddress.Scheme, new string[1] { Uri.UriSchemeHttps }) ? "wss" : "ws", serverAddress.Host, serverAddress.Port, "/api/ws/events/v1").Uri;
		}
		else
		{
			if (!SchemeIsAnyOf(serverAddress.Scheme, schemes2))
			{
				throw new ArgumentMIPException("serverAddressargument defines invalid scheme.");
			}
			uri = serverAddress;
		}
		try
		{
			IWebSocketClient<string> webSocketClient = _webSocketClientfactory.Create();
			IHttpResponseMessage httpResponseMessage = await webSocketClient.ConnectAsync(uri, new KeyValuePair<string, string>[1]
			{
				new KeyValuePair<string, string>("Authorization", "Bearer " + jwt)
			}, cancellationToken);
			if (httpResponseMessage != null)
			{
				HttpStatusCode statusCode = httpResponseMessage.StatusCode;
				if (statusCode != HttpStatusCode.SwitchingProtocols)
				{
					throw new CommunicationContentMIPException((int)statusCode, httpResponseMessage.ReasonPhrase);
				}
				WebSocketClient = webSocketClient;
			}
			else
			{
				WebSocketClient = webSocketClient;
			}
		}
		catch (WebSocketException ex)
		{
			string text = ex.GetType().FullName + ": " + ex.Message;
			if (ex.InnerException != null)
			{
				text = text + " ---> " + ex.InnerException.GetType().FullName + ": " + ex.InnerException.Message;
			}
			throw new CommunicationContentMIPException(text, ex);
		}
		ConnectionTask = RunAsync();
	}

	private static bool SchemeIsAnyOf(string scheme, string[] schemes)
	{
		return Array.Exists(schemes, (string x) => scheme.Equals(x, StringComparison.OrdinalIgnoreCase));
	}

	public async Task DisconnectAsync()
	{
		if (Connected)
		{
			await WebSocketClient.DisconnectAsync(1000).ConfigureAwait(continueOnCapturedContext: false);
			WebSocketClient = null;
		}
	}

	private async Task RunAsync()
	{
		await WebSocketClient.RunAsync(CancellationToken.None);
		WebSocketClient = null;
	}

	public async Task<(Guid currentSessionId, int inactiveTimeoutSeconds)> CreateSession(CancellationToken cancellationToken)
	{
		var (num, item, item2) = await StartSession(Guid.Empty, Guid.Empty, cancellationToken);
		if (num != 201)
		{
			throw new CommunicationContentMIPException($"Expected the status of the response to be \"201 - created\". Status was {num}");
		}
		return (currentSessionId: item, inactiveTimeoutSeconds: item2);
	}

	public async Task<(bool resumedSuccessfully, Guid currentSessionId, int inactiveTimeoutSeconds)> ResumeSession(Guid sessionId, CancellationToken cancellationToken)
	{
		var (num, item, item2) = await StartSession(sessionId, Guid.Empty, cancellationToken);
		return (resumedSuccessfully: num == 200, currentSessionId: item, inactiveTimeoutSeconds: item2);
	}

	public async Task<(bool resumedSuccessfully, Guid currentSessionId, int inactiveTimeoutSeconds)> ResumeSession(Guid sessionId, Guid lastReceivedEvent, CancellationToken cancellationToken)
	{
		var (num, item, item2) = await StartSession(sessionId, lastReceivedEvent, cancellationToken);
		return (resumedSuccessfully: num == 200, currentSessionId: item, inactiveTimeoutSeconds: item2);
	}

	private async Task<(int statusCode, Guid currentSessionId, int inactiveTimeoutSeconds)> StartSession(Guid sessionId, Guid lastReceivedEvent, CancellationToken cancellationToken)
	{
		StartSessionResponse startSessionResponse = ResponseBase.FromJson<StartSessionResponse>(await _commandHandler.SendCommand(new StartSessionCommand
		{
			SessionId = ((sessionId == Guid.Empty) ? null : sessionId.ToString()),
			EventId = ((lastReceivedEvent == Guid.Empty) ? null : lastReceivedEvent.ToString())
		}, cancellationToken));
		if (startSessionResponse.Status != 201 && startSessionResponse.Status != 200)
		{
			throw new CommunicationContentMIPException($"Expected the status of the response 'OK or 'Created'. Status was {startSessionResponse.Status}");
		}
		return (statusCode: startSessionResponse.Status, currentSessionId: startSessionResponse.SessionId, inactiveTimeoutSeconds: startSessionResponse.InactiveTimeoutSeconds);
	}

	public void Dispose()
	{
		WebSocketClient?.Dispose();
	}

	public async Task<IEnumerable<State>> GetCurrentStateAsync(CancellationToken cancellationToken)
	{
		return ResponseBase.FromJson<GetStateResponse>(await _commandHandler.SendCommand(new GetStateCommand(), cancellationToken)).States;
	}

	public bool RemoveEventReceiver(IEventReceiver eventReceiver)
	{
		return _eventHandler.RemoveEventReceiver(eventReceiver);
	}

	public async Task RemoveSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken)
	{
		await _commandHandler.SendCommand(new RemoveSubscriptionCommand
		{
			SubscriptionId = ((subscriptionId == Guid.Empty) ? null : subscriptionId.ToString())
		}, cancellationToken);
	}

	public async Task SendAllStatefulEventsAsync(CancellationToken cancellationToken)
	{
		IEnumerable<State> source = await GetCurrentStateAsync(cancellationToken);
		await _eventReceiver.OnEventsReceivedAsync(source.Select((State x) => new StateAsEventWrapper(x)));
	}
}
