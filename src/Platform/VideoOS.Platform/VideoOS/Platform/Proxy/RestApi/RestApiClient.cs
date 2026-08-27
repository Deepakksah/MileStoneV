using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Proxy.RestApi;

public class RestApiClient : IRestApiClient, IDisposable
{
	private Dictionary<string, RestResourceCache> _cache = new Dictionary<string, RestResourceCache>();

	private static bool _enableCache = true;

	private string BaseUrl;

	private HttpClient _httpClient;

	private LoginSettings _loginSettings;

	private bool _debugLog;

	private const string ErrorKey = "error";

	private const string Tasks = "tasks";

	public bool EnableCache
	{
		get
		{
			return _enableCache;
		}
		set
		{
			_enableCache = value;
		}
	}

	public void Initialize(LoginSettings loginSettings, bool debugLog)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		_loginSettings = loginSettings ?? throw new ArgumentNullException("loginSettings");
		_httpClient = new HttpClient();
		_httpClient.DefaultRequestHeaders.Accept.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_httpClient.Timeout = TimeSpan.FromSeconds(120.0);
		_debugLog = debugLog;
		BaseUrl = (ConfigurationApiManager.BypassApiGateway ? string.Format(CultureInfo.InvariantCulture, "{0}ManagementServer/rest", loginSettings.UriCorporate) : WellKnownInformation.GetApiGatewayUri(loginSettings)?.ToString());
		if (BaseUrl == null)
		{
			throw new CommunicationMIPException("Unable to contact server");
		}
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
	}

	public JsonObject GetSpecificResource(string resourceType, string id)
	{
		return GetSpecificResource(resourceType, id, askResources: true);
	}

	public void ClearCache()
	{
		lock (_cache)
		{
			_cache.Clear();
		}
	}

	public void ClearCache(string resourceType)
	{
		lock (_cache)
		{
			if (_cache.ContainsKey(resourceType.ToLower()))
			{
				_cache[resourceType.ToLower()].Clear();
			}
		}
	}

	public JsonObject GetSpecificResource(string resourceType, string id, bool askResources)
	{
		string text = BaseUrl + "/" + resourceType + "/" + id + "?definitions&tasks";
		if (askResources)
		{
			text += "&resources";
		}
		string cachedRequest = GetCachedRequest(text, resourceType);
		if (_debugLog)
		{
			DebugLog("--- Get RestAPI: Request: " + text);
			DebugLog("---              Response: " + cachedRequest);
		}
		return JsonObject.Parse(cachedRequest) ?? throw new PathNotFoundMIPException("/" + resourceType + "/" + id);
	}

	public JsonObject GetSpecificResourceLastModifiedOnly(string resourceType, string id)
	{
		string text = BaseUrl + "/" + resourceType + "/" + id + "?includeFields=(lastModified)&includeChildren=views";
		string cachedRequest = GetCachedRequest(text, resourceType);
		if (_debugLog)
		{
			DebugLog("--- Get RestAPI: Request: " + text);
			DebugLog("---              Response: " + cachedRequest);
		}
		return JsonObject.Parse(cachedRequest) ?? throw new PathNotFoundMIPException("/" + resourceType + "/" + id);
	}

	public JsonObject GetSpecificResource(string resourceType, string id, string childResourceType, string childId)
	{
		string text = BaseUrl + "/" + resourceType + "/" + id + "/" + childResourceType + "/" + childId + "?definitions&tasks&resources";
		string cachedRequest = GetCachedRequest(text, resourceType);
		JsonObject jsonObject = JsonObject.Parse(cachedRequest);
		if (_debugLog)
		{
			DebugLog("--- Get RestAPI: Request: " + text);
			DebugLog("---              Response: " + cachedRequest);
		}
		if (jsonObject == null)
		{
			throw new PathNotFoundMIPException("/" + resourceType + "/" + id + "/" + childResourceType + "/" + childId);
		}
		return jsonObject;
	}

	public JsonObject GetResources(string resourceType)
	{
		string text = BaseUrl + "/" + resourceType + "?disabled";
		string cachedRequest = GetCachedRequest(text, resourceType);
		if (_debugLog)
		{
			DebugLog("--- Get RestAPI: Request: " + text);
			DebugLog("---              Response: " + cachedRequest);
		}
		return JsonObject.Parse(cachedRequest) ?? throw new PathNotFoundMIPException("/" + resourceType);
	}

	public JsonObject GetResourcesUrl(string relativeUrl)
	{
		string text = BaseUrl + "/" + relativeUrl;
		string result = GetRequest(text).GetAwaiter().GetResult();
		if (_debugLog)
		{
			DebugLog("--- Get RestAPI: Request: " + text);
			DebugLog("---              Response: " + result);
		}
		return JsonObject.Parse(result) ?? throw new PathNotFoundMIPException("/" + relativeUrl);
	}

	public JsonObject GetResources(string resourceType, string id, string childResourceType)
	{
		string text = BaseUrl + "/" + resourceType + "/" + id + "/" + childResourceType + "?definitions&tasks&resources&disabled";
		string cachedRequest = GetCachedRequest(text, resourceType);
		if (_debugLog)
		{
			DebugLog("--- Get RestAPI: Request: " + text);
			DebugLog("---              Response: " + cachedRequest);
		}
		JsonObject jsonObject = JsonObject.Parse(cachedRequest);
		if (jsonObject == null)
		{
			throw new PathNotFoundMIPException("/" + resourceType + "/" + id + "/" + childResourceType);
		}
		return jsonObject;
	}

	public JsonObject Patch(JsonObject theObject)
	{
		if (theObject == null)
		{
			throw new ArgumentNullMIPException("theObject");
		}
		JsonObject obj = (theObject.GetChild("relations") ?? throw new ArgumentException("relations not found")).GetChild("self") ?? throw new ArgumentException("relations.self not found");
		string resourceType = obj.GetString("type");
		string id = obj.GetString("id");
		return Patch(resourceType, id, theObject.ToJson());
	}

	public JsonObject Patch(string parentResourceName, string parentId, JsonObject theObject)
	{
		if (theObject == null)
		{
			throw new ArgumentNullMIPException("theObject");
		}
		JsonObject obj = (theObject.GetChild("relations") ?? throw new ArgumentMIPException("relations not found")).GetChild("self") ?? throw new ArgumentMIPException("relations.self not found");
		string resourceType = obj.GetString("type");
		string id = obj.GetString("id");
		return Patch(parentResourceName, parentId, resourceType, id, theObject.ToJson());
	}

	public JsonObject Patch(string resourceType, string id, string json)
	{
		if (resourceType == null)
		{
			throw new ArgumentNullMIPException("resourceType");
		}
		if (id == null)
		{
			throw new ArgumentNullMIPException("id");
		}
		if (json == null)
		{
			throw new ArgumentNullMIPException("json");
		}
		string text = BaseUrl + "/" + resourceType + "/" + id;
		if (_debugLog)
		{
			DebugLog("---Patch RestAPI: Request: " + text + " - " + json);
		}
		ClearCache(resourceType);
		RefreshCache();
		string result = PatchRequest(text, json).GetAwaiter().GetResult();
		if (_debugLog)
		{
			DebugLog("---         Response: " + result);
		}
		return JsonObject.Parse(result) ?? throw new ArgumentMIPException("PATCH No response from: /" + resourceType + "/" + id);
	}

	public JsonObject Patch(string parentResourceType, string parentId, string resourceType, string id, string json)
	{
		if (parentResourceType == null)
		{
			throw new ArgumentNullMIPException("parentResourceType");
		}
		if (parentId == null)
		{
			throw new ArgumentNullMIPException("parentId");
		}
		if (resourceType == null)
		{
			throw new ArgumentNullMIPException("resourceType");
		}
		if (id == null)
		{
			throw new ArgumentNullMIPException("id");
		}
		if (json == null)
		{
			throw new ArgumentNullMIPException("json");
		}
		string text = BaseUrl + "/" + parentResourceType + "/" + parentId + "/" + resourceType + "/" + id;
		if (_debugLog)
		{
			DebugLog("---Patch RestAPI: Request: " + text + " - " + json);
		}
		ClearCache(parentResourceType);
		RefreshCache();
		string result = PatchRequest(text, json).GetAwaiter().GetResult();
		if (_debugLog)
		{
			DebugLog("---         Response: " + result);
		}
		JsonObject jsonObject = JsonObject.Parse(result);
		if (jsonObject == null)
		{
			throw new ArgumentMIPException("PATCH No response from: /" + parentResourceType + "/" + parentId + "/" + resourceType + "/" + id);
		}
		return jsonObject;
	}

	public JsonObject Put(string resourceType, string id, string json)
	{
		if (resourceType == null)
		{
			throw new ArgumentNullMIPException("resourceType");
		}
		if (id == null)
		{
			throw new ArgumentNullMIPException("id");
		}
		if (json == null)
		{
			throw new ArgumentNullMIPException("json");
		}
		string text = BaseUrl + "/" + resourceType + "/" + id;
		if (_debugLog)
		{
			DebugLog("---Put RestAPI: Request: " + text + " - " + json);
		}
		ClearCache(resourceType);
		RefreshCache();
		string result = PutRequest(text, json).GetAwaiter().GetResult();
		if (_debugLog)
		{
			DebugLog("---         Response: " + result);
		}
		return JsonObject.Parse(result) ?? throw new ArgumentMIPException("PUT No response from: /" + resourceType + "/" + id);
	}

	public JsonObject Delete(string resourceType, string id)
	{
		if (resourceType == null)
		{
			throw new ArgumentNullMIPException("resourceType");
		}
		if (id == null)
		{
			throw new ArgumentNullMIPException("id");
		}
		string text = BaseUrl + "/" + resourceType + "/" + id;
		if (_debugLog)
		{
			DebugLog("---Delete RestAPI: Request: " + text);
		}
		ClearCache(resourceType);
		RefreshCache();
		string result = DeleteRequest(text).GetAwaiter().GetResult();
		if (_debugLog)
		{
			DebugLog("---         Response: " + result);
		}
		if (result != null)
		{
			return JsonObject.Parse(result);
		}
		return null;
	}

	public JsonObject Delete(JsonObject theObject)
	{
		if (theObject == null)
		{
			throw new ArgumentNullMIPException("theObject");
		}
		JsonObject obj = (theObject.GetChild("relations") ?? throw new ArgumentMIPException("relations not found")).GetChild("self") ?? throw new ArgumentMIPException("relations.self not found");
		string resourceType = obj.GetString("type");
		string id = obj.GetString("id");
		return Delete(resourceType, id);
	}

	public JsonObject Post(string resourceType, string id, string methodId, string json, out int httpStatus)
	{
		if (resourceType == null)
		{
			throw new ArgumentNullMIPException("resourceType");
		}
		if (json == null)
		{
			throw new ArgumentNullMIPException("json");
		}
		string text = (string.IsNullOrEmpty(id) ? (BaseUrl + "/" + resourceType) : (BaseUrl + "/" + resourceType + "/" + id));
		text = (string.IsNullOrEmpty(methodId) ? (text + "?definitions") : (text + "?task=" + methodId + "&definitions"));
		if (_debugLog)
		{
			DebugLog("---Post RestAPI: Request: " + text + ", Body:" + json);
		}
		ClearCache(resourceType);
		RefreshCache();
		(string, int) result = PostRequest(text, json).GetAwaiter().GetResult();
		string item = result.Item1;
		int item2 = result.Item2;
		httpStatus = item2;
		if (_debugLog)
		{
			DebugLog("---         Response: " + item);
		}
		if (item != null)
		{
			return JsonObject.Parse(item);
		}
		return null;
	}

	public JsonObject Post(string parentResourceType, string parentId, string resourceType, string id, string methodId, string json, out int httpStatus)
	{
		if (parentResourceType == null)
		{
			throw new ArgumentNullMIPException("parentResourceType");
		}
		if (parentId == null)
		{
			throw new ArgumentNullMIPException("parentId");
		}
		if (resourceType == null)
		{
			throw new ArgumentNullMIPException("resourceType");
		}
		string text = (string.IsNullOrEmpty(id) ? (BaseUrl + "/" + parentResourceType + "/" + parentId + "/" + resourceType) : (BaseUrl + "/" + parentResourceType + "/" + parentId + "/" + resourceType + "/" + id));
		text = (string.IsNullOrEmpty(methodId) ? (text + "?definitions") : (text + "?task=" + methodId + "&definitions"));
		if (_debugLog)
		{
			DebugLog("---Post RestAPI: Request: " + text + ", Body:" + json);
		}
		ClearCache(resourceType);
		RefreshCache();
		(string, int) result = PostRequest(text, json).GetAwaiter().GetResult();
		string item = result.Item1;
		int item2 = result.Item2;
		httpStatus = item2;
		if (_debugLog)
		{
			DebugLog("---         Response: " + item);
		}
		if (item != null)
		{
			return JsonObject.Parse(item);
		}
		return null;
	}

	public JsonObject Post(string methodId, JsonObject methodObject, JsonObject parameterObject, out int httpStatus)
	{
		if (methodObject == null)
		{
			throw new ArgumentNullMIPException("methodObject");
		}
		if (parameterObject == null)
		{
			throw new ArgumentNullMIPException("parameterObject");
		}
		JsonObject obj = (methodObject.GetChild("relations") ?? throw new ArgumentMIPException("relations not found")).GetChild("self") ?? throw new ArgumentMIPException("relations.self not found");
		string resourceType = obj.GetString("type");
		string id = obj.GetString("id");
		return Post(resourceType, id, methodId, parameterObject.ToJson(), out httpStatus);
	}

	public JsonObject Post(string methodId, JsonObject methodObject, string childResourceName, JsonObject parameterObject, out int httpStatus)
	{
		if (methodObject == null)
		{
			throw new ArgumentNullMIPException("methodObject");
		}
		if (parameterObject == null)
		{
			throw new ArgumentNullMIPException("parameterObject");
		}
		JsonObject obj = (methodObject.GetChild("relations") ?? throw new ArgumentMIPException("relations not found")).GetChild("self") ?? throw new ArgumentMIPException("relations.self not found");
		string parentResourceType = obj.GetString("type");
		string parentId = obj.GetString("id");
		return Post(parentResourceType, parentId, childResourceName, "", methodId, parameterObject.ToJson(), out httpStatus);
	}

	public JsonObject PostChild(string methodId, JsonObject childObject, JsonObject parameterObject, out int httpStatus)
	{
		if (childObject == null)
		{
			throw new ArgumentNullMIPException("childObject");
		}
		if (parameterObject == null)
		{
			throw new ArgumentNullMIPException("parameterObject");
		}
		JsonObject obj = childObject.GetChild("relations") ?? throw new ArgumentMIPException("relations not found");
		JsonObject obj2 = obj.GetChild("parent") ?? throw new ArgumentMIPException("relations.parent not found");
		string parentResourceType = obj2.GetString("type");
		string parentId = obj2.GetString("id");
		if (obj == null)
		{
			throw new ArgumentMIPException("relations not found");
		}
		JsonObject obj3 = obj.GetChild("self") ?? throw new ArgumentMIPException("relations.self not found");
		string resourceType = obj3.GetString("type");
		string id = obj3.GetString("id");
		return Post(parentResourceType, parentId, resourceType, id, methodId, parameterObject.ToJson(), out httpStatus);
	}

	private HttpClient GetClient()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (_httpClient == null)
		{
			EnvironmentManager.Instance.Log(error: true, "RestApiClient", "_httpClient is null - will try to reconstruct");
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_httpClient.Timeout = TimeSpan.FromSeconds(120.0);
		}
		return _httpClient;
	}

	private string GetCachedRequest(string url, string resourceType)
	{
		if (EnableCache && !string.Equals(resourceType, "tasks", StringComparison.OrdinalIgnoreCase))
		{
			RefreshCache();
			lock (_cache)
			{
				if (_cache.ContainsKey(resourceType.ToLower()) && !string.Equals(resourceType, "tasks", StringComparison.OrdinalIgnoreCase) && _cache[resourceType.ToLower()].Cache.ContainsKey(url.ToLower()))
				{
					return _cache[resourceType.ToLower()].Cache[url.ToLower()];
				}
			}
		}
		string result = GetRequest(url).GetAwaiter().GetResult();
		if (EnableCache && !string.Equals(resourceType, "tasks", StringComparison.OrdinalIgnoreCase))
		{
			lock (_cache)
			{
				if (!_cache.ContainsKey(resourceType.ToLower()))
				{
					_cache[resourceType.ToLower()] = new RestResourceCache();
					_cache[resourceType.ToLower()].Clear();
				}
				if (result == null)
				{
					_cache[resourceType.ToLower()].ResetTime = DateTime.UtcNow + TimeSpan.FromSeconds(10.0);
				}
				_cache[resourceType.ToLower()].Cache[url.ToLower()] = result;
			}
		}
		return result;
	}

	private void RefreshCache()
	{
		lock (_cache)
		{
			foreach (RestResourceCache value in _cache.Values)
			{
				value.Refresh();
			}
		}
	}

	private async Task<string> GetRequest(string url)
	{
		_ = 1;
		try
		{
			HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, url);
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
						string text2 = FormattableString.Invariant($"GET Unexpected statuscode {response.StatusCode} in '{url}'");
						try
						{
							JsonObject jsonObject = JsonObject.Parse(text)?.GetChild("error");
							if (jsonObject != null)
							{
								text2 = new ErrorRest(jsonObject).details.Aggregate(text2, (string current, DetailRest detail) => current + " - " + detail.errorText);
							}
						}
						catch
						{
						}
						throw new RestApiMIPException(text2);
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
		catch (HttpRequestException ex)
		{
			HttpRequestException ex2 = ex;
			throw new RestApiMIPException((((Exception)(object)ex2).InnerException != null) ? ((Exception)(object)ex2).InnerException.Message : ((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	private async Task<string> PatchRequest(string url, string body)
	{
		_ = 1;
		try
		{
			HttpRequestMessage request = CreateRequestMessage(new HttpMethod("PATCH"), url, body);
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
					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
						{
							throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
						}
						string text2 = FormattableString.Invariant($"PATCH Unexpected statuscode {response.StatusCode} in '{url}'");
						try
						{
							JsonObject jsonObject = JsonObject.Parse(text)?.GetChild("error");
							if (jsonObject != null)
							{
								text2 = new ErrorRest(jsonObject).details.Aggregate(text2, (string current, DetailRest detail) => current + " - " + detail.errorText);
							}
						}
						catch
						{
						}
						throw new RestApiMIPException(text2);
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
		catch (HttpRequestException ex)
		{
			HttpRequestException ex2 = ex;
			throw new RestApiMIPException((((Exception)(object)ex2).InnerException != null) ? ((Exception)(object)ex2).InnerException.Message : ((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	private async Task<string> PutRequest(string url, string body)
	{
		_ = 1;
		try
		{
			HttpRequestMessage request = CreateRequestMessage(HttpMethod.Put, url, body);
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
					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
						{
							throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
						}
						string text2 = FormattableString.Invariant($"PUT Unexpected statuscode {response.StatusCode} in '{url}'");
						try
						{
							JsonObject jsonObject = JsonObject.Parse(text)?.GetChild("error");
							if (jsonObject != null)
							{
								text2 = new ErrorRest(jsonObject).details.Aggregate(text2, (string current, DetailRest detail) => current + " - " + detail.errorText);
							}
						}
						catch
						{
						}
						throw new RestApiMIPException(text2);
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
		catch (HttpRequestException ex)
		{
			HttpRequestException ex2 = ex;
			throw new RestApiMIPException((((Exception)(object)ex2).InnerException != null) ? ((Exception)(object)ex2).InnerException.Message : ((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	private async Task<string> DeleteRequest(string url)
	{
		_ = 1;
		try
		{
			HttpRequestMessage request = CreateRequestMessage(HttpMethod.Delete, url);
			try
			{
				HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					string result = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
						{
							throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
						}
						throw new RestApiMIPException(FormattableString.Invariant($"DELETE Unexpected statuscode {response.StatusCode} in '{url}'"));
					}
					return result;
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
		catch (HttpRequestException ex)
		{
			HttpRequestException ex2 = ex;
			throw new RestApiMIPException((((Exception)(object)ex2).InnerException != null) ? ((Exception)(object)ex2).InnerException.Message : ((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	private async Task<(string, int)> PostRequest(string url, string json)
	{
		_ = 1;
		try
		{
			HttpRequestMessage request = CreateRequestMessage(HttpMethod.Post, url, json);
			try
			{
				HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					string item = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
					if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						throw new CommunicationMIPException("HttpCode: " + response.StatusCode);
					}
					return (item, (int)response.StatusCode);
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
		catch (HttpRequestException ex)
		{
			HttpRequestException ex2 = ex;
			throw new RestApiMIPException((((Exception)(object)ex2).InnerException != null) ? ((Exception)(object)ex2).InnerException.Message : ((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, string body = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		HttpRequestMessage val = new HttpRequestMessage(method, url);
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
		return val;
	}

	private void DebugLog(string text)
	{
		if (_debugLog)
		{
			EnvironmentManager.Instance.Log(error: false, "RestAPI", text);
		}
	}
}
