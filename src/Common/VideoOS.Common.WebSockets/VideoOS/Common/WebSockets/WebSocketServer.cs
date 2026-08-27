using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices.Web;

namespace VideoOS.Common.WebSockets;

public sealed class WebSocketServer<TIncoming, TOutgoing> : WebSocketConnectionHandlerBase<TIncoming, TOutgoing>, IServerEndpoint, IDisposable
{
	private readonly IHttpListener _httpListener;

	private readonly IIncomingMessageProcessorFactory<TIncoming, TOutgoing> _messageProcessorFactory;

	public WebSocketServer(IHttpListener httpListener, IWebSocketConnectionFactory<TIncoming, TOutgoing> webSocketConnectionFactory, IIncomingMessageProcessorFactory<TIncoming, TOutgoing> messageProcessorFactory, ILogger logger)
		: base(webSocketConnectionFactory, messageProcessorFactory, logger)
	{
		_httpListener = httpListener ?? throw new ArgumentNullException("httpListener");
		_messageProcessorFactory = messageProcessorFactory;
	}

	public async Task RunAsync(IEnumerable<string> urlPrefixes, CancellationToken cancellationToken)
	{
		await Task.Yield();
		try
		{
			foreach (string urlPrefix in urlPrefixes)
			{
				_httpListener.Prefixes.Add(urlPrefix);
			}
			cancellationToken.Register(delegate
			{
				_httpListener.Stop();
			});
			_httpListener.Start();
			LoggerExtensions.LogInformation(_logger, "Web socket server running", Array.Empty<object>());
			await HandleConnection(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "Fatal error in web socket server: {Exception}", new object[1] { ex });
			throw;
		}
	}

	private async Task HandleConnection(CancellationToken cancellationToken)
	{
		List<Task> tasks = new List<Task>();
		int connectionId = 0;
		try
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				IHttpListenerContext context = await _httpListener.GetContextAsync().ConfigureAwait(continueOnCapturedContext: false);
				IIncomingServerMessageProcessor<TIncoming, TOutgoing> messageProcessor = null;
				try
				{
					connectionId = ((connectionId >= int.MaxValue) ? 1 : (connectionId + 1));
					HttpStatus httpStatus = HttpStatus.Success;
					try
					{
						messageProcessor = await _messageProcessorFactory.CreateAsync(connectionId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (HttpResponseException ex)
					{
						ILogger logger = _logger;
						if (logger != null)
						{
							LoggerExtensions.LogError(logger, "Unexpected error creating message processor: {Exception}", new object[1] { ex });
						}
						httpStatus = new HttpStatus(ex.HttpStatusCode, "Unexpected error creating message processor");
					}
					catch (Exception ex2)
					{
						LoggerExtensions.LogError(_logger, "Unexpected error creating message processor: {Exception}", new object[1] { ex2 });
						httpStatus = new HttpStatus(HttpStatusCode.InternalServerError, "Unexpected error creating message processor");
					}
					if (httpStatus.IsSuccess())
					{
						httpStatus = await ValidateConnectionAsync(messageProcessor, context.Request.Headers, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
					if (!httpStatus.IsSuccess())
					{
						context.Response.StatusCode = (int)httpStatus.StatusCode;
						byte[] bytes = Encoding.UTF8.GetBytes(httpStatus.StatusDescription);
						context.Response.ContentLength64 = bytes.Length;
						Stream outputStream = context.Response.OutputStream;
						outputStream.Write(bytes, 0, bytes.Length);
						outputStream.Close();
						context.Response.Close();
						continue;
					}
					Task item = RunConnectionAsync(messageProcessor, (await context.AcceptWebSocketAsync(null).ConfigureAwait(continueOnCapturedContext: false)).WebSocket, connectionId, cancellationToken);
					tasks.Add(item);
					messageProcessor = null;
					Task[] array = tasks.Where((Task t) => t.IsCompleted).ToArray();
					foreach (Task task in array)
					{
						await task.ConfigureAwait(continueOnCapturedContext: false);
						tasks.Remove(task);
					}
				}
				catch (WebSocketException ex3)
				{
					messageProcessor?.Dispose();
					string text = ex3.GetType().FullName + ": " + ex3.Message;
					LoggerExtensions.LogWarning(_logger, "Failed to accept web socket request: {Exception}", new object[1] { text });
					context.Response.Headers.Add("Upgrade", "websocket");
					context.Response.Headers.Add("Connection", "Upgrade");
					context.Response.Headers.Add("Sec-WebSocket-Version", "13");
					context.Response.StatusCode = 426;
					context.Response.Close();
				}
			}
		}
		catch (HttpListenerException) when (cancellationToken.IsCancellationRequested)
		{
		}
		await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false);
	}

	public void Dispose()
	{
		_httpListener.Dispose();
	}
}
