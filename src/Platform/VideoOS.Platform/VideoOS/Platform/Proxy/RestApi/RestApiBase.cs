using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Proxy.RestApi;

public class RestApiBase : IDisposable
{
	private HttpClient _httpClient;

	private LoginSettings _loginSettings;

	private bool _debugLog;

	private string _baseUrl;

	private object _lockOpenCall = new object();

	private string BaseUrl
	{
		get
		{
			if (_baseUrl == null)
			{
				if (!ConfigurationApiManager.BypassApiGateway)
				{
					_baseUrl = WellKnownInformation.GetApiGatewayUri(_loginSettings)?.ToString().Replace("\\rest\\", "\\api\\");
				}
				if (_baseUrl == null)
				{
					_baseUrl = string.Format(CultureInfo.InvariantCulture, "{0}ManagementServer/api", _loginSettings.UriCorporate);
				}
				if (_baseUrl == null)
				{
					throw new CommunicationMIPException("Unable to locate service endpoint for: " + _loginSettings.UriCorporate);
				}
			}
			return _baseUrl;
		}
		set
		{
			_baseUrl = value;
		}
	}

	public void Initialize(LoginSettings loginSettings, bool debugLog)
	{
		_loginSettings = loginSettings;
		_debugLog = debugLog;
	}

	public void Dispose()
	{
		Dispose(all: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool all)
	{
		if (_httpClient != null)
		{
			((HttpMessageInvoker)_httpClient).Dispose();
			_httpClient = null;
		}
		_baseUrl = null;
	}

	private HttpClient GetClient()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		if (_httpClient == null)
		{
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_httpClient.Timeout = TimeSpan.FromSeconds(120.0);
		}
		return _httpClient;
	}

	public string GetRequestWithRetry(string relativeUrl)
	{
		return CallWithRetry(() => GetRequest(relativeUrl).GetAwaiter().GetResult());
	}

