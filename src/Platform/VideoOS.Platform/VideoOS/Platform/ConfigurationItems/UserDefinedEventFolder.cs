using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class UserDefinedEventFolder : IConfigurationItem
{
	public ICollection<UserDefinedEvent> UserDefinedEvents
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<UserDefinedEvent> list = new List<UserDefinedEvent>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "UserDefinedEvent")
				{
					list.Add(new UserDefinedEvent(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public UserDefinedEventFolder(ServerId serverId, ConfigurationItem userDefinedEventFolder)
		: base(serverId, userDefinedEventFolder)
	{
	}

	public UserDefinedEventFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public UserDefinedEventFolder(FQID fqid)
		: base(fqid)
	{
	}

	public UserDefinedEventFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/UserDefinedEventFolder")
	{
	}

	public ServerTask AddUserDefinedEvent(string name)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddUserDefinedEvent");
			configurationApiItemWrapper.SetProperty("Name", name);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddUserDefinedEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserDefinedEventFolder", "Unable to call method 'AddUserDefinedEvent':" + ex.Message);
		}
		return null;
	}

	public AddUserDefinedEventServerTask AddUserDefinedEvent()
	{
		try
		{
			return new AddUserDefinedEventServerTask(base.Wrapper.ExecuteMethod("AddUserDefinedEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserDefinedEventFolder", "Unable to call method 'AddUserDefinedEvent':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveUserDefinedEvent(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveUserDefinedEvent");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveUserDefinedEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserDefinedEventFolder", "Unable to call method 'RemoveUserDefinedEvent':" + ex.Message);
		}
		return null;
	}

	public RemoveUserDefinedEventServerTask RemoveUserDefinedEvent()
	{
		try
		{
			return new RemoveUserDefinedEventServerTask(base.Wrapper.ExecuteMethod("RemoveUserDefinedEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserDefinedEventFolder", "Unable to call method 'RemoveUserDefinedEvent':" + ex.Message);
		}
		return null;
	}
}
