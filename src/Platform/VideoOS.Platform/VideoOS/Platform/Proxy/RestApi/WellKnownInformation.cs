using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.RestApi;

public static class WellKnownInformation
{
	private const string WellKnownUrisManagementServer = "ManagementServer/.well-known/uris";

	private const string WellKnownApiGateway = "api/.well-known/uris";

	private const string ServerUpTestUri = ".well-known/uris";

	private const string ApiGatewayKey = "ApiGateways";

	private const string ProductVersionKey = "ProductVersion";

	private const string SecureManagementServerKey = "SecureManagementServer";

	private const int DefaultHttpTimeoutInSeconds = 5;

	private static readonly Dictionary<Guid, WellKnownInformationResponse> _wellKnownInformationResponses = new Dictionary<Guid, WellKnownInformationResponse>();

	private static int HttpTimeoutInSeconds
	{
		get
		{
			int result = 5;
			if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("ConnectionCheckTimeout", out var value))
			{
				int.TryParse(value, out result);
			}
			return result;
		}
	}

	public static Uri GetApiGatewayUri(LoginSettings loginSettings)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullMIPException("loginSettings");
		}
		return GetApiGatewayUri(GetWellKnownInfo(loginSettings), loginSettings);
	}

	public static Uri GetApiGatewayUri(Uri serverUri)
	{
		return GetApiGatewayUri(GetWellKnownInfo(serverUri));
	}

	private static Uri GetApiGatewayUri(JsonObject wellKnownInfo, LoginSettings loginSettings = null)
	{
		if (wellKnownInfo == null)
		{
			return null;
		}
		string text = new Uri(wellKnownInfo.GetChild("SecureManagementServer").Value).Host.ToLower();
		JsonObject child = wellKnownInfo.GetChild("ApiGateways");
		string text2 = wellKnownInfo.GetString("ProductVersion");
		if (!string.IsNullOrEmpty(text2) && text2.CompareTo("23.1") > 0 && child != null && child.IsArray && !child.IsEmptyArray)
		{
			Collection<JsonObject> children = child.GetChildren();
			if (children.Count > 0)
			{
				int num = RandomIndex(children.Count);
				int num2 = num;
				Uri uri = null;
				do
				{
					Uri uri2 = new Uri(children[num].Value);
					if (uri2.Host.ToLower() == text)
					{
						uri = uri2;
					}
					else if (PerformHTTPGetAuthOrNoAuth(new Uri(uri2, ".well-known/uris"), loginSettings) != null)
					{
						return new Uri(uri2, "rest/v1");
					}
					num++;
					if (num >= children.Count)
					{
						num = 0;
					}
				}
				while (num != num2);
				if (uri != null && PerformHTTPGetAuthOrNoAuth(new Uri(uri, ".well-known/uris"), loginSettings) != null)
				{
					return new Uri(uri, "rest/v1");
				}
			}
		}
		return null;
	}

	public static string GetProductVersion(LoginSettings loginSettings)
	{
		return GetWellKnownInfo(loginSettings)?.GetString("ProductVersion");
	}

	public static string GetProductVersion(string serverAddress)
	{
		return GetWellKnownInfo(new Uri(serverAddress))?.GetString("ProductVersion");
	}

	private static JsonObject GetWellKnownInfo(LoginSettings loginSettings)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		if (!loginSettings.IsOAuthConnection || loginSettings.IdentityTokenCache == null)
		{
			return null;
		}
		lock (_wellKnownInformationResponses)
		{
			if (_wellKnownInformationResponses.TryGetValue(loginSettings.InstanceGuid, out var value) && value.ValidUntilDateTime > DateTime.UtcNow)
			{
				return value.JsonObject;
			}
		}
		string text = PerformHTTPGet(new Uri(loginSettings.Uri, "api/.well-known/uris"));
		if (text == null)
		{
			text = PerformHTTPGet(new Uri(loginSettings.Uri, "ManagementServer/.well-known/uris"));
		}
		lock (_wellKnownInformationResponses)
		{
			if (text == null)
			{
				_wellKnownInformationResponses[loginSettings.InstanceGuid] = new WellKnownInformationResponse(null);
				return null;
			}
		}
		JsonObject jsonObject = JsonObject.Parse(text);
		lock (_wellKnownInformationResponses)
		{
			_wellKnownInformationResponses[loginSettings.InstanceGuid] = new WellKnownInformationResponse(jsonObject);
			foreach (KeyValuePair<Guid, WellKnownInformationResponse> item in _wellKnownInformationResponses.Where((KeyValuePair<Guid, WellKnownInformationResponse> w) => w.Value.ValidUntilDateTime < DateTime.UtcNow).ToList())
			{
				if (item.Key != Guid.Empty)
				{
					_wellKnownInformationResponses.Remove(item.Key);
				}
			}
			return jsonObject;
		}
	}

	private static JsonObject GetWellKnownInfo(Uri serverUri)
	{
		string text = PerformHTTPGet(new Uri(serverUri, "api/.well-known/uris"));
		if (text == null)
		{
			text = PerformHTTPGet(new Uri(serverUri, "ManagementServer/.well-known/uris"));
		}
		return JsonObject.Parse(text);
	}

	private static int RandomIndex(int max)
	{
		return new Random(DateTime.Now.Millisecond).Next(max);
	}

	private static string PerformHTTPGetAuthOrNoAuth(Uri url, LoginSettings loginSettings)
	{
		if (loginSettings != null)
		{
			return PerformHTTPGet(loginSettings, url);
		}
		return PerformHTTPGet(url);
	}

	private static string PerformHTTPGet(Uri url)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		HttpClient val = new HttpClient();
		try
		{
			val.DefaultRequestHeaders.Accept.Clear();
			val.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			val.Timeout = TimeSpan.FromSeconds(HttpTimeoutInSeconds);
			try
			{
				HttpResponseMessage result = val.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				if (result.Content == null)
				{
					return null;
				}
				string result2 = result.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				if (result.StatusCode != HttpStatusCode.OK)
				{
					return null;
				}
				return result2;
			}
			catch (HttpRequestException)
			{
				return null;
			}
			catch (TaskCanceledException)
			{
				return null;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static string PerformHTTPGet(LoginSettings loginSettings, Uri url)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		if (loginSettings.IdentityTokenCache == null)
		{
			return null;
		}
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				val.DefaultRequestHeaders.Accept.Clear();
				val.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				val.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginSettings.IdentityTokenCache.Token);
				val.Timeout = TimeSpan.FromSeconds(HttpTimeoutInSeconds);
				HttpResponseMessage result = val.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				if (result.Content == null)
				{
					return null;
				}
				string result2 = result.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				if (result.StatusCode != HttpStatusCode.OK)
				{
					return null;
				}
				return result2;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (HttpRequestException)
		{
			return null;
		}
		catch (TaskCanceledException)
		{
			return null;
		}
	}
}
