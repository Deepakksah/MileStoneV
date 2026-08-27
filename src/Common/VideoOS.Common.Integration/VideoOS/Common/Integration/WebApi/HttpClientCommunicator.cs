using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Common.Integration.WebApi;

public class HttpClientCommunicator : ICommunicator, IDisposable
{
	private HttpClientWrapper _httpClient;

	private Uri _baseAddress;

	private IAccessTokenHandler _accessTokenHandler;

	private IServiceConnectionStatus _serviceConnectionStatus;

	private ILogger Logger { get; }

	public HttpClientCommunicator(Uri baseAddress, ILogger logger, IAccessTokenHandler accessTokenHandler, IServiceConnectionStatus serviceConnectionStatus)
		: this(baseAddress, logger, new HttpClient(), accessTokenHandler, serviceConnectionStatus)
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
	//IL_0010: Expected O, but got Unknown


	public HttpClientCommunicator(Uri baseAddress, ILogger logger, HttpClient client, IAccessTokenHandler accessTokenHandler, IServiceConnectionStatus serviceConnectionStatus)
	{
		if (baseAddress == null)
		{
			throw new ArgumentNullException("baseAddress");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (accessTokenHandler == null)
		{
			throw new ArgumentNullException("accessTokenHandler");
		}
		_serviceConnectionStatus = serviceConnectionStatus;
		Logger = logger;
		client.BaseAddress = baseAddress;
		_httpClient = new HttpClientWrapper(client);
		_baseAddress = baseAddress;
		_accessTokenHandler = accessTokenHandler;
	}

	public async Task<TRet> SendAsync<TObj, TRet>(string endpoint, TObj obj)
	{
		return await ParseHttpResponseMessage<TRet>(await SendAsync(endpoint, obj));
	}

	public async Task<TObj> SendAsync<TObj>(string endpoint)
	{
		return await ParseHttpResponseMessage<TObj>(await SendAsync(endpoint));
	}

	public async Task<HttpResponseMessage> SendRawAsync(string endpoint, HttpContent message)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		if (message == null)
		{
			throw new ArgumentNullException("message");
		}
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
		{
			Content = message
		};
		return await ExecuteAsync(httpRequestMessage, _baseAddress?.ToString() + endpoint);
	}

