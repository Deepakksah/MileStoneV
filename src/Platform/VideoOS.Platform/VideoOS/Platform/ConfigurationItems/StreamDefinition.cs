using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StreamDefinition : IConfigurationItem
{
	public ICollection<StreamUsageChildItem> StreamUsageChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<StreamUsageChildItem> list = new List<StreamUsageChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Stream")
				{
					list.Add(new StreamUsageChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public StreamDefinition(ServerId serverId, ConfigurationItem stream)
		: base(serverId, stream)
	{
	}

	public StreamDefinition(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StreamDefinition(FQID fqid)
		: base(fqid)
	{
	}

	public ClassCollectionServerTask AddStream()
	{
		try
		{
			return new ClassCollectionServerTask(base.Wrapper.ExecuteMethod("AddStream").ExecuteMethod("AddStream"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StreamDefinition", "Unable to call method 'AddStream':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveStream(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveStream");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveStream"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StreamDefinition", "Unable to call method 'RemoveStream':" + ex.Message);
		}
		return null;
	}

	public RemoveStreamServerTask RemoveStream()
	{
		try
		{
			return new RemoveStreamServerTask(base.Wrapper.ExecuteMethod("RemoveStream"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StreamDefinition", "Unable to call method 'RemoveStream':" + ex.Message);
		}
		return null;
	}
}
