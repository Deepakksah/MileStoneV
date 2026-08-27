using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class TimeProfileFolder : IConfigurationItem
{
	public ICollection<TimeProfile> TimeProfiles
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<TimeProfile> list = new List<TimeProfile>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "TimeProfile")
				{
					list.Add(new TimeProfile(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public TimeProfileFolder(ServerId serverId, ConfigurationItem timeProfileFolder)
		: base(serverId, timeProfileFolder)
	{
	}

	public TimeProfileFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public TimeProfileFolder(FQID fqid)
		: base(fqid)
	{
	}

	public TimeProfileFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/TimeProfileFolder")
	{
	}

	public ServerTask AddTimeProfile(string name, string description, string timeProfileType)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddTimeProfile");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("TimeProfileType", timeProfileType);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfileFolder", "Unable to call method 'AddTimeProfile':" + ex.Message);
		}
		return null;
	}

	public AddTimeProfileServerTask AddTimeProfile()
	{
		try
		{
			return new AddTimeProfileServerTask(base.Wrapper.ExecuteMethod("AddTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfileFolder", "Unable to call method 'AddTimeProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveTimeProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveTimeProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfileFolder", "Unable to call method 'RemoveTimeProfile':" + ex.Message);
		}
		return null;
	}

	public RemoveTimeProfileServerTask RemoveTimeProfile()
	{
		try
		{
			return new RemoveTimeProfileServerTask(base.Wrapper.ExecuteMethod("RemoveTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfileFolder", "Unable to call method 'RemoveTimeProfile':" + ex.Message);
		}
		return null;
	}
}
