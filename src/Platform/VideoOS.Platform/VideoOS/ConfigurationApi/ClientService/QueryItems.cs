using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Remoting;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.ConfigurationApi.ClientService;

public class QueryItems
{
	private ServerId _serverId;

	public QueryItems(ServerId serverId)
	{
		_serverId = serverId;
	}

	public ReadOnlyCollection<IConfigurationItem> Query(ItemFilter itemFilter, int maxResults)
	{
		if (itemFilter == null)
		{
			throw new ArgumentNullException("itemFilter");
		}
		if (maxResults < 1)
		{
			throw new ArgumentException("maxResults should be bigger than 0");
		}
		IConfigurationClientDecorator configurationClient = ConfigurationApiUtil.GetConfiguration(_serverId).ConfigurationApiManager.GetConfigurationClient(_serverId, "ConfigAPI");
		List<IConfigurationItem> list = new List<IConfigurationItem>();
		ConfigurationItem[] array = configurationClient.QueryItems(itemFilter, maxResults);
		foreach (ConfigurationItem configurationItem in array)
		{
			string typeName = "VideoOS.Platform.ConfigurationItems." + configurationItem.ItemType;
			ObjectHandle objectHandle = Activator.CreateInstanceFrom(Assembly.GetExecutingAssembly().Location, typeName, ignoreCase: false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[2] { _serverId, configurationItem }, null, null);
			if (objectHandle != null && objectHandle.Unwrap() is IConfigurationItem item)
			{
				list.Add(item);
			}
		}
		return list.AsReadOnly();
	}
}
