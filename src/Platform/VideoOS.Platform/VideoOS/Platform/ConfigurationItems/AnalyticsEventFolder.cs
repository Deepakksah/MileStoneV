using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AnalyticsEventFolder : IConfigurationItem
{
	public ICollection<AnalyticsEvent> AnalyticsEvents
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<AnalyticsEvent> list = new List<AnalyticsEvent>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "AnalyticsEvent")
				{
					list.Add(new AnalyticsEvent(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public AnalyticsEventFolder(ServerId serverId, ConfigurationItem analyticsEventFolder)
		: base(serverId, analyticsEventFolder)
	{
	}

	public AnalyticsEventFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AnalyticsEventFolder(FQID fqid)
		: base(fqid)
	{
	}

	public AnalyticsEventFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/AnalyticsEventFolder")
	{
	}

	public ServerTask AddAnalyticsEvent(string name, string description)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAnalyticsEvent");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAnalyticsEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AnalyticsEventFolder", "Unable to call method 'AddAnalyticsEvent':" + ex.Message);
		}
		return null;
	}

	public AddAnalyticsEventServerTask AddAnalyticsEvent()
	{
		try
		{
			return new AddAnalyticsEventServerTask(base.Wrapper.ExecuteMethod("AddAnalyticsEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AnalyticsEventFolder", "Unable to call method 'AddAnalyticsEvent':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveAnalyticsEvent(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveAnalyticsEvent");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveAnalyticsEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AnalyticsEventFolder", "Unable to call method 'RemoveAnalyticsEvent':" + ex.Message);
		}
		return null;
	}

	public RemoveAnalyticsEventServerTask RemoveAnalyticsEvent()
	{
		try
		{
			return new RemoveAnalyticsEventServerTask(base.Wrapper.ExecuteMethod("RemoveAnalyticsEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AnalyticsEventFolder", "Unable to call method 'RemoveAnalyticsEvent':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddAnalyticsEvent(string name)
	{
		return AddAnalyticsEvent(name, "");
	}
}