	public async Task<string> GetRequestWithRetryAsync(string relativeUrl, CancellationToken cancellationToken)
	{
		return await CallWithRetryAsync((CancellationToken ct) => GetRequest(relativeUrl, ct), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public string PatchRequestWithRetry(string relativeUrl, string body)
	{
		return CallWithRetry(() => PatchRequest(relativeUrl, body).GetAwaiter().GetResult());
	}

	public string PutRequestWithRetry(string relativeUrl, string body)
	{
		return CallWithRetry(() => PutRequest(relativeUrl, body).GetAwaiter().GetResult());
	}

	public string DeleteRequestWithRetry(string relativeUrl)
	{
		return CallWithRetry(() => DeleteRequest(relativeUrl).GetAwaiter().GetResult());
	}

	public (string, int) PostRequestWithRetry(string relativeUrl, string body)
	{
		var (item, item2) = CallWithRetry(() => PostRequest(relativeUrl, body).GetAwaiter().GetResult());
		return (item, item2);
	}

	public async Task<string> GetRequest(string relativeUrl, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, relativeUrl);
		try
		{
			HttpResponseMessage response = await ((HttpMessageInvoker)GetClient()).SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (response.Content == null)
				{
					return null;
				}
				string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (_debugLog)
				{
					string text2 = "--- HTTP Response: " + response.StatusCode.ToString() + " : " + (text ?? "(empty)");
					EnvironmentManager.Instance.Log(error: false, "RestAPI", text2);
				}
				if (response.StatusCode != HttpStatusCode.OK)
				{
					if (response.StatusCode == HttpStatusCode.NotFound)
					{
						return null;
					}
					if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
					}
					string text3 = FormattableString.Invariant($"GET Unexpected statuscode {response.StatusCode} in '{relativeUrl}'");
					try
					{
						JsonObject jsonObject = JsonObject.Parse(text)?.GetChild("error");
						if (jsonObject != null)
						{
							text3 = new ErrorRest(jsonObject).details.Aggregate(text3, (string current, DetailRest detail) => current + " - " + detail.errorText);
						}
					}
					catch
					{
					}
					throw new RestApiMIPException(text3);
				}
				return text;
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}

	public async Task<string> PatchRequest(string relativeUrl, string body)
	{
		HttpRequestMessage request = CreateRequestMessage(new HttpMethod("PATCH"), relativeUrl, body);
		try
		{
			HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (response.Content == null)
				{
					return null;
				}
				string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (_debugLog)
				{
					string text2 = "--- HTTP Response: " + response.StatusCode.ToString() + " : " + (text ?? "(empty)");
					EnvironmentManager.Instance.Log(error: false, "RestAPI", text2);
				}
				if (response.StatusCode != HttpStatusCode.OK)
				{
					if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
					}
					string text3 = FormattableString.Invariant($"PATCH Unexpected statuscode {response.StatusCode} in '{relativeUrl}'");
					try
					{
						JsonObject jsonObject = JsonObject.Parse(text)?.GetChild("error");
						if (jsonObject != null)
						{
							text3 = new ErrorRest(jsonObject).details.Aggregate(text3, (string current, DetailRest detail) => current + " - " + detail.errorText);
						}
					}
					catch
					{
					}
					throw new RestApiMIPException(text3);
				}
				return text;
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}

	public async Task<string> PutRequest(string relativeUrl, string body)
	{
		HttpRequestMessage request = CreateRequestMessage(HttpMethod.Put, relativeUrl, body);
		try
		{
			HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (response.Content == null)
				{
					return null;
				}
				string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (_debugLog)
				{
					string text2 = "--- HTTP Response: " + response.StatusCode.ToString() + " : " + (text ?? "(empty)");
					EnvironmentManager.Instance.Log(error: false, "RestAPI", text2);
				}
				if (response.StatusCode != HttpStatusCode.OK)
				{
					if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
					}
					string text3 = FormattableString.Invariant($"PUT Unexpected statuscode {response.StatusCode} in '{relativeUrl}'");
					try
					{
						JsonObject jsonObject = JsonObject.Parse(text)?.GetChild("error");
						if (jsonObject != null)
						{
							text3 = new ErrorRest(jsonObject).details.Aggregate(text3, (string current, DetailRest detail) => current + " - " + detail.errorText);
						}
					}
					catch
					{
					}
					throw new RestApiMIPException(text3);
				}
				return text;
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}

	public async Task<string> DeleteRequest(string relativeUrl)
	{
		HttpRequestMessage request = CreateRequestMessage(HttpMethod.Delete, relativeUrl);
		try
		{
			HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (_debugLog)
				{
					string text2 = "--- HTTP Response: " + response.StatusCode.ToString() + " : " + (text ?? "(empty)");
					EnvironmentManager.Instance.Log(error: false, "RestAPI", text2);
				}
				if (response.StatusCode != HttpStatusCode.OK)
				{
					if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
					}
					throw new RestApiMIPException(FormattableString.Invariant($"DELETE Unexpected statuscode {response.StatusCode} in '{relativeUrl}'"));
				}
				return text;
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}

	public async Task<(string, int)> PostRequest(string relativeUrl, string json)
	{
		HttpRequestMessage request = CreateRequestMessage(HttpMethod.Post, relativeUrl, json);
		try
		{
			HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (_debugLog)
				{
					string text2 = "--- HTTP Response: " + response.StatusCode.ToString() + " : " + (text ?? "(empty)");
					EnvironmentManager.Instance.Log(error: false, "RestAPI", text2);
				}
				if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
				{
					throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
				}
				return (text, (int)response.StatusCode);
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)request)?.Dispose();
		}
	}

	public HttpRequestMessage CreateRequestMessage(HttpMethod method, string relativeUrl, string body = null)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		string text = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", BaseUrl, relativeUrl);
		HttpRequestMessage val = new HttpRequestMessage(method, text);
		if (body != null)
		{
			val.Content = (HttpContent)new StringContent(body, Encoding.UTF8, "application/json");
		}
		if (_loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: true, "RestApiClient", "LoginSettings is null");
		}
		else if (_loginSettings.IdentityTokenCache == null)
		{
			EnvironmentManager.Instance.Log(error: true, "RestApiClient", "LoginSettings issue: LoginSettings.IdentityCache is null" + Environment.NewLine + Environment.StackTrace);
		}
		else
		{
			val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loginSettings.IdentityTokenCache.Token);
		}
		if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("RestApiCallerId", out var value))
		{
			((HttpHeaders)val.Headers).Add("CallerID", value);
		}
		if (_debugLog)
		{
			string text2 = string.Format(CultureInfo.InvariantCulture, "--- HTTP Request {0} - URL:{1}, body: {2}", method.Method, text, body ?? "");
			EnvironmentManager.Instance.Log(error: false, "RestAPI", text2);
		}
		return val;
	}

	private TResult CallWithRetry<TResult>(Func<TResult> call)
	{
		try
		{
			return call();
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationMIPException)
		{
			lock (_lockOpenCall)
			{
				Dispose(all: true);
				GetClient();
				return call();
			}
		}
		catch (CommunicationException ex3)
		{
			lock (_lockOpenCall)
			{
				if (IsUnauthorizedLogin(ex3))
				{
					Dispose(all: true);
					throw;
				}
				Dispose(all: true);
				GetClient();
				return call();
			}
		}
		catch (ObjectDisposedException)
		{
			lock (_lockOpenCall)
			{
				Dispose(all: true);
				GetClient();
				return call();
			}
		}
	}

	private async Task<TResult> CallWithRetryAsync<TResult>(Func<CancellationToken, Task<TResult>> callAsync, CancellationToken cancellationToken)
	{
		try
		{
			return await callAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationMIPException)
		{
			lock (_lockOpenCall)
			{
				Dispose(all: true);
				GetClient();
			}
			return await callAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (CommunicationException ex3)
		{
			CommunicationException ex4 = ex3;
			lock (_lockOpenCall)
			{
				if (IsUnauthorizedLogin(ex4))
				{
					Dispose(all: true);
					ExceptionDispatchInfo.Capture((ex3 as Exception) ?? throw ex3).Throw();
				}
				Dispose(all: true);
				GetClient();
			}
			return await callAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (ObjectDisposedException)
		{
			lock (_lockOpenCall)
			{
				Dispose(all: true);
				GetClient();
			}
			return await callAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private static bool IsUnauthorizedLogin(CommunicationException ex)
	{
		if (ex?.InnerException?.InnerException is WebException ex2)
		{
			HttpWebResponse obj = ex2.Response as HttpWebResponse;
			if (obj == null)
			{
				return false;
			}
			return obj.StatusCode == HttpStatusCode.Unauthorized;
		}
		return false;
	}
}
