using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public static class ConfigurationService
{
	private static string _defaultRestApiSupportVersion = "23.1";

	private static Dictionary<string, string> _itemTypeRestApiSupportVersion = new Dictionary<string, string>
	{
		{ "MIPItem", "23.2" },
		{ "MIPItemFolder", "23.2" },
		{ "ConfigAPI", "99.9" }
	};

	private static Collection<string> _hasNoChildren = new Collection<string> { "PtzPreset", "HardwareDriver" };

	public static string LookupMinimumVersionForRestApi(string itemType)
	{
		if (!_itemTypeRestApiSupportVersion.TryGetValue(itemType, out var value))
		{
			return _defaultRestApiSupportVersion;
		}
		return value;
	}

	public static ConfigurationItem GetItem(ServerId serverId, string path)
	{
		if (serverId == null)
		{
			throw new ArgumentException("serverId is null");
		}
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException("path");
		}
		try
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(path);
			return ConfigurationApiUtil.GetConfiguration(serverId).ConfigurationApiManager.GetConfigurationClient(serverId, configurationItemPath.ItemType).GetItem(path);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "ConfigurationItemService", ex.Message + Environment.NewLine + Environment.StackTrace);
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public static ConfigurationItem[] GetChildren(ServerId serverId, string path)
	{
		if (serverId == null)
		{
			throw new ArgumentException("serverId is null");
		}
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException("path");
		}
		try
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(path);
			IConfigurationClientDecorator configurationClient = ConfigurationApiUtil.GetConfiguration(serverId).ConfigurationApiManager.GetConfigurationClient(serverId, configurationItemPath.ItemType);
			if (HasItemTypeChildItems(path))
			{
				return configurationClient.GetChildItems(path);
			}
			return new ConfigurationItem[0];
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "ConfigurationItemService", ex.Message + Environment.NewLine + Environment.StackTrace);
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public static Dictionary<string, string> GetTranslations(ServerId serverId, string languageId)
	{
		if (serverId == null)
		{
			throw new ArgumentException("serverId is null");
		}
		if (string.IsNullOrEmpty(languageId))
		{
			throw new ArgumentNullException("languageId");
		}
		try
		{
			return ConfigurationApiUtil.GetConfiguration(serverId).ConfigurationApiManager.GetConfigurationClient(serverId, string.Empty).GetTranslations(languageId);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "ConfigurationItemService", ex.Message + Environment.NewLine + Environment.StackTrace);
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	private static bool HasItemTypeChildItems(string path)
	{
		string[] array = path.Split('[');
		return !_hasNoChildren.Contains(array[0]);
	}
}
