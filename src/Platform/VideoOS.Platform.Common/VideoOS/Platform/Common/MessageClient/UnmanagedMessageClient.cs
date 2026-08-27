using System;
using System.Globalization;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using VideoOS.Common.EnvironmentServices.WebSockets;
using VideoOS.Common.WebSockets;

namespace VideoOS.Platform.Common.MessageClient;

public class UnmanagedMessageClient : IUnmanagedMessageClient, IDisposable
{
	private readonly ICommandAcknowledgementHandler _commandAcknowledgementHandler;

	private readonly ILastProcessedMessageSequence _lastProcessedMessageSequence;

	private readonly Func<IClientWebSocket> _webSocketFactory;

	private readonly Func<IClientWebSocket, IWebSocketReader<CommandDto>> _readerFactory;

	private readonly Func<IClientWebSocket, IWebSocketWriter<CommandDto>> _writerFactory;

	private readonly Func<IIncomingCommandProcessor> _processorFactory;

	private readonly Func<IIncomingCommandProcessor, IClientWebSocket, IWebSocketReader<CommandDto>, IWebSocketWriter<CommandDto>, IWebSocketClient<CommandDto>> _webSocketClientFactory;

	private IWebSocketClient<CommandDto> _client;

	private Guid _connectionId = Guid.NewGuid();

	private Task _connectionTask;

	private Task _disConnectionTask;

	private object _pulseMsgAckTimerStopAndDisposeLock = new object();

	private System.Timers.Timer _pulseMessageAcknowledgementTimer;

	private bool _noPulseAckReceived;

	private static readonly TimeSpan _commandAcknowledgementTimeout = TimeSpan.FromSeconds(10.0);

	private bool _disposed;

	public event EventHandler<Guid> SessionIdReceived;

	public event EventHandler<WebSocketCloseStatus> UnexpectedlyDisconnected;

	public event EventHandler<NewMessageArgs> MessageReceived;

	public UnmanagedMessageClient(ICommandAcknowledgementHandler commandAcknowledgementHandler, ILastProcessedMessageSequence lastProcessedMessageSequence, Func<IClientWebSocket> webSocketFactory, Func<IClientWebSocket, IWebSocketReader<CommandDto>> readerFactory, Func<IClientWebSocket, IWebSocketWriter<CommandDto>> writerFactory, Func<IIncomingCommandProcessor> processorFactory, Func<IIncomingCommandProcessor, IClientWebSocket, IWebSocketReader<CommandDto>, IWebSocketWriter<CommandDto>, IWebSocketClient<CommandDto>> webSocketClientFactory)
	{
		_commandAcknowledgementHandler = commandAcknowledgementHandler ?? throw new ArgumentNullMIPException("commandAcknowledgementHandler");
		_lastProcessedMessageSequence = lastProcessedMessageSequence ?? throw new ArgumentNullMIPException("lastProcessedMessageSequence");
		_webSocketFactory = webSocketFactory ?? throw new ArgumentNullMIPException("webSocketFactory");
		_readerFactory = readerFactory ?? throw new ArgumentNullMIPException("readerFactory");
		_writerFactory = writerFactory ?? throw new ArgumentNullMIPException("writerFactory");
		_processorFactory = processorFactory ?? throw new ArgumentNullMIPException("processorFactory");
		_webSocketClientFactory = webSocketClientFactory ?? throw new ArgumentNullMIPException("webSocketClientFactory");
	}

	public UnmanagedMessageClient()
		: this(new CommandAcknowledgementHandler(_commandAcknowledgementTimeout), new LastProcessedMessageSequence(), () => new VideoOS.Common.EnvironmentServices.WebSockets.ClientWebSocket(), WebSocketReader<CommandDto>.Create, WebSocketWriter<CommandDto>.Create, () => new IncomingCommandProcessor(), (IIncomingCommandProcessor processor, IClientWebSocket webSocket, IWebSocketReader<CommandDto> reader, IWebSocketWriter<CommandDto> writer) => new WebSocketClient<CommandDto, CommandDto>(processor, webSocket, reader, writer, 0L, (ILogger)(object)NullLogger.Instance))
	{
	}

