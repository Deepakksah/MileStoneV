using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.WebSockets;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;
using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class CommandHandler : ICommandHandler, IMessageProcessor<JObject>, IDisposable
{
	private readonly IFactory<ITimer> _timerFactory;

	private readonly TimeSpan _responseTimeoutTime;

	private readonly IMessageParser _messageParser;

	private readonly ILogger _logger;

	private readonly object _lockObject = new object();

	private readonly Dictionary<ulong, PendingCommand> _pendingcommands = new Dictionary<ulong, PendingCommand>();

	private ulong _commandId;

	private IMessageProcessor<string> _outgoingMessageProcessor;

	public CommandHandler(IMessageDispatcher messageDispatcher, IFactory<ITimer> timerFactory, TimeSpan responseTimeoutTime, IMessageParser messageParser, ILogger logger)
	{
		_timerFactory = timerFactory;
		_responseTimeoutTime = responseTimeoutTime;
		_messageParser = messageParser;
		_logger = logger;
		messageDispatcher.RegisterMessageReceiver(MessageType.Response, this);
	}

	public void SetOutgoingMessageProcessor(IMessageProcessor<string> outgoingMessageProcessor)
	{
		_outgoingMessageProcessor = outgoingMessageProcessor;
	}

	public async Task<JObject> SendCommand(ICommand command, CancellationToken cancellationToken)
	{
		IMessageProcessor<string> outgoingMessageProcessor = _outgoingMessageProcessor;
		if (outgoingMessageProcessor == null)
		{
			throw new CommunicationMIPException("Client is not connected to the VMS");
		}
		ulong num;
		TaskCompletionSource<JObject> responseSource;
		lock (_lockObject)
		{
			num = ++_commandId;
			responseSource = new TaskCompletionSource<JObject>(TaskCreationOptions.RunContinuationsAsynchronously);
			_pendingcommands.Add(num, new PendingCommand(num, responseSource, _timerFactory.Create(), _responseTimeoutTime, this));
		}
		command.CommandId = num;
		await outgoingMessageProcessor.ProcessMessageAsync(command.ToJson(), cancellationToken);
		JObject val = await responseSource.Task;
		if (!_messageParser.IsSuccessStatusCode(val))
		{
			ErrorResponse errorResponse;
			try
			{
				errorResponse = ResponseBase.FromJson<ErrorResponse>(val);
			}
			catch (InvalidCastException)
			{
				throw new CommunicationContentMIPException("Unexpected reponse: " + ((JToken)val).ToString((Formatting)0, Array.Empty<JsonConverter>()));
			}
			throw new CommunicationContentMIPException(errorResponse.Status, errorResponse.Error?.ErrorText);
		}
		return val;
	}

	public void CommandTimeout(ulong commandId)
	{
		RespondToCommand(commandId, null, new TimeoutCommunicationMIPException("Command timed out before receiving response"));
	}

	private void RespondToCommand(ulong commandId, JObject message, Exception exception)
	{
		PendingCommand value;
		lock (_lockObject)
		{
			if (!_pendingcommands.TryGetValue(commandId, out value))
			{
				return;
			}
			_pendingcommands.Remove(commandId);
		}
		value.ReturnResponse(message, exception);
		value.Dispose();
	}

	public Task ProcessMessageAsync(JObject message, CancellationToken cancellationToken)
	{
		try
		{
			ulong commandId = _messageParser.GetCommandId(message);
			RespondToCommand(commandId, message, null);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogWarning(_logger, "EventsAndState: received response with invalid command id : {ExceptionMessage}", new object[1] { ex.Message });
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		lock (_lockObject)
		{
			foreach (PendingCommand value in _pendingcommands.Values)
			{
				value.Dispose();
			}
			_pendingcommands.Clear();
		}
	}
}