	public async Task<HttpResponseMessage> SendAsync<TObj>(string endpoint, TObj obj)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
		{
			Content = (HttpContent)(object)new ObjectContent<TObj>(obj, (MediaTypeFormatter)new JsonMediaTypeFormatter())
		};
		return await ExecuteAsync(httpRequestMessage, _baseAddress?.ToString() + endpoint);
	}

	public async Task<HttpResponseMessage> SendAsync(string endpoint)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
		return await ExecuteAsync(httpRequestMessage, _baseAddress?.ToString() + endpoint);
	}

	private async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage httpRequestMessage, string route)
	{
		_ = 3;
		try
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await PopulateAndSendAsync(httpRequestMessage);
			}
			catch (UnauthorizedException)
			{
				await _accessTokenHandler.RestartAsync();
				httpResponseMessage = await PopulateAndSendAsync(await httpRequestMessage.CloneAsync());
			}
			return httpResponseMessage;
		}
		catch (InvalidOperationException innerException)
		{
			LoggerExtensions.LogError(Logger, "InvalidOperationException - The SendAsync request message was already sent by the System.Net.Http.HttpClient instance", Array.Empty<object>());
			throw new InternalServerErrorException("InvalidOperationException", innerException)
			{
				Route = route
			};
		}
		catch (HttpRequestException ex2)
		{
			HttpRequestException ex3 = ex2;
			HandleRequestExceptions(ex3, route);
			throw new CommunicationException(((Exception)(object)ex3).Message, (Exception)(object)ex3)
			{
				Route = route
			};
		}
		catch (TaskCanceledException ex4)
		{
			LoggerExtensions.LogError(Logger, "HttpClient time out", Array.Empty<object>());
			throw new TimeoutException(ex4.Message, ex4)
			{
				Route = route
			};
		}
		catch (AccessTokenException)
		{
			_serviceConnectionStatus?.AuthenticationError();
			LoggerExtensions.LogError(Logger, "AccessTokenException - Could not receive a token from identity server", Array.Empty<object>());
			throw;
		}
	}

	private async Task<HttpResponseMessage> PopulateAndSendAsync(HttpRequestMessage httpRequestMessage)
	{
		await _accessTokenHandler.PopulateAuthorizationHeaderAsync(httpRequestMessage);
		HttpResponseMessage val = await _httpClient.SendAsync(httpRequestMessage);
		if (!val.IsSuccessStatusCode)
		{
			throw await WebApiExceptionHelper.CreateResponseException(val);
		}
		return val;
	}

	private HttpResponseMessage PopulateAndSend(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
	{
		Task.Run(async delegate
		{
			await _accessTokenHandler.PopulateAuthorizationHeaderAsync(httpRequestMessage).ConfigureAwait(continueOnCapturedContext: false);
		}).GetAwaiter().GetResult();
		HttpResponseMessage httpResponseMessage = Task.Run<HttpResponseMessage>(async () => await _httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).GetAwaiter().GetResult();
		if (!httpResponseMessage.IsSuccessStatusCode)
		{
			throw Task.Run(async () => await WebApiExceptionHelper.CreateResponseException(httpResponseMessage).ConfigureAwait(continueOnCapturedContext: false)).GetAwaiter().GetResult();
		}
		return httpResponseMessage;
	}

	public async Task<TObj> ReceiveAsync<TObj>(string endpoint)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
		try
		{
			return await ParseHttpResponseMessage<TObj>(await ExecuteAsync(request, endpoint));
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}

	public async Task<HttpResponseMessage> PatchAsync<TObj>(string endpoint, TObj obj)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
		{
			Content = (HttpContent)(object)new ObjectContent<TObj>(obj, (MediaTypeFormatter)new JsonMediaTypeFormatter())
		};
		return await ExecuteAsync(httpRequestMessage, _baseAddress?.ToString() + endpoint);
	}

	public async Task<HttpResponseMessage> PatchAsync(string endpoint)
	{
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);
		return await ExecuteAsync(httpRequestMessage, _baseAddress?.ToString() + endpoint);
	}

	private static async Task<TRet> ParseHttpResponseMessage<TRet>(HttpResponseMessage httpResponseMessage)
	{
		try
		{
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				HttpContent content = httpResponseMessage.Content;
				try
				{
					return await HttpContentExtensions.ReadAsAsync<TRet>(content);
				}
				finally
				{
					((IDisposable)content)?.Dispose();
				}
			}
			throw await WebApiExceptionHelper.CreateResponseException(httpResponseMessage);
		}
		finally
		{
			((IDisposable)httpResponseMessage)?.Dispose();
		}
	}

	public TRet Send<TObj, TRet>(string endpoint, TObj obj, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpResponseMessage httpResponseMessage = Send(endpoint, obj, cancellationToken);
		return Task.Run(async () => await ParseHttpResponseMessage<TRet>(httpResponseMessage)).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
	}

	public TObj Send<TObj>(string endpoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpResponseMessage httpResponse = Send(endpoint, cancellationToken);
		return Task.Run(async () => await ParseHttpResponseMessage<TObj>(httpResponse)).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult();
	}

	public HttpResponseMessage Send<TObj>(string endpoint, TObj obj, CancellationToken cancellationToken = default(CancellationToken))
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		HttpResponseMessage val = null;
		HttpRequestMessage val2 = new HttpRequestMessage(HttpMethod.Post, endpoint);
		try
		{
			val2.Content = (HttpContent)(object)new ObjectContent<TObj>(obj, (MediaTypeFormatter)new JsonMediaTypeFormatter());
			return Execute(val2, _baseAddress?.ToString() + endpoint, cancellationToken);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public HttpResponseMessage Send(string endpoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		HttpResponseMessage val = null;
		HttpRequestMessage val2 = new HttpRequestMessage(HttpMethod.Post, endpoint);
		try
		{
			return Execute(val2, _baseAddress?.ToString() + endpoint, cancellationToken);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private HttpResponseMessage Execute(HttpRequestMessage httpRequestMessage, string route, CancellationToken cancellationToken = default(CancellationToken))
	{
		//IL_00af: Expected O, but got Unknown
		try
		{
			try
			{
				return PopulateAndSend(httpRequestMessage, cancellationToken);
			}
			catch (UnauthorizedException)
			{
				Task.Run(async delegate
				{
					await _accessTokenHandler.RestartAsync().ConfigureAwait(continueOnCapturedContext: false);
				}).GetAwaiter().GetResult();
				HttpRequestMessage result = Task.Run<HttpRequestMessage>(async () => await httpRequestMessage.CloneAsync().ConfigureAwait(continueOnCapturedContext: false)).GetAwaiter().GetResult();
				try
				{
					return PopulateAndSend(result, cancellationToken);
				}
				finally
				{
					((IDisposable)result)?.Dispose();
				}
			}
		}
		catch (InvalidOperationException innerException)
		{
			LoggerExtensions.LogError(Logger, "InvalidOperationException - The Send request message was already sent by the System.Net.Http.HttpClient instance", Array.Empty<object>());
			throw new InternalServerErrorException("Invalid Operation Exception", innerException)
			{
				Route = route
			};
		}
		catch (HttpRequestException ex2)
		{
			HttpRequestException ex3 = ex2;
			HandleRequestExceptions(ex3, route);
			throw new CommunicationException(((Exception)(object)ex3).Message, (Exception)(object)ex3)
			{
				Route = route
			};
		}
		catch (TaskCanceledException ex4)
		{
			LoggerExtensions.LogError(Logger, "HttpClient time out", Array.Empty<object>());
			throw new TimeoutException(ex4.Message, ex4)
			{
				Route = route
			};
		}
		catch (AccessTokenException)
		{
			_serviceConnectionStatus?.AuthenticationError();
			LoggerExtensions.LogError(Logger, "AccessTokenException - Could not receive a token from identity server", Array.Empty<object>());
			throw;
		}
	}

	public TObj Receive<TObj>(string endpoint)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		HttpRequestMessage val = new HttpRequestMessage(HttpMethod.Get, endpoint);
		try
		{
			HttpResponseMessage httpResponse = Execute(val, endpoint);
			return Task.Run(async () => await ParseHttpResponseMessage<TObj>(httpResponse)).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public HttpResponseMessage Patch<TObj>(string endpoint, TObj obj)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		HttpResponseMessage val = null;
		HttpRequestMessage val2 = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);
		try
		{
			val2.Content = (HttpContent)(object)new ObjectContent<TObj>(obj, (MediaTypeFormatter)new JsonMediaTypeFormatter());
			return Execute(val2, _baseAddress?.ToString() + endpoint);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public HttpResponseMessage Patch(string endpoint)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		HttpResponseMessage val = null;
		HttpRequestMessage val2 = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);
		try
		{
			return Execute(val2, _baseAddress?.ToString() + endpoint);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void HandleRequestExceptions(HttpRequestException ex, string route)
	{
		if (Logger.IsEnabled((LogLevel)1))
		{
			LoggerExtensions.LogError(Logger, "HttpRequestException - The SendAsync request message failed: {Exception}", new object[1] { ex });
		}
		else
		{
			LoggerExtensions.LogError(Logger, "HttpRequestException - The SendAsync request message failed: {Exception}", new object[1] { ((Exception)(object)ex).Message });
		}
		if (((Exception)(object)ex).InnerException is WebException)
		{
			WebException obj = (WebException)((Exception)(object)ex).InnerException;
			if (obj.Status == WebExceptionStatus.TrustFailure)
			{
				_serviceConnectionStatus?.InvalidCertificate();
				throw new TrustFailureException("Certificate not trusted", (Exception)(object)ex)
				{
					Route = route
				};
			}
			if (obj.Status == WebExceptionStatus.SendFailure)
			{
				throw new SendFailureException("Send failure", (Exception)(object)ex)
				{
					Route = route
				};
			}
			if (obj.Status == WebExceptionStatus.ConnectFailure)
			{
				_serviceConnectionStatus?.EndpointNotFound();
			}
			if (obj.Status == WebExceptionStatus.NameResolutionFailure)
			{
				_serviceConnectionStatus?.EndpointNotFound();
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			((IDisposable)_httpClient)?.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
