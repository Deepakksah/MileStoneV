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

public class ServiceRegistrationClient : IServiceRegistrationClient, IDisposable
{
	private string BaseUrl;

	private HttpClient _httpClient;

	private LoginSettings _loginSettings;

	private Uri _baseUri;

	private List<string> _options = new List<string>();

	public bool _debugLog;

	private const string ErrorKey = "error";

	private const string RegisteredServices = "registeredServices";

	public void SetOption(string optionValue)
	{
		_options.Add(optionValue);
	}

	public static ServiceRegistrationClient Create(LoginSettings loginSettings, bool debugLog)
	{
		string productVersion = WellKnownInformation.GetProductVersion(loginSettings);
		if (productVersion == null || string.Compare(productVersion, "24.1") < 0)
		{
			return null;
		}
		ServiceRegistrationClient serviceRegistrationClient = new ServiceRegistrationClient();
		serviceRegistrationClient._debugLog = debugLog;
		serviceRegistrationClient._loginSettings = loginSettings;
		serviceRegistrationClient._baseUri = loginSettings.Uri;
		serviceRegistrationClient.Initialize();
		return serviceRegistrationClient;
	}

	public static ServiceRegistrationClient Create(Uri uri, bool debugLog)
	{
		string productVersion = WellKnownInformation.GetProductVersion(uri.ToString());
		if (productVersion == null || string.Compare(productVersion, "24.1") < 0)
		{
			return null;
		}
		ServiceRegistrationClient serviceRegistrationClient = new ServiceRegistrationClient();
		serviceRegistrationClient._debugLog = debugLog;
		serviceRegistrationClient._loginSettings = null;
		serviceRegistrationClient._baseUri = uri;
		serviceRegistrationClient.Initialize();
		return serviceRegistrationClient;
	}

