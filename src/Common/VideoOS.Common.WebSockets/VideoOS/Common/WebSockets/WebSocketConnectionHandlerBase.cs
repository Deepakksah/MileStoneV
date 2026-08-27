using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public abstract class WebSocketConnectionHandlerBase<TIncoming, TOutgoing>
{
	private readonly IWebSocketConnectionFactory<TIncoming, TOutgoing> _webSocketConnectionFactory;

	public readonly ILogger _logger;

	private readonly ConcurrentDictionary<long, IWebSocketConnection<TOutgoing>> _openConnections = new ConcurrentDictionary<long, IWebSocketConnection<TOutgoing>>();

	public WebSocketConnectionHandlerBase(IWebSocketConnectionFactory<TIncoming, TOutgoing> webSocketConnectionFactory, IIncomingMessageProcessorFactory<TIncoming, TOutgoing> messageProcessorFactory, ILogger logger)
	{
		_webSocketConnectionFactory = webSocketConnectionFactory ?? throw new ArgumentNullException("webSocketConnectionFactory");
		_logger = logger ?? throw new ArgumentNullException("logger");
	}

	public async Task<HttpStatus> ValidateConnectionAsync(IIncomingServerMessageProcessor<TIncoming, TOutgoing> messageProcessor, NameValueCollection headers, CancellationToken cancellationToken)
	{
		HttpStatus result;
		try
		{
			result = await messageProcessor.ValidateClientAsync(headers, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "Unexpected error in validating client: {Exception}", new object[1] { ex });
			result = new HttpStatus(HttpStatusCode.InternalServerError, "Unexpected error in validating client: " + ex.Message);
		}
		if (!result.IsSuccess())
		{
			messageProcessor.Dispose();
		}
		return result;
	}

	public async Task RunConnectionAsync(IIncomingServerMessageProcessor<TIncoming, TOutgoing> messageProcessor, IWebSocket webSocket, long id, CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			IWebSocketConnection<TOutgoing> connection = _webSocketConnectionFactory.Create(messageProcessor, webSocket, id);
			try
			{
				using (messageProcessor)
				{
					LoggerExtensions.LogInformation(_logger, "Connection {Id} created", new object[1] { id });
					_openConnections.TryAdd(id, connection);
					messageProcessor.SetResponseMessageProcessor(connection);
					messageProcessor.SetServerDisconnect((CloseStatus closeStatus) => connection.CloseAsync(closeStatus, 1000));
					await connection.RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					await messageProcessor.ClientDisconnectedAsync().ConfigureAwait(continueOnCapturedContext: false);
					_openConnections.TryRemove(id, out var _);
					LoggerExtensions.LogInformation(_logger, "Connection {Id} closed", new object[1] { id });
				}
			}
			finally
			{
				if (connection != null)
				{
					connection.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "Unexpected error in connection {Id}: {Exception}", new object[2] { id, ex });
		}
	}
}
