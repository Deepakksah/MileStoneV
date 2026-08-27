using System;
using System.Text;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.EnvironmentServices.WebSockets;
using VideoOS.Common.WebSockets;
using VideoOS.EventServer.EventServer.Events.EventStatusSubscriptionWsApi.Client;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;
using VideoOS.Platform.EventsAndState;
using VideoOS.Platform.Util;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class IocReplacement
{
	public ILogger Logger { get; }

	public ICommandHandler CommandHandler { get; }

	public IEventHandler EventHandler { get; }

	public IEventReceiver EventReceiver { get; }

	public IFactory<IWebSocketClient<string>> WebSocketClientFactory { get; }

	public IocReplacement(ILogger logger)
	{
		Logger = logger;
		MessageParser messageParser = new MessageParser();
		MessageDispatcher messageDispatcher = new MessageDispatcher(messageParser, logger);
		new ClientWebSocket();
		WebSocketClientFactory = new Factory<IWebSocketClient<string>>(() => CreateWebSocketClient(messageDispatcher, logger));
		CommandHandler = new CommandHandler(messageDispatcher, new Factory<ITimer>(() => new ThreadingTimerService()), TimeSpan.FromSeconds(30.0), messageParser, logger);
		EventReceiver = (IEventReceiver)(EventHandler = new EventHandler(messageDispatcher));
	}

	private static IWebSocketClient<string> CreateWebSocketClient(MessageDispatcher messageDispatcher, ILogger logger)
	{
		IClientWebSocket webSocket = new ClientWebSocket();
		return new WebSocketClient<string, string>(messageDispatcher, webSocket, new WebSocketTextReader(webSocket, Encoding.UTF8), new WebSocketTextWriter(webSocket, Encoding.UTF8), EventsAndStateClient.GetUpdateClientId(), logger);
	}
}