	private void Initialize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		_httpClient = new HttpClient();
		_httpClient.DefaultRequestHeaders.Accept.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_httpClient.Timeout = TimeSpan.FromSeconds(20.0);
		BaseUrl = (ConfigurationApiManager.BypassApiGateway ? string.Format(CultureInfo.InvariantCulture, "{0}ManagementServer/api", _baseUri) : WellKnownInformation.GetApiGatewayUri(_baseUri)?.ToString());
		if (_debugLog)
		{
			DebugLog("BaseUrl = " + BaseUrl);
		}
		if (BaseUrl == null)
		{
			throw new MIPException("Unable to locate server");
		}
	}

	public List<ServiceInformation> GetServices()
	{
		if (BaseUrl == null)
		{
			throw new ArgumentException("Server not ready");
		}
		List<ServiceInformation> list = new List<ServiceInformation>();
		string text = string.Empty;
		if (_options != null && _options.Count > 0 && _options.Contains("VMSinternal"))
		{
			text = "?VMSinternal=";
		}
		string url = BaseUrl + "/registeredServices" + text;
		foreach (JsonObject child in JsonObject.Parse(GetRequest(url).GetAwaiter().GetResult()).GetChild("array").GetChildren())
		{
			list.Add(ConvertJsonToClass(child));
		}
		return list;
	}

	public ServiceInformation GetService(Guid instance)
	{
		if (BaseUrl == null)
		{
			throw new ArgumentException("Server not ready");
		}
		string url = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", BaseUrl, "registeredServices", instance);
		return ConvertJsonToClass(JsonObject.Parse(GetRequest(url).GetAwaiter().GetResult() ?? throw new PathNotFoundMIPException(instance.ToString())).GetChild("data"));
	}

	public List<ServiceInformation> GetServicesOfType(Guid serviceType)
	{
		if (BaseUrl == null)
		{
			throw new ArgumentException("Server not ready");
		}
		List<ServiceInformation> list = new List<ServiceInformation>();
		string url = BaseUrl + "/registeredServices";
		foreach (JsonObject child in JsonObject.Parse(GetRequest(url).GetAwaiter().GetResult()).GetChild("array").GetChildren())
		{
			ServiceInformation serviceInformation = ConvertJsonToClass(child);
			if (serviceInformation.type == serviceType)
			{
				list.Add(serviceInformation);
			}
		}
		return list;
	}

	public void RegisterService(Guid serviceType, Guid instance, Uri[] uris, string name, string description, string customData)
	{
		RegisterService(serviceType, instance, uris, name, description, customData, null);
	}

	public void RegisterService(Guid serviceType, Guid instance, Uri[] uris, string name, string description, string customData, List<ServiceInformationEndpoint> endpoints)
	{
		if (serviceType == Guid.Empty)
		{
			throw new ArgumentNullException("serviceType");
		}
		if (instance == Guid.Empty)
		{
			throw new ArgumentNullException("instance");
		}
		if (uris == null)
		{
			throw new ArgumentNullException("uris");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (description == null)
		{
			throw new ArgumentNullException("description");
		}
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		foreach (Uri uri in uris)
		{
			jsonBuilder.AppendElement("\"" + uri.ToString() + "\"");
		}
		JsonBuilder jsonBuilder2 = null;
		if (endpoints != null)
		{
			jsonBuilder2 = new JsonBuilder(array: true);
			foreach (ServiceInformationEndpoint endpoint in endpoints)
			{
				JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
				jsonBuilder3.AppendKeyStringEscaped("authentication", endpoint.authentication);
				jsonBuilder3.AppendKeyStringEscaped("description", endpoint.description);
				jsonBuilder3.AppendKeyStringEscaped("name", endpoint.name);
				jsonBuilder3.AppendKeyStringEscaped("uri", endpoint.uri);
				jsonBuilder2.AppendElement(jsonBuilder3.ToJson());
			}
		}
		JsonBuilder jsonBuilder4 = new JsonBuilder(array: false);
		jsonBuilder4.AppendKeyStringEscaped("name", name);
		jsonBuilder4.AppendKeyStringEscaped("description", description);
		jsonBuilder4.AppendKeyString("serviceType", serviceType.ToString());
		jsonBuilder4.AppendKeyValue("enabled", "true");
		if (!string.IsNullOrEmpty(customData))
		{
			jsonBuilder4.AppendKeyStringEscaped("customData", customData);
		}
		jsonBuilder4.AppendKeyValue("uris", jsonBuilder.ToJson());
		jsonBuilder4.AppendKeyString("id", instance.ToString());
		if (jsonBuilder2 != null)
		{
			jsonBuilder4.AppendKeyValue("endpoints", jsonBuilder2.ToJson());
		}
		int httpStatus;
		JsonObject response = Post(jsonBuilder4.ToJson(), out httpStatus);
		if (httpStatus != 201)
		{
			ThrowExceptionFromHttpCode(httpStatus, response);
		}
	}

	public void UnregisterService(Guid instance)
	{
		if (instance == Guid.Empty)
		{
			throw new ArgumentNullException("instance");
		}
		int httpStatus;
		JsonObject response = Delete(instance, out httpStatus);
		if (httpStatus != 200)
		{
			ThrowExceptionFromHttpCode(httpStatus, response);
		}
	}

	public void UpdateService(Guid instance, bool? enabled, string customData, string[] uris)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (uris != null)
		{
			JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
			foreach (string elementWithBrackets in uris)
			{
				jsonBuilder2.AppendElement(elementWithBrackets);
			}
			jsonBuilder.AppendKeyValue("uris", jsonBuilder2.ToJson());
		}
		if (enabled.HasValue)
		{
			jsonBuilder.AppendKeyValue("enabled", enabled.Value.ToString());
		}
		if (customData != null)
		{
			jsonBuilder.AppendKeyStringEscaped("customData", customData);
		}
		int httpStatus;
		JsonObject response = Patch(instance, jsonBuilder.ToJson(), out httpStatus);
		if (httpStatus != 200)
		{
			ThrowExceptionFromHttpCode(httpStatus, response);
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

	private void DebugLog(string text)
	{
		if (_debugLog && EnvironmentManager.Instance != null)
		{
			EnvironmentManager.Instance.Log(error: false, "registeredServices", text);
		}
	}

	private static void ThrowExceptionFromHttpCode(int httpCode, JsonObject response)
	{
		string message = string.Empty;
		JsonObject jsonObject = response?.GetChild("error");
		if (jsonObject != null)
		{
			JsonObject child = jsonObject.GetChild("details");
			if (child != null && child.IsArray)
			{
				message = child.GetChildren().First().GetString("errorText");
			}
		}
		switch (httpCode)
		{
		case 404:
			throw new PathNotFoundMIPException();
		case 401:
			throw new NotAuthorizedMIPException(message);
		case 400:
			throw new InvalidParameterMIPException(message);
		case 403:
			throw new NotAuthorizedMIPException(message);
		default:
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "Unknown error: httpCode={0}", httpCode));
		}
	}

	private static ServiceInformation ConvertJsonToClass(JsonObject s)
	{
		ServiceInformation serviceInformation = new ServiceInformation
		{
			description = s.GetString("description"),
			name = s.GetString("name"),
			customData = s.GetString("customData"),
			trusted = s.GetBool("trusted").Value,
			enabled = s.GetBool("enabled").Value
		};
		if (Guid.TryParse(s.GetString("id"), out var result))
		{
			serviceInformation.instance = result;
		}
		if (Guid.TryParse(s.GetString("serviceType"), out var result2))
		{
			serviceInformation.type = result2;
		}
		serviceInformation.uris = new List<string>();
		JsonObject child = s.GetChild("uris");
		if (child != null)
		{
			foreach (JsonObject child3 in child.GetChildren())
			{
				serviceInformation.uris.Add(child3.Value);
			}
		}
		serviceInformation.endpoints = new List<ServiceInformationEndpoint>();
		JsonObject child2 = s.GetChild("endpoints");
		if (child2 != null)
		{
			foreach (JsonObject child4 in child2.GetChildren())
			{
				ServiceInformationEndpoint item = new ServiceInformationEndpoint
				{
					authentication = child4.GetString("authentication"),
					description = child4.GetString("description"),
					name = child4.GetString("name"),
					uri = child4.GetString("uri")
				};
				serviceInformation.endpoints.Add(item);
			}
		}
		return serviceInformation;
	}

	private HttpClient GetClient()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		if (_httpClient == null)
		{
			DebugLog("_httpClient is null - will try to reconstruct");
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loginSettings.IdentityTokenCache.Token);
			_httpClient.Timeout = TimeSpan.FromSeconds(120.0);
		}
		return _httpClient;
	}

	private async Task<string> GetRequest(string url)
	{
		HttpRequestMessage request = CreateRequestMessage(HttpMethod.Get, url);
		try
		{
			HttpResponseMessage response = await GetClient().SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (response.Content == null)
				{
					if (_debugLog)
					{
						DebugLog("---GET API: Request: " + url + ", Response: null");
					}
					return null;
				}
				string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (_debugLog)
				{
					DebugLog("---GET API: Request: " + url + ", Response: " + text);
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

	private JsonObject Post(string json, out int httpStatus)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", BaseUrl, "registeredServices");
		if (_debugLog)
		{
			DebugLog("---Post API: Request: " + text + ", Body:" + json);
		}
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

	private JsonObject Patch(Guid instance, string json, out int httpStatus)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", BaseUrl, "registeredServices", instance);
		if (_debugLog)
		{
			DebugLog("---PATCH API: Request: " + text + ", Body:" + json);
		}
		(string, int) result = PatchRequest(text, json).GetAwaiter().GetResult();
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

	private JsonObject Delete(Guid instance, out int httpStatus)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", BaseUrl, "registeredServices", instance);
		if (_debugLog)
		{
			DebugLog("---DELETE API: Request: " + text);
		}
		(string, int) result = DeleteRequest(text).GetAwaiter().GetResult();
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

	private async Task<(string, int)> PostRequest(string url, string json)
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

	private async Task<(string, int)> PatchRequest(string url, string json)
	{
		HttpRequestMessage request = CreateRequestMessage(new HttpMethod("PATCH"), url, json);
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

	private async Task<(string, int)> DeleteRequest(string url)
	{
		HttpRequestMessage request = CreateRequestMessage(HttpMethod.Delete, url);
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

	private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, string body = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		HttpRequestMessage val = new HttpRequestMessage(method, url);
		if (body != null)
		{
			val.Content = (HttpContent)new StringContent(body, Encoding.UTF8, "application/json");
		}
		if (_loginSettings != null && _loginSettings.IdentityTokenCache != null)
		{
			val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loginSettings.IdentityTokenCache.Token);
		}
		return val;
	}
}
