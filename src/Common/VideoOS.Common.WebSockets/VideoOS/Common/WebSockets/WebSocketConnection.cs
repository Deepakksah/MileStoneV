using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketConnection<TIncoming, TOutgoing> : IWebSocketConnection<TOutgoing>, IMessageProcessor<TOutgoing>, IDisposable
{
	private readonly IWebSocketReader<TIncoming> _reader;

	private readonly IWebSocketWriter<TOutgoing> _writer;

	private readonly IWebSocket _webSocket;

	private readonly ILogger _logger;

	private Task _connectionTask;

	private CancellationTokenSource _connectionCancellationTokenSource;

	private IMessageProcessor<TIncoming> _messageProcessor;

	public long Id { get; }

	public WebSocketConnection(IMessageProcessor<TIncoming> messageProcessor, IWebSocket webSocket, IWebSocketReader<TIncoming> reader, IWebSocketWriter<TOutgoing> writer, long id, ILogger logger)
	{
		_messageProcessor = messageProcessor;
		_webSocket = webSocket ?? throw new ArgumentNullException("webSocket");
		Id = id;
		_logger = logger ?? throw new ArgumentNullException("logger");
		_reader = reader ?? throw new ArgumentNullException("reader");
		_writer = writer ?? throw new ArgumentNullException("writer");
	}

	public virtual async Task RunAsync(CancellationToken cancellationToken)
	{
		if (_connectionTask != null)
		{
			throw new InvalidOperationException("Connection already running");
		}
		TaskCompletionSource<bool> connectionTaskCompletionSource = new TaskCompletionSource<bool>();
		_connectionTask = connectionTaskCompletionSource.Task;
		_connectionCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		CancellationToken connectionCancellationToken = _connectionCancellationTokenSource.Token;
		List<Task> tasks = new List<Task>();
		while (!connectionCancellationToken.IsCancellationRequested)
		{
			try
			{
				TIncoming val = await _reader.ReadAsync(connectionCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (object.Equals(val, default(TIncoming)))
				{
					_connectionCancellationTokenSource.Cancel();
					break;
				}
				Task item = ProcessIncomingMessageAsync(val, connectionCancellationToken);
				tasks.Add(item);
				Task[] array = tasks.Where((Task t) => t.IsCompleted).ToArray();
				foreach (Task task in array)
				{
					await task.ConfigureAwait(continueOnCapturedContext: false);
					tasks.Remove(task);
				}
			}
			catch (OperationCanceledException)
			{
				_connectionCancellationTokenSource.Cancel();
			}
			catch (WebSocketException ex2)
			{
				string text = ex2.GetType().FullName + ": " + ex2.Message;
				if (ex2.InnerException != null)
				{
					text = text + " ---> " + ex2.InnerException.GetType().FullName + ": " + ex2.InnerException.Message;
				}
				LoggerExtensions.LogWarning(_logger, "Receiving data on connection {Id}, state of web socket is {WebSocketState}. {Exception}", new object[3] { Id, _webSocket.State, text });
			}
			catch (WebSocketReaderException ex3)
			{
				LoggerExtensions.LogWarning(_logger, "{Exception}", new object[1] { ex3.Message });
				LoggerExtensions.LogDebug(_logger, "{Exception}", new object[1] { ex3.ToString() });
			}
			catch (Exception ex4)
			{
				LoggerExtensions.LogError(_logger, "Unexpected error in connection {Id}. State of web socket is {WebSocketState}. {Exception}", new object[3] { Id, _webSocket.State, ex4 });
			}
		}
		connectionTaskCompletionSource.SetResult(result: true);
		await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task ProcessMessageAsync(TOutgoing response, CancellationToken cancellationToken)
	{
		try
		{
			await _writer.WriteAsync(response, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (WebSocketException ex) when (ex.WebSocketErrorCode != WebSocketError.InvalidState)
		{
			string text = ex.GetType().FullName + ": " + ex.Message;
			if (ex.InnerException != null)
			{
				text = text + " ---> " + ex.InnerException.GetType().FullName + ": " + ex.InnerException.Message;
			}
			LoggerExtensions.LogWarning(_logger, "Cannot send data on connection {Id}. State of web socket is {WebSocketState}. {Exception}", new object[3] { Id, _webSocket.State, text });
		}
		catch (InvalidOperationException ex2)
		{
			string text2 = ex2.GetType().FullName + ": " + ex2.Message;
			LoggerExtensions.LogWarning(_logger, "Cannot send data on connection {Id}. State of web socket is {WebSocketState}. {Exception}", new object[3] { Id, _webSocket.State, text2 });
		}
	}

	public async Task CloseAsync(CloseStatus closeStatus, int millisecondsTimeout)
	{
		_ = 1;
		try
		{
			if (_webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.CloseReceived)
			{
				await _webSocket.CloseOutputAsync(closeStatus.CloseStatusCode, TruncateString(closeStatus.CloseDescription, 123), CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (_connectionTask == null || await Task.WhenAny(_connectionTask, Task.Delay(millisecondsTimeout)).ConfigureAwait(continueOnCapturedContext: false) == _connectionTask)
			{
				return;
			}
		}
		catch (WebSocketException ex)
		{
			string text = ex.GetType().FullName + ": " + ex.Message;
			if (ex.InnerException != null)
			{
				text = text + " ---> " + ex.InnerException.GetType().FullName + ": " + ex.InnerException.Message;
			}
			LoggerExtensions.LogError(_logger, "Error closing connection {Id}. {Exception}", new object[2] { Id, text });
		}
		catch (Exception ex2)
		{
			LoggerExtensions.LogError(_logger, "Unexpected error closing connection {Id}. {Exception}", new object[2] { Id, ex2 });
		}
		_connectionCancellationTokenSource.Cancel();
	}

	private static string TruncateString(string text, int maxBytes)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		if (bytes.Length <= maxBytes)
		{
			return text;
		}
		return Encoding.UTF8.GetString(bytes, 0, maxBytes);
	}

	private async Task ProcessIncomingMessageAsync(TIncoming message, CancellationToken cancellationToken)
	{
		try
		{
			LoggerExtensions.LogDebug(_logger, $"Connection {Id} | {message}", Array.Empty<object>());
			await _messageProcessor.ProcessMessageAsync(message, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (ProcessMessageException ex)
		{
			LoggerExtensions.LogWarning(_logger, "Closing connection, got exception while processing incoming message in connection {Id}: {Exception}", new object[2] { Id, ex });
			await CloseAsync(ex.closeStatus, 1000);
		}
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
		}
		catch (Exception ex3)
		{
			LoggerExtensions.LogError(_logger, "Unexpected error processing message in connection {Id}: {Exception}", new object[2] { Id, ex3 });
		}
	}

	public virtual void Dispose()
	{
		_connectionCancellationTokenSource?.Cancel();
		_connectionCancellationTokenSource?.Dispose();
		_webSocket.Dispose();
	}
}
