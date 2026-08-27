using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Proxy.ConfigApi;

public class ConfigurationApiManager
{
	private readonly Dictionary<Guid, IConfigurationClientDecorator> _configurationServicesRestApi = new Dictionary<Guid, IConfigurationClientDecorator>();

	private readonly Dictionary<Guid, IConfigurationClientDecorator> _configurationServicesConfigApi = new Dictionary<Guid, IConfigurationClientDecorator>();

	private readonly object _configServicesLock = new object();

	private readonly Dictionary<Guid, string> _productVersions = new Dictionary<Guid, string>();

	public bool UseRestApiWhenAvailable { get; set; } = true;

	public static bool EnableRestApiCaching { get; set; } = true;

	public static bool BypassApiGateway { get; set; } = false;

	public static bool EnableDebugLogging { get; set; } = false;

	public string ActiveApiType { get; private set; } = string.Empty;

	public Collection<ApiCounters> ApiCounters
	{
		get
		{
			Collection<ApiCounters> collection = new Collection<ApiCounters>();
			lock (_configServicesLock)
			{
				foreach (KeyValuePair<Guid, IConfigurationClientDecorator> item in _configurationServicesRestApi)
				{
					if (item.Value is RestApiClientDecorator restApiClientDecorator)
					{
						collection.Add(restApiClientDecorator.ApiCounters);
					}
				}
				return collection;
			}
		}
	}

	public IConfigurationClientDecorator GetConfigurationClient(ServerId serverId, string itemType)
	{
		string strA = ConfigurationService.LookupMinimumVersionForRestApi(itemType);
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
		lock (_configServicesLock)
		{
			if (loginSettings == null)
			{
				throw new ArgumentException("Unable to lookup LoginSettings from provided ServerId", "serverId");
			}
			if (loginSettings.ServerType == "XPCORS")
			{
				throw new ArgumentException("ServerId from recording server is not valid, provide a Management Server's serverId");
			}
			IConfigurationClientDecorator value = null;
			if (!_productVersions.TryGetValue(loginSettings.Guid, out var value2))
			{
				value2 = WellKnownInformation.GetProductVersion(loginSettings);
				if (!string.IsNullOrEmpty(value2))
				{
					_productVersions[loginSettings.Guid] = value2;
				}
			}
			bool flag = true;
			bool flag2 = string.Compare(strA, value2, StringComparison.OrdinalIgnoreCase) > 0 || !UseRestApiWhenAvailable;
			if (flag2)
			{
				if (_configurationServicesConfigApi.TryGetValue(loginSettings.Guid, out value))
				{
					value.UpdateLoginSettingsIfNoToken(loginSettings);
					flag = string.IsNullOrWhiteSpace(loginSettings.Token);
				}
			}
			else if (_configurationServicesRestApi.TryGetValue(loginSettings.Guid, out value))
			{
				value.UpdateLoginSettingsIfNoToken(loginSettings);
				flag = loginSettings.IdentityTokenCache == null;
			}
			if (flag)
			{
				Uri uri = (UseRestApiWhenAvailable ? WellKnownInformation.GetApiGatewayUri(loginSettings) : null);
				if (!flag2 && (uri != null || BypassApiGateway))
				{
					value = (_configurationServicesRestApi[loginSettings.Guid] = new RestApiClientDecorator(loginSettings, EnableDebugLogging)
					{
						CacheEnabled = EnableRestApiCaching
					});
					ActiveApiType = "REST";
				}
				else
				{
					value = (_configurationServicesConfigApi[loginSettings.Guid] = new ConfigurationClientDecorator(loginSettings));
					ActiveApiType = "SOAP";
				}
			}
			return value;
		}
	}

	public void ClearCache()
	{
		foreach (IConfigurationClientDecorator value in _configurationServicesRestApi.Values)
		{
			(value as RestApiClientDecorator)?.ClearCache();
		}
	}

	public void ClearCache(string resourceType)
	{
		foreach (IConfigurationClientDecorator value in _configurationServicesRestApi.Values)
		{
			(value as RestApiClientDecorator)?.ClearCache(resourceType);
		}
	}

	public void Invalidate(Guid serverId)
	{
		lock (_configServicesLock)
		{
			_configurationServicesRestApi.Remove(serverId);
		}
	}

	public void Close()
	{
		lock (_configServicesLock)
		{
			foreach (IConfigurationClientDecorator value in _configurationServicesRestApi.Values)
			{
				value.CloseConnection();
			}
			_configurationServicesRestApi.Clear();
			foreach (IConfigurationClientDecorator value2 in _configurationServicesConfigApi.Values)
			{
				value2.CloseConnection();
			}
			_configurationServicesConfigApi.Clear();
		}
	}
}
