using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using VideoOS.Common.WebSockets;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

namespace VideoOS.EventServer.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class MessageDispatcher : IMessageProcessor<string>, IDisposable, IMessageDispatcher
{
	private readonly IMessageParser _messageParser;

	private readonly ILogger _logger;

	private readonly Dictionary<MessageType, IMessageProcessor<JObject>> _messageReceivers = new Dictionary<MessageType, IMessageProcessor<JObject>>();

	public MessageDispatcher(IMessageParser messageParser, ILogger logger)
	{
		_messageParser = messageParser;
		_logger = logger;
	}

	public void RegisterMessageReceiver(MessageType messageType, IMessageProcessor<JObject> messageProcessor)
	{
		_messageReceivers.Add(messageType, messageProcessor);
	}

	public Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
	{
		JObject val;
		try
		{
			val = _messageParser.ParseMessage(message);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogWarning(_logger, "EventsAndState: Invalid message  received: {EventMessage}, Exception = {Exception}", new object[2]
			{
				message.Truncate(50),
				ex
			});
			return Task.CompletedTask;
		}
		MessageType messageType = _messageParser.GetMessageType(val);
		if (_messageReceivers.TryGetValue(messageType, out var value))
		{
			return value.ProcessMessageAsync(val, cancellationToken);
		}
		LoggerExtensions.LogWarning(_logger, "EventsAndState: Unexpected message type received: {EventMessage}", new object[1] { message.Truncate(50) });
		return Task.CompletedTask;
	}

	public void Dispose()
	{
	}
}