	public async Task ConnectAsync(Uri uri, string token, Guid? sessionId, CancellationToken cancellationToken)
	{
		ThrowIfInvalidConnectRequest(uri, token);
		_noPulseAckReceived = false;
		uri = BuildUriWithSessionAndLastSequence(uri, sessionId, _lastProcessedMessageSequence.GetLastProcessedMessageSequence());
		IClientWebSocket clientWebSocket;
		try
		{
			clientWebSocket = await CreateAndConnectWebSocket(_webSocketFactory, uri, token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (CommunicationMIPException)
		{
			throw;
		}
		catch (NotAuthorizedMIPException)
		{
			throw;
		}
		_lastProcessedMessageSequence.Initialize();
		IIncomingCommandProcessor arg = CreateAndSetupCommandProcessor();
		IWebSocketReader<CommandDto> arg2 = _readerFactory(clientWebSocket);
		IWebSocketWriter<CommandDto> arg3 = _writerFactory(clientWebSocket);
		_client = _webSocketClientFactory(arg, clientWebSocket, arg2, arg3);
		_connectionId = Guid.NewGuid();
		_connectionTask = _client.RunAsync(cancellationToken);
		_disConnectionTask = _connectionTask.ContinueWith((Func<Task, Task>)async delegate
		{
			StopAndDisposePulseLoop();
			CloseStatus closeStatus = await CleanAfterDisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (closeStatus != CloseStatus.NormalClosure || _noPulseAckReceived)
			{
				this.UnexpectedlyDisconnected?.Invoke(this, closeStatus.CloseStatusCode);
			}
		}, cancellationToken);
	}

	private void ThrowIfInvalidConnectRequest(Uri uri, string token)
	{
		if (uri == null)
		{
			throw new ArgumentNullMIPException("uri");
		}
		if (token == null)
		{
			throw new ArgumentNullMIPException("token");
		}
		if (_client != null || _connectionTask != null || _disConnectionTask != null)
		{
			throw new InvalidOperationMIPException("Cannot connect while already connected.");
		}
	}

	public static async Task<IClientWebSocket> CreateAndConnectWebSocket(Func<IClientWebSocket> webSocketFactory, Uri uri, string token, CancellationToken cancellationToken)
	{
		IClientWebSocket webSocket = webSocketFactory();
		webSocket.Options.SetRequestHeader("Authorization", "Bearer " + token);
		try
		{
			await webSocket.ConnectAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return webSocket;
		}
		catch (WebSocketException ex)
		{
			if (ex.InnerException is WebException { Response: HttpWebResponse { StatusCode: var statusCode } })
			{
				switch (statusCode)
				{
				case HttpStatusCode.BadRequest:
					throw new CommunicationMIPException(ex.InnerException.Message);
				case HttpStatusCode.Unauthorized:
					throw new NotAuthorizedMIPException(ex.InnerException.Message);
				}
			}
			throw;
		}
	}

	private IIncomingCommandProcessor CreateAndSetupCommandProcessor()
	{
		IIncomingCommandProcessor incomingCommandProcessor = _processorFactory();
		incomingCommandProcessor.MessageReceived += delegate(object _, MessageCommandBodyDto body)
		{
			this.MessageReceived?.Invoke(this, new NewMessageArgs
			{
				Data = body.Data,
				Topic = body.Topic
			});
			_lastProcessedMessageSequence.UpdateLastProcessedMessageSequence(body.Sequence);
		};
		incomingCommandProcessor.HelloReceived += delegate(object _, HelloCommandBodyDto body)
		{
			this.SessionIdReceived?.Invoke(this, body.SessionId);
			StartPulseLoop(((body.PulsePeriodSeconds >= 1) ? body.PulsePeriodSeconds : 15) * 1000);
		};
		incomingCommandProcessor.AckReceived += delegate(object _, AckCommandBodyDto body)
		{
			_commandAcknowledgementHandler.SetCommandResult(body.AckId);
		};
		return incomingCommandProcessor;
	}

	public async Task DisconnectAsync()
	{
		if (_client == null || _connectionTask == null || _disConnectionTask == null)
		{
			throw new InvalidOperationMIPException("Cannot disconnect when already disconnected.");
		}
		StopAndDisposePulseLoop();
		await _client.DisconnectAsync(10000).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task SubscribeAsync(string topic, CancellationToken cancellationToken)
	{
		SubscribeCommandBodyDto body = new SubscribeCommandBodyDto
		{
			Topic = topic
		};
		await SendAsync(topic, "sub.v1", body, cancellationToken);
	}

	public async Task UnsubscribeAsync(string topic, CancellationToken cancellationToken)
	{
		UnsubscribeCommandBodyDto body = new UnsubscribeCommandBodyDto
		{
			Topic = topic
		};
		await SendAsync(topic, "unsub.v1", body, cancellationToken);
	}

	public async Task PublishAsync(string topic, object data, CancellationToken cancellationToken)
	{
		PublishCommandBodyDto body = new PublishCommandBodyDto
		{
			Topic = topic,
			Data = JObject.FromObject(data)
		};
		await SendAsync(topic, "pub.v1", body, cancellationToken);
	}

	private async Task SendAsync(string topic, string type, object body, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(topic))
		{
			throw new ArgumentNullMIPException("topic");
		}
		try
		{
			CommandDto command = new CommandDto
			{
				Type = type,
				Body = JObject.FromObject(body),
				Id = Guid.NewGuid()
			};
			await SendCommand(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (CommunicationMIPException)
		{
			throw;
		}
		catch (InvalidOperationMIPException)
		{
			throw;
		}
	}

	private async Task SendCommand(CommandDto command, CancellationToken cancellationToken)
	{
		if (_client == null)
		{
			throw new InvalidOperationMIPException("Cannot send command before connecting.");
		}
		if (_client.CloseStatus != null)
		{
			throw new InvalidOperationMIPException("Cannot send command when connection is not open.");
		}
		Task responseTask = _commandAcknowledgementHandler.CommandResult(command.Id);
		await _client.SendAsync(command, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			await responseTask.ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (TimeoutException innerException)
		{
			throw new CommunicationMIPException($"Did not receive the acknowledgement from the server before the {_commandAcknowledgementTimeout.TotalSeconds} seconds timeout.", innerException);
		}
	}

	private async Task<CloseStatus> CleanAfterDisconnectAsync()
	{
		await _connectionTask.ConfigureAwait(continueOnCapturedContext: false);
		await _disConnectionTask.ConfigureAwait(continueOnCapturedContext: false);
		CloseStatus closeStatus = _client.CloseStatus;
		_client = null;
		_connectionTask = null;
		_disConnectionTask = null;
		return closeStatus;
	}

	public static Uri BuildUriWithSessionAndLastSequence(Uri baseUri, Guid? sessionId, int lastSequence)
	{
		Uri uri = baseUri;
		if (sessionId.HasValue)
		{
			uri = uri.AddParameter("sessionId", sessionId.Value.ToString());
		}
		if (lastSequence > -1)
		{
			uri = uri.AddParameter("lastSeq", lastSequence.ToString(CultureInfo.InvariantCulture));
		}
		return uri;
	}

	private void StartPulseLoop(int pulsePeriodMs)
	{
		if (_pulseMessageAcknowledgementTimer != null)
		{
			return;
		}
		_pulseMessageAcknowledgementTimer = new System.Timers.Timer
		{
			Interval = pulsePeriodMs,
			AutoReset = true,
			Enabled = true
		};
		_pulseMessageAcknowledgementTimer.Elapsed += async delegate
		{
			if (_client != null)
			{
				Guid connectionIdWhenPulseSent = _connectionId;
				int lastProcessedMessageSequence = _lastProcessedMessageSequence.GetLastProcessedMessageSequence();
				CommandDto commandDto = new CommandDto
				{
					Type = "pulse.v1",
					Body = JObject.FromObject((object)new PulseCommandBodyDto
					{
						Sequence = lastProcessedMessageSequence
					}),
					Id = Guid.NewGuid()
				};
				Task responseTask = _commandAcknowledgementHandler.CommandResult(commandDto.Id);
				try
				{
					await _client.SendAsync(commandDto, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
					await responseTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception arg)
				{
					if (_connectionId != connectionIdWhenPulseSent)
					{
						EnvironmentManager.Instance.Log(error: true, "UnmanagedMessageClient", "Discarding failed pulse, as connection has already changed after sending the pulse.");
						return;
					}
					_noPulseAckReceived = true;
					try
					{
						EnvironmentManager.Instance.Log(error: true, "UnmanagedMessageClient", $"Disconnecting because of failed pulse: {arg}");
						await DisconnectAsync();
					}
					catch (InvalidOperationMIPException)
					{
						EnvironmentManager.Instance.Log(error: false, "UnmanagedMessageClient", "Client already disconnected.");
					}
					catch (Exception arg2)
					{
						EnvironmentManager.Instance.Log(error: false, "UnmanagedMessageClient", $"Exception while disconnecting: {arg2}");
					}
				}
			}
		};
	}

	private void StopAndDisposePulseLoop()
	{
		lock (_pulseMsgAckTimerStopAndDisposeLock)
		{
			if (_pulseMessageAcknowledgementTimer != null)
			{
				_pulseMessageAcknowledgementTimer.Stop();
				_pulseMessageAcknowledgementTimer.Dispose();
				_pulseMessageAcknowledgementTimer = null;
			}
		}
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_client?.Dispose();
			if (ShouldDisposeTask(_connectionTask))
			{
				_connectionTask?.Dispose();
				_connectionTask = null;
			}
			if (ShouldDisposeTask(_disConnectionTask))
			{
				_disConnectionTask?.Dispose();
				_disConnectionTask = null;
			}
			StopAndDisposePulseLoop();
			GC.SuppressFinalize(this);
		}
	}

	private static bool ShouldDisposeTask(Task task)
	{
		if (task != null)
		{
			return task.Status == TaskStatus.RanToCompletion;
		}
		return false;
	}
}
